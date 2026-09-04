using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Clientes;
using APPLICATION.Features.Equipos;
using APPLICATION.Features.Ordenes;
using DOMAIN.Features.Clientes;
using DOMAIN.Features.Equipos;
using DOMAIN.Features.Ordenes;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Usuarios;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Ordenes
{
    // Dialogo modal: modo nuevo (idOrden 0, solo recepcion + Crear) o modo
    // detalle (5 tabs). Cada boton llama al service, muestra el error del
    // service y recarga todo con CargarOrden. Sin ComboBox libre de Estado.
    // Presupuesto: fila editable simple (controles inline + DGV); no se crea
    // dialogo aparte FrmPresupuestoItemEditar (ver DECISIONES del checkpoint).
    public partial class FrmOrdenServicioDetalle : Form, IObservador
    {
        private class ItemCliente
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class ItemEquipo
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class ItemTecnico
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class ItemTipoItem
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }

        private class FilaHistorial
        {
            public DateTime Fecha { get; set; }
            public string Anterior { get; set; }
            public string Nuevo { get; set; }
            public string Usuario { get; set; }
            public string Observacion { get; set; }
        }

        private readonly SesionIdioma _sesionIdioma;
        private readonly OrdenServicioService _service;
        private readonly ClienteService _clienteService;
        private readonly EquipoService _equipoService;
        private readonly string _tabInicial;
        private int _idOrden;
        private bool _esNuevo;
        private bool _cargandoCombos = false;
        private OrdenServicio _orden = null;
        private Diagnostico _diagnostico = null;
        private Presupuesto _presupuesto = null;
        private Entrega _entrega = null;
        private List<Cliente> _clientes = new List<Cliente>();
        private List<Equipo> _equipos = new List<Equipo>();
        private List<Usuario> _tecnicos = new List<Usuario>();
        private List<DetallePresupuesto> _itemsNuevo = new List<DetallePresupuesto>();

        public FrmOrdenServicioDetalle()
            : this(0, null)
        {
        }

        public FrmOrdenServicioDetalle(int idOrden)
            : this(idOrden, null)
        {
        }

        public FrmOrdenServicioDetalle(int idOrden, string tabInicial)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new OrdenServicioService();
            _clienteService = new ClienteService();
            _equipoService = new EquipoService();
            _idOrden = idOrden;
            _esNuevo = idOrden <= 0;
            _tabInicial = tabInicial;
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            ActualizarTextos();
            CargarComboTiposItem();
            CargarCombosRecepcion();
            CargarOrden();
        }

        private void FrmOrdenServicioDetalle_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);

            if (_esNuevo && !TienePermiso(CodigosPermiso.OrdenesCrear))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            if (!_esNuevo && !TienePermiso(CodigosPermiso.OrdenesVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarCatalogos();
            CargarComboTiposItem();
            ActualizarTextos();
            CargarOrden();

            if (!_esNuevo && !string.IsNullOrEmpty(_tabInicial))
                SeleccionarTab(_tabInicial);
        }

        private void ActualizarTextos()
        {
            if (_sesionIdioma.idioma == null)
                return;

            IIdioma idioma = _sesionIdioma.idioma;
            string claveTitulo = _esNuevo ? "OrdenDetalle.TituloNuevo" : "OrdenDetalle.TituloDetalle";
            Tag = claveTitulo;
            Text = idioma.BuscarTraduccion(Tag.ToString());

            TAB_Recepcion.Text = idioma.BuscarTraduccion(TAB_Recepcion.Tag.ToString());
            TAB_Diagnostico.Text = idioma.BuscarTraduccion(TAB_Diagnostico.Tag.ToString());
            TAB_Presupuesto.Text = idioma.BuscarTraduccion(TAB_Presupuesto.Tag.ToString());
            TAB_Historial.Text = idioma.BuscarTraduccion(TAB_Historial.Tag.ToString());
            TAB_Entrega.Text = idioma.BuscarTraduccion(TAB_Entrega.Tag.ToString());

            LBL_RCliente.Text = idioma.BuscarTraduccion(LBL_RCliente.Tag.ToString());
            LBL_REquipo.Text = idioma.BuscarTraduccion(LBL_REquipo.Tag.ToString());
            LBL_RTecnico.Text = idioma.BuscarTraduccion(LBL_RTecnico.Tag.ToString());
            LBL_RProblema.Text = idioma.BuscarTraduccion(LBL_RProblema.Tag.ToString()) + " *";
            LBL_REstadoFisico.Text = idioma.BuscarTraduccion(LBL_REstadoFisico.Tag.ToString());
            LBL_RAccesorios.Text = idioma.BuscarTraduccion(LBL_RAccesorios.Tag.ToString());
            LBL_RObs.Text = idioma.BuscarTraduccion(LBL_RObs.Tag.ToString());
            BTN_CrearOrden.Text = idioma.BuscarTraduccion(BTN_CrearOrden.Tag.ToString());
            BTN_GuardarRecepcion.Text = idioma.BuscarTraduccion(BTN_GuardarRecepcion.Tag.ToString());
            BTN_AsignarTecnico.Text = idioma.BuscarTraduccion(BTN_AsignarTecnico.Tag.ToString());

            LBL_DDescripcion.Text = idioma.BuscarTraduccion(LBL_DDescripcion.Tag.ToString()) + " *";
            CHK_EsReparable.Text = idioma.BuscarTraduccion(CHK_EsReparable.Tag.ToString());
            LBL_DDias.Text = idioma.BuscarTraduccion(LBL_DDias.Tag.ToString());
            LBL_DObs.Text = idioma.BuscarTraduccion(LBL_DObs.Tag.ToString());
            LBL_AvisoNoReparable.Text = idioma.BuscarTraduccion(LBL_AvisoNoReparable.Tag.ToString());
            BTN_Iniciar.Text = idioma.BuscarTraduccion(BTN_Iniciar.Tag.ToString());
            BTN_Finalizar.Text = idioma.BuscarTraduccion(BTN_Finalizar.Tag.ToString());

            LBL_PTipo.Text = idioma.BuscarTraduccion(LBL_PTipo.Tag.ToString());
            LBL_PDesc.Text = idioma.BuscarTraduccion(LBL_PDesc.Tag.ToString());
            LBL_PCant.Text = idioma.BuscarTraduccion(LBL_PCant.Tag.ToString());
            LBL_PPrecio.Text = idioma.BuscarTraduccion(LBL_PPrecio.Tag.ToString());
            BTN_AgregarItem.Text = idioma.BuscarTraduccion(BTN_AgregarItem.Tag.ToString());
            BTN_QuitarItem.Text = idioma.BuscarTraduccion(BTN_QuitarItem.Tag.ToString());
            LBL_PDescuento.Text = idioma.BuscarTraduccion(LBL_PDescuento.Tag.ToString());
            LBL_PGarantia.Text = idioma.BuscarTraduccion(LBL_PGarantia.Tag.ToString());
            LBL_PMedio.Text = idioma.BuscarTraduccion(LBL_PMedio.Tag.ToString());
            LBL_PMotivo.Text = idioma.BuscarTraduccion(LBL_PMotivo.Tag.ToString());
            LBL_PObs.Text = idioma.BuscarTraduccion(LBL_PObs.Tag.ToString());
            BTN_Emitir.Text = idioma.BuscarTraduccion(BTN_Emitir.Tag.ToString());
            BTN_Aprobar.Text = idioma.BuscarTraduccion(BTN_Aprobar.Tag.ToString());
            BTN_Rechazar.Text = idioma.BuscarTraduccion(BTN_Rechazar.Tag.ToString());

            LBL_EEntregadoA.Text = idioma.BuscarTraduccion(LBL_EEntregadoA.Tag.ToString()) + " *";
            LBL_EDocumento.Text = idioma.BuscarTraduccion(LBL_EDocumento.Tag.ToString());
            LBL_EObs.Text = idioma.BuscarTraduccion(LBL_EObs.Tag.ToString());
            BTN_Entregar.Text = idioma.BuscarTraduccion(BTN_Entregar.Tag.ToString());
            BTN_Cerrar.Text = idioma.BuscarTraduccion(BTN_Cerrar.Tag.ToString());
        }

        private void CargarCatalogos()
        {
            try
            {
                _clientes = _clienteService.Listar(false);
                _tecnicos = _service.ListarTecnicosElegibles();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarComboTiposItem()
        {
            _cargandoCombos = true;

            try
            {
                string seleccionado = TipoItemPresupuesto.ManoObra;

                if (CBO_PTipo.SelectedValue is string)
                    seleccionado = (string)CBO_PTipo.SelectedValue;

                List<ItemTipoItem> items = new List<ItemTipoItem>();
                items.Add(new ItemTipoItem { Id = TipoItemPresupuesto.ManoObra, Nombre = TraducirTipoItem(TipoItemPresupuesto.ManoObra) });
                items.Add(new ItemTipoItem { Id = TipoItemPresupuesto.Servicio, Nombre = TraducirTipoItem(TipoItemPresupuesto.Servicio) });

                CBO_PTipo.DataSource = null;
                CBO_PTipo.DisplayMember = "Nombre";
                CBO_PTipo.ValueMember = "Id";
                CBO_PTipo.DataSource = items;
                CBO_PTipo.SelectedValue = seleccionado;
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void CargarCombosRecepcion()
        {
            _cargandoCombos = true;

            try
            {
                int idClienteActual = _orden != null ? _orden.IdCliente : 0;
                int idEquipoActual = _orden != null ? _orden.IdEquipo : 0;
                int idTecnicoActual = (_orden != null && _orden.IdTecnicoAsignado.HasValue) ? _orden.IdTecnicoAsignado.Value : 0;

                if (_esNuevo && CBO_RCliente.SelectedValue is int && (int)CBO_RCliente.SelectedValue > 0)
                    idClienteActual = (int)CBO_RCliente.SelectedValue;

                List<ItemCliente> itemsClientes = new List<ItemCliente>();

                foreach (Cliente c in _clientes)
                    itemsClientes.Add(new ItemCliente { Id = c.Id, Nombre = c.Apellido + ", " + c.Nombre + " (" + c.Documento + ")" });

                if (idClienteActual > 0 && itemsClientes.Find(x => x.Id == idClienteActual) == null)
                {
                    try
                    {
                        Cliente extra = _clienteService.ObtenerPorId(idClienteActual);

                        if (extra != null)
                            itemsClientes.Add(new ItemCliente { Id = extra.Id, Nombre = extra.Apellido + ", " + extra.Nombre + " (" + extra.Documento + ")" });
                    }
                    catch
                    {
                    }
                }

                CBO_RCliente.DataSource = null;
                CBO_RCliente.DisplayMember = "Nombre";
                CBO_RCliente.ValueMember = "Id";
                CBO_RCliente.DataSource = itemsClientes;

                if (idClienteActual > 0)
                    CBO_RCliente.SelectedValue = idClienteActual;

                CargarComboEquipos(idClienteActual, idEquipoActual);

                string sinAsignar = T("OrdenDetalle.SinAsignar");
                List<ItemTecnico> itemsTecnicos = new List<ItemTecnico>();
                itemsTecnicos.Add(new ItemTecnico { Id = 0, Nombre = sinAsignar });

                foreach (Usuario u in _tecnicos)
                    itemsTecnicos.Add(new ItemTecnico { Id = u.Id, Nombre = u.Username });

                if (idTecnicoActual > 0 && itemsTecnicos.Find(x => x.Id == idTecnicoActual) == null)
                    itemsTecnicos.Add(new ItemTecnico { Id = idTecnicoActual, Nombre = "#" + idTecnicoActual });

                CBO_RTecnico.DataSource = null;
                CBO_RTecnico.DisplayMember = "Nombre";
                CBO_RTecnico.ValueMember = "Id";
                CBO_RTecnico.DataSource = itemsTecnicos;
                CBO_RTecnico.SelectedValue = idTecnicoActual;
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void CargarComboEquipos(int idCliente, int idEquipoActual)
        {
            List<ItemEquipo> items = new List<ItemEquipo>();

            try
            {
                if (idCliente > 0)
                {
                    bool incluirInactivos = !_esNuevo;
                    _equipos = _equipoService.ListarPorCliente(idCliente, incluirInactivos);

                    foreach (Equipo e in _equipos)
                        items.Add(new ItemEquipo { Id = e.Id, Nombre = DescribirEquipo(e) });
                }

                if (idEquipoActual > 0 && items.Find(x => x.Id == idEquipoActual) == null)
                {
                    try
                    {
                        Equipo extra = _equipoService.ObtenerPorId(idEquipoActual);

                        if (extra != null)
                            items.Add(new ItemEquipo { Id = extra.Id, Nombre = DescribirEquipo(extra) });
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }

            CBO_REquipo.DataSource = null;
            CBO_REquipo.DisplayMember = "Nombre";
            CBO_REquipo.ValueMember = "Id";
            CBO_REquipo.DataSource = items;

            if (idEquipoActual > 0)
                CBO_REquipo.SelectedValue = idEquipoActual;
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

        private void CargarOrden()
        {
            if (_esNuevo)
            {
                CargarModoNuevo();
                return;
            }

            try
            {
                _orden = _service.ObtenerPorId(_idOrden);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                Close();
                return;
            }

            try
            {
                _diagnostico = _service.ObtenerDiagnostico(_idOrden);
                _presupuesto = _service.ObtenerPresupuesto(_idOrden);
                _entrega = _service.ObtenerEntrega(_idOrden);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                return;
            }

            CargarEncabezado();
            CargarCombosRecepcion();

            CBO_RCliente.Enabled = false;
            CBO_REquipo.Enabled = false;
            TXT_RProblema.Text = _orden.ProblemaInformado;
            TXT_REstadoFisico.Text = _orden.EstadoFisicoIngreso;
            TXT_RAccesorios.Text = _orden.AccesoriosIngreso;
            TXT_RObs.Text = _orden.ObservacionesIngreso;

            CargarTabDiagnostico();
            CargarTabPresupuesto();
            CargarTabHistorial();
            CargarTabEntrega();
            AplicarPermisosDetalle();
            ConfigurarColumnasDetalle();
            ConfigurarColumnasHistorial();
        }

        private void CargarModoNuevo()
        {
            _orden = null;
            _diagnostico = null;
            _presupuesto = null;
            _entrega = null;

            LBL_Numero.Text = T("OrdenDetalle.TituloNuevo");
            LBL_Estado.Text = "";
            LBL_Resultado.Text = "";
            LBL_Tipo.Text = "";
            LBL_HCliente.Text = T("OrdenDetalle.Cliente");
            LBL_HEquipo.Text = T("OrdenDetalle.Equipo");
            LBL_HTecnico.Text = T("OrdenDetalle.Tecnico");
            LBL_HFecha.Text = T("OrdenDetalle.Fecha");

            CargarCombosRecepcion();
            CBO_RCliente.Enabled = true;
            CBO_REquipo.Enabled = true;
            CBO_RTecnico.Enabled = false;

            TAB_Diagnostico.Enabled = false;
            TAB_Presupuesto.Enabled = false;
            TAB_Historial.Enabled = false;
            TAB_Entrega.Enabled = false;
            TAB_Detalle.SelectedTab = TAB_Recepcion;

            AplicarPermisosDetalle();
        }

        private void CargarEncabezado()
        {
            LBL_Numero.Text = T("OrdenDetalle.Numero").Replace("{0}", _orden.NumeroOrden.ToString());
            LBL_Estado.Text = T("OrdenDetalle.Estado") + ": " + TraducirEstado(_orden.Estado);
            LBL_Resultado.Text = T("OrdenDetalle.Resultado") + ": " + TraducirResultado(_orden.Resultado);
            LBL_Tipo.Text = T("OrdenDetalle.Tipo") + ": " + TraducirTipo(_orden.TipoOrden);
            LBL_HCliente.Text = T("OrdenDetalle.Cliente") + ": " + ResolverCliente(_orden.IdCliente);
            LBL_HEquipo.Text = T("OrdenDetalle.Equipo") + ": " + ResolverEquipo(_orden.IdEquipo);
            LBL_HTecnico.Text = T("OrdenDetalle.Tecnico") + ": " + ResolverTecnico(_orden.IdTecnicoAsignado);
            LBL_HFecha.Text = T("OrdenDetalle.Fecha") + ": " + _orden.FechaIngreso.ToString("g");
        }

        private void CargarTabDiagnostico()
        {
            if (_diagnostico != null)
            {
                TXT_DDescripcion.Text = _diagnostico.Descripcion;
                CHK_EsReparable.Checked = _diagnostico.EsReparable;
                NUM_DDias.Value = _diagnostico.TiempoEstimadoDias.HasValue ? _diagnostico.TiempoEstimadoDias.Value : 0;
                TXT_DObs.Text = _diagnostico.Observaciones;
                LBL_AvisoNoReparable.Visible = !_diagnostico.EsReparable;
            }
            else
            {
                LBL_AvisoNoReparable.Visible = false;
            }
        }

        private void CargarTabPresupuesto()
        {
            try
            {
                if (_presupuesto != null)
                {
                    List<DetallePresupuesto> detalle = _service.ListarDetalle(_presupuesto.Id);
                    DGV_Detalle.DataSource = new BindingList<DetallePresupuesto>(detalle);
                    NUM_PDescuento.Value = _presupuesto.Descuento;
                    NUM_PGarantia.Value = _presupuesto.DiasGarantia;
                    TXT_PMedio.Text = _presupuesto.MedioRespuesta ?? "";
                    TXT_PMotivo.Text = _presupuesto.MotivoRechazo ?? "";
                    TXT_PObs.Text = _presupuesto.Observaciones;
                }
                else
                {
                    DGV_Detalle.DataSource = new BindingList<DetallePresupuesto>(_itemsNuevo);
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }

            RefrescarTotales();
        }

        private void CargarTabHistorial()
        {
            try
            {
                List<HistorialEstadoOrden> historial = _service.ListarHistorial(_idOrden);
                List<FilaHistorial> filas = new List<FilaHistorial>();

                foreach (HistorialEstadoOrden h in historial)
                {
                    filas.Add(new FilaHistorial
                    {
                        Fecha = h.FechaHora,
                        Anterior = TraducirEstado(h.EstadoAnterior),
                        Nuevo = TraducirEstado(h.EstadoNuevo),
                        Usuario = ResolverTecnico(h.IdUsuario),
                        Observacion = h.Observacion
                    });
                }

                DGV_Historial.DataSource = new BindingList<FilaHistorial>(filas);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarTabEntrega()
        {
            if (_entrega != null)
            {
                TXT_EEntregadoA.Text = _entrega.EntregadoA;
                TXT_EDocumento.Text = _entrega.DocumentoReceptor;
                TXT_EObs.Text = _entrega.Observaciones;
                LBL_FechaEntrega.Text = T("OrdenDetalle.FechaEntrega") + ": " + _entrega.FechaEntrega.ToString("g");
            }
            else
            {
                LBL_FechaEntrega.Text = T("OrdenDetalle.FechaEntrega") + ": " + T("Resultado.SinResultado");
            }
        }

        private void RefrescarTotales()
        {
            decimal subtotal = 0;
            decimal descuento = 0;

            if (_presupuesto != null)
            {
                subtotal = _presupuesto.Subtotal;
                descuento = _presupuesto.Descuento;
                LBL_EstadoPresupuesto.Text = T("OrdenDetalle.EstadoPresupuesto") + ": " + TraducirEstadoPresupuesto(_presupuesto.Estado);
            }
            else
            {
                foreach (DetallePresupuesto item in _itemsNuevo)
                    subtotal += item.Subtotal;

                descuento = NUM_PDescuento.Value;
                LBL_EstadoPresupuesto.Text = T("OrdenDetalle.EstadoPresupuesto") + ": " + T("Resultado.SinResultado");
            }

            LBL_Totales.Text = T("OrdenDetalle.Totales")
                .Replace("{0}", subtotal.ToString("F2"))
                .Replace("{1}", descuento.ToString("F2"))
                .Replace("{2}", (subtotal - descuento).ToString("F2"));
        }

        private void SeleccionarTab(string claveTag)
        {
            foreach (TabPage pagina in TAB_Detalle.TabPages)
            {
                if (pagina.Tag != null && pagina.Tag.ToString() == claveTag && pagina.Enabled)
                {
                    TAB_Detalle.SelectedTab = pagina;
                    return;
                }
            }
        }

        private string ResolverCliente(int idCliente)
        {
            Cliente c = _clientes.FirstOrDefault(x => x.Id == idCliente);

            if (c == null)
            {
                try
                {
                    c = _clienteService.ObtenerPorId(idCliente);
                }
                catch
                {
                    return "#" + idCliente;
                }
            }

            if (c == null)
                return "#" + idCliente;

            return c.Apellido + ", " + c.Nombre;
        }

        private string ResolverEquipo(int idEquipo)
        {
            Equipo e = _equipos.FirstOrDefault(x => x.Id == idEquipo);

            if (e == null)
            {
                try
                {
                    e = _equipoService.ObtenerPorId(idEquipo);
                }
                catch
                {
                    return "#" + idEquipo;
                }
            }

            if (e == null)
                return "#" + idEquipo;

            return DescribirEquipo(e);
        }

        private string ResolverTecnico(int? idTecnico)
        {
            if (!idTecnico.HasValue || idTecnico.Value <= 0)
                return T("OrdenDetalle.SinAsignar");

            Usuario u = _tecnicos.FirstOrDefault(x => x.Id == idTecnico.Value);
            return u != null ? u.Username : "#" + idTecnico.Value;
        }

        private string T(string clave)
        {
            if (_sesionIdioma.idioma == null)
                return clave;

            return _sesionIdioma.idioma.BuscarTraduccion(clave);
        }

        private string TraducirEstado(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return "";

            return T("Estado." + estado);
        }

        private string TraducirResultado(string resultado)
        {
            if (string.IsNullOrEmpty(resultado))
                return T("Resultado.SinResultado");

            return T("Resultado." + resultado);
        }

        private string TraducirTipo(string tipo)
        {
            if (string.IsNullOrEmpty(tipo))
                return "";

            return T("Tipo." + tipo);
        }

        private string TraducirTipoItem(string tipo)
        {
            if (string.IsNullOrEmpty(tipo))
                return "";

            return T("TipoItem." + tipo);
        }

        private string TraducirEstadoPresupuesto(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return "";

            return T("PresupuestoEstado." + estado);
        }

        private void ConfigurarColumnasDetalle()
        {
            if (DGV_Detalle.Columns.Count == 0)
                return;

            ConfigurarColumnaDetalle("Descripcion", "Columna.Descripcion");
            ConfigurarColumnaDetalle("TipoItem", "Columna.Tipo");
            ConfigurarColumnaDetalle("Cantidad", "Columna.Cantidad");
            ConfigurarColumnaDetalle("PrecioUnitario", "Columna.Precio");
            ConfigurarColumnaDetalle("Subtotal", "Columna.Subtotal");

            if (DGV_Detalle.Columns.Contains("Id"))
                DGV_Detalle.Columns["Id"].Visible = false;
            if (DGV_Detalle.Columns.Contains("IdPresupuesto"))
                DGV_Detalle.Columns["IdPresupuesto"].Visible = false;
            if (DGV_Detalle.Columns.Contains("IdRepuesto"))
                DGV_Detalle.Columns["IdRepuesto"].Visible = false;

            DGV_Detalle.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumnaDetalle(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Detalle.Columns.Contains(nombreColumna))
                return;

            DGV_Detalle.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Detalle.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private void ConfigurarColumnasHistorial()
        {
            if (DGV_Historial.Columns.Count == 0)
                return;

            ConfigurarColumnaHistorial("Fecha", "Columna.Fecha");
            ConfigurarColumnaHistorial("Anterior", "Columna.Anterior");
            ConfigurarColumnaHistorial("Nuevo", "Columna.Nuevo");
            ConfigurarColumnaHistorial("Usuario", "Columna.Usuario");
            ConfigurarColumnaHistorial("Observacion", "Columna.Observacion");

            DGV_Historial.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumnaHistorial(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Historial.Columns.Contains(nombreColumna))
                return;

            DGV_Historial.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Historial.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private void DGV_Detalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DGV_Detalle.Columns.Count == 0)
                return;

            if (DGV_Detalle.Columns[e.ColumnIndex].Name != "TipoItem")
                return;

            if (e.Value is string)
                e.Value = TraducirTipoItem((string)e.Value);
        }

        private void NUM_PDescuento_ValueChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (_presupuesto != null)
                return;

            RefrescarTotales();
        }

        private void CBO_RCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_RCliente.DataSource == null)
                return;

            if (!_esNuevo)
                return;

            int idCliente = 0;

            if (CBO_RCliente.SelectedValue is int)
                idCliente = (int)CBO_RCliente.SelectedValue;

            _cargandoCombos = true;

            try
            {
                CargarComboEquipos(idCliente, 0);
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void BTN_CrearOrden_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            int idCliente = 0;
            int idEquipo = 0;

            if (CBO_RCliente.SelectedValue is int)
                idCliente = (int)CBO_RCliente.SelectedValue;

            if (CBO_REquipo.SelectedValue is int)
                idEquipo = (int)CBO_REquipo.SelectedValue;

            if (idCliente <= 0 || idEquipo <= 0 || string.IsNullOrWhiteSpace(TXT_RProblema.Text))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                OrdenServicio creada = _service.CrearOrden(idCliente, idEquipo, TXT_RProblema.Text.Trim(),
                    TXT_REstadoFisico.Text.Trim(), TXT_RAccesorios.Text.Trim(), TXT_RObs.Text.Trim());
                _idOrden = creada.Id;
                _esNuevo = false;
                TAB_Diagnostico.Enabled = true;
                TAB_Presupuesto.Enabled = true;
                TAB_Historial.Enabled = true;
                TAB_Entrega.Enabled = true;
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_GuardarRecepcion_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (string.IsNullOrWhiteSpace(TXT_RProblema.Text))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                _service.ModificarRecepcion(_idOrden, TXT_RProblema.Text.Trim(),
                    TXT_REstadoFisico.Text.Trim(), TXT_RAccesorios.Text.Trim(), TXT_RObs.Text.Trim());
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_AsignarTecnico_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            int idTecnico = 0;

            if (CBO_RTecnico.SelectedValue is int)
                idTecnico = (int)CBO_RTecnico.SelectedValue;

            if (idTecnico <= 0)
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                _service.AsignarTecnico(_idOrden, idTecnico);
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Iniciar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            try
            {
                _service.IniciarDiagnostico(_idOrden);
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Finalizar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (string.IsNullOrWhiteSpace(TXT_DDescripcion.Text))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            int? dias = NUM_DDias.Value > 0 ? (int?)((int)NUM_DDias.Value) : null;

            try
            {
                _service.FinalizarDiagnostico(_idOrden, TXT_DDescripcion.Text.Trim(),
                    CHK_EsReparable.Checked, dias, TXT_DObs.Text.Trim());
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_AgregarItem_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (string.IsNullOrWhiteSpace(TXT_PDesc.Text))
            {
                MostrarAdvertencia("Mensaje.ItemCamposObligatorios");
                return;
            }

            string tipo = TipoItemPresupuesto.ManoObra;

            if (CBO_PTipo.SelectedValue is string)
                tipo = (string)CBO_PTipo.SelectedValue;

            try
            {
                DetallePresupuesto item = DetallePresupuesto.CrearNuevo(0, tipo,
                    TXT_PDesc.Text.Trim(), (int)NUM_PCant.Value, NUM_PPrecio.Value);
                _itemsNuevo.Add(item);
                TXT_PDesc.Text = "";
                NUM_PCant.Value = 1;
                NUM_PPrecio.Value = 0;
                DGV_Detalle.DataSource = new BindingList<DetallePresupuesto>(_itemsNuevo);
                ConfigurarColumnasDetalle();
                RefrescarTotales();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_QuitarItem_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (DGV_Detalle.SelectedRows.Count == 0)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            DetallePresupuesto seleccionado = DGV_Detalle.SelectedRows[0].DataBoundItem as DetallePresupuesto;

            if (seleccionado == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            _itemsNuevo.Remove(seleccionado);
            DGV_Detalle.DataSource = new BindingList<DetallePresupuesto>(_itemsNuevo);
            ConfigurarColumnasDetalle();
            RefrescarTotales();
        }

        private void BTN_Emitir_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (_itemsNuevo.Count == 0)
            {
                MostrarAdvertencia("Mensaje.ItemCamposObligatorios");
                return;
            }

            try
            {
                _service.EmitirPresupuesto(_idOrden, new List<DetallePresupuesto>(_itemsNuevo),
                    NUM_PDescuento.Value, (int)NUM_PGarantia.Value, TXT_PObs.Text.Trim());
                _itemsNuevo.Clear();
                TXT_PDesc.Text = "";
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Aprobar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PresupuestosDecidir))
            {
                MostrarAccesoDenegado();
                return;
            }

            try
            {
                _service.RegistrarAprobacionPresupuesto(_idOrden, TXT_PMedio.Text.Trim(), TXT_PObs.Text.Trim());
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Rechazar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PresupuestosDecidir))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (string.IsNullOrWhiteSpace(TXT_PMotivo.Text))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                _service.RegistrarRechazoPresupuesto(_idOrden, TXT_PMotivo.Text.Trim(),
                    TXT_PMedio.Text.Trim(), TXT_PObs.Text.Trim());
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Entregar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEntregar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (string.IsNullOrWhiteSpace(TXT_EEntregadoA.Text))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                T("Mensaje.ConfirmarEntregar"),
                T("Titulo.ConfirmarEntrega"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
                return;

            try
            {
                _service.EntregarOrden(_idOrden, TXT_EEntregadoA.Text.Trim(),
                    TXT_EDocumento.Text.Trim(), TXT_EObs.Text.Trim());
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AplicarPermisosDetalle()
        {
            bool puedeCrear = TienePermiso(CodigosPermiso.OrdenesCrear);
            bool puedeEditar = TienePermiso(CodigosPermiso.OrdenesEditar);
            bool puedeDecidir = TienePermiso(CodigosPermiso.PresupuestosDecidir);
            bool puedeEntregar = TienePermiso(CodigosPermiso.OrdenesEntregar);

            bool entregado = !_esNuevo && _orden != null && _orden.Estado == EstadoOrdenServicio.Entregado;
            bool editable = !_esNuevo && !entregado && puedeEditar;

            BTN_CrearOrden.Visible = _esNuevo && puedeCrear;
            BTN_CrearOrden.Enabled = _esNuevo && puedeCrear;

            TXT_RProblema.Enabled = _esNuevo || editable;
            TXT_REstadoFisico.Enabled = _esNuevo || editable;
            TXT_RAccesorios.Enabled = _esNuevo || editable;
            TXT_RObs.Enabled = _esNuevo || editable;

            BTN_GuardarRecepcion.Visible = !_esNuevo && puedeEditar;
            BTN_GuardarRecepcion.Enabled = editable;
            BTN_AsignarTecnico.Visible = !_esNuevo && puedeEditar;
            BTN_AsignarTecnico.Enabled = editable;

            if (!_esNuevo)
                CBO_RTecnico.Enabled = editable;

            bool puedeIniciar = editable && _orden != null
                && _orden.Estado == EstadoOrdenServicio.Recibido
                && _orden.IdTecnicoAsignado.HasValue;
            bool puedeFinalizar = editable && _orden != null
                && _orden.Estado == EstadoOrdenServicio.EnDiagnostico
                && _diagnostico == null;

            TXT_DDescripcion.Enabled = puedeFinalizar;
            CHK_EsReparable.Enabled = puedeFinalizar;
            NUM_DDias.Enabled = puedeFinalizar;
            TXT_DObs.Enabled = puedeFinalizar;
            BTN_Iniciar.Visible = !_esNuevo && puedeEditar;
            BTN_Iniciar.Enabled = puedeIniciar;
            BTN_Finalizar.Visible = !_esNuevo && puedeEditar;
            BTN_Finalizar.Enabled = puedeFinalizar;

            bool editaItems = editable && _orden != null
                && _orden.Estado == EstadoOrdenServicio.PendientePresupuesto
                && _presupuesto == null;

            CBO_PTipo.Enabled = editaItems;
            TXT_PDesc.Enabled = editaItems;
            NUM_PCant.Enabled = editaItems;
            NUM_PPrecio.Enabled = editaItems;
            BTN_AgregarItem.Visible = !_esNuevo && puedeEditar;
            BTN_AgregarItem.Enabled = editaItems;
            BTN_QuitarItem.Visible = !_esNuevo && puedeEditar;
            BTN_QuitarItem.Enabled = editaItems;
            NUM_PDescuento.Enabled = editaItems;
            NUM_PGarantia.Enabled = editaItems;
            BTN_Emitir.Visible = !_esNuevo && puedeEditar;
            BTN_Emitir.Enabled = editaItems && _itemsNuevo.Count > 0;

            bool presupuestoPendiente = !_esNuevo && _presupuesto != null
                && _presupuesto.Estado == EstadoPresupuesto.Pendiente
                && _orden != null && _orden.Estado == EstadoOrdenServicio.EsperandoRespuesta;

            TXT_PMedio.Enabled = !_esNuevo && !entregado && (editaItems || (puedeDecidir && presupuestoPendiente));
            TXT_PMotivo.Enabled = !_esNuevo && !entregado && puedeDecidir && presupuestoPendiente;
            TXT_PObs.Enabled = !_esNuevo && !entregado && (editaItems || (puedeDecidir && presupuestoPendiente));
            BTN_Aprobar.Visible = !_esNuevo && puedeDecidir;
            BTN_Aprobar.Enabled = puedeDecidir && presupuestoPendiente;
            BTN_Rechazar.Visible = !_esNuevo && puedeDecidir;
            BTN_Rechazar.Enabled = puedeDecidir && presupuestoPendiente;

            bool puedeEntregarAhora = editable && puedeEntregar && _orden != null
                && _orden.Estado == EstadoOrdenServicio.ListoRetiro
                && _entrega == null;

            TXT_EEntregadoA.Enabled = puedeEntregarAhora;
            TXT_EDocumento.Enabled = puedeEntregarAhora;
            TXT_EObs.Enabled = puedeEntregarAhora;
            BTN_Entregar.Visible = !_esNuevo && puedeEntregar;
            BTN_Entregar.Enabled = puedeEntregarAhora;
        }

        private bool TienePermiso(string codigo)
        {
            return SessionManager.HaySesionActiva() && SessionManager.TienePermiso(codigo);
        }

        private void MostrarAccesoDenegado()
        {
            MessageBox.Show(
                T("Mensaje.SinPermisos"),
                T("Titulo.AccesoDenegado"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void MostrarExito(string claveMensaje)
        {
            MessageBox.Show(
                T(claveMensaje),
                T("Titulo.Exito"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void MostrarAdvertencia(string claveMensaje)
        {
            MessageBox.Show(
                T(claveMensaje),
                T("Titulo.Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            string detalle = ex != null ? ex.Message : "";
            MessageBox.Show(
                T("Mensaje.ErrorOperacion").Replace("{0}", detalle),
                T("Titulo.Error"),
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
