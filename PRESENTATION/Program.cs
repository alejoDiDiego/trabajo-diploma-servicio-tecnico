using System;
using System.Windows.Forms;
using APPLICATION.Features.ControlCambios;
using APPLICATION.Features.Integridad;
using APPLICATION.Features.Idiomas;
using APPLICATION.Features.Permisos;
using APPLICATION.Features.Usuarios;
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

            IntegridadService integridadService = new IntegridadService();
            integridadService.Inicializar();

            IdiomaService idiomaService = new IdiomaService();
            idiomaService.Inicializar();

            PermisoService permisoService = new PermisoService();
            permisoService.Inicializar();

            ControlCambioService controlCambioService = new ControlCambioService();
            controlCambioService.Inicializar();

            UsuarioService usuarioService = new UsuarioService();
            usuarioService.Inicializar();

            SesionIdioma.GetInstance().CambiarIdioma(idiomaService.ObtenerIdiomaPorDefecto());

            Application.Run(new FrmPrincipal());
        }
    }
}
