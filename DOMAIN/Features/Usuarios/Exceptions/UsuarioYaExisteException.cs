using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Features.Usuarios.Exceptions
{
    public class UsuarioYaExisteException : Exception
    {
        public UsuarioYaExisteException() : base("El usuario ya existe.")
        {
        }
    }
}
