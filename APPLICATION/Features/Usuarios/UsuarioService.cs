using System;
using System.Collections.Generic;
using ABSTRACTIONS.Services;
using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Features.Usuarios.Exceptions;
using DOMAIN.Exceptions;
using DOMAIN.Features.Usuarios;
using DOMAIN.Features.Usuarios.Exceptions;
using REPOSITORY.Features.Usuarios;
using SERVICES.Security;

namespace APPLICATION.Features.Usuarios
{
    public class UsuarioService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService()
        {
            _passwordHasher = new PasswordHasher();
            _usuarioRepository = new UsuarioRepository();
        }

        public UsuarioDTO Login(UsuarioLoginDTO usuarioLogin)
        {
            try
            {
                Usuario usuarioForm = Usuario.CrearNuevo(
                    usuarioLogin.Username,
                    usuarioLogin.Password
                );

                Usuario usuarioDb = _usuarioRepository.ObtenerPorUsername(usuarioForm.Username);

                if (usuarioDb == null)
                    throw new DatosUsuarioIncorrectosException("Usuario o contrasena incorrectos");

                bool passwordMatch = _passwordHasher.VerifyHashedPassword(usuarioDb.Password, usuarioForm.Password);

                if (passwordMatch == false)
                    throw new DatosUsuarioIncorrectosException("Usuario o contrasena incorrectos");

                return MapearUsuario(usuarioDb);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (DatosUsuarioIncorrectosException ex)
            {
                throw new Exception("Usuario o contrasena incorrectos", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesion", ex);
            }
        }

        public UsuarioDTO Crear(UsuarioLoginDTO usuarioLogin)
        {
            try
            {
                Usuario usuarioForm = Usuario.CrearNuevo(
                    usuarioLogin.Username,
                    usuarioLogin.Password
                );

                if (_usuarioRepository.ObtenerPorUsername(usuarioForm.Username) != null)
                    throw new UsuarioYaExisteException();

                string passwordHashed = _passwordHasher.HashPassword(usuarioForm.Password);

                Usuario usuarioToSave = Usuario.CrearNuevo(
                    usuarioForm.Username,
                    passwordHashed
                );

                Usuario usuarioDb = _usuarioRepository.Agregar(usuarioToSave);

                return MapearUsuario(usuarioDb);
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
                List<UsuarioDTO> usuarios = new List<UsuarioDTO>();

                foreach (Usuario usuario in _usuarioRepository.Listar())
                    usuarios.Add(MapearUsuario(usuario));

                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar usuarios", ex);
            }
        }

        public void Eliminar(string username)
        {
            try
            {
                Usuario usuarioDb = _usuarioRepository.ObtenerPorUsername(username);

                if (usuarioDb == null)
                    throw new UsuarioNoExisteException();

                _usuarioRepository.Eliminar(usuarioDb.Id);
            }
            catch (UsuarioNoExisteException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario", ex);
            }
        }

        public UsuarioDTO Modificar(UsuarioDTO usuarioForm)
        {
            try
            {
                Usuario usuarioDb = _usuarioRepository.ObtenerPorId(usuarioForm.Id);

                if (usuarioDb == null)
                    throw new UsuarioNoExisteException();

                string passwordHashed = _passwordHasher.HashPassword(usuarioForm.Password);

                Usuario usuarioToUpdate = Usuario.CargarDesdeDB(
                    usuarioDb.Id,
                    usuarioForm.Username,
                    passwordHashed
                );

                _usuarioRepository.Modificar(usuarioToUpdate);

                return MapearUsuario(usuarioToUpdate);
            }
            catch (UsuarioNoExisteException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar usuario", ex);
            }
        }

        private UsuarioDTO MapearUsuario(Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Password = usuario.Password
            };
        }
    }
}
