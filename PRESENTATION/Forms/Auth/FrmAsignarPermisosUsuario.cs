using System;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Usuarios;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Usuarios;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Auth
{
    public partial class FrmAsignarPermisosUsuario : Form, IObservador
    {
        private readonly UsuarioService _usuarioService;
        private readonly UsuarioPermisoService _usuarioPermisoService;
        private readonly SesionIdioma _sesionIdioma;
        private int _idUsuarioSeleccionado;

        public FrmAsignarPermisosUsuario()
        {
            _usuarioService = new UsuarioService();
            _usuarioPermisoService = new UsuarioPermisoService();
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            // Traduce los textos usando las claves guardadas en Tag.
            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            GBX_Usuarios.Text = idiomaObservado.BuscarTraduccion(GBX_Usuarios.Tag.ToString());
            GBX_Disponibles.Text = idiomaObservado.BuscarTraduccion(GBX_Disponibles.Tag.ToString());
            GBX_Asignadas.Text = idiomaObservado.BuscarTraduccion(GBX_Asignadas.Tag.ToString());

            ConfigurarColumnasUsuarios();
        }

        private void FrmAsignarPermisosUsuario_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);

            // La pantalla solo se abre si el usuario puede asignar permisos.
            if (!PuedeAdministrarAsignaciones())
            {
                MostrarAdvertencia("Mensaje.SinPermisos");
                Close();
                return;
            }

            CargarUsuarios();
            Actualizar(_sesionIdioma.idioma);
            ActualizarBotones();
        }

        private void DGV_Usuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Al seleccionar usuario, se cargan sus familias asignadas/disponibles.
            Usuario usuario = ObtenerUsuarioSeleccionado();

            if (usuario == null)
            {
                _idUsuarioSeleccionado = 0;
                LimpiarFamilias();
                ActualizarBotones();
                return;
            }

            _idUsuarioSeleccionado = usuario.Id;
            CargarFamiliasUsuario();
            ActualizarBotones();
        }

        private void LBX_Disponibles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarBotones();
        }

        private void LBX_Asignadas_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarBotones();
        }

        private void BTN_Asignar_Click(object sender, EventArgs e)
        {
            // Asigna la familia elegida de la lista izquierda.
            if (!PuedeAdministrarAsignaciones())
            {
                MostrarAdvertencia("Mensaje.SinPermisos");
                return;
            }

            if (_idUsuarioSeleccionado == 0)
            {
                MostrarAdvertencia("Mensaje.SeleccioneUsuario");
                return;
            }

            FamiliaPermiso familia = LBX_Disponibles.SelectedItem as FamiliaPermiso;

            if (familia == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneFamilia");
                return;
            }

            try
            {
                _usuarioPermisoService.AsignarFamilia(_idUsuarioSeleccionado, familia.Id);
                CargarFamiliasUsuario();
                MostrarExito("Mensaje.FamiliaAsignada");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Quitar_Click(object sender, EventArgs e)
        {
            // Quita la familia elegida de la lista derecha.
            if (!PuedeAdministrarAsignaciones())
            {
                MostrarAdvertencia("Mensaje.SinPermisos");
                return;
            }

            if (_idUsuarioSeleccionado == 0)
            {
                MostrarAdvertencia("Mensaje.SeleccioneUsuario");
                return;
            }

            FamiliaPermiso familia = LBX_Asignadas.SelectedItem as FamiliaPermiso;

            if (familia == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneFamilia");
                return;
            }

            try
            {
                _usuarioPermisoService.QuitarFamilia(_idUsuarioSeleccionado, familia.Id);
                CargarFamiliasUsuario();
                MostrarExito("Mensaje.FamiliaQuitadaUsuario");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarUsuarios()
        {
            // Refresca la grilla principal de usuarios.
            DGV_Usuarios.DataSource = new BindingList<Usuario>(_usuarioService.Listar());
            ConfigurarColumnasUsuarios();
            DGV_Usuarios.ClearSelection();
            _idUsuarioSeleccionado = 0;
            LimpiarFamilias();
        }

        private void CargarFamiliasUsuario()
        {
            // Refresca las dos listas luego de seleccionar/asignar/quitar.
            LBX_Disponibles.DataSource = null;
            LBX_Disponibles.DisplayMember = "Nombre";
            LBX_Disponibles.DataSource = _usuarioPermisoService.ListarFamiliasDisponibles(_idUsuarioSeleccionado);
            LBX_Disponibles.SelectedIndex = -1;

            LBX_Asignadas.DataSource = null;
            LBX_Asignadas.DisplayMember = "Nombre";
            LBX_Asignadas.DataSource = _usuarioPermisoService.ListarFamiliasAsignadas(_idUsuarioSeleccionado);
            LBX_Asignadas.SelectedIndex = -1;

            ActualizarBotones();
        }

        private void LimpiarFamilias()
        {
            LBX_Disponibles.DataSource = null;
            LBX_Asignadas.DataSource = null;
        }

        private Usuario ObtenerUsuarioSeleccionado()
        {
            if (DGV_Usuarios.SelectedRows.Count == 0)
                return null;

            return DGV_Usuarios.SelectedRows[0].DataBoundItem as Usuario;
        }

        private void ActualizarBotones()
        {
            // Los botones se habilitan solo con usuario y familia seleccionados.
            bool puedeAsignar = PuedeAdministrarAsignaciones();
            BTN_Asignar.Enabled = puedeAsignar && _idUsuarioSeleccionado > 0 && LBX_Disponibles.SelectedItem is FamiliaPermiso;
            BTN_Quitar.Enabled = puedeAsignar && _idUsuarioSeleccionado > 0 && LBX_Asignadas.SelectedItem is FamiliaPermiso;
        }

        private bool PuedeAdministrarAsignaciones()
        {
            // Permiso atomico necesario para administrar asignaciones.
            return SessionManager.HaySesionActiva() &&
                SessionManager.TienePermiso(CodigosPermiso.PermisosAsignarUsuarios);
        }

        private void MostrarExito(string claveMensaje)
        {
            MessageBox.Show(
                T(claveMensaje),
                T("Titulo.Exito"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void MostrarAdvertencia(string claveMensaje)
        {
            MessageBox.Show(
                T(claveMensaje),
                T("Titulo.Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(
                T("Mensaje.ErrorAsignarPermisos").Replace("{0}", ex.Message),
                T("Titulo.Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private string T(string clave)
        {
            // Traduce una clave o devuelve la clave si aun no hay idioma cargado.
            if (_sesionIdioma.idioma == null)
                return clave;

            return _sesionIdioma.idioma.BuscarTraduccion(clave);
        }

        private void ConfigurarColumnasUsuarios()
        {
            // Oculta password y traduce encabezados visibles.
            if (DGV_Usuarios.Columns.Contains("Id"))
                DGV_Usuarios.Columns["Id"].HeaderText = T("Columna.Id");
            if (DGV_Usuarios.Columns.Contains("Username"))
                DGV_Usuarios.Columns["Username"].HeaderText = T("Columna.Username");
            if (DGV_Usuarios.Columns.Contains("Password"))
                DGV_Usuarios.Columns["Password"].Visible = false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
