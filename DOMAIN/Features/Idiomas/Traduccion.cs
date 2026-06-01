using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Idiomas
{
    public class Traduccion : ITraduccion
    {
        public int Id { get; private set; }
        public string PalabraTraducida { get; private set; }
        public IPalabra Palabra { get; private set; }

        private Traduccion()
        {
        }

        public static Traduccion Crear(int id, IPalabra palabra, string palabraTraducida)
        {
            if (palabra == null)
                throw new ReglaNegocioException("La palabra a traducir es obligatoria.");
            if (string.IsNullOrEmpty(palabraTraducida))
                throw new ReglaNegocioException("La traduccion es obligatoria.");

            return new Traduccion
            {
                Id = id,
                Palabra = palabra,
                PalabraTraducida = palabraTraducida
            };
        }
    }
}
