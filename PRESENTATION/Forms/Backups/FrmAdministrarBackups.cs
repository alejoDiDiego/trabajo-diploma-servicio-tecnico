using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Backups;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Backups
{
    public partial class FrmAdministrarBackups : Form, IObservador
    {
        private readonly BackupService _servicio = new BackupService();
        private readonly SesionIdioma _idioma = SesionIdioma.GetInstance();
        private bool _ocupado, _cerrarPendiente, _salir;
        private CancellationTokenSource _cancelar;
        private string _estado = "Backups.Listo";
        public bool RequiereSalida { get { return _salir; } }
        public FrmAdministrarBackups()
        {
            InitializeComponent(); _idioma.RegistrarObservador(this); Actualizar(_idioma.idioma);
        }
        private string T(string clave) { return _idioma.idioma == null ? clave : _idioma.idioma.BuscarTraduccion(clave); }
        public void Actualizar(IIdioma idioma)
        {
            BTN_Ayuda.Text = UI.Forms.Ayuda.AyudaManual.Texto("Ayuda.Boton", "? Ayuda");
            Text = T("Menu.AdministrarBackups");
            BTN_Crear.Text = T("Backups.Crear"); BTN_Restaurar.Text = T("Backups.Restaurar");
            LBL_Aviso.Text = T("Backups.Aviso"); LBL_Estado.Text = T(_estado);
            AplicarPermisos();
        }
        private void AplicarPermisos()
        {
            BTN_Ayuda.Enabled = !_ocupado;
            BTN_Crear.Enabled = !_ocupado && BackupService.Puede(CodigosPermiso.BackupsCrear);
            BTN_Restaurar.Enabled = !_ocupado && BackupService.Puede(CodigosPermiso.BackupsRestaurar);
        }
        private bool Autorizar(string codigo)
        {
            if (BackupService.Puede(codigo)) return true;
            MessageBox.Show(this, T("Mensaje.SinPermisos"), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            AplicarPermisos(); return false;
        }
        private async void BTN_Crear_Click(object sender, EventArgs e)
        {
            if (_ocupado || !Autorizar(CodigosPermiso.BackupsCrear)) return;
            using (var dialogo = new SaveFileDialog { Filter = T("Backups.Filtro"), DefaultExt = "bak", AddExtension = true,
                FileName = "TPIntegrador-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bak", OverwritePrompt = true })
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK || !Autorizar(CodigosPermiso.BackupsCrear)) return;
                bool sobrescribir = File.Exists(dialogo.FileName);
                await Ejecutar(async (p, c) => { await _servicio.CrearAsync(dialogo.FileName, sobrescribir, p, c);
                    if (!_cerrarPendiente) MessageBox.Show(this, T("Backups.Creado"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information); });
            }
        }
        private async void BTN_Restaurar_Click(object sender, EventArgs e)
        {
            if (_ocupado || !Autorizar(CodigosPermiso.BackupsRestaurar)) return;
            using (var dialogo = new OpenFileDialog { Filter = T("Backups.Filtro"), CheckFileExists = true, Multiselect = false })
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK || !Autorizar(CodigosPermiso.BackupsRestaurar)) return;
                if (MessageBox.Show(this, T("Backups.CerrarFormularios"), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                var principal = Owner as FrmPrincipal;
                if (principal == null || !principal.PrepararRestauracionBackup()) return;
                await Ejecutar(async (p, c) =>
                {
                    string recuperacion = await _servicio.RestaurarAsync(dialogo.FileName, destino =>
                        Task.FromResult(!_cerrarPendiente && MessageBox.Show(this,
                            T("Backups.Confirmar").Replace("{0}", destino).Replace("{1}", dialogo.FileName), Text,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes), p, c);
                    if (recuperacion == null) return;
                    _salir = true;
                    if (SessionManager.HaySesionActiva()) SessionManager.Logout();
                    MessageBox.Show(this, T("Backups.Restaurado") + "\r\n" + recuperacion, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
            }
        }
        private async Task Ejecutar(Func<IProgress<string>, CancellationToken, Task> accion)
        {
            _ocupado = true; _cancelar = new CancellationTokenSource(); AplicarPermisos();
            PRG_Actividad.Style = ProgressBarStyle.Marquee;
            try
            {
                await accion(new Progress<string>(clave => { _estado = clave; LBL_Estado.Text = T(clave); }), _cancelar.Token);
            }
            catch (OperationCanceledException) { _estado = "Backups.Cancelado"; }
            catch (BackupOperacionException ex)
            {
                _estado = "Backups.Error";
                if (ex.RequiereSalida) { _salir = true; if (SessionManager.HaySesionActiva()) SessionManager.Logout(); }
                MessageBox.Show(this, T(ex.Clave) + (ex.Recuperacion == null ? "" : "\r\n" + ex.Recuperacion), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { _estado = "Backups.Error"; MessageBox.Show(this, T("Backups.Error"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            {
                _ocupado = false; _cancelar.Dispose(); _cancelar = null;
                PRG_Actividad.Style = ProgressBarStyle.Blocks; AplicarPermisos();
                _estado = _estado == "Backups.Error" || _estado == "Backups.Cancelado" ? _estado : "Backups.Listo";
                LBL_Estado.Text = T(_estado);
                if (_cerrarPendiente || _salir) Close();
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_ocupado)
            {
                e.Cancel = true; _cerrarPendiente = true; _cancelar.Cancel();
                _estado = "Backups.Esperando"; LBL_Estado.Text = T(_estado);
            }
            base.OnFormClosing(e);
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        { _idioma.DesregistrarObservador(this); base.OnFormClosed(e); }

        private void BTN_Ayuda_Click(object sender, EventArgs e)
        {
            UI.Forms.Ayuda.AyudaManual.Mostrar(this, "backups");
        }
    }
}
