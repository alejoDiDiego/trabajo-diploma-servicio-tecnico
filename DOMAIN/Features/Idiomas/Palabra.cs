using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Idiomas
{
    public class Palabra : IPalabra
    {
        public int Id { get; private set; }
        public string Texto { get; private set; }

        private Palabra()
        {
        }

        public static Palabra Crear(int id, string texto)
        {
            if (string.IsNullOrEmpty(texto))
                throw new ReglaNegocioException("La palabra es obligatoria.");

            return new Palabra
            {
                Id = id,
                Texto = texto
            };
        }
    }
}
