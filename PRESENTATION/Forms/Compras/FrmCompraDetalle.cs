using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Compras;
using APPLICATION.Features.Proveedores;
using APPLICATION.Features.Repuestos;
using DOMAIN.Features.Compras;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Proveedores;
using DOMAIN.Features.Repuestos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Compras
{
    // Dialogo modal: modo nuevo (idCompra 0, arma items en memoria + Guardar
    // borrador / Confirmar) o modo detalle (Borrador editable contra el service,
    // Confirmada editable solo via Anular con motivo, Cancelada solo lectura).
    // Cancelar borrador pasa a Cancelada persistente (conserva cabecera + detalle).
    // Permiso COMPRAS_CANCELAR cubre Cancelar borrador y Anular confirmada.
    public partial class FrmCompraDetalle : Form, IObservador
    {
        private class ItemProveedor
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class ItemRepuesto
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class FilaDetalle
        {
            public int Id { get; set; }
            public int IdRepuesto { get; set; }
            public string Repuesto { get; set; }
            public int Cantidad { get; set; }
            public decimal Costo { get; set; }
            public decimal Subtotal { get; set; }
        }

        private readonly SesionIdioma _sesionIdioma;
        private readonly CompraService _service;
        private readonly ProveedorService _proveedorService;
        private readonly RepuestoService _repuestoService;
        private int _idCompra;
        private bool _esNuevo;
        private Compra _compra = null;
        private List<DetalleCompra> _itemsNuevo = new List<DetalleCompra>();
        private List<DetalleCompra> _detalleActual = new List<DetalleCompra>();
        private List<Proveedor> _proveedores = new List<Proveedor>();
        private Dictionary<int, string> _nombresRepuestos = new Dictionary<int, string>();

        public FrmCompraDetalle()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new CompraService();
            _proveedorService = new ProveedorService();
            _repuestoService = new RepuestoService();
            _idCompra = 0;
            _esNuevo = true;
            InitializeComponent();
        }

        public FrmCompraDetalle(int idCompra)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new CompraService();
            _proveedorService = new ProveedorService();
            _repuestoService = new RepuestoService();
            _idCompra = idCompra;
            _esNuevo = idCompra <= 0;
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            ActualizarTextos();
            CargarComboProveedores();
            CargarComboRepuestos();
            CargarCompra();
        }

        private void FrmCompraDetalle_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);

            if (_esNuevo && !TienePermiso(CodigosPermiso.ComprasCrear))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            if (!_esNuevo && !TienePermiso(CodigosPermiso.ComprasVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarCatalogos();
            ActualizarTextos();
            CargarCompra();
        }

        private void ActualizarTextos()
        {
            if (_sesionIdioma.idioma == null)
                return;

            IIdioma idioma = _sesionIdioma.idioma;
            string claveTitulo = _esNuevo ? "CompraDetalle.TituloNuevo" : "CompraDetalle.TituloDetalle";
            Tag = claveTitulo;
            Text = idioma.BuscarTraduccion(Tag.ToString());

            string idHeader = _esNuevo ? "0" : _idCompra.ToString();
            LBL_Numero.Text = idioma.BuscarTraduccion(LBL_Numero.Tag.ToString()).Replace("{0}", idHeader);
            LBL_Proveedor.Text = idioma.BuscarTraduccion(LBL_Proveedor.Tag.ToString());
            LBL_Fecha.Text = idioma.BuscarTraduccion(LBL_Fecha.Tag.ToString());
            LBL_Estado.Text = idioma.BuscarTraduccion(LBL_Estado.Tag.ToString());
            LBL_Total.Text = idioma.BuscarTraduccion(LBL_Total.Tag.ToString());
            LBL_Obs.Text = idioma.BuscarTraduccion(LBL_Obs.Tag.ToString());
            LBL_Repuesto.Text = idioma.BuscarTraduccion(LBL_Repuesto.Tag.ToString());
            LBL_Cantidad.Text = idioma.BuscarTraduccion(LBL_Cantidad.Tag.ToString());
            LBL_Costo.Text = idioma.BuscarTraduccion(LBL_Costo.Tag.ToString());
            BTN_AgregarItem.Text = idioma.BuscarTraduccion(BTN_AgregarItem.Tag.ToString());
            BTN_QuitarItem.Text = idioma.BuscarTraduccion(BTN_QuitarItem.Tag.ToString());
            BTN_GuardarBorrador.Text = idioma.BuscarTraduccion(BTN_GuardarBorrador.Tag.ToString());
            BTN_Confirmar.Text = idioma.BuscarTraduccion(BTN_Confirmar.Tag.ToString());
            BTN_CancelarCompra.Text = idioma.BuscarTraduccion(BTN_CancelarCompra.Tag.ToString());
            BTN_AnularCompra.Text = idioma.BuscarTraduccion(BTN_AnularCompra.Tag.ToString());
            BTN_Cerrar.Text = idioma.BuscarTraduccion(BTN_Cerrar.Tag.ToString());
            LBL_Motivo.Text = idioma.BuscarTraduccion(LBL_Motivo.Tag.ToString());
        }

        private void CargarCatalogos()
        {
            try
            {
                _proveedores = _proveedorService.Listar(true);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }

            CargarComboProveedores();
            CargarComboRepuestos();
        }

        private void CargarComboProveedores()
        {
            int seleccionado = 0;

            if (CBO_Proveedor.SelectedValue is int && (int)CBO_Proveedor.SelectedValue > 0)
                seleccionado = (int)CBO_Proveedor.SelectedValue;
            else if (!_esNuevo && _compra != null)
                seleccionado = _compra.IdProveedor;

            List<ItemProveedor> items = new List<ItemProveedor>();

            foreach (Proveedor p in _proveedores)
            {
                if (!p.Activo && (seleccionado <= 0 || p.Id != seleccionado))
                    continue;

                items.Add(new ItemProveedor { Id = p.Id, Nombre = p.RazonSocial });
            }

            CBO_Proveedor.DataSource = null;
            CBO_Proveedor.DisplayMember = "Nombre";
            CBO_Proveedor.ValueMember = "Id";
            CBO_Proveedor.DataSource = items;

            if (seleccionado > 0)
                CBO_Proveedor.SelectedValue = seleccionado;
        }

        private void CargarComboRepuestos()
        {
            List<ItemRepuesto> items = new List<ItemRepuesto>();
            _nombresRepuestos.Clear();

            try
            {
                foreach (Repuesto r in _repuestoService.Listar(false))
                {
                    string nombre = r.Codigo + " - " + r.Descripcion;
                    _nombresRepuestos[r.Id] = nombre;
                    items.Add(new ItemRepuesto { Id = r.Id, Nombre = nombre });
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }

            CBO_Repuesto.DataSource = null;
            CBO_Repuesto.DisplayMember = "Nombre";
            CBO_Repuesto.ValueMember = "Id";
            CBO_Repuesto.DataSource = items;
        }

        private void CargarCompra()
        {
            if (_esNuevo)
            {
                CargarModoNuevo();
                return;
            }

            try
            {
                _compra = _service.ObtenerPorId(_idCompra);
                _detalleActual = _service.ListarDetalle(_idCompra);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
                Close();
                return;
            }

            CargarComboProveedores();

            if (_sesionIdioma.idioma != null)
                LBL_Numero.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Numero.Tag.ToString()).Replace("{0}", _compra.Id.ToString());
            LBL_FechaValor.Text = _compra.Fecha.ToString("g");
            LBL_EstadoValor.Text = TraducirEstado(_compra.Estado);
            LBL_TotalValor.Text = _compra.Total.ToString("F2");
            TXT_Obs.Text = _compra.Observaciones;
            TXT_Obs.Enabled = false;
            CBO_Proveedor.Enabled = false;

            // Motivo visible solo si la compra fue anulada (Confirmada->Cancelada con motivo).
            bool anulada = _compra.Estado == EstadoCompra.Cancelada
                && !string.IsNullOrWhiteSpace(_compra.MotivoAnulacion);
            LBL_Motivo.Visible = anulada;
            TXT_Motivo.Visible = anulada;
            TXT_Motivo.Text = anulada ? _compra.MotivoAnulacion : "";

            MostrarDetalle(_detalleActual);
            AplicarPermisos();
        }

        private void CargarModoNuevo()
        {
            _compra = null;
            LBL_Numero.Text = _sesionIdioma.idioma == null ? "Compra" : _sesionIdioma.idioma.BuscarTraduccion(LBL_Numero.Tag.ToString()).Replace("{0}", "0");
            LBL_FechaValor.Text = DateTime.Now.ToString("g");
            LBL_EstadoValor.Text = TraducirEstado(EstadoCompra.Borrador);
            CBO_Proveedor.Enabled = true;
            TXT_Obs.Enabled = true;
            LBL_Motivo.Visible = false;
            TXT_Motivo.Visible = false;
            TXT_Motivo.Text = "";

            MostrarDetalle(_itemsNuevo);
            AplicarPermisos();
        }

        private void MostrarDetalle(List<DetalleCompra> detalle)
        {
            List<FilaDetalle> filas = new List<FilaDetalle>();
            decimal total = 0;

            foreach (DetalleCompra d in detalle)
            {
                filas.Add(new FilaDetalle
                {
                    Id = d.Id,
                    IdRepuesto = d.IdRepuesto,
                    Repuesto = ResolverRepuesto(d.IdRepuesto),
                    Cantidad = d.Cantidad,
                    Costo = d.CostoUnitario,
                    Subtotal = d.Subtotal
                });
                total += d.Subtotal;
            }

            DGV_Detalle.DataSource = new BindingList<FilaDetalle>(filas);
            ConfigurarColumnas();

            if (_esNuevo)
                LBL_TotalValor.Text = total.ToString("F2");
        }

        private string ResolverRepuesto(int idRepuesto)
        {
            if (_nombresRepuestos.ContainsKey(idRepuesto))
                return _nombresRepuestos[idRepuesto];

            return "#" + idRepuesto;
        }

        private string TraducirEstado(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return "";

            if (_sesionIdioma.idioma == null)
                return estado;

            return _sesionIdioma.idioma.BuscarTraduccion("CompraEstado." + estado);
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Detalle.Columns.Count == 0)
                return;

            ConfigurarColumna("Repuesto", "Columna.Repuesto");
            ConfigurarColumna("Cantidad", "Columna.Cantidad");
            ConfigurarColumna("Costo", "Columna.Costo");
            ConfigurarColumna("Subtotal", "Columna.Subtotal");

            if (DGV_Detalle.Columns.Contains("Id"))
                DGV_Detalle.Columns["Id"].Visible = false;
            if (DGV_Detalle.Columns.Contains("IdRepuesto"))
                DGV_Detalle.Columns["IdRepuesto"].Visible = false;

            DGV_Detalle.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Detalle.Columns.Contains(nombreColumna))
                return;

            DGV_Detalle.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Detalle.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private bool EsBorrador()
        {
            return _esNuevo || (_compra != null && _compra.Estado == EstadoCompra.Borrador);
        }

        private void BTN_AgregarItem_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ComprasCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (!EsBorrador())
                return;

            int idRepuesto = 0;

            if (CBO_Repuesto.SelectedValue is int)
                idRepuesto = (int)CBO_Repuesto.SelectedValue;

            if (idRepuesto <= 0)
            {
                MostrarAdvertencia("Mensaje.CompraCamposObligatorios");
                return;
            }

            try
            {
                if (_esNuevo)
                {
                    DetalleCompra item = DetalleCompra.CrearNuevo(0, idRepuesto,
                        (int)NUM_Cantidad.Value, NUM_Costo.Value);
                    _itemsNuevo.Add(item);
                    NUM_Cantidad.Value = 1;
                    NUM_Costo.Value = 0;
                    MostrarDetalle(_itemsNuevo);
                    AplicarPermisos();
                }
                else
                {
                    _service.AgregarItem(_idCompra, idRepuesto,
                        (int)NUM_Cantidad.Value, NUM_Costo.Value);
                    NUM_Cantidad.Value = 1;
                    NUM_Costo.Value = 0;
                    CargarCompra();
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_QuitarItem_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ComprasCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (!EsBorrador())
                return;

            if (DGV_Detalle.SelectedRows.Count == 0)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            FilaDetalle fila = DGV_Detalle.SelectedRows[0].DataBoundItem as FilaDetalle;

            if (fila == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            try
            {
                if (_esNuevo)
                {
                    DetalleCompra quitar = null;

                    foreach (DetalleCompra d in _itemsNuevo)
                    {
                        if (d.IdRepuesto == fila.IdRepuesto && d.Cantidad == fila.Cantidad
                            && d.CostoUnitario == fila.Costo)
                        {
                            quitar = d;
                            break;
                        }
                    }

                    if (quitar != null)
                        _itemsNuevo.Remove(quitar);

                    MostrarDetalle(_itemsNuevo);
                    AplicarPermisos();
                }
                else
                {
                    _service.QuitarItem(_idCompra, fila.Id);
                    CargarCompra();
                }
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private bool ValidarNuevo(out int idProveedor)
        {
            idProveedor = 0;

            if (CBO_Proveedor.SelectedValue is int)
                idProveedor = (int)CBO_Proveedor.SelectedValue;

            if (idProveedor <= 0 || _itemsNuevo.Count == 0)
            {
                MostrarAdvertencia("Mensaje.CompraCamposObligatorios");
                return false;
            }

            return true;
        }

        private void BTN_GuardarBorrador_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ComprasCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (!_esNuevo)
                return;

            int idProveedor;

            if (!ValidarNuevo(out idProveedor))
                return;

            try
            {
                Compra creada = _service.CrearBorrador(idProveedor,
                    new List<DetalleCompra>(_itemsNuevo), TXT_Obs.Text.Trim());
                _idCompra = creada.Id;
                _esNuevo = false;
                _itemsNuevo.Clear();
                ActualizarTextos();
                CargarCompra();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Confirmar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ComprasCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (!EsBorrador())
                return;

            if (!Confirmar("Mensaje.ConfirmarConfirmacion", "Titulo.ConfirmarConfirmacion"))
                return;

            try
            {
                if (_esNuevo)
                {
                    int idProveedor;

                    if (!ValidarNuevo(out idProveedor))
                        return;

                    Compra creada = _service.CrearBorrador(idProveedor,
                        new List<DetalleCompra>(_itemsNuevo), TXT_Obs.Text.Trim());
                    _idCompra = creada.Id;
                    _esNuevo = false;
                    _itemsNuevo.Clear();
                    ActualizarTextos();
                }

                _service.Confirmar(_idCompra);
                CargarCompra();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_CancelarCompra_Click(object sender, EventArgs e)
        {
            // Cancelar borrador -> Cancelada persistente (ya no elimina el borrador fisico).
            if (!TienePermiso(CodigosPermiso.ComprasCancelar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (_esNuevo || _compra == null || _compra.Estado != EstadoCompra.Borrador)
                return;

            if (!Confirmar("Mensaje.ConfirmarCancelarCompra", "Titulo.ConfirmarCancelacion"))
                return;

            try
            {
                _service.Cancelar(_idCompra);
                CargarCompra();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_AnularCompra_Click(object sender, EventArgs e)
        {
            // Anular confirmada con motivo obligatorio -> Cancelada + reversion de stock.
            if (!TienePermiso(CodigosPermiso.ComprasCancelar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (_esNuevo || _compra == null || _compra.Estado != EstadoCompra.Confirmada)
            {
                MostrarAdvertencia("Mensaje.CompraNoAnulable");
                return;
            }

            string motivo = PedirMotivo();

            if (motivo == null)
                return;

            if (string.IsNullOrWhiteSpace(motivo))
            {
                MostrarAdvertencia("Mensaje.CompraMotivoObligatorio");
                return;
            }

            if (!Confirmar("Mensaje.ConfirmarAnularCompra", "Titulo.ConfirmarAnulacion"))
                return;

            try
            {
                _service.Anular(_idCompra, motivo.Trim());
                CargarCompra();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private string PedirMotivo()
        {
            // Dialogo simple con TextBox (sin ComboBox): motivo obligatorio para anular.
            using (Form dialogo = new Form())
            {
                dialogo.Text = _sesionIdioma.idioma == null
                    ? "Anular"
                    : _sesionIdioma.idioma.BuscarTraduccion("CompraDetalle.Anular");
                dialogo.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialogo.StartPosition = FormStartPosition.CenterParent;
                dialogo.MaximizeBox = false;
                dialogo.MinimizeBox = false;
                dialogo.ShowInTaskbar = false;
                dialogo.ClientSize = new System.Drawing.Size(400, 150);

                Label lbl = new Label();
                lbl.Text = _sesionIdioma.idioma == null
                    ? "Motivo anulacion:"
                    : _sesionIdioma.idioma.BuscarTraduccion("CompraDetalle.MotivoAnulacion");
                lbl.AutoSize = true;
                lbl.Location = new System.Drawing.Point(12, 12);
                dialogo.Controls.Add(lbl);

                TextBox txt = new TextBox();
                txt.Multiline = true;
                txt.Location = new System.Drawing.Point(12, 34);
                txt.Size = new System.Drawing.Size(376, 66);
                dialogo.Controls.Add(txt);

                Button btnAceptar = new Button();
                btnAceptar.Text = _sesionIdioma.idioma == null
                    ? "Aceptar"
                    : _sesionIdioma.idioma.BuscarTraduccion("Accion.Aceptar");
                btnAceptar.DialogResult = DialogResult.OK;
                btnAceptar.Location = new System.Drawing.Point(212, 108);
                btnAceptar.Size = new System.Drawing.Size(85, 28);
                dialogo.Controls.Add(btnAceptar);

                Button btnCancelar = new Button();
                btnCancelar.Text = _sesionIdioma.idioma == null
                    ? "Cancelar"
                    : _sesionIdioma.idioma.BuscarTraduccion("Accion.Cancelar");
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

        private bool Confirmar(string claveMensaje, string claveTitulo)
        {
            DialogResult confirmacion = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion(claveMensaje),
                _sesionIdioma.idioma.BuscarTraduccion(claveTitulo),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            return confirmacion == DialogResult.Yes;
        }

        private void AplicarPermisos()
        {
            bool puedeCrear = TienePermiso(CodigosPermiso.ComprasCrear);
            bool puedeCancelar = TienePermiso(CodigosPermiso.ComprasCancelar);
            bool borrador = EsBorrador();
            bool confirmada = !_esNuevo && _compra != null && _compra.Estado == EstadoCompra.Confirmada;
            bool hayItems = DGV_Detalle.Rows.Count > 0;

            CBO_Repuesto.Enabled = borrador && puedeCrear;
            NUM_Cantidad.Enabled = borrador && puedeCrear;
            NUM_Costo.Enabled = borrador && puedeCrear;

            BTN_AgregarItem.Visible = puedeCrear;
            BTN_AgregarItem.Enabled = puedeCrear && borrador;
            BTN_QuitarItem.Visible = puedeCrear;
            BTN_QuitarItem.Enabled = puedeCrear && borrador && hayItems;

            BTN_GuardarBorrador.Visible = _esNuevo && puedeCrear;
            BTN_GuardarBorrador.Enabled = _esNuevo && puedeCrear && hayItems;

            BTN_Confirmar.Visible = puedeCrear;
            BTN_Confirmar.Enabled = puedeCrear && borrador && (!_esNuevo || hayItems);

            BTN_CancelarCompra.Visible = !_esNuevo && puedeCancelar;
            BTN_CancelarCompra.Enabled = !_esNuevo && puedeCancelar && borrador;

            // Anular: solo Confirmada con COMPRAS_CANCELAR; Cancelada terminal sin acciones.
            BTN_AnularCompra.Visible = !_esNuevo && puedeCancelar;
            BTN_AnularCompra.Enabled = !_esNuevo && puedeCancelar && confirmada;
        }

        private void DGV_Detalle_SelectionChanged(object sender, EventArgs e)
        {
            AplicarPermisos();
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

        private void MostrarExito(string claveMensaje)
        {
            MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion(claveMensaje),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void MostrarAdvertencia(string claveMensaje)
        {
            MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion(claveMensaje),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
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
