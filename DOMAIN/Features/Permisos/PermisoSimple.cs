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
    }
}
