using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Features.Usuarios.Exceptions;
using APPLICATION.Features.Usuarios.Interfaces;
using APPLICATION.Interfaces;
using DOMAIN.Exceptions;
using DOMAIN.Features.Usuarios;
using DOMAIN.Features.Usuarios.Exceptions;

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

                if(usuarioDb == null)
                    throw new DatosUsuarioIncorrectosException("Usuario o Constraseña incorrectos");

                bool passwordMatch = _passwordHasher.VerifyHashedPassword(usuarioDb.Password, usuarioForm.Password);

                if (passwordMatch == false)
                    throw new DatosUsuarioIncorrectosException("Usuario o Constraseña incorrectos");

                return new UsuarioDTO
                {
                    Id = usuarioDb.Id,
                    Username = usuarioDb.Username,
                    Password = usuarioDb.Password
                };
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (DatosUsuarioIncorrectosException ex)
            {
                throw new Exception("Usuario o Constraseña incorrectos", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesión", ex);
            }
        }

        public UsuarioDTO Create(UsuarioLoginDTO uDTO)
        {
            try
            {
                Usuario usuarioForm = Usuario.CrearNuevo(
                    uDTO.Username,
                    uDTO.Password
                );

                string passwordHashed = _passwordHasher.HashPassword(usuarioForm.Password);

                Usuario usuarioToSave = Usuario.CrearNuevo(
                    usuarioForm.Username,
                    passwordHashed
                );

                Usuario usuarioDb = _usuarioRepository.Agregar(usuarioToSave);

                return new UsuarioDTO
                {
                    Id = usuarioDb.Id,
                    Username = usuarioDb.Username,
                    Password = usuarioDb.Password
                };
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (UsuarioYaExisteException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear usuario", ex);
            }
        }

        public List<UsuarioDTO> Listar()
        {
            try
            {
                List<UsuarioDTO> usuarioDTOs = new List<UsuarioDTO>();

                foreach (var usuario in _usuarioRepository.Listar())
                {
                    usuarioDTOs.Add(new UsuarioDTO
                    {
                        Id = usuario.Id,
                        Username = usuario.Username,
                        Password = usuario.Password
                    });
                }

                return usuarioDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar usuarios", ex);
            }
        }
    }
}
