using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Repuestos;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Repuestos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Repuestos
{
    public partial class FrmRepuestos : Form, IObservador
    {
        private class FilaRepuesto
        {
            public int Id { get; set; }
            public string Codigo { get; set; }
            public string Descripcion { get; set; }
            public int Stock { get; set; }
            public int StockMinimo { get; set; }
            public string Alerta { get; set; }
            public decimal Costo { get; set; }
            public decimal Precio { get; set; }
            public bool Activo { get; set; }
        }

        private BindingList<FilaRepuesto> _repuestosBindingList = null;
        private List<Repuesto> _todos = new List<Repuesto>();
        private Dictionary<int, Repuesto> _porId = new Dictionary<int, Repuesto>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly RepuestoService _service;

        public FrmRepuestos()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new RepuestoService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Codigo.Text = idiomaObservado.BuscarTraduccion(LBL_Codigo.Tag.ToString());
            LBL_Descripcion.Text = idiomaObservado.BuscarTraduccion(LBL_Descripcion.Tag.ToString());
            CHK_Inactivos.Text = idiomaObservado.BuscarTraduccion(CHK_Inactivos.Tag.ToString());
            CHK_BajoMinimo.Text = idiomaObservado.BuscarTraduccion(CHK_BajoMinimo.Tag.ToString());
            BTN_Crear.Text = idiomaObservado.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Editar.Text = idiomaObservado.BuscarTraduccion(BTN_Editar.Tag.ToString());
            BTN_Desactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Desactivar.Tag.ToString());
            BTN_Reactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Reactivar.Tag.ToString());
            BTN_Ajustar.Text = idiomaObservado.BuscarTraduccion(BTN_Ajustar.Tag.ToString());
            BTN_Movimientos.Text = idiomaObservado.BuscarTraduccion(BTN_Movimientos.Tag.ToString());

            AplicarFiltro();
        }

        private void FrmRepuestos_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            if (!TienePermiso(CodigosPermiso.RepuestosVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarRepuestos();
        }

        private void CargarRepuestos()
        {
            try
            {
                _todos = _service.Listar(CHK_Inactivos.Checked);
                _porId = _todos.ToDictionary(r => r.Id, r => r);
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AplicarFiltro()
        {
            string codigo = TXT_Codigo != null ? TXT_Codigo.Text.Trim().ToLowerInvariant() : "";
            string descripcion = TXT_Descripcion != null ? TXT_Descripcion.Text.Trim().ToLowerInvariant() : "";
            bool soloBajoMinimo = CHK_BajoMinimo != null && CHK_BajoMinimo.Checked;

            List<FilaRepuesto> filas = new List<FilaRepuesto>();

            foreach (Repuesto r in _todos)
            {
                if (!string.IsNullOrEmpty(codigo) && (r.Codigo == null || !r.Codigo.ToLowerInvariant().Contains(codigo)))
                    continue;

                if (!string.IsNullOrEmpty(descripcion) && (r.Descripcion == null || !r.Descripcion.ToLowerInvariant().Contains(descripcion)))
                    continue;

                if (soloBajoMinimo && r.StockActual > r.StockMinimo)
                    continue;

                filas.Add(new FilaRepuesto
                {
                    Id = r.Id,
                    Codigo = r.Codigo,
                    Descripcion = r.Descripcion,
                    Stock = r.StockActual,
                    StockMinimo = r.StockMinimo,
                    Alerta = r.StockActual <= r.StockMinimo ? T("Repuestos.AlertaBajoMinimo") : "",
                    Costo = r.CostoActual,
                    Precio = r.PrecioReferencia,
                    Activo = r.Activo
                });
            }

            _repuestosBindingList = new BindingList<FilaRepuesto>(filas);
            DGV_Repuestos.DataSource = _repuestosBindingList;
            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Repuestos.Columns.Count == 0)
                return;

            ConfigurarColumna("Id", "Columna.Id");
            ConfigurarColumna("Codigo", "Repuestos.CodigoGrilla");
            ConfigurarColumna("Descripcion", "Columna.Descripcion");
            ConfigurarColumna("Stock", "Columna.Stock");
            ConfigurarColumna("StockMinimo", "Columna.StockMinimo");
            ConfigurarColumna("Alerta", "Columna.Alerta");
            ConfigurarColumna("Costo", "Columna.Costo");
            ConfigurarColumna("Precio", "Columna.Precio");
            ConfigurarColumna("Activo", "Columna.Activo");

            DGV_Repuestos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Repuestos.Columns.Contains(nombreColumna))
                return;

            DGV_Repuestos.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Repuestos.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private void DGV_Repuestos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DGV_Repuestos.Columns.Count == 0)
                return;

            if (DGV_Repuestos.Columns[e.ColumnIndex].Name != "Stock")
                return;

            FilaRepuesto fila = e.RowIndex >= 0 && e.RowIndex < DGV_Repuestos.Rows.Count
                ? DGV_Repuestos.Rows[e.RowIndex].DataBoundItem as FilaRepuesto
                : null;

            if (fila != null && fila.Stock <= fila.StockMinimo)
            {
                e.CellStyle.BackColor = Color.LightSalmon;
                e.CellStyle.SelectionBackColor = Color.IndianRed;
            }
        }

        private Repuesto RepuestoSeleccionado()
        {
            if (DGV_Repuestos.SelectedRows.Count == 0)
                return null;

            FilaRepuesto fila = DGV_Repuestos.SelectedRows[0].DataBoundItem as FilaRepuesto;

            if (fila == null || !_porId.ContainsKey(fila.Id))
                return null;

            return _porId[fila.Id];
        }

        private void TXT_Filtro_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void CHK_Inactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarRepuestos();
        }

        private void CHK_BajoMinimo_CheckedChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void DGV_Repuestos_SelectionChanged(object sender, EventArgs e)
        {
            AplicarPermisos();
        }

        private void BTN_Crear_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.RepuestosCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            using (FrmRepuestoEditar dlg = new FrmRepuestoEditar())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    _service.Crear(dlg.Codigo, dlg.Descripcion, dlg.StockInicial,
                        dlg.StockMinimo, dlg.CostoActual, dlg.PrecioReferencia);
                    CargarRepuestos();
                    MostrarExito("Mensaje.OperacionExitosa");
                }
                catch (Exception ex)
                {
                    MostrarError(ex);
                }
            }
        }

        private void BTN_Editar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.RepuestosEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Repuesto seleccionada = RepuestoSeleccionado();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            using (FrmRepuestoEditar dlg = new FrmRepuestoEditar(seleccionada))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    _service.Modificar(seleccionada.Id, dlg.Codigo, dlg.Descripcion,
                        dlg.StockMinimo, dlg.CostoActual, dlg.PrecioReferencia);
                    CargarRepuestos();
                    MostrarExito("Mensaje.OperacionExitosa");
                }
                catch (Exception ex)
                {
                    MostrarError(ex);
                }
            }
        }

        private void BTN_Desactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.RepuestosDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Repuesto seleccionada = RepuestoSeleccionado();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (!Confirmar("Mensaje.ConfirmarDesactivar", "Titulo.ConfirmarDesactivacion"))
                return;

            try
            {
                _service.Desactivar(seleccionada.Id);
                CargarRepuestos();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Reactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.RepuestosDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Repuesto seleccionada = RepuestoSeleccionado();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (!Confirmar("Mensaje.ConfirmarReactivar", "Titulo.ConfirmarReactivacion"))
                return;

            try
            {
                _service.Reactivar(seleccionada.Id);
                CargarRepuestos();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Ajustar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.RepuestosEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Repuesto seleccionada = RepuestoSeleccionado();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            using (FrmAjusteStock dlg = new FrmAjusteStock(seleccionada))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    _service.AjustarStock(seleccionada.Id, dlg.Cantidad, dlg.Motivo);
                    CargarRepuestos();
                    MostrarExito("Mensaje.OperacionExitosa");
                }
                catch (Exception ex)
                {
                    MostrarError(ex);
                }
            }
        }

        private void BTN_Movimientos_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.RepuestosVer))
            {
                MostrarAccesoDenegado();
                return;
            }

            Repuesto seleccionada = RepuestoSeleccionado();

            using (FrmMovimientosStock dlg = seleccionada != null
                ? new FrmMovimientosStock(seleccionada.Id)
                : new FrmMovimientosStock())
            {
                dlg.ShowDialog(this);
            }
        }

        private string T(string clave)
        {
            if (_sesionIdioma.idioma == null)
                return clave;

            return _sesionIdioma.idioma.BuscarTraduccion(clave);
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
            bool puedeCrear = TienePermiso(CodigosPermiso.RepuestosCrear);
            bool puedeEditar = TienePermiso(CodigosPermiso.RepuestosEditar);
            bool puedeDesactivar = TienePermiso(CodigosPermiso.RepuestosDesactivar);
            bool puedeVer = TienePermiso(CodigosPermiso.RepuestosVer);
            Repuesto seleccionada = RepuestoSeleccionado();
            bool haySeleccion = seleccionada != null;

            BTN_Crear.Visible = puedeCrear;
            BTN_Editar.Visible = puedeEditar;
            BTN_Desactivar.Visible = puedeDesactivar;
            BTN_Reactivar.Visible = puedeDesactivar;
            BTN_Ajustar.Visible = puedeEditar;
            BTN_Movimientos.Visible = puedeVer;

            BTN_Crear.Enabled = puedeCrear;
            BTN_Editar.Enabled = puedeEditar && haySeleccion;
            BTN_Desactivar.Enabled = puedeDesactivar && haySeleccion && seleccionada != null && seleccionada.Activo;
            BTN_Reactivar.Enabled = puedeDesactivar && haySeleccion && seleccionada != null && !seleccionada.Activo;
            BTN_Ajustar.Enabled = puedeEditar && haySeleccion;
            BTN_Movimientos.Enabled = puedeVer;
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
