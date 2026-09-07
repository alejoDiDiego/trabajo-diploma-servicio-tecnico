using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Compras;
using APPLICATION.Features.Proveedores;
using DOMAIN.Features.Compras;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Proveedores;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Compras
{
    public partial class FrmCompras : Form, IObservador
    {
        private class FilaCompra
        {
            public int Id { get; set; }
            public int IdProveedor { get; set; }
            public int Numero { get; set; }
            public string Proveedor { get; set; }
            public DateTime Fecha { get; set; }
            public string EstadoRaw { get; set; }
            public string Estado { get; set; }
            public decimal Total { get; set; }
            public string Observaciones { get; set; }
        }

        private class ItemProveedor
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class ItemEstado
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }

        private static readonly string[] EstadosCompra = new string[]
        {
            EstadoCompra.Borrador,
            EstadoCompra.Confirmada,
            EstadoCompra.Cancelada
        };

        private BindingList<FilaCompra> _comprasBindingList = null;
        private List<Compra> _todas = new List<Compra>();
        private List<Proveedor> _proveedores = new List<Proveedor>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly CompraService _service;
        private readonly ProveedorService _proveedorService;
        private bool _cargandoCombos = false;

        public FrmCompras()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new CompraService();
            _proveedorService = new ProveedorService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Proveedor.Text = idiomaObservado.BuscarTraduccion(LBL_Proveedor.Tag.ToString());
            LBL_Estado.Text = idiomaObservado.BuscarTraduccion(LBL_Estado.Tag.ToString());
            BTN_Crear.Text = idiomaObservado.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Detalle.Text = idiomaObservado.BuscarTraduccion(BTN_Detalle.Tag.ToString());
            BTN_Confirmar.Text = idiomaObservado.BuscarTraduccion(BTN_Confirmar.Tag.ToString());
            BTN_Cancelar.Text = idiomaObservado.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
            BTN_Anular.Text = idiomaObservado.BuscarTraduccion(BTN_Anular.Tag.ToString());

            CargarComboProveedores();
            CargarComboEstados();
            AplicarFiltro();
            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void FrmCompras_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);

            if (!TienePermiso(CodigosPermiso.ComprasVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarProveedores();
            CargarComboProveedores();
            CargarComboEstados();
            ActualizarTextos();
            CargarCompras();
        }

        private void ActualizarTextos()
        {
            if (_sesionIdioma.idioma == null)
                return;

            Text = _sesionIdioma.idioma.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Proveedor.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Proveedor.Tag.ToString());
            LBL_Estado.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Estado.Tag.ToString());
            BTN_Crear.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Detalle.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Detalle.Tag.ToString());
            BTN_Confirmar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Confirmar.Tag.ToString());
            BTN_Cancelar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
            BTN_Anular.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Anular.Tag.ToString());
        }

        private void CargarProveedores()
        {
            try
            {
                _proveedores = _proveedorService.Listar(true);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarComboProveedores()
        {
            _cargandoCombos = true;

            try
            {
                string todos = _sesionIdioma.idioma != null
                    ? _sesionIdioma.idioma.BuscarTraduccion("Compras.Todos")
                    : "Todos";

                List<ItemProveedor> items = new List<ItemProveedor>();
                items.Add(new ItemProveedor { Id = 0, Nombre = todos });

                foreach (Proveedor p in _proveedores)
                    items.Add(new ItemProveedor { Id = p.Id, Nombre = p.RazonSocial });

                int seleccionado = 0;

                if (CBO_Proveedor.SelectedValue is int)
                    seleccionado = (int)CBO_Proveedor.SelectedValue;

                CBO_Proveedor.DataSource = null;
                CBO_Proveedor.DisplayMember = "Nombre";
                CBO_Proveedor.ValueMember = "Id";
                CBO_Proveedor.DataSource = items;

                CBO_Proveedor.SelectedValue = seleccionado;
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void CargarComboEstados()
        {
            _cargandoCombos = true;

            try
            {
                string todos = _sesionIdioma.idioma != null
                    ? _sesionIdioma.idioma.BuscarTraduccion("Compras.Todos")
                    : "Todos";

                List<ItemEstado> items = new List<ItemEstado>();
                items.Add(new ItemEstado { Id = "", Nombre = todos });

                foreach (string estado in EstadosCompra)
                    items.Add(new ItemEstado { Id = estado, Nombre = TraducirEstado(estado) });

                string seleccionado = "";

                if (CBO_Estado.SelectedValue is string)
                    seleccionado = (string)CBO_Estado.SelectedValue;

                CBO_Estado.DataSource = null;
                CBO_Estado.DisplayMember = "Nombre";
                CBO_Estado.ValueMember = "Id";
                CBO_Estado.DataSource = items;

                CBO_Estado.SelectedValue = seleccionado;
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void CargarCompras()
        {
            try
            {
                _todas = _service.Listar();
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AplicarFiltro()
        {
            if (CBO_Proveedor == null || CBO_Estado == null)
                return;

            int idProveedor = 0;

            if (CBO_Proveedor.SelectedValue is int)
                idProveedor = (int)CBO_Proveedor.SelectedValue;

            string estadoFiltro = "";

            if (CBO_Estado.SelectedValue is string)
                estadoFiltro = (string)CBO_Estado.SelectedValue;

            List<FilaCompra> filas = new List<FilaCompra>();

            foreach (Compra c in _todas)
            {
                if (idProveedor > 0 && c.IdProveedor != idProveedor)
                    continue;

                if (!string.IsNullOrEmpty(estadoFiltro) && c.Estado != estadoFiltro)
                    continue;

                filas.Add(new FilaCompra
                {
                    Id = c.Id,
                    IdProveedor = c.IdProveedor,
                    Numero = c.Id,
                    Proveedor = ResolverProveedor(c.IdProveedor),
                    Fecha = c.Fecha,
                    EstadoRaw = c.Estado,
                    Estado = TraducirEstado(c.Estado),
                    Total = c.Total,
                    Observaciones = c.Observaciones
                });
            }

            _comprasBindingList = new BindingList<FilaCompra>(filas);
            DGV_Compras.DataSource = _comprasBindingList;
            ConfigurarColumnas();
            AplicarPermisos();
        }

        private string ResolverProveedor(int idProveedor)
        {
            foreach (Proveedor p in _proveedores)
            {
                if (p.Id == idProveedor)
                    return p.RazonSocial;
            }

            return "#" + idProveedor;
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
            if (DGV_Compras.Columns.Count == 0)
                return;

            ConfigurarColumna("Numero", "Columna.Numero");
            ConfigurarColumna("Proveedor", "Columna.Proveedor");
            ConfigurarColumna("Fecha", "Columna.Fecha");
            ConfigurarColumna("Estado", "Columna.Estado");
            ConfigurarColumna("Total", "Columna.Total");

            if (DGV_Compras.Columns.Contains("Id"))
                DGV_Compras.Columns["Id"].Visible = false;
            if (DGV_Compras.Columns.Contains("IdProveedor"))
                DGV_Compras.Columns["IdProveedor"].Visible = false;
            if (DGV_Compras.Columns.Contains("EstadoRaw"))
                DGV_Compras.Columns["EstadoRaw"].Visible = false;
            if (DGV_Compras.Columns.Contains("Observaciones"))
                DGV_Compras.Columns["Observaciones"].Visible = false;

            DGV_Compras.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Compras.Columns.Contains(nombreColumna))
                return;

            DGV_Compras.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Compras.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private FilaCompra CompraSeleccionada()
        {
            if (DGV_Compras.SelectedRows.Count == 0)
                return null;

            return DGV_Compras.SelectedRows[0].DataBoundItem as FilaCompra;
        }

        private void CBO_Proveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_Proveedor.DataSource == null)
                return;

            AplicarFiltro();
        }

        private void CBO_Estado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_Estado.DataSource == null)
                return;

            AplicarFiltro();
        }

        private void DGV_Compras_SelectionChanged(object sender, EventArgs e)
        {
            AplicarPermisos();
        }

        private void DGV_Compras_DoubleClick(object sender, EventArgs e)
        {
            BTN_Detalle_Click(sender, e);
        }

        private void BTN_Crear_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ComprasCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            using (FrmCompraDetalle dlg = new FrmCompraDetalle())
            {
                dlg.ShowDialog(this);
            }

            CargarProveedores();
            CargarComboProveedores();
            CargarCompras();
        }

        private void BTN_Detalle_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ComprasVer))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaCompra seleccionada = CompraSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            using (FrmCompraDetalle dlg = new FrmCompraDetalle(seleccionada.Id))
            {
                dlg.ShowDialog(this);
            }

            CargarProveedores();
            CargarComboProveedores();
            CargarCompras();
        }

        private void BTN_Confirmar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ComprasCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaCompra seleccionada = CompraSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (seleccionada.EstadoRaw != EstadoCompra.Borrador)
            {
                MostrarAdvertencia("Mensaje.CompraNoConfirmable");
                return;
            }

            if (!Confirmar("Mensaje.ConfirmarConfirmarCompra", "Titulo.ConfirmarConfirmacion"))
                return;

            try
            {
                _service.Confirmar(seleccionada.Id);
                CargarCompras();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Cancelar_Click(object sender, EventArgs e)
        {
            // Cancelar borrador -> Cancelada persistente (conserva cabecera + detalle).
            // Permiso COMPRAS_CANCELAR (mismo codigo que Anular confirmada).
            if (!TienePermiso(CodigosPermiso.ComprasCancelar))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaCompra seleccionada = CompraSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (seleccionada.EstadoRaw != EstadoCompra.Borrador)
            {
                MostrarAdvertencia("Mensaje.CompraNoCancelabe");
                return;
            }

            if (!Confirmar("Mensaje.ConfirmarCancelarCompra", "Titulo.ConfirmarCancelacion"))
                return;

            try
            {
                _service.Cancelar(seleccionada.Id);
                CargarCompras();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Anular_Click(object sender, EventArgs e)
        {
            // Anular confirmada con motivo obligatorio -> Cancelada + reversion de stock.
            // Permiso COMPRAS_CANCELAR (mismo codigo que Cancelar borrador).
            if (!TienePermiso(CodigosPermiso.ComprasCancelar))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaCompra seleccionada = CompraSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (seleccionada.EstadoRaw != EstadoCompra.Confirmada)
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
                _service.Anular(seleccionada.Id, motivo.Trim());
                CargarCompras();
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
                    : _sesionIdioma.idioma.BuscarTraduccion("Compras.Anular");
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
            bool puedeVer = TienePermiso(CodigosPermiso.ComprasVer);
            bool puedeCancelar = TienePermiso(CodigosPermiso.ComprasCancelar);
            FilaCompra seleccionada = CompraSeleccionada();
            bool haySeleccion = seleccionada != null;
            bool esBorrador = haySeleccion && seleccionada.EstadoRaw == EstadoCompra.Borrador;
            bool esConfirmada = haySeleccion && seleccionada.EstadoRaw == EstadoCompra.Confirmada;

            BTN_Crear.Visible = puedeCrear;
            BTN_Detalle.Visible = puedeVer;
            BTN_Confirmar.Visible = puedeCrear;
            BTN_Cancelar.Visible = puedeCancelar;
            BTN_Anular.Visible = puedeCancelar;

            BTN_Crear.Enabled = puedeCrear;
            BTN_Detalle.Enabled = puedeVer && haySeleccion;
            BTN_Confirmar.Enabled = puedeCrear && esBorrador;
            // Cancelada: terminal, sin acciones (solo lectura via Detalle).
            BTN_Cancelar.Enabled = puedeCancelar && esBorrador;
            BTN_Anular.Enabled = puedeCancelar && esConfirmada;
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
