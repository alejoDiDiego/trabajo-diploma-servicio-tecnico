using System;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Usuarios;
using DOMAIN.Features.Usuarios;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Auth
{
    public partial class FrmAdministrarUsuarios : Form, IObservador
    {
        private BindingList<Usuario> _usuariosBindingList = null;
        private readonly SesionIdioma _sesionIdioma;

        public FrmAdministrarUsuarios()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            BTN_CerrarSesion.Text = idiomaObservado.BuscarTraduccion(BTN_CerrarSesion.Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            BTN_CrearUsuario.Text = idiomaObservado.BuscarTraduccion(BTN_CrearUsuario.Tag.ToString());
            label2.Text = idiomaObservado.BuscarTraduccion(label2.Tag.ToString());
            label1.Text = idiomaObservado.BuscarTraduccion(label1.Tag.ToString());
            LBL_NuevoUsuario.Text = idiomaObservado.BuscarTraduccion(LBL_NuevoUsuario.Tag.ToString());
            BTN_EliminarUsuario.Text = idiomaObservado.BuscarTraduccion(BTN_EliminarUsuario.Tag.ToString());
            BTN_EditarUsuario.Text = idiomaObservado.BuscarTraduccion(BTN_EditarUsuario.Tag.ToString());

            ActualizarDatosSesion();
            ConfigurarColumnasUsuarios();
        }

        private void ActualizarLista() { 
            UsuarioService usuarioService = new UsuarioService();
            _usuariosBindingList = new BindingList<Usuario>(usuarioService.Listar());
            DGV_Usuarios.DataSource = _usuariosBindingList;
            ConfigurarColumnasUsuarios();
        }

        private void FrmAdministrarCuentas_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            BTN_CerrarSesion.Visible = false;

            if (!SessionManager.HaySesionActiva())
            {
                PNL_Permisos.Visible = false;
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            PNL_Permisos.Visible = true;
            ActualizarDatosSesion();

            UsuarioService usuarioService = new UsuarioService();
            _usuariosBindingList = new BindingList<Usuario>(usuarioService.Listar());
            DGV_Usuarios.DataSource = _usuariosBindingList;
            ConfigurarColumnasUsuarios();
        }

        private void BTN_CrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioService usuarioService = new UsuarioService();

                Usuario usuario = usuarioService.Crear(TBX_Username.Text, TBX_Password.Text);
                _usuariosBindingList.Add(usuario);

                TBX_Username.Clear();
                TBX_Password.Clear();

                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.UsuarioCreado"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorCrearUsuario").Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BTN_CerrarSesion_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DGV_Usuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV_Usuarios.SelectedRows.Count == 0)
            {
                BTN_EliminarUsuario.Enabled = false;
                BTN_EditarUsuario.Enabled = false;
                return;
            }

            BTN_EliminarUsuario.Enabled = true;
            BTN_EditarUsuario.Enabled = true;

            var usuarioSeleccionado = (Usuario)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            TBX_Username.Text = usuarioSeleccionado.Username;
        }

        private void BTN_EliminarUsuario_Click(object sender, EventArgs e)
        {

            SessionManager sesion = SessionManager.GetInstance();
            var usuarioSeleccionado = (Usuario)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            if (sesion.Usuario.Id == usuarioSeleccionado.Id)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.NoEliminarPropio"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }


            var confirmResult = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarEliminarUsuario").Replace("{0}", usuarioSeleccionado.Username),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarEliminacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                UsuarioService usuarioService = new UsuarioService();

                usuarioService.Eliminar(usuarioSeleccionado.Username);

                _usuariosBindingList.Remove(usuarioSeleccionado);

                TBX_Username.Clear();
                TBX_Password.Clear();

                DGV_Usuarios.ClearSelection();
                BTN_EliminarUsuario.Enabled = false;
                BTN_EditarUsuario.Enabled = false;

                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.UsuarioEliminado"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorEliminarUsuario").Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BTN_EditarUsuario_Click(object sender, EventArgs e)
        {
            var usuarioSeleccionado = (Usuario)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            var confirmResult = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarEditarUsuario").Replace("{0}", usuarioSeleccionado.Username),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarEdicion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                UsuarioService usuarioService = new UsuarioService();

                Usuario usuarioModificado = usuarioService.Modificar(usuarioSeleccionado.Id, TBX_Username.Text, TBX_Password.Text);

                TBX_Username.Clear();
                TBX_Password.Clear();

                DGV_Usuarios.ClearSelection();
                BTN_EliminarUsuario.Enabled = false;
                BTN_EditarUsuario.Enabled = false;
                ActualizarLista();
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.UsuarioEditado"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorEditarUsuario").Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

        }

        private void ActualizarDatosSesion()
        {
            if (!SessionManager.HaySesionActiva())
                return;

            SessionManager sesion = SessionManager.GetInstance();
            Usuario usuario = (Usuario)sesion.Usuario;

            LBL_Username.Text = _sesionIdioma.idioma.BuscarTraduccion("AdministrarUsuarios.UsuarioActual").Replace("{0}", usuario.Username);
            LBL_FechaInicio.Text = _sesionIdioma.idioma.BuscarTraduccion("AdministrarUsuarios.SesionIniciada").Replace("{0}", sesion.FechaInicio.ToString());
        }

        private void ConfigurarColumnasUsuarios()
        {
            ConfigurarColumna("Id", "Columna.Id");
            ConfigurarColumna("Username", "Columna.Username");
            ConfigurarColumna("Password", "Columna.Password");
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Usuarios.Columns.Contains(nombreColumna))
                return;

            DGV_Usuarios.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Usuarios.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
