using APPLICATION.Features.Usuarios;
using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Interfaces;
using CROSSCUTTING.Configuration;
using System;
using System.Windows.Forms;

namespace PRESENTATION.Forms.Auth
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void BTN_IniciarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                ISesionUsuario sesion = InicializadorAplicacion.ObtenerSesion();
                UsuarioService usuarioService = InicializadorAplicacion.CrearUsuarioService();

                UsuarioLoginDTO usuarioForm = new UsuarioLoginDTO
                {
                    Username = TBX_Username.Text,
                    Password = TBX_Password.Text
                };

                UsuarioDTO usuarioLogin = usuarioService.Login(usuarioForm);
                sesion.Login(usuarioLogin);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
