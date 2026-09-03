using System.Collections.Generic;
using System.Linq;
using DOMAIN.Exceptions;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Usuarios;
using APPLICATION.Features.Bitacora;
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
            ValidarUsuarioActivo(idUsuario);
            PermisoComponent permisoValidado = ValidarPermiso(idPermiso);
            ValidarNoEsRaiz(permisoValidado);
            _usuarioPermisoRepository.AsignarPermiso(idUsuario, idPermiso);

            BitacoraService bitacoraService = new BitacoraService();
            Usuario usuario = _usuarioRepository.ObtenerPorId(idUsuario);
            PermisoComponent permiso = _permisoRepository.ObtenerPorId(idPermiso);
            string detalle = "permiso=" + (permiso != null ? permiso.Nombre : "#" + idPermiso)
                + " | usuario=" + (usuario != null ? usuario.Username : "#" + idUsuario);
            bitacoraService.Registrar("Asignacion de permiso", detalle, "USUARIOS");
        }

        public void QuitarPermiso(int idUsuario, int idPermiso)
        {
            // Quita la relacion directa sin borrar el componente del catalogo.
            ValidarUsuarioActivo(idUsuario);
            ValidarPermiso(idPermiso);
            _usuarioPermisoRepository.QuitarPermiso(idUsuario, idPermiso);

            BitacoraService bitacoraService = new BitacoraService();
            Usuario usuario = _usuarioRepository.ObtenerPorId(idUsuario);
            PermisoComponent permiso = _permisoRepository.ObtenerPorId(idPermiso);
            string detalle = "permiso=" + (permiso != null ? permiso.Nombre : "#" + idPermiso)
                + " | usuario=" + (usuario != null ? usuario.Username : "#" + idUsuario);
            bitacoraService.Registrar("Desasignacion de permiso", detalle, "USUARIOS");
        }

        public List<PermisoComponent> ListarPermisosEfectivos(int idUsuario)
        {
            ValidarUsuario(idUsuario);

            // Devuelve los componentes asignados al usuario; las familias se cargan con sus hijos reales.
            List<PermisoComponent> permisosEfectivos = new List<PermisoComponent>();
            List<PermisoComponent> permisosAsignados = _usuarioPermisoRepository.ListarPermisosAsignados(idUsuario);

            foreach (PermisoComponent permisoAsignado in permisosAsignados)
            {
                if (!permisoAsignado.EsFamilia)
                {
                    AgregarPermisoEfectivo(permisoAsignado, permisosEfectivos);
                    continue;
                }

                foreach (PermisoComponent raiz in _permisoRepository.ListarSubArbol(permisoAsignado.Id))
                    AgregarPermisoEfectivo(raiz, permisosEfectivos);
            }

            return permisosEfectivos.OrderBy(x => x.Nombre).ToList();
        }

        private void AgregarPermisoEfectivo(PermisoComponent permiso, List<PermisoComponent> permisos)
        {
            if (permiso == null)
                return;

            if (!permisos.Any(x => MismoPermiso(x, permiso)))
                permisos.Add(permiso);
        }

        private bool MismoPermiso(PermisoComponent permisoA, PermisoComponent permisoB)
        {
            if (permisoA == null || permisoB == null)
                return false;

            if (permisoA.Id > 0 && permisoB.Id > 0)
                return permisoA.Id == permisoB.Id;

            return string.Equals(permisoA.Nombre, permisoB.Nombre, System.StringComparison.OrdinalIgnoreCase);
        }

        private void ValidarUsuario(int idUsuario)
        {
            // Asegura que la operacion apunte a un usuario real.
            Usuario usuario = _usuarioRepository.ObtenerPorId(idUsuario);

            if (usuario == null)
                throw new ReglaNegocioException("El usuario seleccionado no existe.");
        }

        private void ValidarUsuarioActivo(int idUsuario)
        {
            // T1: no se asignan ni se quitan permisos a usuarios dados de baja.
            Usuario usuario = _usuarioRepository.ObtenerPorId(idUsuario);

            if (usuario == null)
                throw new ReglaNegocioException("El usuario seleccionado no existe.");

            if (usuario.Activo == false)
                throw new ReglaNegocioException("No se pueden modificar permisos de un usuario inactivo.");
        }

        private void ValidarNoEsRaiz(PermisoComponent permiso)
        {
            PermisoComponent raiz = _permisoRepository.ObtenerRaizSistema();

            if (raiz != null && permiso != null && raiz.Id == permiso.Id)
                throw new ReglaNegocioException("La raiz de permisos no se puede asignar a usuarios.");
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
