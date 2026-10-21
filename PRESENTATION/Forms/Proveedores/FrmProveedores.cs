using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Proveedores;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Proveedores;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Proveedores
{
    public partial class FrmProveedores : Form, IObservador
    {
        private BindingList<Proveedor> _proveedoresBindingList = null;
        private List<Proveedor> _todos = new List<Proveedor>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly ProveedorService _service;

        public FrmProveedores()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new ProveedorService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_RazonSocial.Text = idiomaObservado.BuscarTraduccion(LBL_RazonSocial.Tag.ToString());
            LBL_Cuit.Text = idiomaObservado.BuscarTraduccion(LBL_Cuit.Tag.ToString());
            CHK_Inactivos.Text = idiomaObservado.BuscarTraduccion(CHK_Inactivos.Tag.ToString());
            BTN_Crear.Text = idiomaObservado.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Editar.Text = idiomaObservado.BuscarTraduccion(BTN_Editar.Tag.ToString());
            BTN_Desactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Desactivar.Tag.ToString());
            BTN_Reactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Reactivar.Tag.ToString());

            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            if (!TienePermiso(CodigosPermiso.ProveedoresVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarProveedores();
        }

        private void CargarProveedores()
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
            string razon = TXT_RazonSocial != null ? TXT_RazonSocial.Text.Trim().ToLowerInvariant() : "";
            string cuit = TXT_Cuit != null ? TXT_Cuit.Text.Trim().ToLowerInvariant() : "";

            List<Proveedor> filtrados = _todos.Where(p =>
                (string.IsNullOrEmpty(razon) || (p.RazonSocial != null && p.RazonSocial.ToLowerInvariant().Contains(razon))) &&
                (string.IsNullOrEmpty(cuit) || (p.Cuit != null && p.Cuit.ToLowerInvariant().Contains(cuit)))
            ).ToList();

            _proveedoresBindingList = new BindingList<Proveedor>(filtrados);
            DGV_Proveedores.DataSource = _proveedoresBindingList;
            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Proveedores.Columns.Count == 0)
                return;

            ConfigurarColumna("Id", "Columna.Id");
            ConfigurarColumna("RazonSocial", "Columna.RazonSocial");
            ConfigurarColumna("Cuit", "Columna.Cuit");
            ConfigurarColumna("Telefono", "Columna.Telefono");
            ConfigurarColumna("Email", "Columna.Email");
            ConfigurarColumna("Direccion", "Columna.Direccion");
            ConfigurarColumna("Contacto", "Columna.Contacto");
            ConfigurarColumna("Activo", "Columna.Activo");

            DGV_Proveedores.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Proveedores.Columns.Contains(nombreColumna))
                return;

            DGV_Proveedores.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Proveedores.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private Proveedor ProveedorSeleccionado()
        {
            if (DGV_Proveedores.SelectedRows.Count == 0)
                return null;

            return DGV_Proveedores.SelectedRows[0].DataBoundItem as Proveedor;
        }

        private void TXT_Filtro_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void CHK_Inactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarProveedores();
        }

        private void DGV_Proveedores_SelectionChanged(object sender, EventArgs e)
        {
            AplicarPermisos();
        }

        private void BTN_Crear_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ProveedoresCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            using (FrmProveedorEditar dlg = new FrmProveedorEditar(null))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    _service.Crear(dlg.RazonSocial, dlg.Cuit, dlg.Telefono,
                        dlg.Email, dlg.Direccion, dlg.Contacto);
                    CargarProveedores();
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
            if (!TienePermiso(CodigosPermiso.ProveedoresEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Proveedor seleccionado = ProveedorSeleccionado();

            if (seleccionado == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            using (FrmProveedorEditar dlg = new FrmProveedorEditar(seleccionado))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    _service.Modificar(seleccionado.Id, dlg.RazonSocial, dlg.Cuit,
                        dlg.Telefono, dlg.Email, dlg.Direccion, dlg.Contacto);
                    CargarProveedores();
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
            if (!TienePermiso(CodigosPermiso.ProveedoresDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Proveedor seleccionado = ProveedorSeleccionado();

            if (seleccionado == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (!Confirmar("Mensaje.ConfirmarDesactivar", "Titulo.ConfirmarDesactivacion"))
                return;

            try
            {
                _service.Desactivar(seleccionado.Id);
                CargarProveedores();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Reactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ProveedoresDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Proveedor seleccionado = ProveedorSeleccionado();

            if (seleccionado == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (!Confirmar("Mensaje.ConfirmarReactivar", "Titulo.ConfirmarReactivacion"))
                return;

            try
            {
                _service.Reactivar(seleccionado.Id);
                CargarProveedores();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
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
            bool puedeCrear = TienePermiso(CodigosPermiso.ProveedoresCrear);
            bool puedeEditar = TienePermiso(CodigosPermiso.ProveedoresEditar);
            bool puedeDesactivar = TienePermiso(CodigosPermiso.ProveedoresDesactivar);
            Proveedor seleccionado = ProveedorSeleccionado();
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
