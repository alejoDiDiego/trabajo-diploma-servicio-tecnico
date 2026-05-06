using System;
using System.Windows.Forms;
using ABSTRACTIONS.Services;
using APPLICATION.Features.Usuarios;
using APPLICATION.Features.Usuarios.DTOs;
using SERVICES.Auth;

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
                ISesionUsuario sesion = SessionManager.GetInstance();
                UsuarioService usuarioService = new UsuarioService();

                UsuarioLoginDTO usuarioForm = new UsuarioLoginDTO
                {
                    Username = TBX_Username.Text,
                    Password = TBX_Password.Text
                };

                UsuarioDTO usuarioLogin = usuarioService.Login(usuarioForm);
                sesion.Login(usuarioLogin);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
