using System;
using ABSTRACTIONS.Features.Usuarios;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.Features.Usuarios.DTOs
{
    public class UsuarioDTO : IUsuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
