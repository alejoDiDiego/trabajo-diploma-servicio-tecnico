using System;
using System.Collections.Generic;
using ABSTRACTIONS.Services;
using APPLICATION.Features.Usuarios.Exceptions;
using DOMAIN.Exceptions;
using DOMAIN.Features.Usuarios;
using DOMAIN.Features.Usuarios.Exceptions;
using REPOSITORY.Features.Usuarios;
using SERVICES.Auth;
using SERVICES.Security;
using static System.Collections.Specialized.BitVector32;

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

        public void Login(string username, string password)
        {
            try
            {
                Usuario usuarioForm = Usuario.CrearNuevo(
                    username,
                    password
                );

                Usuario usuarioDb = _usuarioRepository.ObtenerPorUsername(usuarioForm.Username);

                if (usuarioDb == null)
                    throw new DatosUsuarioIncorrectosException("Usuario o contrasena incorrectos");

                bool passwordMatch = _passwordHasher.VerifyHashedPassword(usuarioDb.Password, usuarioForm.Password);

                if (passwordMatch == false)
                    throw new DatosUsuarioIncorrectosException("Usuario o contrasena incorrectos");
                
                SessionManager.Login(usuarioDb);
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

        public Usuario Crear(string username, string password)
        {
            try
            {
                if (_usuarioRepository.ObtenerPorUsername(username) != null)
                    throw new UsuarioYaExisteException();

                string passwordHashed = _passwordHasher.HashPassword(password);

                Usuario usuarioToSave = Usuario.CrearNuevo(
                    username,
                    passwordHashed
                );

                Usuario usuarioDb = _usuarioRepository.Agregar(usuarioToSave);

                return usuarioDb;
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

        public List<Usuario> Listar()
        {
            try
            {
                List<Usuario> usuarios = new List<Usuario>();

                foreach (Usuario usuario in _usuarioRepository.Listar())
                    usuarios.Add(usuario);

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

        public Usuario Modificar(int id, string username, string password)
        {
            try
            {
                Usuario usuarioDb = _usuarioRepository.ObtenerPorId(id);

                if (usuarioDb == null)
                    throw new UsuarioNoExisteException();

                string passwordHashed = _passwordHasher.HashPassword(password);

                Usuario usuarioToUpdate = Usuario.CargarDesdeDB(
                    usuarioDb.Id,
                    username,
                    passwordHashed
                );

                _usuarioRepository.Modificar(usuarioToUpdate);

                return usuarioToUpdate;
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

    }
}
