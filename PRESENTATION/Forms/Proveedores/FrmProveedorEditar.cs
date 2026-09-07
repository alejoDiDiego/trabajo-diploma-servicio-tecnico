using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Features.Proveedores;
using SERVICES.Idiomas;

namespace UI.Forms.Proveedores
{
    public partial class FrmProveedorEditar : Form, IObservador
    {
        private readonly SesionIdioma _sesionIdioma;
        private readonly bool _esEdicion;

        public string RazonSocial { get { return TXT_RazonSocial.Text.Trim(); } }
        public string Cuit { get { return TXT_Cuit.Text.Trim(); } }
        public string Telefono { get { return TXT_Telefono.Text.Trim(); } }
        public string Email { get { return TXT_Email.Text.Trim(); } }
        public string Direccion { get { return TXT_Direccion.Text.Trim(); } }
        public string Contacto { get { return TXT_Contacto.Text.Trim(); } }

        // Si proveedor es null => modo nuevo; si no => edicion (precarga campos).
        public FrmProveedorEditar(Proveedor proveedor)
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();

            if (proveedor != null)
            {
                _esEdicion = true;
                TXT_RazonSocial.Text = proveedor.RazonSocial;
                TXT_Cuit.Text = proveedor.Cuit;
                TXT_Telefono.Text = proveedor.Telefono;
                TXT_Email.Text = proveedor.Email;
                TXT_Direccion.Text = proveedor.Direccion;
                TXT_Contacto.Text = proveedor.Contacto;
            }
        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            string claveTitulo = _esEdicion ? "ProveedorEditar.TituloEditar" : "ProveedorEditar.TituloNuevo";
            LBL_Titulo.Tag = claveTitulo;
            Tag = claveTitulo;
            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            LBL_RazonSocial.Text = idiomaObservado.BuscarTraduccion(LBL_RazonSocial.Tag.ToString()) + " *";
            LBL_Cuit.Text = idiomaObservado.BuscarTraduccion(LBL_Cuit.Tag.ToString());
            LBL_Telefono.Text = idiomaObservado.BuscarTraduccion(LBL_Telefono.Tag.ToString());
            LBL_Email.Text = idiomaObservado.BuscarTraduccion(LBL_Email.Tag.ToString());
            LBL_Direccion.Text = idiomaObservado.BuscarTraduccion(LBL_Direccion.Tag.ToString());
            LBL_Contacto.Text = idiomaObservado.BuscarTraduccion(LBL_Contacto.Tag.ToString());
            BTN_Aceptar.Text = idiomaObservado.BuscarTraduccion(BTN_Aceptar.Tag.ToString());
            BTN_Cancelar.Text = idiomaObservado.BuscarTraduccion(BTN_Cancelar.Tag.ToString());
        }

        private void FrmProveedorEditar_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(TXT_RazonSocial.Text))
                {
                    MessageBox.Show(
                        _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ProveedorCamposObligatorios"),
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
