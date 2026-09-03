using DOMAIN.Exceptions;

namespace DOMAIN.Features.Marcas
{
    public class Marca
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public bool Activo { get; private set; }

        private Marca() { }

        public static Marca CrearNuevo(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ReglaNegocioException("El nombre de la marca es obligatorio.");

            return new Marca
            {
                Nombre = nombre.Trim(),
                Activo = true
            };
        }

        public static Marca CargarDesdeDB(int id, string nombre, bool activo)
        {
            return new Marca
            {
                Id = id,
                Nombre = nombre ?? "",
                Activo = activo
            };
        }
    }
}
