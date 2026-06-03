using System.Collections.Generic;
using System.Linq;
using DOMAIN.Exceptions;
using DOMAIN.Features.Permisos;
using REPOSITORY.Features.Permisos;

namespace APPLICATION.Features.Permisos
{
    public class PermisoService
    {
        private readonly PermisoRepository _permisoRepository;

        public PermisoService()
        {
            _permisoRepository = new PermisoRepository();
        }

        public void Inicializar()
        {
            _permisoRepository.Inicializar();
        }

        public List<PermisoComponent> ListarArbol()
        {
            return _permisoRepository.ListarArbol();
        }

        public PermisoComponent CrearPermiso(string nombre, string codigo, string descripcion, int? idPadre)
        {
            PermisoSimple.CrearNuevo(nombre, codigo, descripcion);
            ValidarPadre(idPadre);

            return _permisoRepository.Agregar(nombre.Trim(), codigo.Trim(), descripcion, false, idPadre);
        }

        public FamiliaPermiso CrearFamilia(string nombre, string codigo, string descripcion, int? idPadre)
        {
            FamiliaPermiso.CrearNuevo(nombre, codigo, descripcion);
            ValidarPadre(idPadre);

            return (FamiliaPermiso)_permisoRepository.Agregar(nombre.Trim(), codigo.Trim(), descripcion, true, idPadre);
        }

        public void Modificar(int id, string nombre, string codigo, string descripcion)
        {
            PermisoComponent permiso = ObtenerPermisoExistente(id);
            PermisoComponent permisoValidado;

            if (permiso.EsFamilia)
                permisoValidado = FamiliaPermiso.CargarDesdeDB(id, nombre, codigo, descripcion);
            else
                permisoValidado = PermisoSimple.CargarDesdeDB(id, nombre, codigo, descripcion);

            _permisoRepository.Modificar(
                permisoValidado.Id,
                permisoValidado.Nombre,
                permisoValidado.Codigo,
                permisoValidado.Descripcion);
        }

        public void Eliminar(int id)
        {
            ObtenerPermisoExistente(id);
            _permisoRepository.Eliminar(id);
        }

        public void Mover(int idPermiso, int? idNuevoPadre)
        {
            List<PermisoComponent> arbol = _permisoRepository.ListarArbol();
            PermisoComponent permiso = BuscarPorId(arbol, idPermiso);

            if (permiso == null)
                throw new ReglaNegocioException("El permiso seleccionado no existe.");

            if (!idNuevoPadre.HasValue)
            {
                _permisoRepository.Mover(idPermiso, null);
                return;
            }

            PermisoComponent nuevoPadre = BuscarPorId(arbol, idNuevoPadre.Value);

            if (nuevoPadre == null)
                throw new ReglaNegocioException("La familia padre seleccionada no existe.");
            if (!nuevoPadre.EsFamilia)
                throw new ReglaNegocioException("El padre seleccionado debe ser una familia.");
            if (permiso.Contiene(nuevoPadre))
                throw new ReglaNegocioException("No se puede mover un permiso dentro de si mismo o de sus hijos.");

            _permisoRepository.Mover(idPermiso, idNuevoPadre);
        }

        private void ValidarPadre(int? idPadre)
        {
            if (!idPadre.HasValue)
                return;

            PermisoComponent padre = _permisoRepository.ObtenerPorId(idPadre.Value);

            if (padre == null)
                throw new ReglaNegocioException("La familia padre seleccionada no existe.");
            if (!padre.EsFamilia)
                throw new ReglaNegocioException("El padre seleccionado debe ser una familia.");
        }

        private PermisoComponent ObtenerPermisoExistente(int id)
        {
            PermisoComponent permiso = _permisoRepository.ObtenerPorId(id);

            if (permiso == null)
                throw new ReglaNegocioException("El permiso seleccionado no existe.");

            return permiso;
        }

        private PermisoComponent BuscarPorId(IEnumerable<PermisoComponent> permisos, int id)
        {
            foreach (PermisoComponent permiso in permisos)
            {
                if (permiso.Id == id)
                    return permiso;

                PermisoComponent permisoEncontrado = BuscarPorId(permiso.Hijos.OfType<PermisoComponent>(), id);

                if (permisoEncontrado != null)
                    return permisoEncontrado;
            }

            return null;
        }
    }
}
