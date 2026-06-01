using System;
using ABSTRACTIONS.Features.Usuarios;
using ABSTRACTIONS.Services;

namespace SERVICES.Auth
{
    public class SessionManager
    {
        private static readonly object _lock = new object();
        private static SessionManager _session;
        public IUsuario Usuario;
        public DateTime FechaInicio { get; private set; }

        private SessionManager()
        {
        }

        public static SessionManager GetInstance()
        {
            lock (_lock)
            {
                if (_session == null)
                    throw new Exception("No hay ninguna sesion iniciada.");

                return _session;
            }
        }

        public static bool HaySesionActiva()
        {
            lock (_lock)
            {
                return _session != null && _session.Usuario != null;
            }
        }

        public static IUsuario ObtenerUsuarioActual()
        {
            lock (_lock)
            {
                if (_session == null)
                    return null;

                return _session.Usuario;
            }
        }

        public static void Login(IUsuario usuario)
        {
            lock (_lock)
            {
                if(_session != null)
                    throw new Exception("La sesion ya esta iniciada.");

                _session = new SessionManager();
                _session.Usuario = usuario;
                _session.FechaInicio = DateTime.Now;
            }
        }

        public static void Logout()
        {
            lock (_lock)
            {
                if(_session == null)
                    throw new Exception("No hay ninguna sesion iniciada.");

                _session = null;
            }
        }

    }
}
