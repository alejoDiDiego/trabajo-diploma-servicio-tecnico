using System;
using System.Collections.Generic;
using ABSTRACTIONS.Services;
using APPLICATION.Features.Bitacora;
using APPLICATION.Features.Integridad;
using APPLICATION.Features.Usuarios.Exceptions;
using DOMAIN.Exceptions;
using DOMAIN.Features.Permisos;
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
            // T1: tras agregar activo al DVH, los DVH viejos quedan invalidos. Recalcular todos mantiene DVH/DVV consistente.
            integridadService.RecalcularTodosDV();
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

                if (usuarioDb.Activo == false)
                    throw new DatosUsuarioIncorrectosException("Usuario o contrasena incorrectos");

                bool passwordMatch = _passwordHasher.VerifyHashedPassword(usuarioDb.Password, usuarioForm.Password);

                if (passwordMatch == false)
                    throw new DatosUsuarioIncorrectosException("Usuario o contrasena incorrectos");

                UsuarioPermisoService usuarioPermisoService = new UsuarioPermisoService();
                List<PermisoComponent> permisos = usuarioPermisoService.ListarPermisosEfectivos(usuarioDb.Id);

                SessionManager.Login(usuarioDb, permisos);

                IntegridadService integridadService = new IntegridadService();
                bool integridadOK = integridadService.VerificarIntegridadUsuarios();

                SessionManager.GetInstance().IntegridadComprometida = !integridadOK;

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Inicio de sesion", "username=" + username, "SESION");
            }

            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (DatosUsuarioIncorrectosException ex)
            {
                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Inicio de sesion fallido", "username=" + username, "SESION");

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

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Creacion de usuario", "username=" + username, "USUARIOS");

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

                // Baja logica: la fila sigue existiendo con activo=0 y se conserva UsuarioPermisos.
                _usuarioRepository.CambiarEstado(usuarioDb.Id, false);

                Usuario usuarioBaja = _usuarioRepository.ObtenerPorId(usuarioDb.Id);
                string dvh = DigitoVerificadorHelper.CalcularDVH(usuarioBaja);
                usuarioBaja.SetDVH(dvh);
                _usuarioRepository.ActualizarDVH(usuarioBaja.Id, dvh);

                IntegridadService integridadService = new IntegridadService();
                integridadService.RecalcularDVVUsuarios();

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Baja logica de usuario", "username=" + username, "USUARIOS");
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

        public void Reactivar(string username)
        {
            try
            {
                Usuario usuarioDb = _usuarioRepository.ObtenerPorUsername(username);

                if (usuarioDb == null)
                    throw new UsuarioNoExisteException();

                _usuarioRepository.CambiarEstado(usuarioDb.Id, true);

                Usuario usuarioAlta = _usuarioRepository.ObtenerPorId(usuarioDb.Id);
                string dvh = DigitoVerificadorHelper.CalcularDVH(usuarioAlta);
                usuarioAlta.SetDVH(dvh);
                _usuarioRepository.ActualizarDVH(usuarioAlta.Id, dvh);

                IntegridadService integridadService = new IntegridadService();
                integridadService.RecalcularDVVUsuarios();

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Alta de usuario", "username=" + username, "USUARIOS");
            }
            catch (UsuarioNoExisteException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reactivar usuario", ex);
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
                    passwordHashed,
                    "",
                    usuarioDb.Activo
                );

                string dvh = DigitoVerificadorHelper.CalcularDVH(usuarioToUpdate);
                usuarioToUpdate.SetDVH(dvh);

                _usuarioRepository.Modificar(usuarioToUpdate);

                IntegridadService integridadService = new IntegridadService();
                integridadService.RecalcularDVVUsuarios();

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Modificacion de usuario", "id=" + id + " | username=" + username, "USUARIOS");

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
