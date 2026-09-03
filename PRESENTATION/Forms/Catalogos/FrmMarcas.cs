using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Marcas;
using DOMAIN.Features.Marcas;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Catalogos
{
    public partial class FrmMarcas : Form, IObservador
    {
        private BindingList<Marca> _marcasBindingList = null;
        private List<Marca> _todos = new List<Marca>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly MarcaService _service;

        public FrmMarcas()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new MarcaService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Nombre.Text = idiomaObservado.BuscarTraduccion(LBL_Nombre.Tag.ToString());
            CHK_Inactivos.Text = idiomaObservado.BuscarTraduccion(CHK_Inactivos.Tag.ToString());
            BTN_Crear.Text = idiomaObservado.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Editar.Text = idiomaObservado.BuscarTraduccion(BTN_Editar.Tag.ToString());
            BTN_Desactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Desactivar.Tag.ToString());
            BTN_Reactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Reactivar.Tag.ToString());

            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void FrmMarcas_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            if (!TienePermiso(CodigosPermiso.MarcasVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarMarcas();
        }

        private void CargarMarcas()
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

            List<Marca> filtrados = _todos.Where(m =>
                string.IsNullOrEmpty(nombre) || (m.Nombre != null && m.Nombre.ToLowerInvariant().Contains(nombre))
            ).ToList();

            _marcasBindingList = new BindingList<Marca>(filtrados);
            DGV_Marcas.DataSource = _marcasBindingList;
            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Marcas.Columns.Count == 0)
                return;

            ConfigurarColumna("Id", "Columna.Id");
            ConfigurarColumna("Nombre", "Columna.Nombre");
            ConfigurarColumna("Activo", "Columna.Activo");

            DGV_Marcas.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Marcas.Columns.Contains(nombreColumna))
                return;

            DGV_Marcas.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Marcas.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private Marca MarcaSeleccionada()
        {
            if (DGV_Marcas.SelectedRows.Count == 0)
                return null;

            return DGV_Marcas.SelectedRows[0].DataBoundItem as Marca;
        }

        private void TXT_Nombre_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void CHK_Inactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarMarcas();
        }

        private void DGV_Marcas_SelectionChanged(object sender, EventArgs e)
        {
            AplicarPermisos();
        }

        private void BTN_Crear_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.MarcasCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            string nombre = PedirNombre(null);

            if (nombre == null)
                return;

            try
            {
                _service.Crear(nombre);
                CargarMarcas();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Editar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.MarcasEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Marca seleccionada = MarcaSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            string nombre = PedirNombre(seleccionada.Nombre);

            if (nombre == null)
                return;

            try
            {
                _service.Modificar(seleccionada.Id, nombre);
                CargarMarcas();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Desactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.MarcasDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Marca seleccionada = MarcaSeleccionada();

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
                CargarMarcas();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Reactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.MarcasDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            Marca seleccionada = MarcaSeleccionada();

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
                CargarMarcas();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private string PedirNombre(string valorInicial)
        {
            using (FrmCatalogoEditar dlg = new FrmCatalogoEditar(
                _sesionIdioma.idioma.BuscarTraduccion("Marcas.Titulo"),
                valorInicial))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return null;

                return dlg.Nombre;
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
            bool puedeCrear = TienePermiso(CodigosPermiso.MarcasCrear);
            bool puedeEditar = TienePermiso(CodigosPermiso.MarcasEditar);
            bool puedeDesactivar = TienePermiso(CodigosPermiso.MarcasDesactivar);
            Marca seleccionada = MarcaSeleccionada();
            bool haySeleccion = seleccionada != null;

            BTN_Crear.Visible = puedeCrear;
            BTN_Editar.Visible = puedeEditar;
            BTN_Desactivar.Visible = puedeDesactivar;
            BTN_Reactivar.Visible = puedeDesactivar;

            BTN_Crear.Enabled = puedeCrear;
            BTN_Editar.Enabled = puedeEditar && haySeleccion;
            BTN_Desactivar.Enabled = puedeDesactivar && haySeleccion && seleccionada != null && seleccionada.Activo;
            BTN_Reactivar.Enabled = puedeDesactivar && haySeleccion && seleccionada != null && !seleccionada.Activo;
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
