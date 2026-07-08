using ABSTRACTIONS.Entities;

namespace ABSTRACTIONS.Features.Idiomas
{
public interface ITraduccion : IEntity
{
    int IdIdioma { get; }
    int IdPalabra { get; }
    string PalabraTraducida { get; }
    IPalabra Palabra { get; }
}
}
