using System;
using System.Windows.Forms;
using APPLICATION.Features.Usuarios;
using DOMAIN.Features.Usuarios;
using SERVICES.Auth;

namespace UI.Forms.Auth
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();

            UsuarioService usuarioService = new UsuarioService();

            if(usuarioService.Listar().Count == 0)
            {
                MessageBox.Show("No hay usuarios registrados. Se creara un usuario por defecto con username 'admin' y password '123'.", "Usuario por defecto creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                usuarioService.Crear("admin", "123");
            }

        }

        private void BTN_IniciarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioService usuarioService = new UsuarioService();

                usuarioService.Login(TBX_Username.Text, TBX_Password.Text);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
