using System;
using System.Collections.Generic;
using ABSTRACTIONS.Services;
using APPLICATION.Features.Integridad;
using APPLICATION.Features.Usuarios.Exceptions;
using DOMAIN.Exceptions;
using DOMAIN.Features.Usuarios;
using DOMAIN.Features.Usuarios.Exceptions;
using REPOSITORY.Features.Permisos;
using REPOSITORY.Features.Usuarios;
using SERVICES.Auth;
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

        public void Inicializar()
        {
            // Prepara Usuarios y crea cuentas iniciales de prueba.
            _usuarioRepository.Inicializar();

            // Usuarios de prueba iniciales. Password para todos: 123.
            CrearUsuarioBase("admin", "123");
            CrearUsuarioBase("usuarios", "123");
            CrearUsuarioBase("permisos", "123");
            CrearUsuarioBase("idiomas", "123");
            CrearUsuarioBase("lector", "123");

            UsuarioPermisoRepository usuarioPermisoRepository = new UsuarioPermisoRepository();
            usuarioPermisoRepository.Inicializar();

            IntegridadService integridadService = new IntegridadService();
            integridadService.RecalcularDVVUsuarios();
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

                UsuarioPermisoService usuarioPermisoService = new UsuarioPermisoService();
                List<string> codigosPermisos = usuarioPermisoService.ListarCodigosPermisosEfectivos(usuarioDb.Id);

                SessionManager.Login(usuarioDb, codigosPermisos);

                IntegridadService integridadService = new IntegridadService();
                bool integridadOK = integridadService.VerificarIntegridadUsuarios();

                SessionManager.GetInstance().IntegridadComprometida = !integridadOK;
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

                if (string.IsNullOrEmpty(usuarioDb.DVH))
                {
                    string dvh = DigitoVerificadorHelper.CalcularDVH(usuarioDb);
                    usuarioDb.SetDVH(dvh);
                    _usuarioRepository.ActualizarDVH(usuarioDb.Id, dvh);
                }

                IntegridadService integridadService = new IntegridadService();
                integridadService.RecalcularDVVUsuarios();

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

                string dvh = DigitoVerificadorHelper.CalcularDVH(usuarioToUpdate);
                usuarioToUpdate.SetDVH(dvh);

                _usuarioRepository.Modificar(usuarioToUpdate);

                IntegridadService integridadService = new IntegridadService();
                integridadService.RecalcularDVVUsuarios();

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

        private void CrearUsuarioBase(string username, string password)
        {
            if (_usuarioRepository.ObtenerPorUsername(username) != null)
                return;

            string passwordHashed = _passwordHasher.HashPassword(password);
            Usuario usuario = Usuario.CrearNuevo(username, passwordHashed);

            Usuario usuarioDb = _usuarioRepository.Agregar(usuario);

            if (string.IsNullOrEmpty(usuarioDb.DVH))
            {
                string dvh = DigitoVerificadorHelper.CalcularDVH(usuarioDb);
                usuarioDb.SetDVH(dvh);
                _usuarioRepository.ActualizarDVH(usuarioDb.Id, dvh);
            }
        }

    }
}
