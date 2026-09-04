using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Repuestos;
using APPLICATION.Features.Usuarios;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Repuestos;
using DOMAIN.Features.Usuarios;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Repuestos
{
    public partial class FrmMovimientosStock : Form, IObservador
    {
        private class ItemRepuesto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class ItemTipo
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }

        private class FilaMovimiento
        {
            public DateTime Fecha { get; set; }
            public string Repuesto { get; set; }
            public string Tipo { get; set; }
            public int Cantidad { get; set; }
            public int Anterior { get; set; }
            public int Posterior { get; set; }
            public string Usuario { get; set; }
            public string Obs { get; set; }
        }

        private BindingList<FilaMovimiento> _movimientosBindingList = null;
        private Dictionary<int, string> _nombresRepuestos = new Dictionary<int, string>();
        private Dictionary<int, string> _nombresUsuarios = new Dictionary<int, string>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly RepuestoService _service;
        private readonly int? _idRepuestoInicial;

        public FrmMovimientosStock()
            : this(null)
        {
        }

        public FrmMovimientosStock(int idRepuesto)
            : this((int?)idRepuesto)
        {
        }

        private FrmMovimientosStock(int? idRepuesto)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new RepuestoService();
            _idRepuestoInicial = idRepuesto;
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Repuesto.Text = idiomaObservado.BuscarTraduccion(LBL_Repuesto.Tag.ToString());
            LBL_Tipo.Text = idiomaObservado.BuscarTraduccion(LBL_Tipo.Tag.ToString());
            LBL_Desde.Text = idiomaObservado.BuscarTraduccion(LBL_Desde.Tag.ToString());
            LBL_Hasta.Text = idiomaObservado.BuscarTraduccion(LBL_Hasta.Tag.ToString());
            LBL_Observacion.Text = idiomaObservado.BuscarTraduccion(LBL_Observacion.Tag.ToString());
            BTN_Buscar.Text = idiomaObservado.BuscarTraduccion(BTN_Buscar.Tag.ToString());

            CargarCombos();
            CargarMovimientos();
        }

        private void FrmMovimientosStock_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            if (!TienePermiso(CodigosPermiso.RepuestosVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarNombresUsuarios();
            CargarCombos();
            CargarMovimientos();
        }

        private void CargarNombresUsuarios()
        {
            _nombresUsuarios.Clear();

            try
            {
                UsuarioService usuarioService = new UsuarioService();

                foreach (Usuario u in usuarioService.Listar())
                    _nombresUsuarios[u.Id] = u.Username;
            }
            catch
            {
            }
        }

        private void CargarCombos()
        {
            int? idRepuestoActual = _idRepuestoInicial;

            if (CBO_Repuesto.DataSource != null && CBO_Repuesto.SelectedValue is int && (int)CBO_Repuesto.SelectedValue > 0)
                idRepuestoActual = (int)CBO_Repuesto.SelectedValue;

            List<ItemRepuesto> itemsRepuestos = new List<ItemRepuesto>();
            itemsRepuestos.Add(new ItemRepuesto { Id = 0, Nombre = T("Movimientos.Todos") });

            try
            {
                List<Repuesto> repuestos = _service.Listar(true);
                _nombresRepuestos.Clear();

                foreach (Repuesto r in repuestos)
                {
                    string nombre = r.Codigo + " - " + r.Descripcion;
                    _nombresRepuestos[r.Id] = nombre;

                    if (r.Activo || (_idRepuestoInicial.HasValue && r.Id == _idRepuestoInicial.Value))
                        itemsRepuestos.Add(new ItemRepuesto { Id = r.Id, Nombre = nombre });
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }

            CBO_Repuesto.DataSource = null;
            CBO_Repuesto.DisplayMember = "Nombre";
            CBO_Repuesto.ValueMember = "Id";
            CBO_Repuesto.DataSource = itemsRepuestos;

            if (idRepuestoActual.HasValue && idRepuestoActual.Value > 0)
                CBO_Repuesto.SelectedValue = idRepuestoActual.Value;

            string tipoActual = CBO_Tipo.SelectedValue as string;

            List<ItemTipo> itemsTipos = new List<ItemTipo>();
            itemsTipos.Add(new ItemTipo { Id = "", Nombre = T("Movimientos.Todos") });
            itemsTipos.Add(new ItemTipo { Id = TipoMovimientoStock.Compra, Nombre = T("MovimientoTipo." + TipoMovimientoStock.Compra) });
            itemsTipos.Add(new ItemTipo { Id = TipoMovimientoStock.ConsumoReparacion, Nombre = T("MovimientoTipo." + TipoMovimientoStock.ConsumoReparacion) });
            itemsTipos.Add(new ItemTipo { Id = TipoMovimientoStock.AjustePositivo, Nombre = T("MovimientoTipo." + TipoMovimientoStock.AjustePositivo) });
            itemsTipos.Add(new ItemTipo { Id = TipoMovimientoStock.AjusteNegativo, Nombre = T("MovimientoTipo." + TipoMovimientoStock.AjusteNegativo) });

            CBO_Tipo.DataSource = null;
            CBO_Tipo.DisplayMember = "Nombre";
            CBO_Tipo.ValueMember = "Id";
            CBO_Tipo.DataSource = itemsTipos;

            if (!string.IsNullOrEmpty(tipoActual))
                CBO_Tipo.SelectedValue = tipoActual;
        }

        private void CargarMovimientos()
        {
            if (CBO_Repuesto.DataSource == null || CBO_Tipo.DataSource == null)
                return;

            try
            {
                int? idRepuesto = null;

                if (CBO_Repuesto.SelectedValue is int && (int)CBO_Repuesto.SelectedValue > 0)
                    idRepuesto = (int)CBO_Repuesto.SelectedValue;

                string tipo = CBO_Tipo.SelectedValue as string;

                if (string.IsNullOrEmpty(tipo))
                    tipo = null;

                DateTime? desde = DT_Desde.Checked ? (DateTime?)DT_Desde.Value : null;
                DateTime? hasta = DT_Hasta.Checked ? (DateTime?)DT_Hasta.Value : null;
                string observacion = TXT_Observacion.Text.Trim().ToLowerInvariant();

                List<MovimientoStock> movimientos = _service.ListarMovimientosFiltros(idRepuesto, tipo, desde, hasta);
                List<FilaMovimiento> filas = new List<FilaMovimiento>();

                foreach (MovimientoStock m in movimientos)
                {
                    if (!string.IsNullOrEmpty(observacion)
                        && (m.Observacion == null || !m.Observacion.ToLowerInvariant().Contains(observacion)))
                        continue;

                    filas.Add(new FilaMovimiento
                    {
                        Fecha = m.Fecha,
                        Repuesto = ResolverRepuesto(m.IdRepuesto),
                        Tipo = T("MovimientoTipo." + m.Tipo),
                        Cantidad = m.Cantidad,
                        Anterior = m.StockAnterior,
                        Posterior = m.StockPosterior,
                        Usuario = ResolverUsuario(m.IdUsuario),
                        Obs = m.Observacion
                    });
                }

                _movimientosBindingList = new BindingList<FilaMovimiento>(filas);
                DGV_Movimientos.DataSource = _movimientosBindingList;
                ConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private string ResolverRepuesto(int idRepuesto)
        {
            if (_nombresRepuestos.ContainsKey(idRepuesto))
                return _nombresRepuestos[idRepuesto];

            return "#" + idRepuesto;
        }

        private string ResolverUsuario(int idUsuario)
        {
            if (_nombresUsuarios.ContainsKey(idUsuario))
                return _nombresUsuarios[idUsuario];

            return "#" + idUsuario;
        }

        private string T(string clave)
        {
            if (_sesionIdioma.idioma == null)
                return clave;

            return _sesionIdioma.idioma.BuscarTraduccion(clave);
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Movimientos.Columns.Count == 0)
                return;

            ConfigurarColumna("Fecha", "Columna.Fecha");
            ConfigurarColumna("Repuesto", "Columna.Repuesto");
            ConfigurarColumna("Tipo", "Columna.Tipo");
            ConfigurarColumna("Cantidad", "Columna.Cantidad");
            ConfigurarColumna("Anterior", "Columna.Anterior");
            ConfigurarColumna("Posterior", "Columna.Posterior");
            ConfigurarColumna("Usuario", "Columna.Usuario");
            ConfigurarColumna("Obs", "Columna.Observacion");

            DGV_Movimientos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Movimientos.Columns.Contains(nombreColumna))
                return;

            DGV_Movimientos.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Movimientos.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private void BTN_Buscar_Click(object sender, EventArgs e)
        {
            CargarMovimientos();
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
