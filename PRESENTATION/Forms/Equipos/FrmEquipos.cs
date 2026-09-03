using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Clientes;
using APPLICATION.Features.Equipos;
using APPLICATION.Features.Marcas;
using APPLICATION.Features.TiposEquipo;
using DOMAIN.Features.Clientes;
using DOMAIN.Features.Equipos;
using DOMAIN.Features.Marcas;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.TiposEquipo;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Equipos
{
    // Dialogo solo edita campos; el llamador (FrmEquipos) invoca a EquipoService.Crear/Modificar.
    public partial class FrmEquipos : Form, IObservador
    {
        private class FilaEquipo
        {
            public int Id { get; set; }
            public int IdCliente { get; set; }
            public string Cliente { get; set; }
            public int IdTipoEquipo { get; set; }
            public string TipoEquipo { get; set; }
            public int IdMarca { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string NumeroSerie { get; set; }
            public string Imei { get; set; }
            public string Color { get; set; }
            public string Observaciones { get; set; }
            public bool Activo { get; set; }
        }

        private class ItemCliente
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private BindingList<FilaEquipo> _equiposBindingList = null;
        private List<Equipo> _todos = new List<Equipo>();
        private List<Cliente> _clientes = new List<Cliente>();
        private List<TipoEquipo> _tipos = new List<TipoEquipo>();
        private List<Marca> _marcas = new List<Marca>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly EquipoService _service;
        private readonly ClienteService _clienteService;
        private readonly TipoEquipoService _tipoService;
        private readonly MarcaService _marcaService;
        private bool _cargandoCombos = false;

        public FrmEquipos()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new EquipoService();
            _clienteService = new ClienteService();
            _tipoService = new TipoEquipoService();
            _marcaService = new MarcaService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Cliente.Text = idiomaObservado.BuscarTraduccion(LBL_Cliente.Tag.ToString());
            LBL_Busqueda.Text = idiomaObservado.BuscarTraduccion(LBL_Busqueda.Tag.ToString());
            CHK_Inactivos.Text = idiomaObservado.BuscarTraduccion(CHK_Inactivos.Tag.ToString());
            BTN_Crear.Text = idiomaObservado.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Editar.Text = idiomaObservado.BuscarTraduccion(BTN_Editar.Tag.ToString());
            BTN_Desactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Desactivar.Tag.ToString());
            BTN_Reactivar.Text = idiomaObservado.BuscarTraduccion(BTN_Reactivar.Tag.ToString());

            CargarComboClientes();
            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void FrmEquipos_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            CargarCatalogos();
            CargarComboClientes();
            ActualizarTextos();
            ConfigurarColumnas();
            AplicarPermisos();

            if (!TienePermiso(CodigosPermiso.EquiposVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarEquipos();
        }

        private void ActualizarTextos()
        {
            if (_sesionIdioma.idioma == null)
                return;

            Text = _sesionIdioma.idioma.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Cliente.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Cliente.Tag.ToString());
            LBL_Busqueda.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Busqueda.Tag.ToString());
            CHK_Inactivos.Text = _sesionIdioma.idioma.BuscarTraduccion(CHK_Inactivos.Tag.ToString());
            BTN_Crear.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Editar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Editar.Tag.ToString());
            BTN_Desactivar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Desactivar.Tag.ToString());
            BTN_Reactivar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Reactivar.Tag.ToString());
        }

        private void CargarCatalogos()
        {
            try
            {
                _clientes = _clienteService.Listar(false);
                _tipos = _tipoService.Listar(true);
                _marcas = _marcaService.Listar(true);
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void CargarComboClientes()
        {
            _cargandoCombos = true;

            try
            {
                string todos = _sesionIdioma.idioma != null
                    ? _sesionIdioma.idioma.BuscarTraduccion("Equipos.TodosClientes")
                    : "Todos";

                List<ItemCliente> items = new List<ItemCliente>();
                items.Add(new ItemCliente { Id = 0, Nombre = todos });

                foreach (Cliente c in _clientes)
                    items.Add(new ItemCliente { Id = c.Id, Nombre = c.Apellido + ", " + c.Nombre + " (" + c.Documento + ")" });

                int seleccionado = 0;

                if (CBO_Cliente.SelectedValue is int)
                    seleccionado = (int)CBO_Cliente.SelectedValue;

                CBO_Cliente.DataSource = null;
                CBO_Cliente.DisplayMember = "Nombre";
                CBO_Cliente.ValueMember = "Id";
                CBO_Cliente.DataSource = items;

                CBO_Cliente.SelectedValue = seleccionado;
            }
            finally
            {
                _cargandoCombos = false;
            }
        }

        private void CargarEquipos()
        {
            try
            {
                int idCliente = 0;

                if (CBO_Cliente.SelectedValue is int)
                    idCliente = (int)CBO_Cliente.SelectedValue;

                bool incluirInactivos = CHK_Inactivos.Checked;

                if (idCliente > 0)
                    _todos = _service.ListarPorCliente(idCliente, incluirInactivos);
                else
                    _todos = _service.Listar(incluirInactivos);

                AplicarFiltro();
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AplicarFiltro()
        {
            string texto = TXT_Busqueda.Text.Trim().ToLowerInvariant();

            List<FilaEquipo> filas = new List<FilaEquipo>();

            foreach (Equipo e in _todos)
            {
                string nombreCliente = ResolverCliente(e.IdCliente);
                string nombreTipo = ResolverTipo(e.IdTipoEquipo);
                string nombreMarca = ResolverMarca(e.IdMarca);

                FilaEquipo fila = new FilaEquipo
                {
                    Id = e.Id,
                    IdCliente = e.IdCliente,
                    Cliente = nombreCliente,
                    IdTipoEquipo = e.IdTipoEquipo,
                    TipoEquipo = nombreTipo,
                    IdMarca = e.IdMarca,
                    Marca = nombreMarca,
                    Modelo = e.Modelo,
                    NumeroSerie = e.NumeroSerie,
                    Imei = e.Imei,
                    Color = e.Color,
                    Observaciones = e.Observaciones,
                    Activo = e.Activo
                };

                if (!string.IsNullOrEmpty(texto))
                {
                    string modelo = (fila.Modelo ?? "").ToLowerInvariant();
                    string serie = (fila.NumeroSerie ?? "").ToLowerInvariant();

                    if (!modelo.Contains(texto) && !serie.Contains(texto))
                        continue;
                }

                filas.Add(fila);
            }

            _equiposBindingList = new BindingList<FilaEquipo>(filas);
            DGV_Equipos.DataSource = _equiposBindingList;
            ConfigurarColumnas();
            AplicarPermisos();
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

        private string ResolverTipo(int idTipo)
        {
            TipoEquipo t = _tipos.FirstOrDefault(x => x.Id == idTipo);
            return t != null ? t.Nombre : "#" + idTipo;
        }

        private string ResolverMarca(int idMarca)
        {
            Marca m = _marcas.FirstOrDefault(x => x.Id == idMarca);
            return m != null ? m.Nombre : "#" + idMarca;
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Equipos.Columns.Count == 0)
                return;

            ConfigurarColumna("Id", "Columna.Id");
            ConfigurarColumna("Cliente", "Columna.Cliente");
            ConfigurarColumna("TipoEquipo", "Columna.TipoEquipo");
            ConfigurarColumna("Marca", "Columna.Marca");
            ConfigurarColumna("Modelo", "Columna.Modelo");
            ConfigurarColumna("NumeroSerie", "Columna.NumeroSerie");
            ConfigurarColumna("Imei", "Columna.Imei");
            ConfigurarColumna("Color", "Columna.Color");
            ConfigurarColumna("Activo", "Columna.Activo");

            if (DGV_Equipos.Columns.Contains("IdCliente"))
                DGV_Equipos.Columns["IdCliente"].Visible = false;
            if (DGV_Equipos.Columns.Contains("IdTipoEquipo"))
                DGV_Equipos.Columns["IdTipoEquipo"].Visible = false;
            if (DGV_Equipos.Columns.Contains("IdMarca"))
                DGV_Equipos.Columns["IdMarca"].Visible = false;
            if (DGV_Equipos.Columns.Contains("Observaciones"))
                DGV_Equipos.Columns["Observaciones"].Visible = false;

            DGV_Equipos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Equipos.Columns.Contains(nombreColumna))
                return;

            DGV_Equipos.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Equipos.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private FilaEquipo EquipoSeleccionado()
        {
            if (DGV_Equipos.SelectedRows.Count == 0)
                return null;

            return DGV_Equipos.SelectedRows[0].DataBoundItem as FilaEquipo;
        }

        private void CBO_Cliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_Cliente.DataSource == null)
                return;

            CargarEquipos();
        }

        private void TXT_Busqueda_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void CHK_Inactivos_CheckedChanged(object sender, EventArgs e)
        {
            CargarEquipos();
        }

        private void DGV_Equipos_SelectionChanged(object sender, EventArgs e)
        {
            AplicarPermisos();
        }

        private void BTN_Crear_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.EquiposCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            try
            {
                int idClienteFiltro = 0;

                if (CBO_Cliente.SelectedValue is int)
                    idClienteFiltro = (int)CBO_Cliente.SelectedValue;

                using (FrmEquipoEditar dlg = new FrmEquipoEditar(null, idClienteFiltro))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                        return;

                    _service.Crear(dlg.IdCliente, dlg.IdTipoEquipo, dlg.IdMarca, dlg.Modelo, dlg.NumeroSerie, dlg.Imei, dlg.Color, dlg.Observaciones);
                }

                CargarCatalogos();
                CargarComboClientes();
                CargarEquipos();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Editar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.EquiposEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaEquipo seleccionado = EquipoSeleccionado();

            if (seleccionado == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            try
            {
                Equipo equipo = _service.ObtenerPorId(seleccionado.Id);

                using (FrmEquipoEditar dlg = new FrmEquipoEditar(equipo, 0))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                        return;

                    _service.Modificar(equipo.Id, dlg.IdCliente, dlg.IdTipoEquipo, dlg.IdMarca, dlg.Modelo, dlg.NumeroSerie, dlg.Imei, dlg.Color, dlg.Observaciones);
                }

                CargarCatalogos();
                CargarComboClientes();
                CargarEquipos();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Desactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.EquiposDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaEquipo seleccionado = EquipoSeleccionado();

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
                CargarEquipos();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_Reactivar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.EquiposDesactivar))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaEquipo seleccionado = EquipoSeleccionado();

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
                CargarEquipos();
                MostrarExito("Mensaje.OperacionExitosa");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void AplicarPermisos()
        {
            bool puedeCrear = TienePermiso(CodigosPermiso.EquiposCrear);
            bool puedeEditar = TienePermiso(CodigosPermiso.EquiposEditar);
            bool puedeDesactivar = TienePermiso(CodigosPermiso.EquiposDesactivar);
            FilaEquipo seleccionado = EquipoSeleccionado();
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
