using ABSTRACTIONS.Features.Permisos;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Permisos
{
    public class PermisoSimple : PermisoComponent
    {
        public override bool EsFamilia { get { return false; } }

        private PermisoSimple()
        {
        }

        public static PermisoSimple CrearNuevo(string nombre)
        {
            return Crear(0, nombre);
        }

        public static PermisoSimple CargarDesdeDB(int id, string nombre)
        {
            return Crear(id, nombre);
        }

        private static PermisoSimple Crear(int id, string nombre)
        {
            ValidarDatos(nombre);

            return new PermisoSimple
            {
                Id = id,
                Nombre = nombre.Trim()
            };
        }

        public override void AgregarHijo(IPermisoComponent permiso)
        {
            throw new ReglaNegocioException("Un permiso simple no puede tener permisos hijos.");
        }

        public override void QuitarHijo(IPermisoComponent permiso)
        {
            throw new ReglaNegocioException("Un permiso simple no tiene permisos hijos.");
        }
    }
}
