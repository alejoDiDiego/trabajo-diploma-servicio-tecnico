using System.Collections.Generic;
using System.Linq;
using DOMAIN.Exceptions;
using DOMAIN.Features.Permisos;
using APPLICATION.Features.Bitacora;
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

        public List<FamiliaPermiso> ListarFamilias()
        {
            return _permisoRepository.ListarFamilias();
        }

        public List<PermisoSimple> ListarPermisosSimples()
        {
            return _permisoRepository.ListarPermisosSimples();
        }

        public FamiliaPermiso CrearFamilia(string nombre)
        {
            string nombreNormalizado = NormalizarNombre(nombre);

            // La UI solo crea familias. Los permisos simples se siembran desde base/repositorio.
            FamiliaPermiso.CrearNuevo(nombreNormalizado);
            FamiliaPermiso familia = _permisoRepository.AgregarFamilia(nombreNormalizado);

            BitacoraService bitacoraService = new BitacoraService();
            bitacoraService.Registrar("Creacion de familia", "nombre=" + nombreNormalizado, "PERMISOS");

            return familia;
        }

        public FamiliaPermiso EditarFamilia(int idFamilia, string nombre)
        {
            FamiliaPermiso familia = ObtenerFamiliaExistente(idFamilia);
            string nombreNormalizado = NormalizarNombre(nombre);

            FamiliaPermiso.CargarDesdeDB(familia.Id, nombreNormalizado);
            _permisoRepository.ModificarFamilia(familia.Id, nombreNormalizado);

            BitacoraService bitacoraService = new BitacoraService();
            bitacoraService.Registrar("Modificacion de familia", "id=" + idFamilia + " | nombre=" + nombreNormalizado, "PERMISOS");

            return FamiliaPermiso.CargarDesdeDB(familia.Id, nombreNormalizado);
        }

        public void EliminarFamilia(int idFamilia)
        {
            PermisoComponent familia = ObtenerFamiliaExistente(idFamilia);
            _permisoRepository.EliminarFamilia(idFamilia);

            BitacoraService bitacoraService = new BitacoraService();
            bitacoraService.Registrar("Eliminacion de familia", "nombre=" + familia.Nombre, "PERMISOS");
        }

        public void AgregarComponente(int? idPadre, int idHijo)
        {
            PermisoComponent hijo = ObtenerPermisoExistente(idHijo);

            // La raiz es virtual: no existe como fila en Permisos y solo puede contener familias.
            if (!idPadre.HasValue && !hijo.EsFamilia)
                throw new ReglaNegocioException("La raiz solo puede contener familias.");

            BitacoraService bitacoraService = new BitacoraService();

            if (!idPadre.HasValue)
            {
                ValidarDuplicadoDirecto(null, idHijo);
                _permisoRepository.AgregarComponente(null, idHijo);

                bitacoraService.Registrar("Agregar componente a raiz", "hijo=" + hijo.Nombre, "PERMISOS");
                return;
            }

            FamiliaPermiso padre = ObtenerFamiliaExistente(idPadre.Value);

            if (padre.Id == hijo.Id)
                throw new ReglaNegocioException("Una familia no puede agregarse como hija de si misma.");

            ValidarDuplicadoDirecto(padre.Id, hijo.Id);

            // Solo las familias pueden generar ciclos, porque los permisos simples no tienen hijos.
            if (hijo.EsFamilia && GeneraRelacionCircular(padre.Id, hijo.Id))
                throw new ReglaNegocioException("No se puede crear una relacion circular de permisos.");

            _permisoRepository.AgregarComponente(padre.Id, hijo.Id);

            string detalle = "hijo=" + hijo.Nombre + " | padre=" + padre.Nombre;
            bitacoraService.Registrar("Agregar componente a familia", detalle, "PERMISOS");
        }

        public void QuitarComponente(int? idPadre, int idHijo)
        {
            if (idPadre.HasValue)
                ObtenerFamiliaExistente(idPadre.Value);

            PermisoComponent hijo = ObtenerPermisoExistente(idHijo);
            _permisoRepository.QuitarComponente(idPadre, idHijo);

            BitacoraService bitacoraService = new BitacoraService();
            string padreNombre = idPadre.HasValue
                ? _permisoRepository.ObtenerPorId(idPadre.Value)?.Nombre ?? "#" + idPadre.Value
                : "Raiz";
            string detalle = "hijo=" + hijo.Nombre + " | padre=" + padreNombre;
            bitacoraService.Registrar("Quitar componente de familia", detalle, "PERMISOS");
        }

        private void ValidarDuplicadoDirecto(int? idPadre, int idHijo)
        {
            if (_permisoRepository.ExisteRelacion(idPadre, idHijo))
                throw new ReglaNegocioException("El permiso ya forma parte del nivel seleccionado.");
        }

        private bool GeneraRelacionCircular(int idPadre, int idHijo)
        {
            // Para agregar idHijo dentro de idPadre, reviso si idPadre ya vive dentro del subarbol de idHijo.
            // Si aparece, la nueva relacion cerraria un ciclo.
            List<PermisoComponent> arbolHijo = _permisoRepository.ListarSubArbol(idHijo);

            foreach (PermisoComponent permiso in arbolHijo)
            {
                if (ContienePermiso(permiso, idPadre))
                    return true;
            }

            return false;
        }

        private bool ContienePermiso(PermisoComponent permiso, int idBuscado)
        {
            if (permiso.Id == idBuscado)
                return true;

            // Busqueda recursiva dentro del Composite: baja por cada familia hija.
            foreach (PermisoComponent hijo in permiso.Hijos.OfType<PermisoComponent>())
            {
                if (ContienePermiso(hijo, idBuscado))
                    return true;
            }

            return false;
        }

        private FamiliaPermiso ObtenerFamiliaExistente(int id)
        {
            PermisoComponent permiso = ObtenerPermisoExistente(id);

            if (!permiso.EsFamilia)
                throw new ReglaNegocioException("El permiso seleccionado debe ser una familia.");

            return (FamiliaPermiso)permiso;
        }

        private PermisoComponent ObtenerPermisoExistente(int id)
        {
            PermisoComponent permiso = _permisoRepository.ObtenerPorId(id);

            if (permiso == null)
                throw new ReglaNegocioException("El permiso seleccionado no existe.");

            return permiso;
        }

        private string NormalizarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ReglaNegocioException("El nombre de la familia es obligatorio.");

            return nombre.Trim();
        }

    }
}
