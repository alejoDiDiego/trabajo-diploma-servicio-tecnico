using System.Collections.Generic;
using ABSTRACTIONS.Entities;

namespace ABSTRACTIONS.Features.Idiomas
{
    public interface IIdioma : IEntity
    {
        string Nombre { get; }
        IList<ITraduccion> Traducciones { get; }
        void AgregarTraduccion(ITraduccion traduccion);
        string BuscarTraduccion(string texto);
    }
}
