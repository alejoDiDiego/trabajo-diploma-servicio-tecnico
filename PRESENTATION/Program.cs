using System;
using System.Windows.Forms;
using APPLICATION.Features.Bitacora;
using APPLICATION.Features.Clientes;
using APPLICATION.Features.ControlCambios;
using APPLICATION.Features.Equipos;
using APPLICATION.Features.Integridad;
using APPLICATION.Features.Idiomas;
using APPLICATION.Features.Marcas;
using APPLICATION.Features.Permisos;
using APPLICATION.Features.TiposEquipo;
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

            BitacoraService bitacoraService = new BitacoraService();
            bitacoraService.Inicializar();

            ControlCambioService controlCambioService = new ControlCambioService();
            controlCambioService.Inicializar();

            UsuarioService usuarioService = new UsuarioService();
            usuarioService.Inicializar();

            TipoEquipoService tipoEquipoService = new TipoEquipoService();
            tipoEquipoService.Inicializar();

            MarcaService marcaService = new MarcaService();
            marcaService.Inicializar();

            ClienteService clienteService = new ClienteService();
            clienteService.Inicializar();

            EquipoService equipoService = new EquipoService();
            equipoService.Inicializar();

            SesionIdioma.GetInstance().CambiarIdioma(idiomaService.ObtenerIdiomaPorDefecto());

            Application.Run(new FrmPrincipal());
        }
    }
}
