using APPLICATION.Interfaces;
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
using APPLICATION.Features.Usuarios.DTOs;

namespace PRESENTATION.Forms.Auth
{
    public partial class FrmCuenta : Form
    {
        public FrmCuenta()
        {
            InitializeComponent();

            ISesionUsuario sesion = InicializadorAplicacion.ObtenerSesion();
            if (sesion.ObtenerUsuarioActual() != null)
            {
                UsuarioDTO usuario = sesion.ObtenerUsuarioActual();
                LBL_Username.Text = $"Username: {usuario.Username}";
                LBL_FechaInicio.Text = $"Fecha de Inicio de Sesión: {sesion.ObtenerFechaInicio()}";
            }
        }
    }
}
