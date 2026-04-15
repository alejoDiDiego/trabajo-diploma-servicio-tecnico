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
            try
            {
                ISesionUsuario sesion = InicializadorAplicacion.ObtenerSesion();

                UsuarioDTO usuario = new UsuarioDTO
                {
                    Username = TBX_Username.Text,
                    Password = TBX_Password.Text
                };

                sesion.Login(usuario);

                MessageBox.Show($"¡Inicio de sesión exitoso!. Bienvenido {sesion.ObtenerUsuarioActual().Username}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
