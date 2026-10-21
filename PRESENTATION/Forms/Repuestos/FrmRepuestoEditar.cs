using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Features.Repuestos;
using SERVICES.Idiomas;

namespace UI.Forms.Repuestos
{
    public partial class FrmRepuestoEditar : Form, IObservador
    {
        private readonly SesionIdioma _sesionIdioma;
        private readonly bool _esEdicion;

        public string Codigo { get { return TXT_Codigo.Text.Trim(); } }
        public string Descripcion { get { return TXT_Descripcion.Text.Trim(); } }
        public int StockInicial { get { return (int)NUM_StockInicial.Value; } }
        public int StockMinimo { get { return (int)NUM_StockMinimo.Value; } }
        public decimal CostoActual { get { return NUM_Costo.Value; } }
        public decimal PrecioReferencia { get { return NUM_Precio.Value; } }

        public FrmRepuestoEditar()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _esEdicion = false;
            InitializeComponent();
        }

        public FrmRepuestoEditar(Repuesto repuesto)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _esEdicion = true;
            InitializeComponent();

            if (repuesto != null)
            {
                TXT_Codigo.Text = repuesto.Codigo;
                TXT_Descripcion.Text = repuesto.Descripcion;
                NUM_StockMinimo.Value = repuesto.StockMinimo;
                NUM_Costo.Value = repuesto.CostoActual;
                NUM_Precio.Value = repuesto.PrecioReferencia;
            }
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            string claveTitulo = _esEdicion ? "RepuestoEditar.TituloEditar" : "RepuestoEditar.TituloNuevo";
            LBL_Titulo.Tag = claveTitulo;
            Tag = claveTitulo;
            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Codigo.Text = idiomaObservado.BuscarTraduccion(LBL_Codigo.Tag.ToString()) + " *";
            LBL_Descripcion.Text = idiomaObservado.BuscarTraduccion(LBL_Descripcion.Tag.ToString()) + " *";
            LBL_StockInicial.Text = idiomaObservado.BuscarTraduccion(LBL_StockInicial.Tag.ToString());
            LBL_StockMinimo.Text = idiomaObservado.BuscarTraduccion(LBL_StockMinimo.Tag.ToString());
            LBL_Costo.Text = idiomaObservado.BuscarTraduccion(LBL_Costo.Tag.ToString());
            LBL_Precio.Text = idiomaObservado.BuscarTraduccion(LBL_Precio.Tag.ToString());
            BTN_Aceptar.Text = idiomaObservado.BuscarTraduccion(BTN_Aceptar.Tag.ToString());
            BTN_Cancelar.Text = idiomaObservado.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
        }

        private void FrmRepuestoEditar_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);

            // El stock solo se carga al crear; en edicion se ajusta con FrmAjusteStock.
            LBL_StockInicial.Visible = !_esEdicion;
            NUM_StockInicial.Visible = !_esEdicion;

            Actualizar(_sesionIdioma.idioma);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(TXT_Codigo.Text) || string.IsNullOrWhiteSpace(TXT_Descripcion.Text))
                {
                    MessageBox.Show(
                        _sesionIdioma.idioma.BuscarTraduccion("Mensaje.RepuestoCamposObligatorios"),
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
