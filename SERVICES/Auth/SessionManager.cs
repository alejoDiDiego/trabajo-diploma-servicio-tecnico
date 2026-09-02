using System;
using System.Collections.Generic;
using System.Linq;
using ABSTRACTIONS.Features.Permisos;
using ABSTRACTIONS.Features.Usuarios;

namespace SERVICES.Auth
{
    public class SessionManager
    {
        private static readonly object _lock = new object();
        private static SessionManager _session;
        private readonly List<IPermisoComponent> _permisos;
        public IUsuario Usuario;
        public DateTime FechaInicio { get; private set; }
        public bool IntegridadComprometida { get; set; }

        private SessionManager()
        {
            _permisos = new List<IPermisoComponent>();
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
            Login(usuario, new List<IPermisoComponent>());
        }

        public static void Login(IUsuario usuario, IEnumerable<IPermisoComponent> permisos)
        {
            lock (_lock)
            {
                if (_session != null)
                    throw new Exception("La sesion ya esta iniciada.");

                // Guarda el usuario logueado y sus permisos efectivos como Composite.
                _session = new SessionManager();
                _session.Usuario = usuario;
                _session.FechaInicio = DateTime.Now;

                if (permisos == null)
                    return;

                foreach (IPermisoComponent permiso in permisos)
                {
                    if (permiso == null)
                        continue;

                    if (!_session._permisos.Any(x => MismoPermiso(x, permiso)))
                        _session._permisos.Add(permiso);
                }
            }
        }

        public static bool TienePermiso(string codigo)
        {
            lock (_lock)
            {
                if (_session == null || string.IsNullOrWhiteSpace(codigo))
                    return false;

                // Consulta un permiso puntual recorriendo el Composite guardado en sesion.
                return _session._permisos.Any(permiso => ContieneCodigo(permiso, codigo.Trim()));
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
                    _session._permisos.Any(permiso => ContieneCodigo(permiso, codigo.Trim())));
            }
        }

        public static List<IPermisoComponent> ListarPermisos()
        {
            lock (_lock)
            {
                if (_session == null)
                    return new List<IPermisoComponent>();

                // Devuelve una copia para no exponer la lista interna.
                return _session._permisos.ToList();
            }
        }

        private static bool ContieneCodigo(IPermisoComponent permiso, string codigo)
        {
            if (permiso == null || string.IsNullOrWhiteSpace(codigo))
                return false;

            if (!permiso.EsFamilia && !string.IsNullOrWhiteSpace(permiso.Codigo) &&
                string.Equals(permiso.Codigo, codigo, StringComparison.OrdinalIgnoreCase))
                return true;

            foreach (IPermisoComponent hijo in permiso.Hijos)
            {
                if (ContieneCodigo(hijo, codigo))
                    return true;
            }

            return false;
        }

        private static bool MismoPermiso(IPermisoComponent permisoA, IPermisoComponent permisoB)
        {
            if (permisoA == null || permisoB == null)
                return false;

            if (permisoA.Id > 0 && permisoB.Id > 0)
                return permisoA.Id == permisoB.Id;

            return string.Equals(permisoA.Nombre, permisoB.Nombre, StringComparison.OrdinalIgnoreCase);
        }

        public static void Logout()
        {
            lock (_lock)
            {
                // Limpia usuario y permisos guardados en memoria.
                if (_session == null)
                    throw new Exception("No hay ninguna sesion iniciada.");

                _session = null;
            }
        }

    }
}
