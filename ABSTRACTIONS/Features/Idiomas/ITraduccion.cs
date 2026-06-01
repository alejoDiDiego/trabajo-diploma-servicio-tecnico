using ABSTRACTIONS.Entities;

namespace ABSTRACTIONS.Features.Idiomas
{
    public interface ITraduccion : IEntity
    {
        string PalabraTraducida { get; }
        IPalabra Palabra { get; }
    }
}
