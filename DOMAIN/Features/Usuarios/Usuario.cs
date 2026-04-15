using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Features.Usuarios
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
    }
}
