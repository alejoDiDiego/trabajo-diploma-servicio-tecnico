using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Clientes;
using DOMAIN.Features.Clientes;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Clientes
{
    // Dialogo solo edita campos; el llamador (FrmClientes) invoca a ClienteService.Crear/Modificar.
    public partial class FrmClientes : Form, IObservador
    {
        private BindingList<Cliente> _clientesBindingList = null;
        private List<Cliente> _todos = new List<Cliente>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly ClienteService _service;

        public FrmClientes()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new ClienteService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Nombre.Text = idiomaObservado.BuscarTraduccion(LBL_Nombre.Tag.ToString());
            LBL_Apellido.Text = idiomaObservado.BuscarTraduccion(LBL_Apellido.Tag.ToString());
            LBL_Documento.Text = idiomaObservado.BuscarTraduccion(LBL_Documento.Tag.ToString());
            CHK_Inactivos.Text = idiomaObservado.BuscarTraduccion(CHK_Inactivos.Tag.ToString());
            BTN_Crear.Text = idiomaObservado.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Editar.Text = idiomaObservado.BuscarTraduccion(BTN_Editar.Tag.ToString());
            BTN_Desactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Desactivar.Tag.ToString());
            BTN_Reactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Reactivar.Tag.ToString());

            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            if (!TienePermiso(CodigosPermiso.ClientesVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarClientes();
        }

        private void CargarClientes()
        {
            try
            {
                _todos = _service.Listar(CHK_Inactivos.Checked);
                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AplicarFiltro()
        {
            string nombre = TXT_Nombre.Text.Trim().ToLowerInvariant();
            string apellido = TXT_Apellido.Text.Trim().ToLowerInvariant();
            string documento = TXT_Documento.Text.Trim().ToLowerInvariant();

            List<Cliente> filtrados = _todos.Where(c =>
                (string.IsNullOrEmpty(nombre) || (c.Nombre != null && c.Nombre.ToLowerInvariant().Contains(nombre))) &&
                (string.IsNullOrEmpty(apellido) || (c.Apellido != null && c.Apellido.ToLowerInvariant().Contains(apellido))) &&
                (string.IsNullOrEmpty(documento) || (c.Documento != null && c.Documento.ToLowerInvariant().Contains(documento)))
            ).ToList();

            _clientesBindingList = new BindingList<Cliente>(filtrados);
            DGV_Clientes.DataSource = _clientesBindingList;
            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Clientes.Columns.Count == 0)
                return;

            ConfigurarColumna("Id", "Columna.Id");
            ConfigurarColumna("Nombre", "Columna.Nombre");
            ConfigurarColumna("Apellido", "Columna.Apellido");
            ConfigurarColumna("Documento", "Columna.Documento");
            ConfigurarColumna("Telefono", "Columna.Telefono");
            ConfigurarColumna("Email", "Columna.Email");
            ConfigurarColumna("Activo", "Columna.Activo");

            if (DGV_Clientes.Columns.Contains("Direccion"))
                DGV_Clientes.Columns["Direccion"].Visible = false;
            if (DGV_Clientes.Columns.Contains("Observaciones"))
                DGV_Clientes.Columns["Observaciones"].Visible = false;
            if (DGV_Clientes.Columns.Contains("FechaAlta"))
                DGV_Clientes.Columns["FechaAlta"].Visible = false;

            DGV_Clientes.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Clientes.Columns.Contains(nombreColumna))
                return;

            DGV_Clientes.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Clientes.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private Cliente ClienteSeleccionado()
        {
            if (DGV_Clientes.SelectedRows.Count == 0)
                return null;

            return DGV_Clientes.SelectedRows[0].DataBoundItem as Cliente;
        }

        private void Filtros_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void CHK_Inactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void DGV_Clientes_SelectionChanged(object sender, EventArgs e)
        {
            AplicarPermisos();
        }

        private void BTN_Crear_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ClientesCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            try
            {
                using (FrmClienteEditar dlg = new FrmClienteEditar(null))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                        return;

                    _service.Crear(dlg.Nombre, dlg.Apellido, dlg.Documento, dlg.Telefono, dlg.Email, dlg.Direccion, dlg.Observaciones);
                }

                CargarClientes();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Editar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ClientesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Cliente seleccionado = ClienteSeleccionado();

            if (seleccionado == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            try
            {
                using (FrmClienteEditar dlg = new FrmClienteEditar(seleccionado))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                        return;

                    _service.Modificar(seleccionado.Id, dlg.Nombre, dlg.Apellido, dlg.Documento, dlg.Telefono, dlg.Email, dlg.Direccion, dlg.Observaciones);
                }

                CargarClientes();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Desactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ClientesDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Cliente seleccionado = ClienteSeleccionado();

            if (seleccionado == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarDesactivar"),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarDesactivacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
                return;

            try
            {
                _service.Desactivar(seleccionado.Id);
                CargarClientes();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Reactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ClientesDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Cliente seleccionado = ClienteSeleccionado();

            if (seleccionado == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarReactivar"),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarReactivacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
                return;

            try
            {
                _service.Reactivar(seleccionado.Id);
                CargarClientes();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AplicarPermisos()
        {
            bool puedeCrear = TienePermiso(CodigosPermiso.ClientesCrear);
            bool puedeEditar = TienePermiso(CodigosPermiso.ClientesEditar);
            bool puedeDesactivar = TienePermiso(CodigosPermiso.ClientesDesactivar);
            Cliente seleccionado = ClienteSeleccionado();
            bool haySeleccion = seleccionado != null;

            BTN_Crear.Visible = puedeCrear;
            BTN_Editar.Visible = puedeEditar;
            BTN_Desactivar.Visible = puedeDesactivar;
            BTN_Reactivar.Visible = puedeDesactivar;

            BTN_Crear.Enabled = puedeCrear;
            BTN_Editar.Enabled = puedeEditar && haySeleccion;
            BTN_Desactivar.Enabled = puedeDesactivar && haySeleccion && seleccionado != null && seleccionado.Activo;
            BTN_Reactivar.Enabled = puedeDesactivar && haySeleccion && seleccionado != null && !seleccionado.Activo;
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
