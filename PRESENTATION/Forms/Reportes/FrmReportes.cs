using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Reportes;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Reportes
{
    // Visor de solo lectura de los 11 reportes del backend (ReporteService).
    // Cada tipo llama a su metodo correspondiente con los filtros de fecha
    // opcionales (DTP con check) y top (solo mas-utilizados). Los escalares
    // (tasa/promedio/monto/reingresos) van en LBL_Resultado como texto;
    // nunca se dice "facturacion/ingresos/ventas": es monto comprometido.
    public partial class FrmReportes : Form, IObservador
    {
        private class ItemReporte
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }

        private static readonly string[] TiposReporte = new string[]
        {
            "OrdenesEstado",
            "OrdenesResultado",
            "ReparacionesTecnico",
            "TiempoPromedio",
            "TasaAprobacion",
            "RepuestosMasUtilizados",
            "ComprasProveedor",
            "Reingresos",
            "EvaluacionesEstado",
            "TasaGarantia",
            "MontoAprobados"
        };

        private readonly SesionIdioma _sesionIdioma;
        private readonly ReporteService _service;
        private bool _cargandoCombos = false;

        public FrmReportes()
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
            LBL_Tipo.Text = idiomaObservado.BuscarTraduccion(LBL_Tipo.Tag.ToString());
            LBL_Desde.Text = idiomaObservado.BuscarTraduccion(LBL_Desde.Tag.ToString());
            LBL_Hasta.Text = idiomaObservado.BuscarTraduccion(LBL_Hasta.Tag.ToString());
            LBL_Top.Text = idiomaObservado.BuscarTraduccion(LBL_Top.Tag.ToString());
            BTN_Buscar.Text = idiomaObservado.BuscarTraduccion(BTN_Buscar.Tag.ToString());
            LBL_Resultado.Text = idiomaObservado.BuscarTraduccion(LBL_Resultado.Tag.ToString());

            CargarComboTipos();
            EjecutarReporte();
        }

        private void FrmReportes_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            ActualizarTextos();

            if (!TienePermiso(CodigosPermiso.ReportesVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarComboTipos();
            EjecutarReporte();
        }

        private void ActualizarTextos()
        {
            if (_sesionIdioma.idioma == null)
                return;

            Text = _sesionIdioma.idioma.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Tipo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Tipo.Tag.ToString());
            LBL_Desde.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Desde.Tag.ToString());
            LBL_Hasta.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Hasta.Tag.ToString());
            LBL_Top.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Top.Tag.ToString());
            BTN_Buscar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Buscar.Tag.ToString());
            LBL_Resultado.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Resultado.Tag.ToString());
        }

        private void CargarComboTipos()
        {
            _cargandoCombos = true;

            try
            {
                List<ItemReporte> items = new List<ItemReporte>();

                foreach (string id in TiposReporte)
                    items.Add(new ItemReporte { Id = id, Nombre = T("ReporteTipo." + id) });

                string seleccionado = CBO_TipoReporte.SelectedValue as string;

                CBO_TipoReporte.DataSource = null;
                CBO_TipoReporte.DisplayMember = "Nombre";
                CBO_TipoReporte.ValueMember = "Id";
                CBO_TipoReporte.DataSource = items;

                if (!string.IsNullOrEmpty(seleccionado))
                    CBO_TipoReporte.SelectedValue = seleccionado;

                ActualizarTopEnabled();
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void EjecutarReporte()
        {
            if (CBO_TipoReporte.DataSource == null)
                return;

            string tipo = CBO_TipoReporte.SelectedValue as string;

            if (string.IsNullOrEmpty(tipo))
                return;

            DateTime? desde = DT_Desde.Checked ? (DateTime?)DT_Desde.Value.Date : null;
            DateTime? hasta = DT_Hasta.Checked ? (DateTime?)DT_Hasta.Value.Date : null;
            int top = (int)NUM_Top.Value;

            try
            {
                switch (tipo)
                {
                    case "OrdenesEstado":
                        DGV_Reporte.DataSource = _service.ContarOrdenesPorEstado(desde, hasta);
                        LimpiarResultado();
                        break;
                    case "OrdenesResultado":
                        DGV_Reporte.DataSource = _service.ContarOrdenesPorResultado(desde, hasta);
                        LimpiarResultado();
                        break;
                    case "ReparacionesTecnico":
                        DGV_Reporte.DataSource = _service.ReparacionesPorTecnico(desde, hasta);
                        LimpiarResultado();
                        break;
                    case "TiempoPromedio":
                        MostrarTiempoPromedio(_service.TiempoPromedioIngresoEntrega(desde, hasta));
                        break;
                    case "TasaAprobacion":
                        DGV_Reporte.DataSource = _service.ContarPresupuestosPorEstado(desde, hasta);
                        MostrarTasa("Reportes.TasaAprobacionResultado", _service.TasaAprobacionPresupuestos(desde, hasta));
                        break;
                    case "RepuestosMasUtilizados":
                        DGV_Reporte.DataSource = _service.RepuestosMasUtilizados(top, desde, hasta);
                        LimpiarResultado();
                        break;
                    case "ComprasProveedor":
                        DGV_Reporte.DataSource = _service.ComprasPorProveedor(desde, hasta);
                        LimpiarResultado();
                        break;
                    case "Reingresos":
                        DGV_Reporte.DataSource = null;
                        LBL_Resultado.Text = T("Reportes.ReingresosResultado").Replace("{0}", _service.ContarReingresosGarantia(desde, hasta).ToString());
                        break;
                    case "EvaluacionesEstado":
                        DGV_Reporte.DataSource = _service.EvaluacionesPorEstado(desde, hasta);
                        LimpiarResultado();
                        break;
                    case "TasaGarantia":
                        DGV_Reporte.DataSource = _service.EvaluacionesPorEstado(desde, hasta);
                        MostrarTasa("Reportes.TasaGarantiaResultado", _service.TasaAceptacionGarantia(desde, hasta));
                        break;
                    case "MontoAprobados":
                        DGV_Reporte.DataSource = null;
                        LBL_Resultado.Text = T("Reportes.MontoAprobadosResultado").Replace("{0}", _service.MontoPresupuestosAprobados(desde, hasta).ToString("N2"));
                        break;
                    default:
                        DGV_Reporte.DataSource = null;
                        LimpiarResultado();
                        break;
                }

                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void MostrarTiempoPromedio(decimal promedio)
        {
            DGV_Reporte.DataSource = null;
            LBL_Resultado.Text = T("Reportes.TiempoPromedioResultado").Replace("{0}", promedio.ToString("N1"));
        }

        private void MostrarTasa(string clave, decimal tasa)
        {
            LBL_Resultado.Text = T(clave).Replace("{0}", tasa.ToString("P1"));
        }

        private void LimpiarResultado()
        {
            LBL_Resultado.Text = T(LBL_Resultado.Tag.ToString());
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Reporte.Columns.Count == 0)
                return;

            string tipo = CBO_TipoReporte.SelectedValue as string;

            switch (tipo)
            {
                case "OrdenesEstado":
                case "TasaAprobacion":
                    ConfigurarColumna("estado", "Columna.Estado");
                    ConfigurarColumna("cantidad", "Columna.Cantidad");
                    break;
                case "OrdenesResultado":
                    ConfigurarColumna("resultado", "Columna.Resultado");
                    ConfigurarColumna("cantidad", "Columna.Cantidad");
                    break;
                case "ReparacionesTecnico":
                    ConfigurarColumna("tecnico", "Columna.Tecnico");
                    ConfigurarColumna("cantidad", "Columna.Cantidad");
                    break;
                case "RepuestosMasUtilizados":
                    ConfigurarColumna("codigo", "Columna.Codigo");
                    ConfigurarColumna("descripcion", "Columna.Descripcion");
                    ConfigurarColumna("cantidad", "Columna.Cantidad");
                    break;
                case "ComprasProveedor":
                    ConfigurarColumna("proveedor", "Columna.Proveedor");
                    ConfigurarColumna("cantidad", "Columna.Cantidad");
                    ConfigurarColumna("monto", "Columna.Monto");
                    break;
                case "EvaluacionesEstado":
                case "TasaGarantia":
                    ConfigurarColumna("estado", "Columna.Estado");
                    ConfigurarColumna("cantidad", "Columna.Cantidad");
                    break;
                default:
                    break;
            }

            DGV_Reporte.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Reporte.Columns.Contains(nombreColumna))
                return;

            DGV_Reporte.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Reporte.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private string T(string clave)
        {
            if (_sesionIdioma.idioma == null)
                return clave;

            return _sesionIdioma.idioma.BuscarTraduccion(clave);
        }

        private void ActualizarTopEnabled()
        {
            string tipo = CBO_TipoReporte.SelectedValue as string;
            bool esTop = string.Equals(tipo, "RepuestosMasUtilizados", StringComparison.Ordinal);
            NUM_Top.Enabled = esTop;
            LBL_Top.Enabled = esTop;
        }

        private void CBO_TipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_TipoReporte.DataSource == null)
                return;

            ActualizarTopEnabled();
        }

        private void BTN_Buscar_Click(object sender, EventArgs e)
        {
            EjecutarReporte();
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
