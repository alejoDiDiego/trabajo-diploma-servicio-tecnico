using System;
using System.Windows.Forms;
using APPLICATION.Features.Idiomas;
using SERVICES.Idiomas;
using UI.Forms;

namespace UI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IdiomaService idiomaService = new IdiomaService();
            idiomaService.Inicializar();
            SesionIdioma.GetInstance().CambiarIdioma(idiomaService.ObtenerIdiomaPorDefecto());

            Application.Run(new FrmPrincipal());
        }
    }
}
