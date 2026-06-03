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

        public static PermisoSimple CrearNuevo(string nombre, string codigo, string descripcion)
        {
            return Crear(0, nombre, codigo, descripcion);
        }

        public static PermisoSimple CargarDesdeDB(int id, string nombre, string codigo, string descripcion)
        {
            return Crear(id, nombre, codigo, descripcion);
        }

        private static PermisoSimple Crear(int id, string nombre, string codigo, string descripcion)
        {
            ValidarDatos(nombre, codigo);

            return new PermisoSimple
            {
                Id = id,
                Nombre = nombre.Trim(),
                Codigo = codigo.Trim(),
                Descripcion = descripcion ?? string.Empty
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
