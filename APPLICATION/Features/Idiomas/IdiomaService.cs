using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN.Features.Idiomas;

namespace APPLICATION.Features.Idiomas
{
    public class IdiomaService
    {
        private static readonly List<Idioma> _idiomas = CrearIdiomas();

        public List<Idioma> Listar()
        {
            return _idiomas.ToList();
        }

        public Idioma ObtenerPorId(int id)
        {
            return _idiomas.FirstOrDefault(x => x.Id == id);
        }

        public Idioma ObtenerPorNombre(string nombre)
        {
            return _idiomas.FirstOrDefault(x =>
                string.Equals(x.Nombre, nombre, StringComparison.OrdinalIgnoreCase));
        }

        public Idioma ObtenerIdiomaPorDefecto()
        {
            return ObtenerPorId(1);
        }

        private static List<Idioma> CrearIdiomas()
        {
            Idioma espanol = Idioma.Crear(1, "Espanol");
            Idioma ingles = Idioma.Crear(2, "Ingles");

            int idPalabra = 1;
            int idTraduccion = 1;

            Action<string, string, string> agregar = (clave, textoEspanol, textoIngles) =>
            {
                Palabra palabra = Palabra.Crear(idPalabra++, clave);

                espanol.AgregarTraduccion(Traduccion.Crear(idTraduccion++, palabra, textoEspanol));
                ingles.AgregarTraduccion(Traduccion.Crear(idTraduccion++, palabra, textoIngles));
            };

            agregar("FrmPrincipal.Text", "Sistema", "System");
            agregar("Menu.Usuario", "Usuario", "User");
            agregar("Menu.UsuarioActual", "Usuario: {0}", "User: {0}");
            agregar("Menu.IniciarSesion", "Iniciar sesion", "Log in");
            agregar("Menu.CerrarSesion", "Cerrar sesion", "Log out");
            agregar("Menu.AdministrarUsuarios", "Administrar usuarios", "Manage users");
            agregar("Menu.Idioma", "Idioma", "Language");
            agregar("Idioma.Espanol", "Espanol", "Spanish");
            agregar("Idioma.Ingles", "Ingles", "English");

            agregar("FrmLogin.Text", "Iniciar sesion", "Log in");
            agregar("Login.Titulo", "Iniciar sesion", "Log in");
            agregar("Campo.Username", "Nombre de usuario", "Username");
            agregar("Campo.Password", "Contrasena", "Password");

            agregar("FrmAdministrarUsuarios.Text", "Administracion de usuarios", "User administration");
            agregar("AdministrarUsuarios.Titulo", "Administracion de usuarios", "User administration");
            agregar("AdministrarUsuarios.NuevoUsuario", "Nuevo usuario", "New user");
            agregar("AdministrarUsuarios.CrearUsuario", "Crear usuario", "Create user");
            agregar("AdministrarUsuarios.EditarUsuario", "Editar usuario", "Edit user");
            agregar("AdministrarUsuarios.EliminarUsuario", "Eliminar usuario", "Delete user");
            agregar("AdministrarUsuarios.UsuarioActual", "Usuario: {0}", "User: {0}");
            agregar("AdministrarUsuarios.SesionIniciada", "Sesion iniciada: {0}", "Session started: {0}");

            agregar("Columna.Id", "Id", "Id");
            agregar("Columna.Username", "Nombre de usuario", "Username");
            agregar("Columna.Password", "Contrasena", "Password");

            agregar("Titulo.AccesoDenegado", "Acceso denegado", "Access denied");
            agregar("Titulo.Error", "Error", "Error");
            agregar("Titulo.Exito", "Exito", "Success");
            agregar("Titulo.ConfirmarEliminacion", "Confirmar eliminacion", "Confirm deletion");
            agregar("Titulo.ConfirmarEdicion", "Confirmar edicion", "Confirm edition");
            agregar("Titulo.UsuarioDefectoCreado", "Usuario por defecto creado", "Default user created");

            agregar("Mensaje.DebeIniciarSesion", "Debes iniciar sesion para acceder a esta seccion.", "You must log in to access this section.");
            agregar("Mensaje.SinPermisos", "No tenes permisos para acceder a esta seccion.", "You do not have permission to access this section.");
            agregar("Mensaje.UsuarioDefectoCreado", "No hay usuarios registrados. Se creara un usuario por defecto con username 'admin' y password '123'.", "There are no registered users. A default user will be created with username 'admin' and password '123'.");
            agregar("Mensaje.ErrorIniciarSesion", "Error al iniciar sesion: {0}", "Login error: {0}");
            agregar("Mensaje.UsuarioCreado", "Usuario creado exitosamente.", "User created successfully.");
            agregar("Mensaje.ErrorCrearUsuario", "Error al crear usuario: {0}", "Error creating user: {0}");
            agregar("Mensaje.NoEliminarPropio", "No podes eliminarte a vos mismo.", "You cannot delete yourself.");
            agregar("Mensaje.ConfirmarEliminarUsuario", "Estas seguro de eliminar al usuario '{0}'?", "Are you sure you want to delete user '{0}'?");
            agregar("Mensaje.UsuarioEliminado", "Usuario eliminado exitosamente.", "User deleted successfully.");
            agregar("Mensaje.ErrorEliminarUsuario", "Error al eliminar usuario: {0}", "Error deleting user: {0}");
            agregar("Mensaje.ConfirmarEditarUsuario", "Estas seguro de editar al usuario '{0}'?", "Are you sure you want to edit user '{0}'?");
            agregar("Mensaje.UsuarioEditado", "Usuario editado exitosamente.", "User edited successfully.");
            agregar("Mensaje.ErrorEditarUsuario", "Error al editar usuario: {0}", "Error editing user: {0}");

            return new List<Idioma> { espanol, ingles };
        }
    }
}
