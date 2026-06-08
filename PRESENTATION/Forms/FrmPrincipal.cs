using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Idiomas;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Idiomas;
using SERVICES.Auth;
using SERVICES.Idiomas;
using UI.Forms.Auth;
using UI.Forms.Idiomas;

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
            TSMI_AdministrarPermisos.Text = idiomaObservado.BuscarTraduccion(TSMI_AdministrarPermisos.Tag.ToString());
            TSMI_AsignarPermisosUsuarios.Text = idiomaObservado.BuscarTraduccion(TSMI_AsignarPermisosUsuarios.Tag.ToString());
            TSMI_Idioma.Text = idiomaObservado.BuscarTraduccion(TSMI_Idioma.Tag.ToString());
            TSMI_AdministrarTraducciones.Text = idiomaObservado.BuscarTraduccion(TSMI_AdministrarTraducciones.Tag.ToString());

            ActualizarMenuUsuario();
            CargarMenuIdiomas();
            ActualizarMenuIdioma();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            CargarMenuIdiomas();
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
            if (!TienePermiso(CodigosPermiso.UsuariosVer))
            {
                MostrarAccesoDenegado();
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
            bool puedeVerUsuarios = TienePermiso(CodigosPermiso.UsuariosVer);
            bool puedeVerPermisos = TienePermiso(CodigosPermiso.PermisosVer);
            bool puedeAsignarPermisos = TienePermiso(CodigosPermiso.PermisosAsignarUsuarios);
            bool puedeVerTraducciones = TieneAlgunPermiso(CodigosPermiso.TraduccionesVer, CodigosPermiso.IdiomasVer);

            TSMI_IniciarSesion.Visible = !haySesionActiva;
            TSMI_CerrarSesion.Visible = haySesionActiva;
            TSMI_AdministrarUsuarios.Visible = haySesionActiva && puedeVerUsuarios;
            TSMI_AdministrarPermisos.Visible = haySesionActiva && puedeVerPermisos;
            TSMI_AsignarPermisosUsuarios.Visible = haySesionActiva && puedeAsignarPermisos;
            TSMI_AdministrarTraducciones.Visible = haySesionActiva && puedeVerTraducciones;

            TSMI_IniciarSesion.Enabled = !haySesionActiva;
            TSMI_CerrarSesion.Enabled = haySesionActiva;
            TSMI_AdministrarUsuarios.Enabled = haySesionActiva && puedeVerUsuarios;
            TSMI_AdministrarPermisos.Enabled = haySesionActiva && puedeVerPermisos;
            TSMI_AsignarPermisosUsuarios.Enabled = haySesionActiva && puedeAsignarPermisos;
            TSMI_AdministrarTraducciones.Enabled = haySesionActiva && puedeVerTraducciones;

            var usuario = SessionManager.ObtenerUsuarioActual();
            TSMI_Usuario.Text = haySesionActiva
                ? _sesionIdioma.idioma.BuscarTraduccion("Menu.UsuarioActual").Replace("{0}", usuario.Username)
                : _sesionIdioma.idioma.BuscarTraduccion("Menu.Usuario");
        }

        private void CargarMenuIdiomas()
        {
            TSMI_Idioma.DropDownItems.Clear();
            TSMI_Idioma.DropDownItems.Add(TSMI_AdministrarTraducciones);
            TSMI_Idioma.DropDownItems.Add(new ToolStripSeparator());

            foreach (Idioma idioma in _idiomaService.Listar())
            {
                ToolStripMenuItem itemIdioma = new ToolStripMenuItem();
                itemIdioma.Name = "TSMI_Idioma_" + idioma.Id;
                itemIdioma.Text = idioma.Nombre;
                itemIdioma.Tag = idioma.Id;
                itemIdioma.Enabled = PuedeCambiarIdioma();
                itemIdioma.Click += TSMI_Idioma_Click;
                TSMI_Idioma.DropDownItems.Add(itemIdioma);
            }
        }

        private void ActualizarMenuIdioma()
        {
            IIdioma idiomaActual = _sesionIdioma.idioma;

            foreach (ToolStripItem item in TSMI_Idioma.DropDownItems)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;

                if (menuItem == null || !(menuItem.Tag is int))
                    continue;

                menuItem.Checked = idiomaActual != null && (int)menuItem.Tag == idiomaActual.Id;
            }
        }

        private void FormularioHijo_FormClosed(object sender, FormClosedEventArgs e)
        {
            ActualizarMenuUsuario();
        }

        private void TSMI_Idioma_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            if (item == null || !(item.Tag is int))
                return;

            CambiarIdioma((int)item.Tag);
        }

        private void TSMI_AdministrarTraducciones_Click(object sender, EventArgs e)
        {
            if (!TieneAlgunPermiso(CodigosPermiso.TraduccionesVer, CodigosPermiso.IdiomasVer))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmAdministrarTraducciones)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmAdministrarTraducciones frmAdministrarTraducciones = new FrmAdministrarTraducciones();
            frmAdministrarTraducciones.MdiParent = this;
            frmAdministrarTraducciones.FormClosed += FormularioHijo_FormClosed;
            frmAdministrarTraducciones.Show();
        }

        private void TSMI_AdministrarPermisos_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PermisosVer))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmAdministrarPermisos)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmAdministrarPermisos frmAdministrarPermisos = new FrmAdministrarPermisos();
            frmAdministrarPermisos.MdiParent = this;
            frmAdministrarPermisos.FormClosed += FormularioHijo_FormClosed;
            frmAdministrarPermisos.Show();
        }

        private void TSMI_AsignarPermisosUsuarios_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PermisosAsignarUsuarios))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmAsignarPermisosUsuario)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmAsignarPermisosUsuario frmAsignarPermisosUsuario = new FrmAsignarPermisosUsuario();
            frmAsignarPermisosUsuario.MdiParent = this;
            frmAsignarPermisosUsuario.FormClosed += FormularioHijo_FormClosed;
            frmAsignarPermisosUsuario.Show();
        }

        private void CambiarIdioma(int idIdioma)
        {
            if (!PuedeCambiarIdioma())
            {
                MostrarAccesoDenegado();
                return;
            }

            IIdioma idioma = _idiomaService.ObtenerPorId(idIdioma);
            _sesionIdioma.CambiarIdioma(idioma);
        }

        private bool TienePermiso(string codigo)
        {
            return SessionManager.HaySesionActiva() && SessionManager.TienePermiso(codigo);
        }

        private bool TieneAlgunPermiso(params string[] codigos)
        {
            return SessionManager.HaySesionActiva() && SessionManager.TieneAlgunPermiso(codigos);
        }

        private bool PuedeCambiarIdioma()
        {
            return !SessionManager.HaySesionActiva() || SessionManager.TienePermiso(CodigosPermiso.IdiomasCambiar);
        }

        private void MostrarAccesoDenegado()
        {
            MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
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
