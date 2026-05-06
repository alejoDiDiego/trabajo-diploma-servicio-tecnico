using System;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Usuarios;
using ABSTRACTIONS.Services;
using APPLICATION.Features.Usuarios;
using APPLICATION.Features.Usuarios.DTOs;
using SERVICES.Auth;

namespace PRESENTATION.Forms.Auth
{
    public partial class FrmAdministrarUsuarios : Form
    {
        private BindingList<UsuarioDTO> _usuariosBindingList = null;

        public FrmAdministrarUsuarios()
        {
            InitializeComponent();
        }

        private void FrmAdministrarCuentas_Load(object sender, EventArgs e)
        {
            ISesionUsuario sesion = SessionManager.GetInstance();

            if (sesion.ObtenerUsuarioActual() == null)
            {
                PNL_Permisos.Visible = false;
                MessageBox.Show("No tenes permisos para acceder a esta seccion.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PNL_Permisos.Visible = true;

            IUsuario usuario = sesion.ObtenerUsuarioActual();
            LBL_Username.Text = $"Usuario: {usuario.Username}";
            LBL_FechaInicio.Text = $"Sesion iniciada: {sesion.ObtenerFechaInicio()}";

            UsuarioService usuarioService = new UsuarioService();
            _usuariosBindingList = new BindingList<UsuarioDTO>(usuarioService.Listar());
            DGV_Usuarios.DataSource = _usuariosBindingList;
        }

        private void BTN_CrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioService usuarioService = new UsuarioService();

                UsuarioLoginDTO nuevoUsuario = new UsuarioLoginDTO
                {
                    Username = TBX_Username.Text,
                    Password = TBX_Password.Text
                };

                UsuarioDTO usuarioCreado = usuarioService.Crear(nuevoUsuario);
                _usuariosBindingList.Add(usuarioCreado);

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
            SessionManager.GetInstance().Logout();
            Hide();

            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    FrmAdministrarCuentas_Load(this, EventArgs.Empty);
                    Show();
                }
                else
                {
                    Close();
                }
            }
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

            var usuarioSeleccionado = (UsuarioDTO)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            TBX_Username.Text = usuarioSeleccionado.Username;
        }

        private void BTN_EliminarUsuario_Click(object sender, EventArgs e)
        {
            var usuarioSeleccionado = (UsuarioDTO)DGV_Usuarios.SelectedRows[0].DataBoundItem;

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
            var usuarioSeleccionado = (UsuarioDTO)DGV_Usuarios.SelectedRows[0].DataBoundItem;

            var confirmResult = MessageBox.Show($"Estas seguro de editar al usuario '{usuarioSeleccionado.Username}'?", "Confirmar Edicion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                UsuarioService usuarioService = new UsuarioService();

                usuarioSeleccionado.Username = TBX_Username.Text;
                usuarioSeleccionado.Password = TBX_Password.Text;

                UsuarioDTO usuarioModificado = usuarioService.Modificar(usuarioSeleccionado);

                usuarioSeleccionado.Username = usuarioModificado.Username;
                usuarioSeleccionado.Password = usuarioModificado.Password;

                TBX_Username.Clear();
                TBX_Password.Clear();

                DGV_Usuarios.ClearSelection();
                BTN_EliminarUsuario.Enabled = false;
                BTN_EditarUsuario.Enabled = false;

                MessageBox.Show("Usuario editado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
