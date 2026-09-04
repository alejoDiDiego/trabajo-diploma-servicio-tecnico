using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Features.Repuestos;
using SERVICES.Idiomas;

namespace UI.Forms.Repuestos
{
    public partial class FrmAjusteStock : Form, IObservador
    {
        private readonly SesionIdioma _sesionIdioma;
        private readonly int _stockActual;

        public int Cantidad { get { return (int)NUM_Cantidad.Value; } }
        public string Motivo { get { return TXT_Motivo.Text.Trim(); } }

        public FrmAjusteStock(Repuesto repuesto)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _stockActual = repuesto != null ? repuesto.StockActual : 0;
            InitializeComponent();
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_StockActual.Text = idiomaObservado.BuscarTraduccion(LBL_StockActual.Tag.ToString()).Replace("{0}", _stockActual.ToString());
            LBL_Cantidad.Text = idiomaObservado.BuscarTraduccion(LBL_Cantidad.Tag.ToString()) + " *";
            LBL_Motivo.Text = idiomaObservado.BuscarTraduccion(LBL_Motivo.Tag.ToString()) + " *";
            BTN_Aceptar.Text = idiomaObservado.BuscarTraduccion(BTN_Aceptar.Tag.ToString());
            BTN_Cancelar.Text = idiomaObservado.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
        }

        private void FrmAjusteStock_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (NUM_Cantidad.Value == 0 || string.IsNullOrWhiteSpace(TXT_Motivo.Text))
                {
                    MessageBox.Show(
                        _sesionIdioma.idioma.BuscarTraduccion("Mensaje.AjusteCamposObligatorios"),
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
