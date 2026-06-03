using System;
using System.Windows.Forms;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.Usuarios;
using SERVICES.Idiomas;

namespace UI.Forms.Auth
{
    public partial class FrmLogin : Form, IObservador
    {
        private readonly SesionIdioma _sesionIdioma;

        public FrmLogin()
        {
            _sesionIdioma = SesionIdioma.GetInstance();
            InitializeComponent();

            UsuarioService usuarioService = new UsuarioService();

            if(usuarioService.Listar().Count == 0)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.UsuarioDefectoCreado"),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.UsuarioDefectoCreado"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                usuarioService.Crear("admin", "123");
            }

        }

        public void Actualizar(IIdioma idiomaObservado)
        {
            if (idiomaObservado == null)
                return;

            Text = idiomaObservado.BuscarTraduccion(Tag.ToString());
            LBL_Titulo.Text = idiomaObservado.BuscarTraduccion(LBL_Titulo.Tag.ToString());
            label1.Text = idiomaObservado.BuscarTraduccion(label1.Tag.ToString());
            label2.Text = idiomaObservado.BuscarTraduccion(label2.Tag.ToString());
            BTN_IniciarSesion.Text = idiomaObservado.BuscarTraduccion(BTN_IniciarSesion.Tag.ToString());
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            _sesionIdioma.RegistrarObservador(this);
            Actualizar(_sesionIdioma.idioma);
        }

        private void BTN_IniciarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioService usuarioService = new UsuarioService();

                usuarioService.Login(TBX_Username.Text, TBX_Password.Text);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _sesionIdioma.idioma.BuscarTraduccion("Mensaje.ErrorIniciarSesion").Replace("{0}", ex.Message),
                    _sesionIdioma.idioma.BuscarTraduccion("Titulo.Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _sesionIdioma.DesregistrarObservador(this);
            base.OnFormClosed(e);
        }
    }
}
