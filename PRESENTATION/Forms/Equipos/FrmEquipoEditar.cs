using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Clientes;
using APPLICATION.Features.Marcas;
using APPLICATION.Features.TiposEquipo;
using DOMAIN.Features.Clientes;
using DOMAIN.Features.Equipos;
using DOMAIN.Features.Marcas;
using DOMAIN.Features.TiposEquipo;
using SERVICES.Idiomas;

namespace UI.Forms.Equipos
{
    public partial class FrmEquipoEditar : Form, IObservador
    {
        private class ItemCliente
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
        }

        private readonly SesionIdioma _sesionIdioma;
        private readonly bool _esEdicion;
        private readonly int _idClientePreseleccionado;
        private readonly int _idTipoPreseleccionado;
        private readonly int _idMarcaPreseleccionada;
        private readonly string _modeloInicial;
        private readonly string _numeroSerieInicial;
        private readonly string _imeiInicial;
        private readonly string _colorInicial;
        private readonly string _observacionesInicial;

        public int IdCliente
        {
            get
            {
                if (CBO_Cliente.SelectedValue is int)
                    return (int)CBO_Cliente.SelectedValue;

                ItemCliente item = CBO_Cliente.SelectedItem as ItemCliente;
                return item != null ? item.Id : 0;
            }
        }

        public int IdTipoEquipo
        {
            get
            {
                if (CBO_Tipo.SelectedValue is int)
                    return (int)CBO_Tipo.SelectedValue;

                TipoEquipo t = CBO_Tipo.SelectedItem as TipoEquipo;
                return t != null ? t.Id : 0;
            }
        }

        public int IdMarca
        {
            get
            {
                if (CBO_Marca.SelectedValue is int)
                    return (int)CBO_Marca.SelectedValue;

                Marca m = CBO_Marca.SelectedItem as Marca;
                return m != null ? m.Id : 0;
            }
        }

        public string Modelo { get { return TXT_Modelo.Text.Trim(); } }
        public string NumeroSerie { get { return TXT_NumeroSerie.Text.Trim(); } }
        public string Imei { get { return TXT_Imei.Text.Trim(); } }
        public string Color { get { return TXT_Color.Text.Trim(); } }
        public string Observaciones { get { return TXT_Observaciones.Text.Trim(); } }

        // Si equipo es null => modo nuevo (idClientePreseleccionado precarga el combo);
        // si no => edicion (precarga todos los campos). Se copian valores simples
        // porque DOMAIN expone solo getters y el service se invoca desde el llamador.
        public FrmEquipoEditar(Equipo equipo, int idClientePreseleccionado)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();

