using System.Collections.Generic;
using System.Linq;
using DOMAIN.Exceptions;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Usuarios;
using REPOSITORY.Features.Permisos;
using REPOSITORY.Features.Usuarios;

namespace APPLICATION.Features.Usuarios
{
    public class UsuarioPermisoService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly UsuarioPermisoRepository _usuarioPermisoRepository;
        private readonly PermisoRepository _permisoRepository;

        public UsuarioPermisoService()
        {
            _usuarioRepository = new UsuarioRepository();
            _usuarioPermisoRepository = new UsuarioPermisoRepository();
            _permisoRepository = new PermisoRepository();
        }

        public List<PermisoComponent> ListarPermisosAsignados(int idUsuario)
        {
            // Componentes que el usuario ya tiene asignados directamente.
            ValidarUsuario(idUsuario);
            return _usuarioPermisoRepository.ListarPermisosAsignados(idUsuario);
        }

        public List<PermisoComponent> ListarPermisosDisponibles(int idUsuario)
        {
            // Componentes que aun pueden asignarse directamente al usuario.
            ValidarUsuario(idUsuario);
            return _usuarioPermisoRepository.ListarPermisosDisponibles(idUsuario);
        }

        public void AsignarPermiso(int idUsuario, int idPermiso)
        {
            ValidarUsuario(idUsuario);
            ValidarPermiso(idPermiso);
            _usuarioPermisoRepository.AsignarPermiso(idUsuario, idPermiso);
        }

        public void QuitarPermiso(int idUsuario, int idPermiso)
        {
            // Quita la relacion directa sin borrar el componente del catalogo.
            ValidarUsuario(idUsuario);
            ValidarPermiso(idPermiso);
            _usuarioPermisoRepository.QuitarPermiso(idUsuario, idPermiso);
        }

        public List<string> ListarCodigosPermisosEfectivos(int idUsuario)
        {
            ValidarUsuario(idUsuario);

            // Expande familias y agrega directamente los permisos simples asignados.
            List<string> codigos = new List<string>();
            List<PermisoComponent> permisosAsignados = _usuarioPermisoRepository.ListarPermisosAsignados(idUsuario);

            foreach (PermisoComponent permisoAsignado in permisosAsignados)
            {
                if (!permisoAsignado.EsFamilia)
                {
                    AgregarCodigos(permisoAsignado, codigos);
                    continue;
                }

                foreach (PermisoComponent raiz in _permisoRepository.ListarSubArbol(permisoAsignado.Id))
                    AgregarCodigos(raiz, codigos);
            }

            return codigos.OrderBy(x => x).ToList();
        }

        private void AgregarCodigos(PermisoComponent permiso, List<string> codigos)
        {
            if (permiso == null)
                return;

            if (!permiso.EsFamilia && !string.IsNullOrWhiteSpace(permiso.Codigo))
            {
                // Evita repetir codigos cuando dos familias contienen el mismo permiso.
                if (!codigos.Any(x => string.Equals(x, permiso.Codigo, System.StringComparison.OrdinalIgnoreCase)))
                    codigos.Add(permiso.Codigo);
            }

            // Recursion sobre el Composite: baja por familias hijas hasta permisos simples.
            foreach (PermisoComponent hijo in permiso.Hijos.OfType<PermisoComponent>())
                AgregarCodigos(hijo, codigos);
        }

        private void ValidarUsuario(int idUsuario)
        {
            // Asegura que la operacion apunte a un usuario real.
            Usuario usuario = _usuarioRepository.ObtenerPorId(idUsuario);

            if (usuario == null)
                throw new ReglaNegocioException("El usuario seleccionado no existe.");
        }

        private PermisoComponent ValidarPermiso(int idPermiso)
        {
            PermisoComponent permiso = _permisoRepository.ObtenerPorId(idPermiso);

            if (permiso == null)
                throw new ReglaNegocioException("El permiso seleccionado no existe.");

            return permiso;
        }
    }
}
