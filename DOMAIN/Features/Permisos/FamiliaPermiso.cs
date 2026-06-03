using System.Linq;
using ABSTRACTIONS.Features.Permisos;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Permisos
{
    public class FamiliaPermiso : PermisoComponent
    {
        public override bool EsFamilia { get { return true; } }

        private FamiliaPermiso()
        {
        }

        public static FamiliaPermiso CrearNuevo(string nombre, string codigo, string descripcion)
        {
            return Crear(0, nombre, codigo, descripcion);
        }

        public static FamiliaPermiso CargarDesdeDB(int id, string nombre, string codigo, string descripcion)
        {
            return Crear(id, nombre, codigo, descripcion);
        }

        private static FamiliaPermiso Crear(int id, string nombre, string codigo, string descripcion)
        {
            ValidarDatos(nombre, codigo);

            return new FamiliaPermiso
            {
                Id = id,
                Nombre = nombre.Trim(),
                Codigo = codigo.Trim(),
                Descripcion = descripcion ?? string.Empty
            };
        }

        public override void AgregarHijo(IPermisoComponent permiso)
        {
            if (permiso == null)
                throw new ReglaNegocioException("El permiso hijo es obligatorio.");
            if (TieneMismoIdentificador(permiso))
                throw new ReglaNegocioException("Una familia no puede agregarse como hija de si misma.");
            if (permiso.Contiene(this))
                throw new ReglaNegocioException("No se puede crear una relacion circular de permisos.");
            if (Hijos.Any(x => MismoPermiso(x, permiso) || x.Contiene(permiso)))
                throw new ReglaNegocioException("El permiso ya forma parte de esta familia.");

            Hijos.Add(permiso);
        }

        public override void QuitarHijo(IPermisoComponent permiso)
        {
            if (permiso == null)
                throw new ReglaNegocioException("El permiso hijo es obligatorio.");

            IPermisoComponent permisoActual = Hijos.FirstOrDefault(x => MismoPermiso(x, permiso));

            if (permisoActual == null)
                throw new ReglaNegocioException("El permiso no forma parte de esta familia.");

            Hijos.Remove(permisoActual);
        }

        public override bool Contiene(IPermisoComponent permiso)
        {
            if (base.Contiene(permiso))
                return true;

            return Hijos.Any(x => x.Contiene(permiso));
        }

        private bool MismoPermiso(IPermisoComponent permisoA, IPermisoComponent permisoB)
        {
            if (permisoA == null || permisoB == null)
                return false;

            if (permisoA.Id > 0 && permisoB.Id > 0)
                return permisoA.Id == permisoB.Id;

            return string.Equals(permisoA.Codigo, permisoB.Codigo, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