            if (equipo != null)
            {
                _esEdicion = true;
                _idClientePreseleccionado = equipo.IdCliente;
                _idTipoPreseleccionado = equipo.IdTipoEquipo;
                _idMarcaPreseleccionada = equipo.IdMarca;
                _modeloInicial = equipo.Modelo;
                _numeroSerieInicial = equipo.NumeroSerie;
                _imeiInicial = equipo.Imei;
                _colorInicial = equipo.Color;
                _observacionesInicial = equipo.Observaciones;
                TXT_Modelo.Text = equipo.Modelo;
                TXT_NumeroSerie.Text = equipo.NumeroSerie;
                TXT_Imei.Text = equipo.Imei;
                TXT_Color.Text = equipo.Color;
                TXT_Observaciones.Text = equipo.Observaciones;
            }
            else
            {
                _idClientePreseleccionado = idClientePreseleccionado;
            }
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            string claveTitulo = _esEdicion ? "EquipoEditar.TituloEditar" : "EquipoEditar.TituloNuevo";
            LBL_Titulo.Tag = claveTitulo;
            Tag = claveTitulo;
            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Cliente.Text = idiomaObservado.BuscarTraduccion(LBL_Cliente.Tag.ToString()) + " *";
            LBL_Tipo.Text = idiomaObservado.BuscarTraduccion(LBL_Tipo.Tag.ToString()) + " *";
            LBL_Marca.Text = idiomaObservado.BuscarTraduccion(LBL_Marca.Tag.ToString()) + " *";
            LBL_Modelo.Text = idiomaObservado.BuscarTraduccion(LBL_Modelo.Tag.ToString());
            LBL_NumeroSerie.Text = idiomaObservado.BuscarTraduccion(LBL_NumeroSerie.Tag.ToString());
            LBL_Imei.Text = idiomaObservado.BuscarTraduccion(LBL_Imei.Tag.ToString());
            LBL_Color.Text = idiomaObservado.BuscarTraduccion(LBL_Color.Tag.ToString());
            LBL_Observaciones.Text = idiomaObservado.BuscarTraduccion(LBL_Observaciones.Tag.ToString());
            BTN_Aceptar.Text = idiomaObservado.BuscarTraduccion(BTN_Aceptar.Tag.ToString());
            BTN_Cancelar.Text = idiomaObservado.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
        }

        private void FrmEquipoEditar_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);

            // Cargar combos en Load (no en ctor) para que el Designer ya este listo;
            // los TEXT se restauran despues por si el Designer los piso.
            CargarCombos(_idClientePreseleccionado, _idTipoPreseleccionado, _idMarcaPreseleccionada, _esEdicion);

            if (_esEdicion)
            {
                TXT_Modelo.Text = _modeloInicial;
                TXT_NumeroSerie.Text = _numeroSerieInicial;
                TXT_Imei.Text = _imeiInicial;
                TXT_Color.Text = _colorInicial;
                TXT_Observaciones.Text = _observacionesInicial;
            }

            Actualizar(_sesionIdioma.idioma);
        }

        private void CargarCombos(int idCliente, int idTipo, int idMarca, bool incluirInactivosCatalogos)
        {
            ClienteService clienteService = new ClienteService();
            TipoEquipoService tipoService = new TipoEquipoService();
            MarcaService marcaService = new MarcaService();

            // Al crear solo se ofrecen clientes activos; al editar se resuelve el cliente aunque este inactivo.
            // Se usa ItemCliente (Id/Nombre) para DisplayMember estable sin depender de props de DOMAIN.
            List<Cliente> clientes = clienteService.Listar(false);

            if (idCliente > 0 && clientes.Find(x => x.Id == idCliente) == null)
            {
                try
                {
                    Cliente extra = clienteService.ObtenerPorId(idCliente);
                    if (extra != null)
                        clientes.Add(extra);
                }
                catch
                {
                }
            }

            List<ItemCliente> itemsClientes = new List<ItemCliente>();

            foreach (Cliente c in clientes)
                itemsClientes.Add(new ItemCliente { Id = c.Id, Nombre = c.Apellido + ", " + c.Nombre + " (" + c.Documento + ")" });

            CBO_Cliente.DataSource = null;
            CBO_Cliente.DisplayMember = "Nombre";
            CBO_Cliente.ValueMember = "Id";
            CBO_Cliente.DataSource = itemsClientes;

            if (idCliente > 0)
                CBO_Cliente.SelectedValue = idCliente;

            List<TipoEquipo> tipos = tipoService.Listar(incluirInactivosCatalogos);
            CBO_Tipo.DataSource = null;
            CBO_Tipo.DisplayMember = "Nombre";
            CBO_Tipo.ValueMember = "Id";
            CBO_Tipo.DataSource = tipos;

            if (idTipo > 0)
                CBO_Tipo.SelectedValue = idTipo;

            List<Marca> marcas = marcaService.Listar(incluirInactivosCatalogos);
            CBO_Marca.DataSource = null;
            CBO_Marca.DisplayMember = "Nombre";
            CBO_Marca.ValueMember = "Id";
            CBO_Marca.DataSource = marcas;

            if (idMarca > 0)
                CBO_Marca.SelectedValue = idMarca;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (IdCliente <= 0 || IdTipoEquipo <= 0 || IdMarca <= 0)
                {
                    MessageBox.Show(
                        _sesionIdioma.idioma.BuscarTraduccion("Mensaje.EquipoCamposObligatorios"),
                        _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
            }

            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
