using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Interfaces;
using CROSSCUTTING.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CROSSCUTTING.Auth
{
    public class SessionManager : ISesionUsuario
    {
        private static SessionManager _session;
        private UsuarioDTO _usuarioActual { get; set; }
        public DateTime FechaInicio { get; private set; }

        private static object _lock = new object();
        private readonly IPasswordHasher _passwordHasher;

        private SessionManager()
        {
            _passwordHasher = new PasswordHasher();
        }

        public static SessionManager GetInstance()
        {
            lock (_lock)
            {
                if (_session == null)
                {
                    _session = new SessionManager();
                }
                return _session;
            }
        }

        public void Login(UsuarioDTO usuario)
        {
            lock (_lock)
            {
                if(_usuarioActual != null)
                    throw new Exception("La sesión ya está iniciada.");
                

                _usuarioActual = new UsuarioDTO
                {
                    Id = usuario.Id,
                    Username = usuario.Username,
                    Password = _passwordHasher.HashPassword(usuario.Password)
                };
                FechaInicio = DateTime.Now;
            }
        }

        public void Logout()
        {
            lock (_lock)
            {
                if (_usuarioActual == null)
                    throw new Exception("No hay ninguna sesión iniciada.");

                _usuarioActual = null;
            }
        }

        public UsuarioDTO ObtenerUsuarioActual()
        {
            lock (_lock)
            {
                if (_usuarioActual == null)
                    return null;

                return new UsuarioDTO
                {
                    Id = _usuarioActual.Id,
                    Username = _usuarioActual.Username,
                    Password = _usuarioActual.Password
                };
            }
        }

        public DateTime ObtenerFechaInicio()
        {
            lock (_lock)
            {
                if (_usuarioActual == null)
                    throw new Exception("No hay ninguna sesión iniciada.");
                return FechaInicio;
            }
        }

    }
}
