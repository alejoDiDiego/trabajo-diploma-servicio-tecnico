using System;
using ABSTRACTIONS.Features.Usuarios;
using ABSTRACTIONS.Services;

namespace SERVICES.Auth
{
    public class SessionManager : ISesionUsuario
    {
        private static readonly object _lock = new object();
        private static SessionManager _session;
        private UsuarioSesion _usuarioActual;

        public DateTime FechaInicio { get; private set; }

        private SessionManager()
        {
        }

        public static SessionManager GetInstance()
        {
            lock (_lock)
            {
                if (_session == null)
                    _session = new SessionManager();

                return _session;
            }
        }

        public void Login(IUsuario usuario)
        {
            lock (_lock)
            {
                if (_usuarioActual != null)
                    throw new Exception("La sesion ya esta iniciada.");

                _usuarioActual = new UsuarioSesion
                {
                    Id = usuario.Id,
                    Username = usuario.Username,
                    Password = usuario.Password
                };
                FechaInicio = DateTime.Now;
            }
        }

        public void Logout()
        {
            lock (_lock)
            {
                if (_usuarioActual == null)
                    throw new Exception("No hay ninguna sesion iniciada.");

                _usuarioActual = null;
            }
        }

        public IUsuario ObtenerUsuarioActual()
        {
            lock (_lock)
            {
                if (_usuarioActual == null)
                    return null;

                return new UsuarioSesion
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
                    throw new Exception("No hay ninguna sesion iniciada.");

                return FechaInicio;
            }
        }

        private class UsuarioSesion : IUsuario
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
