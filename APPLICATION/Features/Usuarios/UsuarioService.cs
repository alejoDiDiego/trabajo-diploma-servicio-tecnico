using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Interfaces;

namespace APPLICATION.Features.Usuarios
{
    public class UsuarioService
    {
        IPasswordHasher _passwordHasher;

        public class UsuarioService(IPasswordHasher _passwordHasher)
        {
            _passwordHasher = _passwordHasher;
        }

        public bool Login(string username, string password)
        {
            /**Todo Implementar repository */
            UsuarioDTO usuario = new UsuarioDTO
            {
                Id = 1,
                Username = username,
                Password = password
            };

            return _passwordHasher.VerifyHashedPassword(usuario.Password, password);
        }

        public UsuarioDTO Create(string username, string password)
        {
            /**Validar username antes*/
            UsuarioDTO usuario = new UsuarioDTO
            {
                Id = 1,
                Username = username,
                Password = _passwordHasher.HashPassword(password)
            };

            /**Todo Implementar repository */
            return usuario;
        }
    }
}
