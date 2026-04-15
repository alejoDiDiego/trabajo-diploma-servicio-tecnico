using APPLICATION.Features.Usuarios.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Interfaces
{
    public interface ISesionUsuario
    {
        void Login(UsuarioDTO usuario);
        void Logout();
        UsuarioDTO ObtenerUsuarioActual();
        DateTime ObtenerFechaInicio();
    }
}
