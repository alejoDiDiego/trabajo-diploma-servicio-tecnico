using ABSTRACTIONS.Entities;
using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Idiomas
{
    public class Traduccion : ITraduccion
    {
        public int IdIdioma { get; private set; }
        public int IdPalabra { get; private set; }
        public string PalabraTraducida { get; private set; }
        public IPalabra Palabra { get; private set; }

        int IEntity.Id => IdPalabra;

        private Traduccion()
        {
        }

        public static Traduccion Crear(int idIdioma, int idPalabra, IPalabra palabra, string palabraTraducida)
        {
            if (palabra == null)
                throw new ReglaNegocioException("La palabra a traducir es obligatoria.");
            if (string.IsNullOrEmpty(palabraTraducida))
                throw new ReglaNegocioException("La traduccion es obligatoria.");

            return new Traduccion
            {
                IdIdioma = idIdioma,
                IdPalabra = idPalabra,
                Palabra = palabra,
                PalabraTraducida = palabraTraducida
            };
        }
    }
}
