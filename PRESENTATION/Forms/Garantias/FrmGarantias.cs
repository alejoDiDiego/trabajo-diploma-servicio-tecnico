using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Clientes;
using APPLICATION.Features.Equipos;
using APPLICATION.Features.Garantias;
using APPLICATION.Features.Ordenes;
using DOMAIN.Features.Clientes;
using DOMAIN.Features.Equipos;
using DOMAIN.Features.Garantias;
using DOMAIN.Features.Ordenes;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Garantias
{
    // Lista solo lectura de garantias. El backend no expone Listar garantias,
    // asi que se compone en UI: Listar ordenes (con entregadas) + ObtenerPorOrden
    // por fila. Gate ORDENES_VER por ser fase de orden (ver DECISIONES).
    public partial class FrmGarantias : Form, IObservador
    {
        private class FilaGarantia
        {
            public int IdOrden { get; set; }
            public int Orden { get; set; }
            public string Equipo { get; set; }
            public string Cliente { get; set; }
            public DateTime Inicio { get; set; }
            public DateTime Fin { get; set; }
            public string VigenciaRaw { get; set; }
            public string Vigencia { get; set; }
            public string Estado { get; set; }
        }

        private class ItemVigencia
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }

        private BindingList<FilaGarantia> _garantiasBindingList = null;
        private List<FilaGarantia> _todas = new List<FilaGarantia>();
        private List<Cliente> _clientes = new List<Cliente>();
        private List<Equipo> _equipos = new List<Equipo>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly OrdenServicioService _ordenService;
        private readonly GarantiaService _garantiaService;
        private readonly ClienteService _clienteService;
        private readonly EquipoService _equipoService;
        private bool _cargandoCombos = false;

        public FrmGarantias()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _ordenService = new OrdenServicioService();
            _garantiaService = new GarantiaService();
            _clienteService = new ClienteService();
            _equipoService = new EquipoService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Vigencia.Text = idiomaObservado.BuscarTraduccion(LBL_Vigencia.Tag.ToString());
            LBL_Orden.Text = idiomaObservado.BuscarTraduccion(LBL_Orden.Tag.ToString());

            CargarComboVigencia();
            AplicarFiltro();
            ConfigurarColumnas();
        }

        private void FrmGarantias_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            ActualizarTextos();

            if (!TienePermiso(CodigosPermiso.OrdenesVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarCatalogos();
            CargarComboVigencia();
            CargarGarantias();
        }

        private void ActualizarTextos()
        {
            if (_sesionIdioma.idioma == null)
                return;

            Text = _sesionIdioma.idioma.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Vigencia.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Vigencia.Tag.ToString());
            LBL_Orden.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Orden.Tag.ToString());
        }

        private void CargarCatalogos()
        {
            try
            {
                _clientes = _clienteService.Listar(true);
                _equipos = _equipoService.Listar(true);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarComboVigencia()
        {
            _cargandoCombos = true;

            try
            {
                string todos = _sesionIdioma.idioma != null
                    ? _sesionIdioma.idioma.BuscarTraduccion("Garantias.Todas")
                    : "Todas";

                string vigentes = _sesionIdioma.idioma != null
                    ? _sesionIdioma.idioma.BuscarTraduccion("Garantias.Vigentes")
                    : "Vigentes";

                string vencidas = _sesionIdioma.idioma != null
                    ? _sesionIdioma.idioma.BuscarTraduccion("Garantias.Vencidas")
                    : "Vencidas";

                List<ItemVigencia> items = new List<ItemVigencia>();
                items.Add(new ItemVigencia { Id = "", Nombre = todos });
                items.Add(new ItemVigencia { Id = "Vigente", Nombre = vigentes });
                items.Add(new ItemVigencia { Id = "Vencida", Nombre = vencidas });

                string seleccionado = "";

                if (CBO_Vigencia.SelectedValue is string)
                    seleccionado = (string)CBO_Vigencia.SelectedValue;

                CBO_Vigencia.DataSource = null;
                CBO_Vigencia.DisplayMember = "Nombre";
                CBO_Vigencia.ValueMember = "Id";
                CBO_Vigencia.DataSource = items;

                CBO_Vigencia.SelectedValue = seleccionado;
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void CargarGarantias()
        {
            _todas = new List<FilaGarantia>();

            try
            {
                List<OrdenServicio> ordenes = _ordenService.Listar(true);

                foreach (OrdenServicio o in ordenes)
                {
                    Garantia g = null;

                    try
                    {
                        g = _garantiaService.ObtenerPorOrden(o.Id);
                    }
                    catch
                    {
                        continue;
                    }

                    if (g == null)
                        continue;

                    bool vigente = !g.Anulada && DateTime.Now <= g.FechaFin;
                    string vigenciaRaw = vigente ? "Vigente" : "Vencida";

                    _todas.Add(new FilaGarantia
                    {
                        IdOrden = o.Id,
                        Orden = o.NumeroOrden,
                        Equipo = ResolverEquipo(o.IdEquipo),
                        Cliente = ResolverCliente(o.IdCliente),
                        Inicio = g.FechaInicio,
                        Fin = g.FechaFin,
                        VigenciaRaw = vigenciaRaw,
                        Vigencia = TraducirVigencia(vigenciaRaw),
                        Estado = g.Anulada ? TraducirEstado("Anulada") : TraducirEstado("Activa")
                    });
                }

                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AplicarFiltro()
        {
            if (CBO_Vigencia == null)
                return;

            string vigenciaFiltro = "";

            if (CBO_Vigencia.SelectedValue is string)
                vigenciaFiltro = (string)CBO_Vigencia.SelectedValue;

            string texto = TXT_Orden != null ? TXT_Orden.Text.Trim() : "";

            List<FilaGarantia> filas = new List<FilaGarantia>();

            foreach (FilaGarantia f in _todas)
            {
                if (!string.IsNullOrEmpty(vigenciaFiltro) && f.VigenciaRaw != vigenciaFiltro)
                    continue;

                if (!string.IsNullOrEmpty(texto) && !f.Orden.ToString().Contains(texto))
                    continue;

                filas.Add(new FilaGarantia
                {
                    IdOrden = f.IdOrden,
                    Orden = f.Orden,
                    Equipo = f.Equipo,
                    Cliente = f.Cliente,
                    Inicio = f.Inicio,
                    Fin = f.Fin,
                    VigenciaRaw = f.VigenciaRaw,
                    Vigencia = TraducirVigencia(f.VigenciaRaw),
                    Estado = f.Estado
                });
            }

            _garantiasBindingList = new BindingList<FilaGarantia>(filas);
            DGV_Garantias.DataSource = _garantiasBindingList;
            ConfigurarColumnas();
        }

        private string ResolverCliente(int idCliente)
        {
            foreach (Cliente c in _clientes)
            {
                if (c.Id == idCliente)
                    return c.Apellido + ", " + c.Nombre;
            }

            try
            {
                Cliente c = _clienteService.ObtenerPorId(idCliente);

                if (c != null)
                    return c.Apellido + ", " + c.Nombre;
            }
            catch
            {
            }

            return "#" + idCliente;
        }

        private string ResolverEquipo(int idEquipo)
        {
            foreach (Equipo e in _equipos)
            {
                if (e.Id == idEquipo)
                    return DescribirEquipo(e);
            }

            try
            {
                Equipo e = _equipoService.ObtenerPorId(idEquipo);

                if (e != null)
                    return DescribirEquipo(e);
            }
            catch
            {
            }

            return "#" + idEquipo;
        }

        private string DescribirEquipo(Equipo e)
        {
            string desc = (e.Modelo ?? "").Trim();

            if (!string.IsNullOrEmpty(e.NumeroSerie))
                desc = (desc + " (S/N " + e.NumeroSerie.Trim() + ")").Trim();

            if (string.IsNullOrEmpty(desc))
                desc = "#" + e.Id;

            return desc;
        }

        private string TraducirVigencia(string vigenciaRaw)
        {
            if (_sesionIdioma.idioma == null)
                return vigenciaRaw;

            return _sesionIdioma.idioma.BuscarTraduccion("GarantiaVigencia." + vigenciaRaw);
        }

        private string TraducirEstado(string estado)
        {
            if (_sesionIdioma.idioma == null)
                return estado;

            return _sesionIdioma.idioma.BuscarTraduccion("GarantiaEstado." + estado);
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Garantias.Columns.Count == 0)
                return;

            ConfigurarColumna("Orden", "Columna.Numero");
            ConfigurarColumna("Equipo", "Columna.Equipo");
            ConfigurarColumna("Cliente", "Columna.Cliente");
            ConfigurarColumna("Inicio", "Columna.Inicio");
            ConfigurarColumna("Fin", "Columna.Fin");
            ConfigurarColumna("Vigencia", "Columna.Vigencia");
            ConfigurarColumna("Estado", "Columna.Estado");

            if (DGV_Garantias.Columns.Contains("IdOrden"))
                DGV_Garantias.Columns["IdOrden"].Visible = false;
            if (DGV_Garantias.Columns.Contains("VigenciaRaw"))
                DGV_Garantias.Columns["VigenciaRaw"].Visible = false;

            DGV_Garantias.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Garantias.Columns.Contains(nombreColumna))
                return;

            DGV_Garantias.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Garantias.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private void CBO_Vigencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_Vigencia.DataSource == null)
                return;

            AplicarFiltro();
        }

        private void TXT_Orden_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
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
