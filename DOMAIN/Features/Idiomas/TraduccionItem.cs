using DOMAIN.Exceptions;

namespace DOMAIN.Features.Idiomas
{
    public class TraduccionItem
    {
        public int IdTraduccion { get; private set; }
        public int IdPalabra { get; private set; }
        public int IdIdioma { get; private set; }
        public string Clave { get; private set; }
        public string Idioma { get; private set; }
        public string Texto { get; private set; }

        private TraduccionItem()
        {
        }

        public static TraduccionItem Crear(int idTraduccion, int idPalabra, int idIdioma, string clave, string idioma, string texto)
        {
            if (string.IsNullOrEmpty(clave))
                throw new ReglaNegocioException("La clave es obligatoria.");
            if (string.IsNullOrEmpty(idioma))
                throw new ReglaNegocioException("El idioma es obligatorio.");
            if (string.IsNullOrEmpty(texto))
                throw new ReglaNegocioException("La traduccion es obligatoria.");

            return new TraduccionItem
            {
                IdTraduccion = idTraduccion,
                IdPalabra = idPalabra,
                IdIdioma = idIdioma,
                Clave = clave,
                Idioma = idioma,
                Texto = texto
            };
        }
    }
}
