using System;
using System.Collections.Generic;
using System.Linq;
using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Idiomas
{
    public class Idioma : IIdioma
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public IList<ITraduccion> Traducciones { get; private set; }

        private Idioma()
        {
            Traducciones = new List<ITraduccion>();
        }

        public static Idioma Crear(int id, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ReglaNegocioException("El nombre del idioma es obligatorio.");

            return new Idioma
            {
                Id = id,
                Nombre = nombre.Trim()
            };
        }

        public void AgregarTraduccion(ITraduccion traduccion)
        {
            if (traduccion == null)
                throw new ReglaNegocioException("La traduccion es obligatoria.");

            Traducciones.Add(traduccion);
        }

        public string BuscarTraduccion(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            ITraduccion traduccion = Traducciones.FirstOrDefault(x =>
                x.Palabra != null &&
                string.Equals(x.Palabra.Texto, texto, StringComparison.OrdinalIgnoreCase));

            return traduccion != null ? traduccion.PalabraTraducida : texto;
        }
    }
}
