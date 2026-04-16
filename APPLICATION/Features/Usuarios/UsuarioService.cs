using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Features.Usuarios.Interfaces;
using APPLICATION.Interfaces;
using DOMAIN.Features.Usuarios;

namespace APPLICATION.Features.Usuarios
{
    public class UsuarioService
    {
        IPasswordHasher _passwordHasher;
        IUsuarioRepository _usuarioRepository;

        public UsuarioService(IPasswordHasher _passwordHasher, IUsuarioRepository _usuarioRepository)
        {
            this._passwordHasher = _passwordHasher;
            this._usuarioRepository = _usuarioRepository;
        }

        public UsuarioDTO Login(UsuarioLoginDTO uDTO)
        {
            try
            {
                Usuario usuarioForm = Usuario.CrearNuevo(
                uDTO.Username,
                uDTO.Password
            );

                Usuario usuarioDb = _usuarioRepository.ObtenerPorUsername(usuarioForm.Username);

                bool passwordMatch = _passwordHasher.VerifyHashedPassword(usuarioDb.Password, usuarioForm.Password);

                if (passwordMatch == false)
                    throw new Exception("Usuario o Constraseña incorrectos");

                return new UsuarioDTO
                {
                    Id = usuarioDb.Id,
                    Username = usuarioDb.Username,
                    Password = usuarioDb.Password
                };
            }
            catch (Exception ex)
            {

            }
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

        public List<UsuarioDTO> Listar()
        {
            return new List<UsuarioDTO>();
        }
    }
}
