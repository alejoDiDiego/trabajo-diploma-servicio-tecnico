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

        public List<FamiliaPermiso> ListarFamiliasAsignadas(int idUsuario)
        {
            // Familias que el usuario ya tiene asignadas.
            ValidarUsuario(idUsuario);
            return _usuarioPermisoRepository.ListarFamiliasAsignadas(idUsuario);
        }

        public List<FamiliaPermiso> ListarFamiliasDisponibles(int idUsuario)
        {
            // Familias que aun pueden asignarse al usuario.
            ValidarUsuario(idUsuario);
            return _usuarioPermisoRepository.ListarFamiliasDisponibles(idUsuario);
        }

        public void AsignarFamilia(int idUsuario, int idFamilia)
        {
            // Solo se asignan familias, no permisos simples sueltos.
            ValidarUsuario(idUsuario);
            ValidarFamilia(idFamilia);
            _usuarioPermisoRepository.AsignarFamilia(idUsuario, idFamilia);
        }

        public void QuitarFamilia(int idUsuario, int idFamilia)
        {
            // Quita la relacion usuario-familia, sin borrar la familia del catalogo.
            ValidarUsuario(idUsuario);
            ValidarFamilia(idFamilia);
            _usuarioPermisoRepository.QuitarFamilia(idUsuario, idFamilia);
        }

        public List<string> ListarCodigosPermisosEfectivos(int idUsuario)
        {
            ValidarUsuario(idUsuario);

            // Recorre las familias asignadas y obtiene los permisos simples finales.
            List<string> codigos = new List<string>();
            List<FamiliaPermiso> familias = _usuarioPermisoRepository.ListarFamiliasAsignadas(idUsuario);

            foreach (FamiliaPermiso familia in familias)
            {
                foreach (PermisoComponent raiz in _permisoRepository.ListarSubArbol(familia.Id))
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

        private FamiliaPermiso ValidarFamilia(int idFamilia)
        {
            // Las asignaciones trabajan solamente con familias.
            PermisoComponent permiso = _permisoRepository.ObtenerPorId(idFamilia);

            if (permiso == null || !permiso.EsFamilia)
                throw new ReglaNegocioException("El permiso seleccionado debe ser una familia.");

            return (FamiliaPermiso)permiso;
        }
    }
}
