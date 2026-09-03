using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Idiomas;
using APPLICATION.Features.Bitacora;
using APPLICATION.Features.Integridad;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Idiomas;
using SERVICES.Auth;
using SERVICES.Idiomas;
using UI.Forms.Auth;
using UI.Forms.Bitacora;
using UI.Forms.Catalogos;
using UI.Forms.Clientes;
using UI.Forms.ControlCambios;
using UI.Forms.Equipos;
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
            TSMI_ControlCambios.Text = idiomaObservado.BuscarTraduccion(TSMI_ControlCambios.Tag.ToString());
            TSMI_Bitacora.Text = idiomaObservado.BuscarTraduccion(TSMI_Bitacora.Tag.ToString());
            TSMI_RecalcularDV.Text = idiomaObservado.BuscarTraduccion(TSMI_RecalcularDV.Tag.ToString());
            TSMI_Idioma.Text = idiomaObservado.BuscarTraduccion(TSMI_Idioma.Tag.ToString());
            TSMI_AdministrarTraducciones.Text = idiomaObservado.BuscarTraduccion(TSMI_AdministrarTraducciones.Tag.ToString());
            TSMI_Gestion.Text = idiomaObservado.BuscarTraduccion(TSMI_Gestion.Tag.ToString());
            TSMI_Clientes.Text = idiomaObservado.BuscarTraduccion(TSMI_Clientes.Tag.ToString());
            TSMI_Equipos.Text = idiomaObservado.BuscarTraduccion(TSMI_Equipos.Tag.ToString());
            TSMI_Catalogos.Text = idiomaObservado.BuscarTraduccion(TSMI_Catalogos.Tag.ToString());
            TSMI_TiposEquipo.Text = idiomaObservado.BuscarTraduccion(TSMI_TiposEquipo.Tag.ToString());
            TSMI_Marcas.Text = idiomaObservado.BuscarTraduccion(TSMI_Marcas.Tag.ToString());

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

            string username = SessionManager.ObtenerUsuarioActual().Username;

            CerrarFormulariosHijos();
            SessionManager.Logout();

            BitacoraService bitacoraService = new BitacoraService();
            bitacoraService.Registrar("Cierre de sesion", "username=" + username, "SESION");

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
            bool puedeRecalcularDV = TienePermiso(CodigosPermiso.IntegridadRecalcular);
            bool puedeVerTraducciones = TieneAlgunPermiso(CodigosPermiso.TraduccionesVer, CodigosPermiso.IdiomasVer);
            bool puedeVerControlCambios = TienePermiso(CodigosPermiso.ControlCambiosVer);
            bool puedeVerBitacora = TienePermiso(CodigosPermiso.BitacoraVer);
            bool puedeVerClientes = TienePermiso(CodigosPermiso.ClientesVer);
            bool puedeVerEquipos = TienePermiso(CodigosPermiso.EquiposVer);
            bool puedeVerTipos = TienePermiso(CodigosPermiso.TiposEquipoVer);
            bool puedeVerMarcas = TienePermiso(CodigosPermiso.MarcasVer);

            TSMI_IniciarSesion.Visible = !haySesionActiva;
            TSMI_CerrarSesion.Visible = haySesionActiva;
            TSMI_AdministrarUsuarios.Visible = haySesionActiva && puedeVerUsuarios;
            TSMI_AdministrarPermisos.Visible = haySesionActiva && puedeVerPermisos;
            TSMI_AsignarPermisosUsuarios.Visible = haySesionActiva && puedeAsignarPermisos;
            TSMI_ControlCambios.Visible = haySesionActiva && puedeVerControlCambios;
            TSMI_Bitacora.Visible = haySesionActiva && puedeVerBitacora;
            TSMI_RecalcularDV.Visible = haySesionActiva && puedeRecalcularDV;
            TSMI_AdministrarTraducciones.Visible = haySesionActiva && puedeVerTraducciones;
            TSMI_Gestion.Visible = haySesionActiva && (puedeVerClientes || puedeVerEquipos || puedeVerTipos || puedeVerMarcas);
            TSMI_Clientes.Visible = haySesionActiva && puedeVerClientes;
            TSMI_Equipos.Visible = haySesionActiva && puedeVerEquipos;
            TSMI_Catalogos.Visible = haySesionActiva && (puedeVerTipos || puedeVerMarcas);
            TSMI_TiposEquipo.Visible = haySesionActiva && puedeVerTipos;
            TSMI_Marcas.Visible = haySesionActiva && puedeVerMarcas;

            TSMI_IniciarSesion.Enabled = !haySesionActiva;
            TSMI_CerrarSesion.Enabled = haySesionActiva;
            TSMI_AdministrarUsuarios.Enabled = haySesionActiva && puedeVerUsuarios;
            TSMI_AdministrarPermisos.Enabled = haySesionActiva && puedeVerPermisos;
            TSMI_AsignarPermisosUsuarios.Enabled = haySesionActiva && puedeAsignarPermisos;
            TSMI_ControlCambios.Enabled = haySesionActiva && puedeVerControlCambios;
            TSMI_Bitacora.Enabled = haySesionActiva && puedeVerBitacora;
            TSMI_RecalcularDV.Enabled = haySesionActiva && puedeRecalcularDV;
            TSMI_AdministrarTraducciones.Enabled = haySesionActiva && puedeVerTraducciones;
            TSMI_Gestion.Enabled = haySesionActiva && (puedeVerClientes || puedeVerEquipos || puedeVerTipos || puedeVerMarcas);
            TSMI_Clientes.Enabled = haySesionActiva && puedeVerClientes;
            TSMI_Equipos.Enabled = haySesionActiva && puedeVerEquipos;
            TSMI_Catalogos.Enabled = haySesionActiva && (puedeVerTipos || puedeVerMarcas);
            TSMI_TiposEquipo.Enabled = haySesionActiva && puedeVerTipos;
            TSMI_Marcas.Enabled = haySesionActiva && puedeVerMarcas;

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

        private void TSMI_Bitacora_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.BitacoraVer))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmBitacora)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmBitacora frmBitacora = new FrmBitacora();
            frmBitacora.MdiParent = this;
            frmBitacora.FormClosed += FormularioHijo_FormClosed;
            frmBitacora.Show();
        }

        private void TSMI_Clientes_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ClientesVer))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmClientes)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmClientes frmClientes = new FrmClientes();
            frmClientes.MdiParent = this;
            frmClientes.FormClosed += FormularioHijo_FormClosed;
            frmClientes.Show();
        }

        private void TSMI_Equipos_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.EquiposVer))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmEquipos)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmEquipos frmEquipos = new FrmEquipos();
            frmEquipos.MdiParent = this;
            frmEquipos.FormClosed += FormularioHijo_FormClosed;
            frmEquipos.Show();
        }

        private void TSMI_TiposEquipo_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.TiposEquipoVer))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmTiposEquipo)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmTiposEquipo frmTiposEquipo = new FrmTiposEquipo();
            frmTiposEquipo.MdiParent = this;
            frmTiposEquipo.FormClosed += FormularioHijo_FormClosed;
            frmTiposEquipo.Show();
        }

        private void TSMI_Marcas_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.MarcasVer))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmMarcas)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmMarcas frmMarcas = new FrmMarcas();
            frmMarcas.MdiParent = this;
            frmMarcas.FormClosed += FormularioHijo_FormClosed;
            frmMarcas.Show();
        }

        private void TSMI_ControlCambios_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ControlCambiosVer))
            {
                MostrarAccesoDenegado();
                ActualizarMenuUsuario();
                return;
            }

            foreach (Form formulario in MdiChildren)
            {
                if (formulario is FrmControlCambios)
                {
                    formulario.Activate();
                    return;
                }
            }

            FrmControlCambios frmControlCambios = new FrmControlCambios();
            frmControlCambios.MdiParent = this;
            frmControlCambios.FormClosed += FormularioHijo_FormClosed;
            frmControlCambios.Show();
        }
        
        private void TSMI_RecalcularDV_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirmacion = MessageBox.Show(
                    "Se van a recalcular todos los digitos verificadores.\n" +
                    "Esto sobrescribira los valores actuales. Continuar?",
                    "Recalcular DV",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmacion == DialogResult.No)
                    return;

                IntegridadService integridadService = new IntegridadService();
                integridadService.RecalcularTodosDV();

                SessionManager.GetInstance().IntegridadComprometida = false;

                MessageBox.Show(
                    "Digitos verificadores recalculados exitosamente.",
                    "Recalcular DV",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al recalcular: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CambiarIdioma(int idIdioma)
        {
            if (!PuedeCambiarIdioma())
            {
                MostrarAccesoDenegado();
                return;
            }

            IIdioma idioma = _idiomaService.ObtenerPorId(idIdioma);

            if (SessionManager.HaySesionActiva())
            {
                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Cambio de idioma", "idioma=" + idioma?.Nombre, "IDIOMAS");
            }

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
