using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Idiomas;
using SERVICES.Auth;
using SERVICES.Idiomas;
using UI.Forms.Auth;

namespace UI.Forms
{
    public partial class FrmPrincipal : Form, IObservador
    {
        private readonly IdiomaService _idiomaService;
        private readonly SesionIdioma _sesionIdioma;

        public FrmPrincipal()
        {
            _idiomaService = new IdiomaService();
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();
            ActualizarMenuUsuario();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            TSMI_IniciarSesion.Text = idiomaObservado.BuscarTraduccion(TSMI_IniciarSesion.Tag.ToString());
            TSMI_CerrarSesion.Text = idiomaObservado.BuscarTraduccion(TSMI_CerrarSesion.Tag.ToString());
            TSMI_AdministrarUsuarios.Text = idiomaObservado.BuscarTraduccion(TSMI_AdministrarUsuarios.Tag.ToString());
            TSMI_Idioma.Text = idiomaObservado.BuscarTraduccion(TSMI_Idioma.Tag.ToString());
            TSMI_IdiomaEspanol.Text = idiomaObservado.BuscarTraduccion(TSMI_IdiomaEspanol.Tag.ToString());
            TSMI_IdiomaIngles.Text = idiomaObservado.BuscarTraduccion(TSMI_IdiomaIngles.Tag.ToString());

            ActualizarMenuUsuario();
            ActualizarMenuIdioma();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);
            ActualizarMenuUsuario();
            ActualizarMenuIdioma();
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
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.DebeIniciarSesion"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
            TSMI_Usuario.Text = haySesionActiva
                ? string.Format(_sesionIdioma.idioma.BuscarTraduccion("Menu.UsuarioActual"), usuario.Username)
                : _sesionIdioma.idioma.BuscarTraduccion("Menu.Usuario");
        }

        private void ActualizarMenuIdioma()
        {
            IIdioma idiomaActual = _sesionIdioma.idioma;

            TSMI_IdiomaEspanol.Checked = idiomaActual != null && idiomaActual.Id == 1;
            TSMI_IdiomaIngles.Checked = idiomaActual != null && idiomaActual.Id == 2;
        }

        private void FormularioHijo_FormClosed(object sender, FormClosedEventArgs e)
        {
            ActualizarMenuUsuario();
        }

        private void TSMI_IdiomaEspanol_Click(object sender, EventArgs e)
        {
            CambiarIdioma(1);
        }

        private void TSMI_IdiomaIngles_Click(object sender, EventArgs e)
        {
            CambiarIdioma(2);
        }

        private void CambiarIdioma(int idIdioma)
        {
            IIdioma idioma = _idiomaService.ObtenerPorId(idIdioma);
            _sesionIdioma.CambiarIdioma(idioma);
        }

        private void CerrarFormulariosHijos()
        {
            foreach (Form formulario in MdiChildren)
                formulario.Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
