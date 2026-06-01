using System;
using System.Windows.Forms;
using SERVICES.Auth;
using UI.Forms.Auth;

namespace UI.Forms
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            ActualizarMenuUsuario();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            ActualizarMenuUsuario();
        }

        private void TSMI_IniciarSesion_Click(object sender, EventArgs e)
        {
            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmLogin)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmLogin frmLogin = new FrmLogin();
            frmLogin.MdiParent = this;
            frmLogin.FormClosed += FormularioHijo_FormClosed;
            frmLogin.Show();
        }

        private void TSMI_CerrarSesion_Click(object sender, EventArgs e)
        {
            if (!SessionManager.HaySesionActiva())
            {
                ActualizarMenuUsuario();
                return;
            }

            CerrarFormulariosHijos();
            SessionManager.Logout();
            ActualizarMenuUsuario();
        }

        private void TSMI_AdministrarUsuarios_Click(object sender, EventArgs e)
        {
            if (!SessionManager.HaySesionActiva())
            {
                MessageBox.Show("Debes iniciar sesion para acceder a esta seccion.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmAdministrarUsuarios)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmAdministrarUsuarios frmAdministrarUsuarios = new FrmAdministrarUsuarios();
            frmAdministrarUsuarios.MdiParent = this;
            frmAdministrarUsuarios.FormClosed += FormularioHijo_FormClosed;
            frmAdministrarUsuarios.Show();
        }

        private void ActualizarMenuUsuario()
        {
            bool haySesionActiva = SessionManager.HaySesionActiva();

            TSMI_IniciarSesion.Visible = !haySesionActiva;
            TSMI_CerrarSesion.Visible = haySesionActiva;
            TSMI_AdministrarUsuarios.Visible = haySesionActiva;

            TSMI_IniciarSesion.Enabled = !haySesionActiva;
            TSMI_CerrarSesion.Enabled = haySesionActiva;
            TSMI_AdministrarUsuarios.Enabled = haySesionActiva;

            var usuario = SessionManager.ObtenerUsuarioActual();
            TSMI_Usuario.Text = haySesionActiva ? string.Format("Usuario: {0}", usuario.Username) : "Usuario";
        }

        private void FormularioHijo_FormClosed(object sender, FormClosedEventArgs e)
        {
            ActualizarMenuUsuario();
        }

        private void CerrarFormulariosHijos()
        {
            foreach (Form formulario in MdiChildren)
                formulario.Close();
        }
    }
}
