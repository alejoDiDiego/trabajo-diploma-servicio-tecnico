using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    public partial class FrmOrdenesServicio : Form, IObservador
    {
        private class FilaOrden
        {
            public int Id { get; set; }
            public int Numero { get; set; }
            public int IdCliente { get; set; }
            public string ClienteNombre { get; set; }
            public int IdEquipo { get; set; }
            public string EquipoDesc { get; set; }
            public int TecnicoId { get; set; }
            public string TecnicoNombre { get; set; }
            public string EstadoRaw { get; set; }
            public string ResultadoRaw { get; set; }
            public string Problema { get; set; }
            public string Estado { get; set; }
            public string Resultado { get; set; }
            public DateTime Fecha { get; set; }
        }

        private class ItemCliente
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private class ItemEstado
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }

        private static readonly string[] EstadosCP2 = new string[]
        {
            EstadoOrdenServicio.Recibido,
            EstadoOrdenServicio.EnDiagnostico,
            EstadoOrdenServicio.PendientePresupuesto,
            EstadoOrdenServicio.EsperandoRespuesta,
            EstadoOrdenServicio.AutorizadoReparacion,
            EstadoOrdenServicio.ListoRetiro,
            EstadoOrdenServicio.Entregado
        };

        private BindingList<FilaOrden> _ordenesBindingList = null;
        private List<OrdenServicio> _todos = new List<OrdenServicio>();
        private List<Cliente> _clientes = new List<Cliente>();
        private List<Equipo> _equipos = new List<Equipo>();
        private List<Usuario> _tecnicos = new List<Usuario>();
        private readonly SesionIdioma _sesionIdioma;
        private readonly OrdenServicioService _service;
        private readonly ClienteService _clienteService;
        private readonly EquipoService _equipoService;
        private bool _cargandoCombos = false;

        public FrmOrdenesServicio()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new OrdenServicioService();
            _clienteService = new ClienteService();
            _equipoService = new EquipoService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Cliente.Text = idiomaObservado.BuscarTraduccion(LBL_Cliente.Tag.ToString());
            LBL_Estado.Text = idiomaObservado.BuscarTraduccion(LBL_Estado.Tag.ToString());
            LBL_Busqueda.Text = idiomaObservado.BuscarTraduccion(LBL_Busqueda.Tag.ToString());
            CHK_Entregadas.Text = idiomaObservado.BuscarTraduccion(CHK_Entregadas.Tag.ToString());
            BTN_Crear.Text = idiomaObservado.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Detalle.Text = idiomaObservado.BuscarTraduccion(BTN_Detalle.Tag.ToString());
            BTN_Cancelar.Text = idiomaObservado.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
            BTN_Entregar.Text = idiomaObservado.BuscarTraduccion(BTN_Entregar.Tag.ToString());

            CargarComboClientes();
            CargarComboEstados();
            AplicarFiltro();
            ConfigurarColumnas();
            AplicarPermisos();
        }

        private void FrmOrdenesServicio_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            CargarCatalogos();
            CargarComboClientes();
            CargarComboEstados();
            ActualizarTextos();
            ConfigurarColumnas();
            AplicarPermisos();

            if (!TienePermiso(CodigosPermiso.OrdenesVer))
            {
                MostrarAccesoDenegado();
                Close();
                return;
            }

            CargarOrdenes();
        }

        private void ActualizarTextos()
        {
            if (_sesionIdioma.idioma == null)
                return;

            Text = _sesionIdioma.idioma.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Cliente.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Cliente.Tag.ToString());
            LBL_Estado.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Estado.Tag.ToString());
            LBL_Busqueda.Text = _sesionIdioma.idioma.BuscarTraduccion(LBL_Busqueda.Tag.ToString());
            CHK_Entregadas.Text = _sesionIdioma.idioma.BuscarTraduccion(CHK_Entregadas.Tag.ToString());
            BTN_Crear.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Crear.Tag.ToString());
            BTN_Detalle.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Detalle.Tag.ToString());
            BTN_Cancelar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
            BTN_Entregar.Text = _sesionIdioma.idioma.BuscarTraduccion(BTN_Entregar.Tag.ToString());
        }

        private void CargarCatalogos()
        {
            try
            {
                _clientes = _clienteService.Listar(false);
                _equipos = _equipoService.Listar(true);
                _tecnicos = _service.ListarTecnicosElegibles();
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
                    ? _sesionIdioma.idioma.BuscarTraduccion("Ordenes.Todos")
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

        private void CargarComboEstados()
        {
            _cargandoCombos = true;

            try
            {
                string todos = _sesionIdioma.idioma != null
                    ? _sesionIdioma.idioma.BuscarTraduccion("Ordenes.Todos")
                    : "Todos";

                List<ItemEstado> items = new List<ItemEstado>();
                items.Add(new ItemEstado { Id = "", Nombre = todos });

                foreach (string estado in EstadosCP2)
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

        private void CargarOrdenes()
        {
            try
            {
                int idCliente = 0;

                if (CBO_Cliente.SelectedValue is int)
                    idCliente = (int)CBO_Cliente.SelectedValue;

                bool incluirEntregadas = CHK_Entregadas.Checked;

                if (idCliente > 0)
                    _todos = _service.ListarPorCliente(idCliente, incluirEntregadas);
                else
                    _todos = _service.Listar(incluirEntregadas);

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
            string estadoFiltro = "";

            if (CBO_Estado.SelectedValue is string)
                estadoFiltro = (string)CBO_Estado.SelectedValue;

            List<FilaOrden> filas = new List<FilaOrden>();

            foreach (OrdenServicio o in _todos)
            {
                if (!string.IsNullOrEmpty(estadoFiltro) && o.Estado != estadoFiltro)
                    continue;

                if (!string.IsNullOrEmpty(texto))
                {
                    string numero = o.NumeroOrden.ToString();
                    string problema = (o.ProblemaInformado ?? "").ToLowerInvariant();

                    if (!numero.Contains(texto) && !problema.Contains(texto))
                        continue;
                }

                FilaOrden fila = new FilaOrden
                {
                    Id = o.Id,
                    Numero = o.NumeroOrden,
                    IdCliente = o.IdCliente,
                    ClienteNombre = ResolverCliente(o.IdCliente),
                    IdEquipo = o.IdEquipo,
                    EquipoDesc = ResolverEquipo(o.IdEquipo),
                    TecnicoId = o.IdTecnicoAsignado.HasValue ? o.IdTecnicoAsignado.Value : 0,
                    TecnicoNombre = ResolverTecnico(o.IdTecnicoAsignado),
                    EstadoRaw = o.Estado,
                    ResultadoRaw = o.Resultado,
                    Problema = o.ProblemaInformado,
                    Estado = TraducirEstado(o.Estado),
                    Resultado = TraducirResultado(o.Resultado),
                    Fecha = o.FechaIngreso
                };

                filas.Add(fila);
            }

            _ordenesBindingList = new BindingList<FilaOrden>(filas);
            DGV_Ordenes.DataSource = _ordenesBindingList;
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

            string desc = (e.Modelo ?? "").Trim();

            if (!string.IsNullOrEmpty(e.NumeroSerie))
                desc = (desc + " (S/N " + e.NumeroSerie.Trim() + ")").Trim();

            if (string.IsNullOrEmpty(desc))
                desc = "#" + idEquipo;

            return desc;
        }

        private string ResolverTecnico(int? idTecnico)
        {
            if (!idTecnico.HasValue || idTecnico.Value <= 0)
                return _sesionIdioma.idioma != null
                    ? _sesionIdioma.idioma.BuscarTraduccion("OrdenDetalle.SinAsignar")
                    : "-";

            Usuario u = _tecnicos.FirstOrDefault(x => x.Id == idTecnico.Value);
            return u != null ? u.Username : "#" + idTecnico.Value;
        }

        private string TraducirEstado(string estado)
        {
            if (string.IsNullOrEmpty(estado))
                return "";

            if (_sesionIdioma.idioma == null)
                return estado;

            return _sesionIdioma.idioma.BuscarTraduccion("Estado." + estado);
        }

        private string TraducirResultado(string resultado)
        {
            if (_sesionIdioma.idioma == null)
                return resultado ?? "";

            if (string.IsNullOrEmpty(resultado))
                return _sesionIdioma.idioma.BuscarTraduccion("Resultado.SinResultado");

            return _sesionIdioma.idioma.BuscarTraduccion("Resultado." + resultado);
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Ordenes.Columns.Count == 0)
                return;

            ConfigurarColumna("Numero", "Columna.Numero");
            ConfigurarColumna("ClienteNombre", "Columna.Cliente");
            ConfigurarColumna("EquipoDesc", "Columna.Equipo");
            ConfigurarColumna("TecnicoNombre", "Columna.Tecnico");
            ConfigurarColumna("Estado", "Columna.Estado");
            ConfigurarColumna("Resultado", "Columna.Resultado");
            ConfigurarColumna("Fecha", "Columna.Fecha");

            if (DGV_Ordenes.Columns.Contains("Id"))
                DGV_Ordenes.Columns["Id"].Visible = false;
            if (DGV_Ordenes.Columns.Contains("IdCliente"))
                DGV_Ordenes.Columns["IdCliente"].Visible = false;
            if (DGV_Ordenes.Columns.Contains("IdEquipo"))
                DGV_Ordenes.Columns["IdEquipo"].Visible = false;
            if (DGV_Ordenes.Columns.Contains("TecnicoId"))
                DGV_Ordenes.Columns["TecnicoId"].Visible = false;
            if (DGV_Ordenes.Columns.Contains("EstadoRaw"))
                DGV_Ordenes.Columns["EstadoRaw"].Visible = false;
            if (DGV_Ordenes.Columns.Contains("ResultadoRaw"))
                DGV_Ordenes.Columns["ResultadoRaw"].Visible = false;
            if (DGV_Ordenes.Columns.Contains("Problema"))
                DGV_Ordenes.Columns["Problema"].Visible = false;

            DGV_Ordenes.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Ordenes.Columns.Contains(nombreColumna))
                return;

            DGV_Ordenes.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Ordenes.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma == null ? claveTraduccion : _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private FilaOrden OrdenSeleccionada()
        {
            if (DGV_Ordenes.SelectedRows.Count == 0)
                return null;

            return DGV_Ordenes.SelectedRows[0].DataBoundItem as FilaOrden;
        }

        private bool EsCancelable(string estado)
        {
            return estado == EstadoOrdenServicio.Recibido
                || estado == EstadoOrdenServicio.EnDiagnostico
                || estado == EstadoOrdenServicio.PendientePresupuesto
                || estado == EstadoOrdenServicio.EsperandoRespuesta
                || estado == EstadoOrdenServicio.AutorizadoReparacion;
        }

        private void CBO_Cliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_Cliente.DataSource == null)
                return;

            CargarOrdenes();
        }

        private void CBO_Estado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoCombos)
                return;

            if (CBO_Estado.DataSource == null)
                return;

            AplicarFiltro();
        }

        private void TXT_Busqueda_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void CHK_Entregadas_CheckedChanged(object sender, EventArgs e)
        {
            CargarOrdenes();
        }

        private void DGV_Ordenes_SelectionChanged(object sender, EventArgs e)
        {
            AplicarPermisos();
        }

        private void DGV_Ordenes_DoubleClick(object sender, EventArgs e)
        {
            BTN_Detalle_Click(sender, e);
        }

        private void BTN_Crear_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

            using (FrmOrdenServicioDetalle dlg = new FrmOrdenServicioDetalle())
            {
                dlg.ShowDialog(this);
            }

            CargarCatalogos();
            CargarComboClientes();
            CargarOrdenes();
        }

        private void BTN_Detalle_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesVer))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaOrden seleccionada = OrdenSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            using (FrmOrdenServicioDetalle dlg = new FrmOrdenServicioDetalle(seleccionada.Id))
            {
                dlg.ShowDialog(this);
            }

            CargarCatalogos();
            CargarComboClientes();
            CargarOrdenes();
        }

        private void BTN_Cancelar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.OrdenesCancelar))
            {
                MostrarAccesoDenegado();
                return;
            }

            FilaOrden seleccionada = OrdenSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (!EsCancelable(seleccionada.EstadoRaw))
            {
                MostrarAdvertencia("Mensaje.OrdenNoCancelable");
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarCancelar"),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarCancelacion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.No)
                return;

            string motivo = PedirMotivo("Titulo.ConfirmarCancelacion", "OrdenDetalle.Motivo");

            if (motivo == null)
                return;

            if (string.IsNullOrWhiteSpace(motivo))
            {
                MostrarAdvertencia("Mensaje.OrdenCamposObligatorios");
                return;
            }

            try
            {
                _service.CancelarOrden(seleccionada.Id, motivo);
                CargarOrdenes();
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

            FilaOrden seleccionada = OrdenSeleccionada();

            if (seleccionada == null)
            {
                MostrarAdvertencia("Mensaje.SeleccioneRegistro");
                return;
            }

            if (seleccionada.EstadoRaw != EstadoOrdenServicio.ListoRetiro)
            {
                MostrarAdvertencia("Mensaje.OrdenNoEntregable");
                return;
            }

            using (FrmOrdenServicioDetalle dlg = new FrmOrdenServicioDetalle(seleccionada.Id, "OrdenDetalle.TabEntrega"))
            {
                dlg.ShowDialog(this);
            }

            CargarOrdenes();
        }

        private string PedirMotivo(string claveTitulo, string claveEtiqueta)
        {
            IIdioma idioma = _sesionIdioma.idioma;
            string titulo = idioma != null ? idioma.BuscarTraduccion(claveTitulo) : claveTitulo;
            string etiqueta = idioma != null ? idioma.BuscarTraduccion(claveEtiqueta) : claveEtiqueta;
            string aceptar = idioma != null ? idioma.BuscarTraduccion("Accion.Aceptar") : "Aceptar";
            string cancelar = idioma != null ? idioma.BuscarTraduccion("Accion.Cancelar") : "Cancelar";

            using (Form dlg = new Form())
            {
                dlg.Text = titulo;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;
                dlg.MinimizeBox = false;
                dlg.ShowInTaskbar = false;
                dlg.ClientSize = new Size(420, 150);

                Label lbl = new Label();
                lbl.Text = etiqueta;
                lbl.Font = new Font("Segoe UI", 9F);
                lbl.Location = new Point(15, 12);
                lbl.AutoSize = true;

                TextBox txt = new TextBox();
                txt.Multiline = true;
                txt.Location = new Point(15, 34);
                txt.Size = new Size(390, 60);

                Button btnOk = new Button();
                btnOk.Text = aceptar;
                btnOk.DialogResult = DialogResult.OK;
                btnOk.Location = new Point(225, 105);
                btnOk.Size = new Size(95, 30);
                btnOk.BackColor = Color.FromArgb(39, 174, 96);
                btnOk.FlatStyle = FlatStyle.Flat;
                btnOk.ForeColor = Color.White;

                Button btnCancel = new Button();
                btnCancel.Text = cancelar;
                btnCancel.DialogResult = DialogResult.Cancel;
                btnCancel.Location = new Point(330, 105);
                btnCancel.Size = new Size(75, 30);
                btnCancel.BackColor = Color.Gray;
                btnCancel.FlatStyle = FlatStyle.Flat;
                btnCancel.ForeColor = Color.White;

                dlg.Controls.Add(lbl);
                dlg.Controls.Add(txt);
                dlg.Controls.Add(btnOk);
                dlg.Controls.Add(btnCancel);
                dlg.AcceptButton = btnOk;
                dlg.CancelButton = btnCancel;

                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return null;

                return txt.Text.Trim();
            }
        }

        private void AplicarPermisos()
        {
            bool puedeCrear = TienePermiso(CodigosPermiso.OrdenesCrear);
            bool puedeVer = TienePermiso(CodigosPermiso.OrdenesVer);
            bool puedeCancelar = TienePermiso(CodigosPermiso.OrdenesCancelar);
            bool puedeEntregar = TienePermiso(CodigosPermiso.OrdenesEntregar);
            FilaOrden seleccionada = OrdenSeleccionada();
            bool haySeleccion = seleccionada != null;

            BTN_Crear.Visible = puedeCrear;
            BTN_Detalle.Visible = puedeVer;
            BTN_Cancelar.Visible = puedeCancelar;
            BTN_Entregar.Visible = puedeEntregar;

            BTN_Crear.Enabled = puedeCrear;
            BTN_Detalle.Enabled = puedeVer && haySeleccion;
            BTN_Cancelar.Enabled = puedeCancelar && haySeleccion && seleccionada != null && EsCancelable(seleccionada.EstadoRaw);
            BTN_Entregar.Enabled = puedeEntregar && haySeleccion && seleccionada != null && seleccionada.EstadoRaw == EstadoOrdenServicio.ListoRetiro;
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
