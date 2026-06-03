using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Idiomas;
using DOMAIN.Features.Idiomas;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Idiomas
{
    public partial class FrmAdministrarTraducciones : Form, IObservador
    {
        private readonly IdiomaService _idiomaService;
        private readonly SesionIdioma _sesionIdioma;
        private int _idTraduccionSeleccionada;
        private int _idIdiomaSeleccionado;

        public FrmAdministrarTraducciones()
        {
            _idiomaService = new IdiomaService();
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            GBX_Traducciones.Text = idiomaObservado.BuscarTraduccion(GBX_Traducciones.Tag.ToString());
            GBX_Idiomas.Text = idiomaObservado.BuscarTraduccion(GBX_Idiomas.Tag.ToString());
            LBL_Clave.Text = idiomaObservado.BuscarTraduccion(LBL_Clave.Tag.ToString());
            LBL_IdiomaTraduccion.Text = idiomaObservado.BuscarTraduccion(LBL_IdiomaTraduccion.Tag.ToString());
            LBL_Texto.Text = idiomaObservado.BuscarTraduccion(LBL_Texto.Tag.ToString());
            BTN_CrearTraduccion.Text = idiomaObservado.BuscarTraduccion(BTN_CrearTraduccion.Tag.ToString());
            BTN_EditarTraduccion.Text = idiomaObservado.BuscarTraduccion(BTN_EditarTraduccion.Tag.ToString());
            BTN_EliminarTraduccion.Text = idiomaObservado.BuscarTraduccion(BTN_EliminarTraduccion.Tag.ToString());
            BTN_LimpiarTraduccion.Text = idiomaObservado.BuscarTraduccion(BTN_LimpiarTraduccion.Tag.ToString());
            LBL_NombreIdioma.Text = idiomaObservado.BuscarTraduccion(LBL_NombreIdioma.Tag.ToString());
            BTN_CrearIdioma.Text = idiomaObservado.BuscarTraduccion(BTN_CrearIdioma.Tag.ToString());
            BTN_EditarIdioma.Text = idiomaObservado.BuscarTraduccion(BTN_EditarIdioma.Tag.ToString());
            BTN_EliminarIdioma.Text = idiomaObservado.BuscarTraduccion(BTN_EliminarIdioma.Tag.ToString());
            BTN_LimpiarIdioma.Text = idiomaObservado.BuscarTraduccion(BTN_LimpiarIdioma.Tag.ToString());

            ConfigurarColumnasTraducciones();
            ConfigurarColumnasIdiomas();
        }

        private void FrmAdministrarTraducciones_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);

            if (!SessionManager.HaySesionActiva())
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Close();
                return;
            }

            CargarIdiomas();
            CargarTraducciones();
            Actualizar(_sesionIdioma.idioma);
        }

        private void CargarIdiomas()
        {
            var idiomas = _idiomaService.Listar();
            DGV_Idiomas.DataSource = new BindingList<Idioma>(idiomas);
            CBX_Idiomas.DataSource = null;
            CBX_Idiomas.DataSource = idiomas.ToList();
            CBX_Idiomas.DisplayMember = "Nombre";
            CBX_Idiomas.ValueMember = "Id";
            ConfigurarColumnasIdiomas();
        }

        private void CargarTraducciones()
        {
            DGV_Traducciones.DataSource = new BindingList<TraduccionItem>(_idiomaService.ListarTraducciones());
            ConfigurarColumnasTraducciones();
        }

        private void DGV_Traducciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV_Traducciones.SelectedRows.Count == 0)
                return;

            TraduccionItem traduccion = (TraduccionItem)DGV_Traducciones.SelectedRows[0].DataBoundItem;

            _idTraduccionSeleccionada = traduccion.IdTraduccion;
            TBX_Clave.Text = traduccion.Clave;
            TBX_Clave.ReadOnly = true;
            CBX_Idiomas.SelectedValue = traduccion.IdIdioma;
            TBX_Texto.Text = traduccion.Texto;
        }

        private void BTN_CrearTraduccion_Click(object sender, EventArgs e)
        {
            try
            {
                int idIdioma = ObtenerIdIdiomaSeleccionado();

                if (idIdioma == 0)
                {
                    MostrarAdvertencia("Mensaje.SeleccioneIdioma");
                    return;
                }

                _idiomaService.CrearTraduccion(idIdioma, TBX_Clave.Text, TBX_Texto.Text);
                CargarTraducciones();
                LimpiarTraduccion();
                RefrescarIdiomaActual();
                MostrarExito("Mensaje.TraduccionCreada");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_EditarTraduccion_Click(object sender, EventArgs e)
        {
            try
            {
                if (_idTraduccionSeleccionada == 0)
                {
                    MostrarAdvertencia("Mensaje.SeleccioneTraduccion");
                    return;
                }

                int idIdioma = ObtenerIdIdiomaSeleccionado();

                if (idIdioma == 0)
                {
                    MostrarAdvertencia("Mensaje.SeleccioneIdioma");
                    return;
                }

                _idiomaService.ModificarTraduccion(_idTraduccionSeleccionada, idIdioma, TBX_Texto.Text);
                CargarTraducciones();
                LimpiarTraduccion();
                RefrescarIdiomaActual();
                MostrarExito("Mensaje.TraduccionEditada");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_EliminarTraduccion_Click(object sender, EventArgs e)
        {
            try
            {
                if (_idTraduccionSeleccionada == 0)
                {
                    MostrarAdvertencia("Mensaje.SeleccioneTraduccion");
                    return;
                }

                DialogResult confirmacion = MessageBox.Show(
                    string.Format(_sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarEliminarTraduccion"), TBX_Clave.Text),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarEliminacion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmacion == DialogResult.No)
                    return;

                _idiomaService.EliminarTraduccion(_idTraduccionSeleccionada);
                CargarTraducciones();
                LimpiarTraduccion();
                RefrescarIdiomaActual();
                MostrarExito("Mensaje.TraduccionEliminada");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_LimpiarTraduccion_Click(object sender, EventArgs e)
        {
            LimpiarTraduccion();
        }

        private void DGV_Idiomas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV_Idiomas.SelectedRows.Count == 0)
                return;

            Idioma idioma = (Idioma)DGV_Idiomas.SelectedRows[0].DataBoundItem;

            _idIdiomaSeleccionado = idioma.Id;
            TBX_NombreIdioma.Text = idioma.Nombre;
        }

        private void BTN_CrearIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                _idiomaService.CrearIdioma(TBX_NombreIdioma.Text);
                CargarIdiomas();
                LimpiarIdioma();
                RefrescarIdiomaActual();
                MostrarExito("Mensaje.IdiomaCreado");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_EditarIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                if (_idIdiomaSeleccionado == 0)
                {
                    MostrarAdvertencia("Mensaje.SeleccioneIdioma");
                    return;
                }

                _idiomaService.ModificarIdioma(_idIdiomaSeleccionado, TBX_NombreIdioma.Text);
                CargarIdiomas();
                CargarTraducciones();
                LimpiarIdioma();
                RefrescarIdiomaActual();
                MostrarExito("Mensaje.IdiomaEditado");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_EliminarIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                if (_idIdiomaSeleccionado == 0)
                {
                    MostrarAdvertencia("Mensaje.SeleccioneIdioma");
                    return;
                }

                if (_idiomaService.Listar().Count <= 1)
                {
                    MostrarAdvertencia("Mensaje.NoEliminarUltimoIdioma");
                    return;
                }

                DialogResult confirmacion = MessageBox.Show(
                    string.Format(_sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarEliminarIdioma"), TBX_NombreIdioma.Text),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.ConfirmarEliminacion"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmacion == DialogResult.No)
                    return;

                _idiomaService.EliminarIdioma(_idIdiomaSeleccionado);
                CargarIdiomas();
                CargarTraducciones();
                LimpiarIdioma();
                RefrescarIdiomaActual();
                MostrarExito("Mensaje.IdiomaEliminado");
            }
            catch (Exception ex)
            {
                MostrarError(ex);
            }
        }

        private void BTN_LimpiarIdioma_Click(object sender, EventArgs e)
        {
            LimpiarIdioma();
        }

        private void LimpiarTraduccion()
        {
            _idTraduccionSeleccionada = 0;
            TBX_Clave.Clear();
            TBX_Clave.ReadOnly = false;
            TBX_Texto.Clear();
            DGV_Traducciones.ClearSelection();
        }

        private void LimpiarIdioma()
        {
            _idIdiomaSeleccionado = 0;
            TBX_NombreIdioma.Clear();
            DGV_Idiomas.ClearSelection();
        }

        private int ObtenerIdIdiomaSeleccionado()
        {
            if (CBX_Idiomas.SelectedValue == null)
                return 0;

            return Convert.ToInt32(CBX_Idiomas.SelectedValue);
        }

        private void RefrescarIdiomaActual()
        {
            IIdioma idiomaActual = null;

            if (_sesionIdioma.idioma != null)
                idiomaActual = _idiomaService.ObtenerPorId(_sesionIdioma.idioma.Id);

            if (idiomaActual == null)
                idiomaActual = _idiomaService.ObtenerIdiomaPorDefecto();

            if (idiomaActual != null)
                _sesionIdioma.CambiarIdioma(idiomaActual);
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
            MessageBox.Show(
                string.Format(_sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorOperacion"), ex.Message),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void ConfigurarColumnasTraducciones()
        {
            if (DGV_Traducciones.Columns.Contains("IdTraduccion"))
                DGV_Traducciones.Columns["IdTraduccion"].HeaderText = _sesionIdioma.idioma.BuscarTraduccion("Columna.IdTraduccion");
            if (DGV_Traducciones.Columns.Contains("IdPalabra"))
                DGV_Traducciones.Columns["IdPalabra"].Visible = false;
            if (DGV_Traducciones.Columns.Contains("IdIdioma"))
                DGV_Traducciones.Columns["IdIdioma"].Visible = false;
            if (DGV_Traducciones.Columns.Contains("Clave"))
                DGV_Traducciones.Columns["Clave"].HeaderText = _sesionIdioma.idioma.BuscarTraduccion("Columna.Clave");
            if (DGV_Traducciones.Columns.Contains("Idioma"))
                DGV_Traducciones.Columns["Idioma"].HeaderText = _sesionIdioma.idioma.BuscarTraduccion("Columna.Idioma");
            if (DGV_Traducciones.Columns.Contains("Texto"))
                DGV_Traducciones.Columns["Texto"].HeaderText = _sesionIdioma.idioma.BuscarTraduccion("Columna.Texto");
        }

        private void ConfigurarColumnasIdiomas()
        {
            if (DGV_Idiomas.Columns.Contains("Id"))
                DGV_Idiomas.Columns["Id"].HeaderText = _sesionIdioma.idioma.BuscarTraduccion("Columna.Id");
            if (DGV_Idiomas.Columns.Contains("Nombre"))
                DGV_Idiomas.Columns["Nombre"].HeaderText = _sesionIdioma.idioma.BuscarTraduccion("Columna.Nombre");
            if (DGV_Idiomas.Columns.Contains("Traducciones"))
                DGV_Idiomas.Columns["Traducciones"].Visible = false;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
