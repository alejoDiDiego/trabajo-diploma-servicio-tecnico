using APPLICATION.Features.Usuarios;
using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Interfaces;
using CROSSCUTTING.Configuration;
using System;
using System.ComponentModel;
using System.Windows.Forms;

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
            ISesionUsuario sesion = InicializadorAplicacion.ObtenerSesion();

            if (sesion.ObtenerUsuarioActual() == null)
            {
                PNL_Permisos.Visible = false;
                MessageBox.Show("No tenés permisos para acceder a esta sección.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PNL_Permisos.Visible = true;

            UsuarioDTO usuario = sesion.ObtenerUsuarioActual();
            LBL_Username.Text = $"Usuario: {usuario.Username}";
            LBL_FechaInicio.Text = $"Sesión iniciada: {sesion.ObtenerFechaInicio()}";

            var usuarioService = InicializadorAplicacion.CrearUsuarioService();
            var usuarios = usuarioService.Listar();
            _usuariosBindingList = new BindingList<UsuarioDTO>(usuarios);
            DGV_Usuarios.DataSource = _usuariosBindingList;
        }

        private void BTN_CrearUsuario_Click(object sender, EventArgs e)
        {
            ISesionUsuario sesion = InicializadorAplicacion.ObtenerSesion();

            try
            {
                UsuarioService usuarioService = InicializadorAplicacion.CrearUsuarioService();

                UsuarioLoginDTO nuevoUsuario = new UsuarioLoginDTO
                {
                    Username = TBX_Username.Text,
                    Password = TBX_Password.Text
                };

                UsuarioDTO usuarioCreado = usuarioService.Create(nuevoUsuario);
                _usuariosBindingList.Add(usuarioCreado);

                TBX_Username.Clear();
                TBX_Password.Clear();

                MessageBox.Show("Usuario creado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BTN_CerrarSesion_Click(object sender, EventArgs e)
        {
            InicializadorAplicacion.ObtenerSesion().Logout();
            this.Hide();

            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    FrmAdministrarCuentas_Load(this, EventArgs.Empty);
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
        }

    }
}
