using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOMAIN.Features.Usuarios;

namespace APPLICATION.Features.Usuarios.Exceptions
{
    public class DatosUsuarioIncorrectosException : Exception
    {
        public DatosUsuarioIncorrectosException() : base("Datos incorrectos.")
        {
        }

        public DatosUsuarioIncorrectosException(string message) : base(message)
        {
        }
    }
}
