using System;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Bitacora;
using DOMAIN.Features.Bitacora;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Bitacora
{
    public partial class FrmBitacora : Form, IObservador
    {
        private BindingList<EntradaBitacora> _bitacoraBindingList = null;
        private readonly SesionIdioma _sesionIdioma;
        private readonly BitacoraService _service;

        public FrmBitacora()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new BitacoraService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_UsuarioFiltro.Text = idiomaObservado.BuscarTraduccion(LBL_UsuarioFiltro.Tag.ToString());
            LBL_Desde.Text = idiomaObservado.BuscarTraduccion(LBL_Desde.Tag.ToString());
            LBL_Hasta.Text = idiomaObservado.BuscarTraduccion(LBL_Hasta.Tag.ToString());
            LBL_TipoFiltro.Text = idiomaObservado.BuscarTraduccion(LBL_TipoFiltro.Tag.ToString());
            BTN_Buscar.Text = idiomaObservado.BuscarTraduccion(BTN_Buscar.Tag.ToString());

            CargarTiposActividad();
            ConfigurarColumnas();
        }

        private void FrmBitacora_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            if (!PuedeVerBitacora())
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            CargarBitacora();
        }

        private void CargarTiposActividad()
        {
            string idiomaActual = _sesionIdioma.idioma.BuscarTraduccion("Bitacora.TipoTodas");

            CBO_TipoActividad.Items.Clear();
            CBO_TipoActividad.Items.Add(idiomaActual);

            string[] tipos = _service.ObtenerTiposActividad();

            foreach (string tipo in tipos)
            {
                string traducido = _sesionIdioma.idioma.BuscarTraduccion("Bitacora." + tipo);
                CBO_TipoActividad.Items.Add(traducido);
            }

            CBO_TipoActividad.SelectedIndex = 0;
        }

        private string ObtenerTipoSeleccionado()
        {
            if (CBO_TipoActividad.SelectedIndex <= 0)
                return null;

            string[] tipos = _service.ObtenerTiposActividad();
            int indice = CBO_TipoActividad.SelectedIndex - 1;

            if (indice >= 0 && indice < tipos.Length)
                return tipos[indice];

            return null;
        }

        private void CargarBitacora()
        {
            string usuario = TXT_Usuario.Text.Trim();
            DateTime? desde = DT_Desde.Checked ? (DateTime?)DT_Desde.Value : null;
            DateTime? hasta = DT_Hasta.Checked ? (DateTime?)DT_Hasta.Value : null;
            string tipoActividad = ObtenerTipoSeleccionado();

            _bitacoraBindingList = new BindingList<EntradaBitacora>(_service.Buscar(usuario, desde, hasta, tipoActividad));
            DGV_Bitacora.DataSource = _bitacoraBindingList;
            ConfigurarColumnas();
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Bitacora.Columns.Count == 0)
                return;

            ConfigurarColumna("Fecha", "Bitacora.Fecha");
            ConfigurarColumna("Usuario", "Bitacora.Usuario");
            ConfigurarColumna("Actividad", "Bitacora.Actividad");
            ConfigurarColumna("Detalle", "Bitacora.Detalle");
            ConfigurarColumna("TipoActividad", "Bitacora.TipoActividad");

            if (DGV_Bitacora.Columns.Contains("Id"))
                DGV_Bitacora.Columns["Id"].Visible = false;

            DGV_Bitacora.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Bitacora.Columns.Contains(nombreColumna))
                return;

            DGV_Bitacora.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Bitacora.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private void BTN_Buscar_Click(object sender, EventArgs e)
        {
            CargarBitacora();
        }

        private bool PuedeVerBitacora()
        {
            return TienePermiso(CodigosPermiso.BitacoraVer);
        }

        private bool TienePermiso(string codigo)
        {
            return SessionManager.HaySesionActiva() && SessionManager.TienePermiso(codigo);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
