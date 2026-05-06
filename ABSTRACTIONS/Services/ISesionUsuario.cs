using System;
using ABSTRACTIONS.Features.Usuarios;

namespace ABSTRACTIONS.Services
{
    public interface ISesionUsuario
    {
        void Login(IUsuario usuario);
        void Logout();
        IUsuario ObtenerUsuarioActual();
        DateTime ObtenerFechaInicio();
    }
}
