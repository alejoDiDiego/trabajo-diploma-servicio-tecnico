using System;
using System.ComponentModel;
using System.Windows.Forms;
using APPLICATION.Features.Usuarios;
using DOMAIN.Features.Usuarios;
using SERVICES.Auth;

namespace UI.Forms.Auth
{
    public partial class FrmAdministrarUsuarios : Form
    {
        private BindingList<Usuario> _usuariosBindingList = null;

        public FrmAdministrarUsuarios()
        {
            InitializeComponent();
        }

        private void ActualizarLista() { 
            UsuarioService usuarioService = new UsuarioService();
            _usuariosBindingList = new BindingList<Usuario>(usuarioService.Listar());
            DGV_Usuarios.DataSource = _usuariosBindingList;
        }

        private void FrmAdministrarCuentas_Load(object sender, EventArgs e)
        {
            BTN_CerrarSesion.Visible = false;

            if (!SessionManager.HaySesionActiva())
            {
                PNL_Permisos.Visible = false;
                MessageBox.Show("No tenes permisos para acceder a esta seccion.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            SessionManager sesion = SessionManager.GetInstance();

            PNL_Permisos.Visible = true;

            Usuario usuario = (Usuario)sesion.Usuario;
            LBL_Username.Text = $"Usuario: {usuario.Username}";
            LBL_FechaInicio.Text = $"Sesion iniciada: {sesion.FechaInicio}";

            UsuarioService usuarioService = new UsuarioService();
            _usuariosBindingList = new BindingList<Usuario>(usuarioService.Listar());
            DGV_Usuarios.DataSource = _usuariosBindingList;
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

                MessageBox.Show("Usuario creado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("No podes eliminarte a vos mismo.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            var confirmResult = MessageBox.Show($"Estas seguro de eliminar al usuario '{usuarioSeleccionado.Username}'?", "Confirmar Eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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

                MessageBox.Show("Usuario eliminado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTN_EditarUsuario_Click(object sender, EventArgs e)
        {
            var usuarioSeleccionado = (Usuario)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            var confirmResult = MessageBox.Show($"Estas seguro de editar al usuario '{usuarioSeleccionado.Username}'?", "Confirmar Edicion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                MessageBox.Show("Usuario editado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
