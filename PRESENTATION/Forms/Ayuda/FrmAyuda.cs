using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using SERVICES.Idiomas;

namespace UI.Forms.Ayuda
{
    public partial class FrmAyuda : Form, IObservador
    {
        private readonly string _archivo = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ayuda", "manual.html"));
        private readonly string _direccionNativa;
        private string _pendiente;
        private string _solicitada;
        private string _error;
        private bool _listo;
        private readonly Timer _carga;

        public FrmAyuda(string seccion)
        {
            InitializeComponent();
            _direccionNativa = DireccionNativa(new Uri(_archivo).AbsoluteUri);
            _solicitada = seccion ?? "indice";
            _carga = new Timer(components) { Interval = 10000 };
            _carga.Tick += (s, e) =>
            {
                if (CompletarCarga()) return;
                _carga.Stop();
                WEB_Manual.Stop();
                MostrarError("Ayuda.ArchivoAusente");
            };
            WEB_Manual.Navigated += (s, e) =>
            {
                if (Permitida(e.Url)) BeginInvoke((Action)(() => CompletarCarga()));
            };
            SesionIdioma.GetInstance().RegistrarObservador(this);
            Actualizar(SesionIdioma.GetInstance().idioma);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Abrir(_solicitada);
        }

        public void Actualizar(IIdioma idioma)
        {
            Text = AyudaManual.Texto("Ayuda.Titulo", "Manual de ayuda", idioma);
            LBL_Aviso.Text = AyudaManual.Texto("Ayuda.ContenidoEspanol", "El contenido del manual está en español.", idioma);
            BTN_Indice.Text = AyudaManual.Texto("Ayuda.Indice", "Índice", idioma);
            BTN_Reintentar.Text = AyudaManual.Texto("Ayuda.Reintentar", "Reintentar", idioma);
            BTN_Cerrar.Text = AyudaManual.Texto("Accion.Cerrar", "Cerrar", idioma);
            MostrarError(_error);
        }

        private void MostrarError(string clave)
        {
            _error = clave;
            LBL_Error.Visible = clave != null;
            LBL_Error.Text = clave == null ? "" : AyudaManual.Texto(clave,
                clave == "Ayuda.SeccionAusente" ? "No se encontró la sección. Se muestra el índice." : "No se pudo abrir el manual local. Compruebe Ayuda/manual.html y pulse Reintentar, o cierre la ayuda.");
        }

        private void SolicitarSeccion(string seccion)
        {
            _solicitada = seccion ?? "indice";
            _pendiente = AyudaManual.Secciones.Contains(_solicitada) ? _solicitada : "indice";
            MostrarError(_pendiente == _solicitada ? null : "Ayuda.SeccionAusente");
        }

        private void Abrir(string seccion)
        {
            SolicitarSeccion(seccion);
            _carga.Stop();
            try
            {
                using (var lectura = File.Open(_archivo, FileMode.Open, FileAccess.Read, FileShare.Read))
                    if (lectura.Length == 0) throw new IOException();
                WEB_Manual.Visible = true;
                if (_listo && CompletarCarga()) return;
                _listo = false;
                _carga.Start();
                WEB_Manual.Navigate(new Uri(_archivo));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is System.Runtime.InteropServices.COMException)
            {
                _carga.Stop();
                _listo = false;
                WEB_Manual.Visible = false;
                MostrarError("Ayuda.ArchivoAusente");
            }
        }

        private static string DireccionNativa(string direccion)
        {
            foreach (char caracter in Uri.UnescapeDataString(direccion))
            {
                if (caracter <= 127) continue;
                string texto = caracter.ToString();
                byte[] bytes = Encoding.Default.GetBytes(texto);
                if (Encoding.Default.GetString(bytes) != texto) continue;
                string escapado = "";
                foreach (byte valor in bytes) escapado += "%" + valor.ToString("X2");
                direccion = direccion.Replace(Uri.EscapeDataString(texto), escapado);
            }
            return direccion;
        }

        private bool Permitida(Uri uri)
        {
            if (uri == null || !uri.IsAbsoluteUri || !uri.IsFile || uri.IsUnc || uri.Query.Length != 0) return false;
            try
            {
                if (!string.Equals(Path.GetFullPath(uri.LocalPath), _archivo, StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(uri.GetLeftPart(UriPartial.Path), _direccionNativa, StringComparison.OrdinalIgnoreCase)) return false;
                string seccion = Uri.UnescapeDataString(uri.Fragment.TrimStart('#'));
                return seccion.Length == 0 || AyudaManual.Secciones.Contains(seccion);
            }
            catch (ArgumentException) { return false; }
            catch (NotSupportedException) { return false; }
        }

        private void WEB_Manual_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = !Permitida(e.Url) || !string.IsNullOrEmpty(e.TargetFrameName);
            if (!e.Cancel && e.Url.Fragment.Length > 1)
            {
                e.Cancel = true;
                SolicitarSeccion(Uri.UnescapeDataString(e.Url.Fragment.Substring(1)));
                CompletarCarga();
            }
        }

        private void WEB_Manual_NewWindow(object sender, CancelEventArgs e) { e.Cancel = true; }
        private void WEB_Manual_FileDownload(object sender, EventArgs e)
        {
            if (WEB_Manual.Url != null && !Permitida(WEB_Manual.Url)) WEB_Manual.Stop();
        }

        private bool CompletarCarga()
        {
            if (IsDisposed || Disposing || (!_listo && !_carga.Enabled)
                || WEB_Manual.ReadyState != WebBrowserReadyState.Complete
                || !Permitida(WEB_Manual.Url) || WEB_Manual.Document == null
                || !Permitida(WEB_Manual.Document.Url)) return false;
            _carga.Stop();
            _listo = true;
            Desplazar();
            return true;
        }

        private void WEB_Manual_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (Permitida(e.Url) && CompletarCarga()) BTN_Indice.Focus();
        }

        private void Desplazar()
        {
            if (WEB_Manual.Document == null || _pendiente == null) return;
            var destino = WEB_Manual.Document.GetElementById(_pendiente);
            _pendiente = null;
            if (destino == null)
            {
                MostrarError("Ayuda.SeccionAusente");
                destino = WEB_Manual.Document.GetElementById("indice");
            }
            if (destino != null) destino.ScrollIntoView(true);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) { Close(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BTN_Indice_Click(object sender, EventArgs e) { Abrir("indice"); }
        private void BTN_Reintentar_Click(object sender, EventArgs e) { Abrir(_solicitada); }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _carga.Stop();
            SesionIdioma.GetInstance().DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
