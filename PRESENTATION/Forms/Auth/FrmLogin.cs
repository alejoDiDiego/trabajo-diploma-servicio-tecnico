using CROSSCUTTING.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CROSSCUTTING.Configuration;
using APPLICATION.Interfaces;
using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Features.Usuarios;

namespace PRESENTATION.Forms.Auth
{
    public partial class FrmLogin : Form
    {

        public FrmLogin()
        {
            InitializeComponent();


        }

        private void BTN_Volver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BTN_IniciarSesion_Click(object sender, EventArgs e)
        {
            //UsuarioService usuarioService = InicializadorAplicacion.CrearUsuarioService();

            //UsuarioLoginDTO usuarioForm = new UsuarioLoginDTO
            //{
            //    Username = "Admin",
            //    Password = "123"
            //};
            //UsuarioDTO usuarioLogin = usuarioService.Create(usuarioForm);

            //return;

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

                MessageBox.Show($"¡Inicio de sesión exitoso!. Bienvenido {sesion.ObtenerUsuarioActual().Username}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
