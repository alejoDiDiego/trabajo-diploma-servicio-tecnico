using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Features.Clientes;
using SERVICES.Idiomas;

namespace UI.Forms.Clientes
{
    public partial class FrmClienteEditar : Form, IObservador
    {
        private readonly SesionIdioma _sesionIdioma;
        private readonly bool _esEdicion;

        public string Nombre { get { return TXT_Nombre.Text.Trim(); } }
        public string Apellido { get { return TXT_Apellido.Text.Trim(); } }
        public string Documento { get { return TXT_Documento.Text.Trim(); } }
        public string Telefono { get { return TXT_Telefono.Text.Trim(); } }
        public string Email { get { return TXT_Email.Text.Trim(); } }
        public string Direccion { get { return TXT_Direccion.Text.Trim(); } }
        public string Observaciones { get { return TXT_Observaciones.Text.Trim(); } }

        // Si cliente es null => modo nuevo; si no => edicion (precarga campos).
        public FrmClienteEditar(Cliente cliente)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();

            if (cliente != null)
            {
                _esEdicion = true;
                TXT_Nombre.Text = cliente.Nombre;
                TXT_Apellido.Text = cliente.Apellido;
                TXT_Documento.Text = cliente.Documento;
                TXT_Telefono.Text = cliente.Telefono;
                TXT_Email.Text = cliente.Email;
                TXT_Direccion.Text = cliente.Direccion;
                TXT_Observaciones.Text = cliente.Observaciones;
            }
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            string claveTitulo = _esEdicion ? "ClienteEditar.TituloEditar" : "ClienteEditar.TituloNuevo";
            LBL_Titulo.Tag = claveTitulo;
            Tag = claveTitulo;
            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_Nombre.Text = idiomaObservado.BuscarTraduccion(LBL_Nombre.Tag.ToString()) + " *";
            LBL_Apellido.Text = idiomaObservado.BuscarTraduccion(LBL_Apellido.Tag.ToString()) + " *";
            LBL_Documento.Text = idiomaObservado.BuscarTraduccion(LBL_Documento.Tag.ToString()) + " *";
            LBL_Telefono.Text = idiomaObservado.BuscarTraduccion(LBL_Telefono.Tag.ToString()) + " *";
            LBL_Email.Text = idiomaObservado.BuscarTraduccion(LBL_Email.Tag.ToString());
            LBL_Direccion.Text = idiomaObservado.BuscarTraduccion(LBL_Direccion.Tag.ToString());
            LBL_Observaciones.Text = idiomaObservado.BuscarTraduccion(LBL_Observaciones.Tag.ToString());
            BTN_Aceptar.Text = idiomaObservado.BuscarTraduccion(BTN_Aceptar.Tag.ToString());
            BTN_Cancelar.Text = idiomaObservado.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
        }

        private void FrmClienteEditar_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(TXT_Nombre.Text) ||
                    string.IsNullOrWhiteSpace(TXT_Apellido.Text) ||
                    string.IsNullOrWhiteSpace(TXT_Documento.Text) ||
                    string.IsNullOrWhiteSpace(TXT_Telefono.Text))
                {
                    MessageBox.Show(
                        _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ClienteCamposObligatorios"),
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
