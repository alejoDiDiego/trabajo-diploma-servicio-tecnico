using System;
using System.Collections.Generic;
using System.Linq;
using ABSTRACTIONS.Features.Usuarios;

namespace SERVICES.Auth
{
    public class SessionManager
    {
        private static readonly object _lock = new object();
        private static SessionManager _session;
        private readonly List<string> _codigosPermisos;
        public IUsuario Usuario;
        public DateTime FechaInicio { get; private set; }
        public bool IntegridadComprometida { get; set; }

        private SessionManager()
        {
            _codigosPermisos = new List<string>();
        }

        public static SessionManager GetInstance()
        {
            lock (_lock)
            {
                // Devuelve la sesion actual o falla si nadie inicio sesion.
                if (_session == null)
                    throw new Exception("No hay ninguna sesion iniciada.");

                return _session;
            }
        }

        public static bool HaySesionActiva()
        {
            lock (_lock)
            {
                // Indica si hay un usuario logueado.
                return _session != null && _session.Usuario != null;
            }
        }

        public static IUsuario ObtenerUsuarioActual()
        {
            lock (_lock)
            {
                // Permite consultar el usuario sin forzar excepcion.
                if (_session == null)
                    return null;

                return _session.Usuario;
            }
        }

        public static void Login(IUsuario usuario)
        {
            Login(usuario, new List<string>());
        }

        public static void Login(IUsuario usuario, IEnumerable<string> codigosPermisos)
        {
            lock (_lock)
            {
                if(_session != null)
                    throw new Exception("La sesion ya esta iniciada.");

                // Guarda el usuario logueado y sus permisos efectivos en memoria.
                _session = new SessionManager();
                _session.Usuario = usuario;
                _session.FechaInicio = DateTime.Now;

                if (codigosPermisos == null)
                    return;

                foreach (string codigo in codigosPermisos)
                {
                    if (string.IsNullOrWhiteSpace(codigo))
                        continue;

                    string codigoNormalizado = codigo.Trim();

                    // Evita permisos repetidos cuando vienen por mas de una familia.
                    if (!_session._codigosPermisos.Any(x => string.Equals(x, codigoNormalizado, StringComparison.OrdinalIgnoreCase)))
                        _session._codigosPermisos.Add(codigoNormalizado);
                }
            }
        }

        public static bool TienePermiso(string codigo)
        {
            lock (_lock)
            {
                if (_session == null || string.IsNullOrWhiteSpace(codigo))
                    return false;

                // Consulta un permiso puntual de la sesion actual.
                return _session._codigosPermisos.Any(x => string.Equals(x, codigo.Trim(), StringComparison.OrdinalIgnoreCase));
            }
        }

        public static bool TieneAlgunPermiso(params string[] codigos)
        {
            if (codigos == null || codigos.Length == 0)
                return false;

            lock (_lock)
            {
                if (_session == null)
                    return false;

                // Sirve para pantallas que pueden abrirse con mas de un permiso.
                return codigos.Any(codigo =>
                    !string.IsNullOrWhiteSpace(codigo) &&
                    _session._codigosPermisos.Any(x => string.Equals(x, codigo.Trim(), StringComparison.OrdinalIgnoreCase)));
            }
        }

        //public static List<string> ListarPermisos()
        //{
        //    lock (_lock)
        //    {
        //        if (_session == null)
        //            return new List<string>();

        //        // Devuelve una copia ordenada para no exponer la lista interna.
        //        return _session._codigosPermisos.OrderBy(x => x).ToList();
        //    }
        //}

        public static void Logout()
        {
            lock (_lock)
            {
                // Limpia usuario y permisos guardados en memoria.
                if(_session == null)
                    throw new Exception("No hay ninguna sesion iniciada.");

                _session = null;
            }
        }

    }
}
