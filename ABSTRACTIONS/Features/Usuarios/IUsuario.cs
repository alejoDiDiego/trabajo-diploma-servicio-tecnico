using ABSTRACTIONS.Entities;

namespace ABSTRACTIONS.Features.Usuarios
{
    public interface IUsuario : IEntity
    {
        string Username { get; }
        string Password { get; }
    }
}
