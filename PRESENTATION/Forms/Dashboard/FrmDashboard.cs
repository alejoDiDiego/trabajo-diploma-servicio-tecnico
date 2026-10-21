using System;
using System.Data;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Reportes;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Dashboard
{
    // Dashboard informativo de solo lectura: 6 KPIs en 1 query (DashboardResumen).
    // Cards informativas sin navegacion (ver DECISIONES): abrir forms filtrados
    // complicaba sin beneficio y los filtros de destino no existen.
    public partial class FrmDashboard : Form, IObservador
    {
        private readonly SesionIdioma _sesionIdioma;
        private readonly ReporteService _service;

        public FrmDashboard()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new ReporteService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_AbiertasTitulo.Text = idiomaObservado.BuscarTraduccion(LBL_AbiertasTitulo.Tag.ToString());
            LBL_EsperandoTitulo.Text = idiomaObservado.BuscarTraduccion(LBL_EsperandoTitulo.Tag.ToString());
            LBL_ReparacionTitulo.Text = idiomaObservado.BuscarTraduccion(LBL_ReparacionTitulo.Tag.ToString());
            LBL_ListasTitulo.Text = idiomaObservado.BuscarTraduccion(LBL_ListasTitulo.Tag.ToString());
            LBL_GarantiasTitulo.Text = idiomaObservado.BuscarTraduccion(LBL_GarantiasTitulo.Tag.ToString());
            LBL_BajoMinimoTitulo.Text = idiomaObservado.BuscarTraduccion(LBL_BajoMinimoTitulo.Tag.ToString());
            BTN_Actualizar.Text = idiomaObservado.BuscarTraduccion(BTN_Actualizar.Tag.ToString());
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            ActualizarTextos();

            if (!TienePermiso(CodigosPermiso.ReportesVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarResumen();
        }

        private void ActualizarTextos()
        {
            if (_sesionIdioma.idioma == null)
                return;

            Text = _sesionIdioma.idioma.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_AbiertasTitulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_AbiertasTitulo.Tag.ToString());
            LBL_EsperandoTitulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_EsperandoTitulo.Tag.ToString());
            LBL_ReparacionTitulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_ReparacionTitulo.Tag.ToString());
            LBL_ListasTitulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_ListasTitulo.Tag.ToString());
            LBL_GarantiasTitulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_GarantiasTitulo.Tag.ToString());
            LBL_BajoMinimoTitulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_BajoMinimoTitulo.Tag.ToString());
            BTN_Actualizar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Actualizar.Tag.ToString());
        }

        private void CargarResumen()
        {
            try
            {
                DataTable dt = _service.DashboardResumen();

                if (dt.Rows.Count == 0)
                    return;

                DataRow fila = dt.Rows[0];
                LBL_AbiertasValor.Text = LeerEntero(fila, "abiertas_total").ToString();
                LBL_EsperandoValor.Text = LeerEntero(fila, "esperando_respuesta").ToString();
                LBL_ReparacionValor.Text = LeerEntero(fila, "en_reparacion").ToString();
                LBL_ListasValor.Text = LeerEntero(fila, "listas_retiro").ToString();
                LBL_GarantiasValor.Text = LeerEntero(fila, "garantias_abiertas").ToString();
                LBL_BajoMinimoValor.Text = LeerEntero(fila, "bajo_minimo").ToString();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private int LeerEntero(DataRow fila, string columna)
        {
            if (!fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
                return 0;

            return Convert.ToInt32(fila[columna]);
        }

        private void BTN_Actualizar_Click(object sender, EventArgs e)
        {
            CargarResumen();
        }

        private bool TienePermiso(string codigo)
        {
            return SessionManager.HaySesionActiva() && SessionManager.TienePermiso(codigo);
        }

        private void MostrarAccesoDenegado()
        {
            MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            string detalle = ex != null ? ex.Message : "";
            MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorOperacion").Replace("{0}", detalle),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
