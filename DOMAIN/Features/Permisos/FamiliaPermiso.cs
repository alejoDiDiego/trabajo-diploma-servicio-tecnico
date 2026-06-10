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

        public static FamiliaPermiso CrearNuevo(string nombre)
        {
            return Crear(0, nombre);
        }

        public static FamiliaPermiso CargarDesdeDB(int id, string nombre)
        {
            return Crear(id, nombre);
        }

        private static FamiliaPermiso Crear(int id, string nombre)
        {
            ValidarDatos(nombre);

            return new FamiliaPermiso
            {
                Id = id,
                Nombre = nombre.Trim()
            };
        }

        public override void AgregarHijo(IPermisoComponent permiso)
        {
            if (permiso == null)
                throw new ReglaNegocioException("El permiso hijo es obligatorio.");
            if (TieneMismoIdentificador(permiso))
                throw new ReglaNegocioException("Una familia no puede agregarse como hija de si misma.");
            // Bloquea ciclos: por ejemplo F1 -> F2 y luego intentar F2 -> F1.
            if (permiso.Contiene(this))
                throw new ReglaNegocioException("No se puede crear una relacion circular de permisos.");
            // Solo se impide repetir el mismo componente en el mismo nivel.
            // El mismo permiso/familia puede aparecer en ramas distintas.
            if (Hijos.Any(x => MismoPermiso(x, permiso)))
                throw new ReglaNegocioException("El permiso ya forma parte de esta familia.");

            Hijos.Add(permiso);
        }

        public override bool Contiene(IPermisoComponent permiso)
        {
            if (base.Contiene(permiso))
                return true;

            // La familia contiene un permiso si ella misma coincide o si lo contiene alguno de sus hijos.
            return Hijos.Any(x => x.Contiene(permiso));
        }

        private bool MismoPermiso(IPermisoComponent permisoA, IPermisoComponent permisoB)
        {
            if (permisoA == null || permisoB == null)
                return false;

            if (permisoA.Id > 0 && permisoB.Id > 0)
                return permisoA.Id == permisoB.Id;

            return string.Equals(permisoA.Nombre, permisoB.Nombre, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
