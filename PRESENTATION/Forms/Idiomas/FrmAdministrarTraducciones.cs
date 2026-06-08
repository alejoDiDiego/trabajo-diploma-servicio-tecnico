using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Idiomas;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Idiomas;
using SERVICES.Auth;
using SERVICES.Idiomas;

namespace UI.Forms.Idiomas
{
    public partial class FrmAdministrarTraducciones : Form, IObservador
    {
        private readonly IdiomaService _idiomaService;
        private readonly SesionIdioma _sesionIdioma;
        private int _idPalabraSeleccionada;
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
            BTN_EditarTraduccion.Text = idiomaObservado.BuscarTraduccion(BTN_EditarTraduccion.Tag.ToString());
            BTN_LimpiarTraduccion.Text = idiomaObservado.BuscarTraduccion(BTN_LimpiarTraduccion.Tag.ToString());
            LBL_NombreIdioma.Text = idiomaObservado.BuscarTraduccion(LBL_NombreIdioma.Tag.ToString());
            BTN_CrearIdioma.Text = idiomaObservado.BuscarTraduccion(BTN_CrearIdioma.Tag.ToString());
            BTN_EditarIdioma.Text = idiomaObservado.BuscarTraduccion(BTN_EditarIdioma.Tag.ToString());
            BTN_EliminarIdioma.Text = idiomaObservado.BuscarTraduccion(BTN_EliminarIdioma.Tag.ToString());
            BTN_LimpiarIdioma.Text = idiomaObservado.BuscarTraduccion(BTN_LimpiarIdioma.Tag.ToString());

            ConfigurarColumnasTraducciones();
            ConfigurarColumnasIdiomas();
            AplicarPermisos();
        }

        private void FrmAdministrarTraducciones_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);

            if (!PuedeVerPantalla())
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
            AplicarPermisos();
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
            int idIdioma = ObtenerIdIdiomaSeleccionado();

            if (idIdioma == 0)
            {
                DGV_Traducciones.DataSource = new BindingList<TraduccionEditable>();
                return;
            }

            // El combo de idiomas filtra las claves del catalogo para editar solo sus textos.
            DGV_Traducciones.DataSource = new BindingList<TraduccionEditable>(_idiomaService.ListarTraduccionesPorIdioma(idIdioma));
            ConfigurarColumnasTraducciones();
        }

        private void DGV_Traducciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV_Traducciones.SelectedRows.Count == 0)
                return;

            TraduccionEditable traduccion = (TraduccionEditable)DGV_Traducciones.SelectedRows[0].DataBoundItem;

            _idPalabraSeleccionada = traduccion.IdPalabra;
            TBX_Clave.Text = traduccion.Clave;
            TBX_Clave.ReadOnly = true;
            TBX_Texto.Text = traduccion.Texto;
        }

        private void BTN_EditarTraduccion_Click(object sender, EventArgs e)
        {
            if (!TienePermiso(CodigosPermiso.TraduccionesEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

            try
            {
                if (_idPalabraSeleccionada == 0)
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

                _idiomaService.GuardarTraduccion(idIdioma, _idPalabraSeleccionada, TBX_Texto.Text);
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

        private void BTN_LimpiarTraduccion_Click(object sender, EventArgs e)
        {
            LimpiarTraduccion();
        }

        private void CBX_Idiomas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTraducciones();
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
            if (!TienePermiso(CodigosPermiso.IdiomasCrear))
            {
                MostrarAccesoDenegado();
                return;
            }

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
            if (!TienePermiso(CodigosPermiso.IdiomasEditar))
            {
                MostrarAccesoDenegado();
                return;
            }

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
            if (!TienePermiso(CodigosPermiso.IdiomasEliminar))
            {
                MostrarAccesoDenegado();
                return;
            }

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
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ConfirmarEliminarIdioma").Replace("{0}", TBX_NombreIdioma.Text),
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
            _idPalabraSeleccionada = 0;
            TBX_Clave.Clear();
            TBX_Clave.ReadOnly = true;
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
            if (CBX_Idiomas.SelectedValue is int)
                return (int)CBX_Idiomas.SelectedValue;

            Idioma idioma = CBX_Idiomas.SelectedItem as Idioma;

            if (idioma == null)
                return 0;

            return idioma.Id;
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

        private void MostrarAccesoDenegado()
        {
            MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.SinPermisos"),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.AccesoDenegado"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        private void MostrarError(Exception ex)
        {
            MessageBox.Show(
                _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorOperacion").Replace("{0}", ex.Message),
                _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void ConfigurarColumnasTraducciones()
        {
            if (DGV_Traducciones.Columns.Contains("IdPalabra"))
                DGV_Traducciones.Columns["IdPalabra"].Visible = false;
            if (DGV_Traducciones.Columns.Contains("Clave"))
                DGV_Traducciones.Columns["Clave"].HeaderText = _sesionIdioma.idioma.BuscarTraduccion("Columna.Clave");
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

        private void AplicarPermisos()
        {
            bool puedeVerTraducciones = TienePermiso(CodigosPermiso.TraduccionesVer);
            bool puedeEditarTraducciones = TienePermiso(CodigosPermiso.TraduccionesEditar);
            bool puedeVerIdiomas = TienePermiso(CodigosPermiso.IdiomasVer);
            bool puedeCrearIdiomas = TienePermiso(CodigosPermiso.IdiomasCrear);
            bool puedeEditarIdiomas = TienePermiso(CodigosPermiso.IdiomasEditar);
            bool puedeEliminarIdiomas = TienePermiso(CodigosPermiso.IdiomasEliminar);

            GBX_Traducciones.Visible = puedeVerTraducciones || puedeEditarTraducciones;
            BTN_EditarTraduccion.Visible = puedeEditarTraducciones;
            BTN_LimpiarTraduccion.Visible = puedeEditarTraducciones;
            BTN_EditarTraduccion.Enabled = puedeEditarTraducciones;
            TBX_Clave.Enabled = puedeVerTraducciones || puedeEditarTraducciones;
            TBX_Texto.Enabled = puedeEditarTraducciones;
            CBX_Idiomas.Enabled = puedeVerTraducciones || puedeEditarTraducciones;

            GBX_Idiomas.Visible = puedeVerIdiomas || puedeCrearIdiomas || puedeEditarIdiomas || puedeEliminarIdiomas;
            BTN_CrearIdioma.Visible = puedeCrearIdiomas;
            BTN_EditarIdioma.Visible = puedeEditarIdiomas;
            BTN_EliminarIdioma.Visible = puedeEliminarIdiomas;
            BTN_LimpiarIdioma.Visible = puedeCrearIdiomas || puedeEditarIdiomas || puedeEliminarIdiomas;
            BTN_CrearIdioma.Enabled = puedeCrearIdiomas;
            BTN_EditarIdioma.Enabled = puedeEditarIdiomas;
            BTN_EliminarIdioma.Enabled = puedeEliminarIdiomas;
            TBX_NombreIdioma.Enabled = puedeCrearIdiomas || puedeEditarIdiomas;
        }

        private bool PuedeVerPantalla()
        {
            return TienePermiso(CodigosPermiso.TraduccionesVer) ||
                TienePermiso(CodigosPermiso.IdiomasVer);
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
