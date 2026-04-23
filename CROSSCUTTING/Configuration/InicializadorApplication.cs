using APPLICATION.Features.Usuarios;
using APPLICATION.Interfaces;
using CROSSCUTTING.Auth;
using CROSSCUTTING.Security;
using INFRASTRUCTURE.Features.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CROSSCUTTING.Configuration
{
    public static class InicializadorAplicacion
    {

        public static ISesionUsuario ObtenerSesion()
        {
            return SessionManager.GetInstance();
        }

        public static UsuarioService CrearUsuarioService()
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString);
            PasswordHasher passwordHasher = new PasswordHasher();

            return new UsuarioService(passwordHasher, usuarioRepository);
        }
    }
}
