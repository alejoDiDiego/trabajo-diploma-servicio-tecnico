using DOMAIN.Exceptions;

namespace DOMAIN.Features.TiposEquipo
{
    public class TipoEquipo
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public bool Activo { get; private set; }

        private TipoEquipo() { }

        public static TipoEquipo CrearNuevo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ReglaNegocioException("El nombre del tipo de equipo es obligatorio.");

            return new TipoEquipo
            {
                Nombre = nombre.Trim(),
                Activo = true
            };
        }

        public static TipoEquipo CargarDesdeDB(int id, string nombre, bool activo)
        {
            return new TipoEquipo
            {
                Id = id,
                Nombre = nombre ?? "",
                Activo = activo
            };
        }
    }
}
