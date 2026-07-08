using System;
using System.ComponentModel;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.ControlCambios;
using DOMAIN.Features.ControlCambios;
using DOMAIN.Features.Permisos;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.ControlCambios
{
    public partial class FrmControlCambios : Form, IObservador
    {
        private BindingList<ControlCambio> _cambiosBindingList = null;
        private readonly SesionIdioma _sesionIdioma;
        private readonly ControlCambioService _service;

        public FrmControlCambios()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _service = new ControlCambioService();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            BTN_Restaurar.Text = idiomaObservado.BuscarTraduccion(BTN_Restaurar.Tag.ToString());

            ConfigurarColumnas();
            ToggleRestaurarBoton();
        }

        private void FrmControlCambios_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);

            if (!PuedeVerControlCambios())
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            CargarCambios();
            AplicarPermisos();
        }

        private void CargarCambios()
        {
            _cambiosBindingList = new BindingList<ControlCambio>(_service.Listar());
            DGV_Cambios.DataSource = _cambiosBindingList;
            ConfigurarColumnas();
        }

        private void ConfigurarColumnas()
        {
            if (DGV_Cambios.Columns.Count == 0)
                return;

            ConfigurarColumna("FechaCambio", "ControlCambios.Fecha");
            ConfigurarColumna("UsuarioModifico", "ControlCambios.Usuario");
            ConfigurarColumna("TipoCambio", "ControlCambios.Tipo");
            ConfigurarColumna("ClaveRegistro", "ControlCambios.Clave");
            ConfigurarColumna("ValorAnterior", "ControlCambios.ValorAnterior");
            ConfigurarColumna("ValorNuevo", "ControlCambios.ValorNuevo");

            if (DGV_Cambios.Columns.Contains("Id"))
                DGV_Cambios.Columns["Id"].Visible = false;
            if (DGV_Cambios.Columns.Contains("TablaAfectada"))
                DGV_Cambios.Columns["TablaAfectada"].Visible = false;
            if (DGV_Cambios.Columns.Contains("IdIdioma"))
                DGV_Cambios.Columns["IdIdioma"].Visible = false;
            if (DGV_Cambios.Columns.Contains("IdPalabra"))
                DGV_Cambios.Columns["IdPalabra"].Visible = false;
            if (DGV_Cambios.Columns.Contains("CampoModificado"))
                DGV_Cambios.Columns["CampoModificado"].Visible = false;

            DGV_Cambios.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ConfigurarColumna(string nombreColumna, string claveTraduccion)
        {
            if (!DGV_Cambios.Columns.Contains(nombreColumna))
                return;

            DGV_Cambios.Columns[nombreColumna].Tag = claveTraduccion;
            DGV_Cambios.Columns[nombreColumna].HeaderText = _sesionIdioma.idioma.BuscarTraduccion(claveTraduccion);
        }

        private void DGV_Cambios_SelectionChanged(object sender, EventArgs e)
        {
            ToggleRestaurarBoton();
        }

        private void ToggleRestaurarBoton()
        {
            BTN_Restaurar.Enabled = DGV_Cambios.SelectedRows.Count > 0;
        }

        private void BTN_Restaurar_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.ControlCambiosRestaurar))
            {
                MostrarAccesoDenegado();
                return;
            }

            if (DGV_Cambios.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("ControlCambios.SeleccioneCambio"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Atencion"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var cambio = (ControlCambio)DGV_Cambios.SelectedRows[0].DataBoundItem;

            if (cambio.TipoCambio != "UPDATE")
            {
                MostrarAccesoDenegado();
                return;
            }

            var confirmResult = MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarRestaurar"),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarRestauracion"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
                return;

            try
            {
                _service.Restaurar(cambio.Id);
                CargarCambios();

                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("ControlCambios.CambioRestaurado"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Exito"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("ControlCambios.ErrorRestaurar").Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AplicarPermisos()
        {
            BTN_Restaurar.Visible = TienePermiso(CodigosPermiso.ControlCambiosRestaurar);
        }

        private bool PuedeVerControlCambios()
        {
            return TienePermiso(CodigosPermiso.ControlCambiosVer);
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
