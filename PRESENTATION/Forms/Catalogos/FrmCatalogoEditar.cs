using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using SERVICES.Idiomas;

namespace UI.Forms.Catalogos
{
    // Mini-dialogo compartido para crear/editar Tipos de equipo y Marcas (solo campo nombre).
    public partial class FrmCatalogoEditar : Form, IObservador
    {
        private readonly SesionIdioma _sesionIdioma;
        private readonly string _titulo;

        public string Nombre { get { return TXT_Nombre.Text.Trim(); } }

        public FrmCatalogoEditar(string titulo, string valorInicial)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            _titulo = titulo;
            InitializeComponent();

            if (!string.IsNullOrEmpty(valorInicial))
                TXT_Nombre.Text = valorInicial;
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            LBL_Nombre.Text = idiomaObservado.BuscarTraduccion(LBL_Nombre.Tag.ToString()) + " *";
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            BTN_Aceptar.Text = idiomaObservado.BuscarTraduccion(BTN_Aceptar.Tag.ToString());
            BTN_Cancelar.Text = idiomaObservado.BuscarTraduccion(BTN_Cancelar.Tag.ToString());

            if (!string.IsNullOrEmpty(_titulo))
            {
                LBL_Titulo.Text = _titulo;
                Text = _titulo;
            }
        }

        private void FrmCatalogoEditar_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK && string.IsNullOrWhiteSpace(TXT_Nombre.Text))
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.NombreObligatorio"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
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
