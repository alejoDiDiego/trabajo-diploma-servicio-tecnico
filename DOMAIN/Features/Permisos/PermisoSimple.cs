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
            return Crear(0, nombre, null);
        }

        public static PermisoSimple CrearNuevo(string nombre, string codigo)
        {
            return Crear(0, nombre, codigo);
        }

        public static PermisoSimple CargarDesdeDB(int id, string nombre)
        {
            return Crear(id, nombre, null);
        }

        public static PermisoSimple CargarDesdeDB(int id, string nombre, string codigo)
        {
            return Crear(id, nombre, codigo);
        }

        private static PermisoSimple Crear(int id, string nombre, string codigo)
        {
            ValidarDatos(nombre);

            return new PermisoSimple
            {
                Id = id,
                Nombre = nombre.Trim(),
                Codigo = string.IsNullOrWhiteSpace(codigo) ? null : codigo.Trim().ToUpperInvariant()
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
