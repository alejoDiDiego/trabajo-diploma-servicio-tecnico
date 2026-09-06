using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Clientes;
using APPLICATION.Features.Equipos;
using APPLICATION.Features.Ordenes;
using APPLICATION.Features.Repuestos;
using DOMAIN.Features.Clientes;
using DOMAIN.Features.Equipos;
using DOMAIN.Features.Ordenes;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Reparaciones;
using DOMAIN.Features.Repuestos;
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

        private class ItemRepuestoConsumo
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class ItemIntervencion
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class FilaReparacion
        {
            public int Id { get; set; }
            public int Numero { get; set; }
            public string Tecnico { get; set; }
            public DateTime Inicio { get; set; }
            public string Fin { get; set; }
            public string Estado { get; set; }
        }

        private class FilaConsumido
        {
            public int IdRepuesto { get; set; }
            public string Repuesto { get; set; }
            public int Cantidad { get; set; }
            public decimal Costo { get; set; }
            public decimal Subtotal { get; set; }
        }

        private class ItemSelectorPresupuesto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class FilaPrueba
        {
            public DateTime Fecha { get; set; }
            public string Intervencion { get; set; }
            public string Descripcion { get; set; }
            public string Resultado { get; set; }
            public string Usuario { get; set; }
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
        private Presupuesto _original = null;
        private List<Presupuesto> _adicionales = new List<Presupuesto>();
        private List<Presupuesto> _todosPresupuestos = new List<Presupuesto>();
        private bool _puedeCancelarSolicitud = false;
        private int? _idSeleccionPreferida = null;
        private bool _modoNuevoAdicional = false;
        private bool _modoNuevoOriginal = false;
        private bool _hayAdicionalPendiente = false;
        private Entrega _entrega = null;
        private List<Cliente> _clientes = new List<Cliente>();
        private List<Equipo> _equipos = new List<Equipo>();
        private List<Usuario> _tecnicos = new List<Usuario>();
        private List<DetallePresupuesto> _itemsNuevo = new List<DetallePresupuesto>();
        private List<Reparacion> _reparaciones = new List<Reparacion>();
        private Dictionary<int, string> _nombresRepuestos = new Dictionary<int, string>();

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
            TAB_Reparaciones.Text = idioma.BuscarTraduccion(TAB_Reparaciones.Tag.ToString());
            TAB_Pruebas.Text = idioma.BuscarTraduccion(TAB_Pruebas.Tag.ToString());
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
            LBL_SelectorPresupuesto.Text = idioma.BuscarTraduccion(LBL_SelectorPresupuesto.Tag.ToString());
            BTN_SolicitarAdicional.Text = idioma.BuscarTraduccion(BTN_SolicitarAdicional.Tag.ToString());
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
            BTN_GuardarBorrador.Text = idioma.BuscarTraduccion(BTN_GuardarBorrador.Tag.ToString());
            BTN_EliminarBorrador.Text = idioma.BuscarTraduccion(BTN_EliminarBorrador.Tag.ToString());
            BTN_Anular.Text = idioma.BuscarTraduccion(BTN_Anular.Tag.ToString());
            BTN_CancelarSolicitud.Text = idioma.BuscarTraduccion(BTN_CancelarSolicitud.Tag.ToString());
            BTN_Aprobar.Text = idioma.BuscarTraduccion(BTN_Aprobar.Tag.ToString());
            BTN_Rechazar.Text = idioma.BuscarTraduccion(BTN_Rechazar.Tag.ToString());

            LBL_EEntregadoA.Text = idioma.BuscarTraduccion(LBL_EEntregadoA.Tag.ToString()) + " *";
            LBL_EDocumento.Text = idioma.BuscarTraduccion(LBL_EDocumento.Tag.ToString());
            LBL_EObs.Text = idioma.BuscarTraduccion(LBL_EObs.Tag.ToString());
            BTN_Entregar.Text = idioma.BuscarTraduccion(BTN_Entregar.Tag.ToString());
            BTN_Cerrar.Text = idioma.BuscarTraduccion(BTN_Cerrar.Tag.ToString());

            BTN_IniciarReparacion.Text = idioma.BuscarTraduccion(BTN_IniciarReparacion.Tag.ToString());
            LBL_CRepuesto.Text = idioma.BuscarTraduccion(LBL_CRepuesto.Tag.ToString());
            LBL_CCantidad.Text = idioma.BuscarTraduccion(LBL_CCantidad.Tag.ToString());
            BTN_Consumir.Text = idioma.BuscarTraduccion(BTN_Consumir.Tag.ToString());
            BTN_QuitarConsumo.Text = idioma.BuscarTraduccion(BTN_QuitarConsumo.Tag.ToString());
            LBL_Consumidos.Text = idioma.BuscarTraduccion(LBL_Consumidos.Tag.ToString());
            LBL_FTrabajo.Text = idioma.BuscarTraduccion(LBL_FTrabajo.Tag.ToString()) + " *";
            LBL_FObs.Text = idioma.BuscarTraduccion(LBL_FObs.Tag.ToString());
            BTN_FinalizarReparacion.Text = idioma.BuscarTraduccion(BTN_FinalizarReparacion.Tag.ToString());
            LBL_PIntervencion.Text = idioma.BuscarTraduccion(LBL_PIntervencion.Tag.ToString());
            LBL_PDDesc.Text = idioma.BuscarTraduccion(LBL_PDDesc.Tag.ToString()) + " *";
            RDO_Aprobada.Text = idioma.BuscarTraduccion(RDO_Aprobada.Tag.ToString());
            RDO_Fallida.Text = idioma.BuscarTraduccion(RDO_Fallida.Tag.ToString());
            LBL_PDObs.Text = idioma.BuscarTraduccion(LBL_PDObs.Tag.ToString());
            BTN_RegistrarPrueba.Text = idioma.BuscarTraduccion(BTN_RegistrarPrueba.Tag.ToString());
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
            CargarTabReparaciones();
            CargarTabPruebas();
            CargarTabHistorial();
            CargarTabEntrega();
            AplicarPermisosDetalle();
            ConfigurarColumnasDetalle();
            ConfigurarColumnasReparaciones();
            ConfigurarColumnasConsumidos();
            ConfigurarColumnasPruebas();
            ConfigurarColumnasHistorial();
        }

        private void CargarModoNuevo()
        {
            _orden = null;
            _diagnostico = null;
            _presupuesto = null;
            _original = null;
            _adicionales = new List<Presupuesto>();
            _todosPresupuestos = new List<Presupuesto>();
            _puedeCancelarSolicitud = false;
            _idSeleccionPreferida = null;
            _modoNuevoAdicional = false;
            _modoNuevoOriginal = false;
            _hayAdicionalPendiente = false;
            _entrega = null;

            LBL_MontoAutorizado.Text = "";

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
            TAB_Reparaciones.Enabled = false;
            TAB_Pruebas.Enabled = false;
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
                _original = _service.ObtenerPresupuestoOriginal(_idOrden);
                _adicionales = _service.ListarAdicionales(_idOrden);

                try
                {
                    _todosPresupuestos = _service.ListarTodosPresupuestos(_idOrden);
                }
                catch
                {
                    _todosPresupuestos = new List<Presupuesto>();

                    if (_original != null)
                        _todosPresupuestos.Add(_original);

                    _todosPresupuestos.AddRange(_adicionales);
                }

                _hayAdicionalPendiente = _service.ExisteAdicionalPendiente(_idOrden);

                try
                {
                    _puedeCancelarSolicitud = _service.PuedeCancelarSolicitud(_idOrden);
                }
                catch
                {
                    _puedeCancelarSolicitud = false;
                }

                CargarSelectorPresupuesto();
                SeleccionarPresupuestoPorDefecto();
                MostrarPresupuestoSeleccionado();
                ActualizarMontoAutorizado();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }

            RefrescarTotales();
        }

        private string DescribirPresupuesto(Presupuesto p, string titulo)
        {
            string texto = titulo + " | " + TraducirEstadoPresupuesto(p.Estado) + " | $" + p.Total.ToString("F2");

            if (p.Estado == EstadoPresupuesto.Anulado || p.Estado == EstadoPresupuesto.Borrador)
                texto += " (" + TraducirEstadoPresupuesto(p.Estado) + ")";

            return texto;
        }

        private void CargarSelectorPresupuesto()
        {
            _cargandoCombos = true;

            try
            {
                List<ItemSelectorPresupuesto> items = new List<ItemSelectorPresupuesto>();
                List<Presupuesto> originales = new List<Presupuesto>();
                List<Presupuesto> adicionales = new List<Presupuesto>();

                foreach (Presupuesto p in _todosPresupuestos)
                {
                    if (p.Tipo == TipoPresupuesto.Original)
                        originales.Add(p);
                    else
                        adicionales.Add(p);
                }

                originales.Sort((a, b) => a.Id.CompareTo(b.Id));
                adicionales.Sort((a, b) => a.Id.CompareTo(b.Id));

                if (originales.Count == 0)
                    items.Add(new ItemSelectorPresupuesto
                    {
                        Id = 0,
                        Nombre = TraducirTipoPresupuesto(TipoPresupuesto.Original) + " " + T("OrdenDetalle.PresupuestoNuevo")
                    });
                else if (originales.Count == 1)
                    items.Add(new ItemSelectorPresupuesto
                    {
                        Id = originales[0].Id,
                        Nombre = DescribirPresupuesto(originales[0], TraducirTipoPresupuesto(TipoPresupuesto.Original))
                    });
                else
                {
                    for (int i = 0; i < originales.Count; i++)
                    {
                        items.Add(new ItemSelectorPresupuesto
                        {
                            Id = originales[i].Id,
                            Nombre = DescribirPresupuesto(originales[i], TraducirTipoPresupuesto(TipoPresupuesto.Original) + " #" + (i + 1))
                        });
                    }
                }

                int numero = 0;

                foreach (Presupuesto adicional in adicionales)
                {
                    numero++;
                    items.Add(new ItemSelectorPresupuesto
                    {
                        Id = adicional.Id,
                        Nombre = DescribirPresupuesto(adicional, TraducirTipoPresupuesto(TipoPresupuesto.Adicional) + " #" + numero)
                    });
                }

                if (_original != null && _original.Estado == EstadoPresupuesto.Aprobado)
                    items.Add(new ItemSelectorPresupuesto { Id = -1, Nombre = T("OrdenDetalle.NuevoAdicional") });

                // Nuevo Original tras anulacion: sin Original activo, en PendientePresupuesto
                // y sin pausa por solicitud de adicional (pausada => solo nuevo adicional).
                if (_original == null && originales.Count > 0 && _orden != null
                    && _orden.Estado == EstadoOrdenServicio.PendientePresupuesto
                    && !_puedeCancelarSolicitud)
                    items.Add(new ItemSelectorPresupuesto { Id = -2, Nombre = T("OrdenDetalle.NuevoOriginal") });

                CBO_SelectorPresupuesto.DataSource = null;
                CBO_SelectorPresupuesto.DisplayMember = "Nombre";
                CBO_SelectorPresupuesto.ValueMember = "Id";
                CBO_SelectorPresupuesto.DataSource = items;
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void SeleccionarPresupuestoPorDefecto()
        {
            if (CBO_SelectorPresupuesto.DataSource == null)
                return;

            List<ItemSelectorPresupuesto> items = CBO_SelectorPresupuesto.DataSource as List<ItemSelectorPresupuesto>;
            int idSeleccion = 0;
            bool haySeleccion = false;

            if (_idSeleccionPreferida.HasValue && items != null)
            {
                foreach (ItemSelectorPresupuesto item in items)
                {
                    if (item.Id == _idSeleccionPreferida.Value)
                    {
                        idSeleccion = item.Id;
                        haySeleccion = true;
                        break;
                    }
                }
            }

            _idSeleccionPreferida = null;

            if (!haySeleccion)
            {
                List<Presupuesto> ordenados = new List<Presupuesto>(_todosPresupuestos);
                ordenados.Sort((a, b) => b.Id.CompareTo(a.Id));

                foreach (Presupuesto p in ordenados)
                {
                    if (p.Estado == EstadoPresupuesto.Borrador)
                    {
                        idSeleccion = p.Id;
                        haySeleccion = true;
                        break;
                    }
                }
            }

            if (!haySeleccion)
            {
                foreach (Presupuesto adicional in _adicionales)
                {
                    if (adicional.Estado == EstadoPresupuesto.Pendiente)
                    {
                        idSeleccion = adicional.Id;
                        haySeleccion = true;
                        break;
                    }
                }
            }

            if (!haySeleccion && _original != null && _original.Estado == EstadoPresupuesto.Pendiente)
            {
                idSeleccion = _original.Id;
                haySeleccion = true;
            }

            if (!haySeleccion && _original != null && _original.Estado == EstadoPresupuesto.Aprobado
                && _orden != null && _orden.Estado == EstadoOrdenServicio.PendientePresupuesto && !_hayAdicionalPendiente)
            {
                idSeleccion = -1;
                haySeleccion = true;
            }

            if (!haySeleccion && _original != null)
            {
                idSeleccion = _original.Id;
                haySeleccion = true;
            }

            if (!haySeleccion && _original == null && items != null
                && _orden != null && _orden.Estado == EstadoOrdenServicio.PendientePresupuesto
                && !_puedeCancelarSolicitud)
            {
                foreach (ItemSelectorPresupuesto item in items)
                {
                    if (item.Id == -2 || item.Id == 0)
                    {
                        idSeleccion = item.Id;
                        haySeleccion = true;
                        break;
                    }
                }
            }

            if (!haySeleccion && items != null)
            {
                foreach (ItemSelectorPresupuesto item in items)
                {
                    if (item.Id <= 0)
                        continue;

                    Presupuesto candidato = null;

                    foreach (Presupuesto p in _todosPresupuestos)
                    {
                        if (p.Id == item.Id)
                        {
                            candidato = p;
                            break;
                        }
                    }

                    if (candidato != null && candidato.Estado != EstadoPresupuesto.Anulado)
                    {
                        idSeleccion = item.Id;
                        haySeleccion = true;
                        break;
                    }
                }
            }

            if (!haySeleccion && items != null && items.Count > 0)
            {
                int idNuevo = int.MinValue;
                bool hayNuevo = false;

                foreach (ItemSelectorPresupuesto item in items)
                {
                    if (item.Id == 0 || item.Id == -2)
                    {
                        idNuevo = item.Id;
                        hayNuevo = true;
                        break;
                    }
                }

                idSeleccion = hayNuevo ? idNuevo : items[0].Id;
                haySeleccion = true;
            }

            if (!haySeleccion)
                idSeleccion = 0;

            _cargandoCombos = true;

            try
            {
                CBO_SelectorPresupuesto.SelectedValue = idSeleccion;
            }
            finally
            {
                _cargandoCombos = false;
            }

            AplicarSeleccionPresupuesto(idSeleccion);
        }

        private void AplicarSeleccionPresupuesto(int idSeleccion)
        {
            if (idSeleccion == -1)
            {
                _presupuesto = null;
                _modoNuevoAdicional = true;
                _modoNuevoOriginal = false;
                return;
            }

            if (idSeleccion == -2)
            {
                _presupuesto = null;
                _modoNuevoAdicional = false;
                _modoNuevoOriginal = true;
                return;
            }

            _modoNuevoAdicional = false;
            _modoNuevoOriginal = false;

            if (idSeleccion <= 0)
            {
                _presupuesto = null;
                return;
            }

            try
            {
                _presupuesto = _service.ObtenerPresupuestoPorId(idSeleccion);
            }
            catch (Exception ex)
            {
                _presupuesto = null;
                MostrarError(ex);
            }
        }

        private void CBO_SelectorPresupuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_SelectorPresupuesto.DataSource == null)
                return;

            if (_esNuevo)
                return;

            int idSeleccion = 0;

            if (CBO_SelectorPresupuesto.SelectedValue is int)
                idSeleccion = (int)CBO_SelectorPresupuesto.SelectedValue;

            AplicarSeleccionPresupuesto(idSeleccion);
            MostrarPresupuestoSeleccionado();
            RefrescarTotales();
            AplicarPermisosDetalle();
        }

        private void MostrarPresupuestoSeleccionado()
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
                    TXT_PObs.Text = _presupuesto.Observaciones;

                    if (_presupuesto.Estado == EstadoPresupuesto.Anulado)
                    {
                        LBL_PMotivo.Tag = "OrdenDetalle.MotivoAnulacion";
                        LBL_PMotivo.Text = T("OrdenDetalle.MotivoAnulacion");
                        TXT_PMotivo.Text = _presupuesto.MotivoAnulacion ?? "";
                    }
                    else
                    {
                        LBL_PMotivo.Tag = "OrdenDetalle.Motivo";
                        LBL_PMotivo.Text = T("OrdenDetalle.Motivo");
                        TXT_PMotivo.Text = _presupuesto.MotivoRechazo ?? "";
                    }
                }
                else
                {
                    DGV_Detalle.DataSource = new BindingList<DetallePresupuesto>(_itemsNuevo);

                    if (_modoNuevoAdicional || _modoNuevoOriginal)
                    {
                        NUM_PDescuento.Value = 0;
                        NUM_PGarantia.Value = 0;
                        TXT_PMedio.Text = "";
                        TXT_PMotivo.Text = "";
                        TXT_PObs.Text = "";
                    }

                    LBL_PMotivo.Tag = "OrdenDetalle.Motivo";
                    LBL_PMotivo.Text = T("OrdenDetalle.Motivo");
                }

                ConfigurarColumnasDetalle();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void ActualizarMontoAutorizado()
        {
            try
            {
                decimal monto = _service.CalcularMontoAutorizado(_idOrden);
                LBL_MontoAutorizado.Text = T("OrdenDetalle.MontoAutorizado").Replace("{0}", monto.ToString("F2"));
            }
            catch
            {
                LBL_MontoAutorizado.Text = T("OrdenDetalle.MontoAutorizado").Replace("{0}", "0.00");
            }
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

        private void CargarTabReparaciones()
        {
            try
            {
                RepuestoService repuestoService = new RepuestoService();
                _nombresRepuestos.Clear();

                try
                {
                    foreach (Repuesto r in repuestoService.Listar(true))
                        _nombresRepuestos[r.Id] = r.Codigo + " - " + r.Descripcion;
                }
                catch
                {
                }

                _reparaciones = _service.ListarReparaciones(_idOrden);
                _reparaciones.Sort((a, b) => a.NumeroIntervencion.CompareTo(b.NumeroIntervencion));

                List<FilaReparacion> filas = new List<FilaReparacion>();

                foreach (Reparacion r in _reparaciones)
                {
                    filas.Add(new FilaReparacion
                    {
                        Id = r.Id,
                        Numero = r.NumeroIntervencion,
                        Tecnico = ResolverTecnico(r.IdUsuarioTecnico),
                        Inicio = r.FechaInicio,
                        Fin = r.FechaFin.HasValue ? r.FechaFin.Value.ToString("g") : T("Resultado.SinResultado"),
                        Estado = r.FechaFin.HasValue ? T("OrdenDetalle.Finalizada") : T("OrdenDetalle.Abierta")
                    });
                }

                DGV_Reparaciones.DataSource = new BindingList<FilaReparacion>(filas);

                SeleccionarReparacionPorDefecto();
                CargarComboConsumo();
                CargarConsumidosDeSeleccion();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void SeleccionarReparacionPorDefecto()
        {
            if (_reparaciones.Count == 0 || DGV_Reparaciones.Rows.Count == 0)
                return;

            int idSeleccion = 0;
            Reparacion abierta = null;

            foreach (Reparacion r in _reparaciones)
            {
                if (!r.FechaFin.HasValue && (abierta == null || r.NumeroIntervencion < abierta.NumeroIntervencion))
                    abierta = r;
            }

            if (abierta != null)
                idSeleccion = abierta.Id;
            else
                idSeleccion = _reparaciones[_reparaciones.Count - 1].Id;

            foreach (DataGridViewRow fila in DGV_Reparaciones.Rows)
            {
                FilaReparacion dato = fila.DataBoundItem as FilaReparacion;

                if (dato != null && dato.Id == idSeleccion)
                {
                    fila.Selected = true;
                    break;
                }
            }
        }

        private void CargarComboConsumo()
        {
            _cargandoCombos = true;

            try
            {
                List<ItemRepuestoConsumo> items = new List<ItemRepuestoConsumo>();

                try
                {
                    RepuestoService repuestoService = new RepuestoService();

                    foreach (Repuesto r in repuestoService.Listar(false))
                    {
                        if (r.StockActual <= 0)
                            continue;

                        items.Add(new ItemRepuestoConsumo
                        {
                            Id = r.Id,
                            Nombre = r.Codigo + " - " + r.Descripcion + " (" + T("Columna.Stock") + " " + r.StockActual + ")"
                        });
                    }
                }
                catch (Exception ex)
                {
                    MostrarError(ex);
                }

                CBO_ConsumoRepuesto.DataSource = null;
                CBO_ConsumoRepuesto.DisplayMember = "Nombre";
                CBO_ConsumoRepuesto.ValueMember = "Id";
                CBO_ConsumoRepuesto.DataSource = items;
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private Reparacion ReparacionSeleccionada()
        {
            if (_reparaciones == null || DGV_Reparaciones.SelectedRows.Count == 0)
                return null;

            FilaReparacion fila = DGV_Reparaciones.SelectedRows[0].DataBoundItem as FilaReparacion;

            if (fila == null)
                return null;

            foreach (Reparacion r in _reparaciones)
            {
                if (r.Id == fila.Id)
                    return r;
            }

            return null;
        }

        private string ResolverRepuesto(int idRepuesto)
        {
            if (_nombresRepuestos.ContainsKey(idRepuesto))
                return _nombresRepuestos[idRepuesto];

            return "#" + idRepuesto;
        }

        private void CargarConsumidosDeSeleccion()
        {
            Reparacion seleccionada = ReparacionSeleccionada();

            if (seleccionada == null)
            {
                DGV_Consumidos.DataSource = new BindingList<FilaConsumido>(new List<FilaConsumido>());
                LBL_CostoTotal.Text = T("OrdenDetalle.CostoTotal").Replace("{0}", "0.00");
                return;
            }

            try
            {
                List<ReparacionRepuesto> consumidos = _service.ListarConsumidos(seleccionada.Id);
                List<FilaConsumido> filas = new List<FilaConsumido>();
                decimal total = 0;

                foreach (ReparacionRepuesto c in consumidos)
                {
                    decimal subtotal = c.Cantidad * c.CostoUnitario;
                    total += subtotal;

                    filas.Add(new FilaConsumido
                    {
                        IdRepuesto = c.IdRepuesto,
                        Repuesto = ResolverRepuesto(c.IdRepuesto),
                        Cantidad = c.Cantidad,
                        Costo = c.CostoUnitario,
                        Subtotal = subtotal
                    });
                }

                DGV_Consumidos.DataSource = new BindingList<FilaConsumido>(filas);
                ConfigurarColumnasConsumidos();
                LBL_CostoTotal.Text = T("OrdenDetalle.CostoTotal").Replace("{0}", total.ToString("F2"));
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarTabPruebas()
        {
            _cargandoCombos = true;

            try
            {
                List<ItemIntervencion> items = new List<ItemIntervencion>();
                int idUltima = 0;

                if (_reparaciones != null)
                {
                    foreach (Reparacion r in _reparaciones)
                    {
                        items.Add(new ItemIntervencion
                        {
                            Id = r.Id,
                            Nombre = "N° " + r.NumeroIntervencion + " (" + r.FechaInicio.ToString("g") + ")"
                        });
                        idUltima = r.Id;
                    }
                }

                CBO_PruebaReparacion.DataSource = null;
                CBO_PruebaReparacion.DisplayMember = "Nombre";
                CBO_PruebaReparacion.ValueMember = "Id";
                CBO_PruebaReparacion.DataSource = items;

                if (idUltima > 0)
                    CBO_PruebaReparacion.SelectedValue = idUltima;
            }
            finally
            {
                _cargandoCombos = false;
            }

            CargarPruebasDeCombo();
        }

        private void CargarPruebasDeCombo()
        {
            if (CBO_PruebaReparacion.DataSource == null)
                return;

            int idReparacion = 0;

            if (CBO_PruebaReparacion.SelectedValue is int)
                idReparacion = (int)CBO_PruebaReparacion.SelectedValue;

            if (idReparacion <= 0)
            {
                DGV_Pruebas.DataSource = new BindingList<FilaPrueba>(new List<FilaPrueba>());
                return;
            }

            try
            {
                string intervencion = "";

                if (_reparaciones != null)
                {
                    foreach (Reparacion r in _reparaciones)
                    {
                        if (r.Id == idReparacion)
                        {
                            intervencion = "N° " + r.NumeroIntervencion;
                            break;
                        }
                    }
                }

                List<Prueba> pruebas = _service.ListarPruebas(idReparacion);
                List<FilaPrueba> filas = new List<FilaPrueba>();

                foreach (Prueba p in pruebas)
                {
                    filas.Add(new FilaPrueba
                    {
                        Fecha = p.Fecha,
                        Intervencion = intervencion,
                        Descripcion = p.Descripcion,
                        Resultado = p.Resultado == ResultadoPrueba.Aprobada ? T("OrdenDetalle.Aprobada") : T("OrdenDetalle.Fallida"),
                        Usuario = ResolverTecnico(p.IdUsuarioTecnico)
                    });
                }

                DGV_Pruebas.DataSource = new BindingList<FilaPrueba>(filas);
                ConfigurarColumnasPruebas();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void ConfigurarColumnasReparaciones()
        {
            if (DGV_Reparaciones.Columns.Count == 0)
                return;

            if (DGV_Reparaciones.Columns.Contains("Id"))
                DGV_Reparaciones.Columns["Id"].Visible = false;
            ConfigurarColumnaGrid(DGV_Reparaciones, "Numero", "Columna.Numero");
            ConfigurarColumnaGrid(DGV_Reparaciones, "Tecnico", "Columna.Tecnico");
            ConfigurarColumnaGrid(DGV_Reparaciones, "Inicio", "Columna.Inicio");
            ConfigurarColumnaGrid(DGV_Reparaciones, "Fin", "Columna.Fin");
            ConfigurarColumnaGrid(DGV_Reparaciones, "Estado", "Columna.Estado");

            DGV_Reparaciones.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumnasConsumidos()
        {
            if (DGV_Consumidos.Columns.Count == 0)
                return;

            ConfigurarColumnaGrid(DGV_Consumidos, "Repuesto", "Columna.Repuesto");
            ConfigurarColumnaGrid(DGV_Consumidos, "Cantidad", "Columna.Cantidad");
            ConfigurarColumnaGrid(DGV_Consumidos, "Costo", "Columna.Costo");
            ConfigurarColumnaGrid(DGV_Consumidos, "Subtotal", "Columna.Subtotal");

            if (DGV_Consumidos.Columns.Contains("IdRepuesto"))
                DGV_Consumidos.Columns["IdRepuesto"].Visible = false;

            DGV_Consumidos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumnasPruebas()
        {
            if (DGV_Pruebas.Columns.Count == 0)
                return;

            ConfigurarColumnaGrid(DGV_Pruebas, "Fecha", "Columna.Fecha");
            ConfigurarColumnaGrid(DGV_Pruebas, "Intervencion", "Columna.Intervencion");
            ConfigurarColumnaGrid(DGV_Pruebas, "Descripcion", "Columna.Descripcion");
            ConfigurarColumnaGrid(DGV_Pruebas, "Resultado", "Columna.Resultado");
            ConfigurarColumnaGrid(DGV_Pruebas, "Usuario", "Columna.Usuario");

            DGV_Pruebas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumnaGrid(DataGridView grilla, string nombreColumna, string claveTraduccion)
        {
            if (!grilla.Columns.Contains(nombreColumna))
                return;

            if (string.IsNullOrEmpty(claveTraduccion))
                return;

            grilla.Columns[nombreColumna].Tag = claveTraduccion;
            grilla.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
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
                LBL_EstadoPresupuesto.Text = T("OrdenDetalle.EstadoPresupuesto") + ": "
                    + TraducirTipoPresupuesto(_presupuesto.Tipo) + " - " + TraducirEstadoPresupuesto(_presupuesto.Estado);
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

        private string TraducirTipoPresupuesto(string tipo)
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
                TAB_Reparaciones.Enabled = true;
                TAB_Pruebas.Enabled = true;
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

            if (_presupuesto != null)
                return;

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
                AplicarPermisosDetalle();
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

            if (_presupuesto != null)
                return;

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
            AplicarPermisosDetalle();
        }

        private void BTN_Emitir_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (_presupuesto != null && _presupuesto.Estado == EstadoPresupuesto.Borrador)
            {
                try
                {
                    _service.PublicarBorrador(_presupuesto.Id);
                    _idSeleccionPreferida = _presupuesto.Id;
                    CargarOrden();
                    MostrarExito("Mensaje.OperacionExitosa");
                }
                catch (Exception ex)
                {
                    MostrarError(ex);
                }

                return;
            }

            if (_presupuesto != null)
                return;

            if (_itemsNuevo.Count == 0)
            {
                MostrarAdvertencia("Mensaje.ItemCamposObligatorios");
                return;
            }

            try
            {
                if (_modoNuevoAdicional)
                    _service.EmitirAdicional(_idOrden, new List<DetallePresupuesto>(_itemsNuevo),
                        NUM_PDescuento.Value, (int)NUM_PGarantia.Value, TXT_PObs.Text.Trim());
                else
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

        private void BTN_GuardarBorrador_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (_presupuesto != null)
                return;

            if (_itemsNuevo.Count == 0)
            {
                MostrarAdvertencia("Mensaje.ItemCamposObligatorios");
                return;
            }

            string tipo = _modoNuevoAdicional ? TipoPresupuesto.Adicional : TipoPresupuesto.Original;

            try
            {
                Presupuesto borrador = _service.CrearBorrador(_idOrden, tipo,
                    new List<DetallePresupuesto>(_itemsNuevo),
                    NUM_PDescuento.Value, (int)NUM_PGarantia.Value, TXT_PObs.Text.Trim());

                _itemsNuevo.Clear();
                TXT_PDesc.Text = "";
                NUM_PCant.Value = 1;
                NUM_PPrecio.Value = 0;
                _idSeleccionPreferida = borrador.Id;
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_EliminarBorrador_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (_presupuesto == null || _presupuesto.Estado != EstadoPresupuesto.Borrador)
                return;

            DialogResult confirmacion = MessageBox.Show(
                T("Mensaje.ConfirmarEliminarBorrador"),
                T("Titulo.ConfirmarEliminacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
                return;

            try
            {
                _service.EliminarBorrador(_presupuesto.Id);
                _idSeleccionPreferida = null;
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Anular_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.PresupuestosDecidir))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (_presupuesto == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (_presupuesto.Estado == EstadoPresupuesto.Borrador
                || _presupuesto.Estado == EstadoPresupuesto.Anulado)
                return;

            string motivo = PedirTexto(
                T("OrdenDetalle.Anular"),
                T("OrdenDetalle.MotivoAnulacion"));

            if (motivo == null)
                return;

            if (string.IsNullOrWhiteSpace(motivo))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                T("Mensaje.ConfirmarAnular"),
                T("Titulo.ConfirmarAnulacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
                return;

            try
            {
                _service.AnularPresupuesto(_presupuesto.Id, motivo.Trim());
                _idSeleccionPreferida = _presupuesto.Id;
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_CancelarSolicitud_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                T("Mensaje.ConfirmarCancelarSolicitud"),
                T("Titulo.ConfirmarCancelacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
                return;

            try
            {
                _service.CancelarSolicitudAdicional(_idOrden);
                _idSeleccionPreferida = null;
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

            if (_presupuesto == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            try
            {
                if (_presupuesto.Tipo == TipoPresupuesto.Adicional)
                    _service.RegistrarAprobacionAdicional(_presupuesto.Id, TXT_PMedio.Text.Trim(), TXT_PObs.Text.Trim());
                else
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

            if (_presupuesto == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (string.IsNullOrWhiteSpace(TXT_PMotivo.Text))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                if (_presupuesto.Tipo == TipoPresupuesto.Adicional)
                    _service.RegistrarRechazoAdicional(_presupuesto.Id, TXT_PMotivo.Text.Trim(),
                        TXT_PMedio.Text.Trim(), TXT_PObs.Text.Trim());
                else
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

        private void BTN_SolicitarAdicional_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            string motivo = PedirTexto(
                T("OrdenDetalle.SolicitarAdicional"),
                T("OrdenDetalle.Motivo"));

            if (motivo == null)
                return;

            if (string.IsNullOrWhiteSpace(motivo))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                _service.SolicitarAdicional(_idOrden, motivo.Trim());
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private string PedirTexto(string titulo, string etiqueta)
        {
            using (Form dialogo = new Form())
            {
                dialogo.Text = titulo;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.MaximizeBox = false;
                dialogo.MinimizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.ClientSize = new System.Drawing.Size(400, 150);

                Label lbl = new Label();
                lbl.Text = etiqueta;
                lbl.AutoSize = true;
                lbl.Location = new System.Drawing.Point(12, 12);
                dialogo.Controls.Add(lbl);

                TextBox txt = new TextBox();
                txt.Multiline = true;
                txt.Location = new System.Drawing.Point(12, 34);
                txt.Size = new System.Drawing.Size(376, 66);
                dialogo.Controls.Add(txt);

                Button btnAceptar = new Button();
                btnAceptar.Text = T("Accion.Aceptar");
                btnAceptar.DialogResult = DialogResult.OK;
                btnAceptar.Location = new System.Drawing.Point(212, 108);
                btnAceptar.Size = new System.Drawing.Size(85, 28);
                dialogo.Controls.Add(btnAceptar);

                Button btnCancelar = new Button();
                btnCancelar.Text = T("Accion.Cancelar");
                btnCancelar.DialogResult = DialogResult.Cancel;
                btnCancelar.Location = new System.Drawing.Point(303, 108);
                btnCancelar.Size = new System.Drawing.Size(85, 28);
                dialogo.Controls.Add(btnCancelar);

                dialogo.AcceptButton = btnAceptar;
                dialogo.CancelButton = btnCancelar;

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                    return null;

                return txt.Text;
            }
        }

        private int PedirCantidad(string titulo, string etiqueta, int maximo)
        {
            using (Form dialogo = new Form())
            {
                dialogo.Text = titulo;
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.MaximizeBox = false;
                dialogo.MinimizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.ClientSize = new System.Drawing.Size(300, 110);

                Label lbl = new Label();
                lbl.Text = etiqueta;
                lbl.AutoSize = true;
                lbl.Location = new System.Drawing.Point(12, 12);
                dialogo.Controls.Add(lbl);

                NumericUpDown num = new NumericUpDown();
                num.Minimum = 1;
                num.Maximum = maximo;
                num.Value = maximo;
                num.Location = new System.Drawing.Point(12, 34);
                num.Size = new System.Drawing.Size(276, 22);
                dialogo.Controls.Add(num);

                Button btnAceptar = new Button();
                btnAceptar.Text = T("Accion.Aceptar");
                btnAceptar.DialogResult = DialogResult.OK;
                btnAceptar.Location = new System.Drawing.Point(112, 68);
                btnAceptar.Size = new System.Drawing.Size(85, 28);
                dialogo.Controls.Add(btnAceptar);

                Button btnCancelar = new Button();
                btnCancelar.Text = T("Accion.Cancelar");
                btnCancelar.DialogResult = DialogResult.Cancel;
                btnCancelar.Location = new System.Drawing.Point(203, 68);
                btnCancelar.Size = new System.Drawing.Size(85, 28);
                dialogo.Controls.Add(btnCancelar);

                dialogo.AcceptButton = btnAceptar;
                dialogo.CancelButton = btnCancelar;

                if (dialogo.ShowDialog(this) != DialogResult.OK)
                    return -1;

                return (int)num.Value;
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

        private void DGV_Reparaciones_SelectionChanged(object sender, EventArgs e)
        {
            if (_esNuevo || _reparaciones == null)
                return;

            CargarConsumidosDeSeleccion();
            AplicarPermisosDetalle();
        }

        private void CBO_PruebaReparacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_PruebaReparacion.DataSource == null)
                return;

            if (_esNuevo)
                return;

            CargarPruebasDeCombo();
        }

        private void BTN_IniciarReparacion_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            try
            {
                _service.IniciarReparacion(_idOrden);
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Consumir_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Reparacion seleccionada = ReparacionSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            int idRepuesto = 0;

            if (CBO_ConsumoRepuesto.SelectedValue is int)
                idRepuesto = (int)CBO_ConsumoRepuesto.SelectedValue;

            if (idRepuesto <= 0)
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                _service.ConsumirRepuesto(seleccionada.Id, idRepuesto, (int)NUM_ConsumoCantidad.Value);
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_QuitarConsumo_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Reparacion seleccionada = ReparacionSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (DGV_Consumidos.SelectedRows.Count == 0)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            FilaConsumido fila = DGV_Consumidos.SelectedRows[0].DataBoundItem as FilaConsumido;

            if (fila == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            int cantidad = PedirCantidad(
                T("OrdenDetalle.QuitarConsumo"),
                T("OrdenDetalle.CantidadDevolver"),
                fila.Cantidad);

            if (cantidad <= 0)
                return;

            DialogResult confirmacion = MessageBox.Show(
                T("Mensaje.ConfirmarDevolucion").Replace("{0}", cantidad.ToString()).Replace("{1}", fila.Repuesto),
                T("Titulo.ConfirmarDevolucion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
                return;

            try
            {
                _service.QuitarConsumo(seleccionada.Id, fila.IdRepuesto, cantidad);
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_FinalizarReparacion_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Reparacion seleccionada = ReparacionSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (string.IsNullOrWhiteSpace(TXT_FTrabajo.Text))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                _service.FinalizarReparacion(seleccionada.Id, TXT_FTrabajo.Text.Trim(), TXT_FObs.Text.Trim());
                TXT_FTrabajo.Text = "";
                TXT_FObs.Text = "";
                CargarOrden();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_RegistrarPrueba_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            int idReparacion = 0;

            if (CBO_PruebaReparacion.SelectedValue is int)
                idReparacion = (int)CBO_PruebaReparacion.SelectedValue;

            if (idReparacion <= 0)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (string.IsNullOrWhiteSpace(TXT_PruebaDesc.Text))
            {
                MostrarAdvertencia("Mensaje.PruebaCamposObligatorios");
                return;
            }

            try
            {
                _service.RegistrarPrueba(idReparacion, TXT_PruebaDesc.Text.Trim(),
                    RDO_Aprobada.Checked, TXT_PruebaObs.Text.Trim());
                TXT_PruebaDesc.Text = "";
                TXT_PruebaObs.Text = "";
                RDO_Aprobada.Checked = true;
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
                && _presupuesto == null
                && (_modoNuevoAdicional || (_original == null && !_puedeCancelarSolicitud));

            bool esBorrador = !_esNuevo && _presupuesto != null
                && _presupuesto.Estado == EstadoPresupuesto.Borrador;
            bool esAnulable = !_esNuevo && _presupuesto != null
                && (_presupuesto.Estado == EstadoPresupuesto.Pendiente
                    || _presupuesto.Estado == EstadoPresupuesto.Aprobado
                    || _presupuesto.Estado == EstadoPresupuesto.Rechazado);
            bool ordenPendientePresupuesto = _orden != null
                && _orden.Estado == EstadoOrdenServicio.PendientePresupuesto;

            CBO_SelectorPresupuesto.Enabled = !_esNuevo && !entregado;
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

            bool puedePublicar = editable && esBorrador && ordenPendientePresupuesto;

            BTN_Emitir.Visible = !_esNuevo && puedeEditar;
            BTN_Emitir.Enabled = (editaItems && _itemsNuevo.Count > 0) || puedePublicar;
            BTN_GuardarBorrador.Visible = !_esNuevo && puedeEditar;
            BTN_GuardarBorrador.Enabled = editaItems && _itemsNuevo.Count > 0;
            BTN_EliminarBorrador.Visible = !_esNuevo && puedeEditar && esBorrador;
            BTN_EliminarBorrador.Enabled = editable && esBorrador;

            bool presupuestoPendiente = !_esNuevo && _presupuesto != null
                && _presupuesto.Estado == EstadoPresupuesto.Pendiente
                && _orden != null && _orden.Estado == EstadoOrdenServicio.EsperandoRespuesta;

            bool puedeSolicitar = editable && _orden != null
                && (_orden.Estado == EstadoOrdenServicio.EnReparacion
                    || _orden.Estado == EstadoOrdenServicio.EnPruebas)
                && _original != null && _original.Estado == EstadoPresupuesto.Aprobado
                && !_hayAdicionalPendiente;

            BTN_SolicitarAdicional.Visible = !_esNuevo && puedeEditar;
            BTN_SolicitarAdicional.Enabled = puedeSolicitar;
            BTN_CancelarSolicitud.Visible = !_esNuevo && puedeEditar && _puedeCancelarSolicitud;
            BTN_CancelarSolicitud.Enabled = editable && _puedeCancelarSolicitud;

            TXT_PMedio.Enabled = !_esNuevo && !entregado && (editaItems || (puedeDecidir && presupuestoPendiente));
            TXT_PMotivo.Enabled = !_esNuevo && !entregado && puedeDecidir && presupuestoPendiente;
            TXT_PObs.Enabled = !_esNuevo && !entregado && (editaItems || (puedeDecidir && presupuestoPendiente));
            BTN_Aprobar.Visible = !_esNuevo && puedeDecidir;
            BTN_Aprobar.Enabled = puedeDecidir && presupuestoPendiente;
            BTN_Rechazar.Visible = !_esNuevo && puedeDecidir;
            BTN_Rechazar.Enabled = puedeDecidir && presupuestoPendiente;
            BTN_Anular.Visible = !_esNuevo && puedeDecidir && esAnulable;
            BTN_Anular.Enabled = puedeDecidir && esAnulable && !entregado;

            bool puedeEntregarAhora = editable && puedeEntregar && _orden != null
                && _orden.Estado == EstadoOrdenServicio.ListoRetiro
                && _entrega == null;

            TXT_EEntregadoA.Enabled = puedeEntregarAhora;
            TXT_EDocumento.Enabled = puedeEntregarAhora;
            TXT_EObs.Enabled = puedeEntregarAhora;
            BTN_Entregar.Visible = !_esNuevo && puedeEntregar;
            BTN_Entregar.Enabled = puedeEntregarAhora;

            Reparacion reparacionSeleccionada = ReparacionSeleccionada();
            bool hayAbiertaSeleccionada = reparacionSeleccionada != null && !reparacionSeleccionada.FechaFin.HasValue;

            bool puedeIniciarReparacion = editable && _orden != null
                && (_orden.Estado == EstadoOrdenServicio.AutorizadoReparacion
                    || _orden.Estado == EstadoOrdenServicio.EnReparacion);
            bool habilitaConsumo = editable && _orden != null
                && _orden.Estado == EstadoOrdenServicio.EnReparacion
                && hayAbiertaSeleccionada;
            bool habilitaPrueba = editable && _orden != null
                && _orden.Estado == EstadoOrdenServicio.EnPruebas;

            BTN_IniciarReparacion.Visible = !_esNuevo && puedeEditar;
            BTN_IniciarReparacion.Enabled = puedeIniciarReparacion;
            CBO_ConsumoRepuesto.Enabled = habilitaConsumo;
            NUM_ConsumoCantidad.Enabled = habilitaConsumo;
            BTN_Consumir.Visible = !_esNuevo && puedeEditar;
            BTN_Consumir.Enabled = habilitaConsumo;
            BTN_QuitarConsumo.Visible = !_esNuevo && puedeEditar;
            BTN_QuitarConsumo.Enabled = habilitaConsumo;
            TXT_FTrabajo.Enabled = habilitaConsumo;
            TXT_FObs.Enabled = habilitaConsumo;
            BTN_FinalizarReparacion.Visible = !_esNuevo && puedeEditar;
            BTN_FinalizarReparacion.Enabled = habilitaConsumo;
            CBO_PruebaReparacion.Enabled = habilitaPrueba;
            TXT_PruebaDesc.Enabled = habilitaPrueba;
            RDO_Aprobada.Enabled = habilitaPrueba;
            RDO_Fallida.Enabled = habilitaPrueba;
            TXT_PruebaObs.Enabled = habilitaPrueba;
            BTN_RegistrarPrueba.Visible = !_esNuevo && puedeEditar;
            BTN_RegistrarPrueba.Enabled = habilitaPrueba;
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
