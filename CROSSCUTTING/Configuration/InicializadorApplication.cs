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

namespace CROSSCUTTING.Configuration
{
    public static class InicializadorAplicacion
    {
        private static readonly string cadenaConexion = "Server=localhost,1433;Database=TPIntegrador;User Id=sa;Password=TuPasswordSeguro123!;TrustServerCertificate=True";

        public static ISesionUsuario ObtenerSesion()
        {
            return SessionManager.GetInstance();
        }

        public static UsuarioService CrearUsuarioService()
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository(cadenaConexion);
            PasswordHasher passwordHasher = new PasswordHasher();

            return new UsuarioService(passwordHasher, usuarioRepository);
        }
    }
}
