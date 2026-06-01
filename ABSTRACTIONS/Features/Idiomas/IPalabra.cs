using ABSTRACTIONS.Entities;

namespace ABSTRACTIONS.Features.Idiomas
{
    public interface IPalabra : IEntity
    {
        string Texto { get; }
    }
}
