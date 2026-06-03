IF OBJECT_ID('Idiomas', 'U') IS NULL
BEGIN
    CREATE TABLE Idiomas (
        id_idioma int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        nombre nvarchar(100) NOT NULL UNIQUE
    );
END

IF OBJECT_ID('Palabras', 'U') IS NULL
BEGIN
    CREATE TABLE Palabras (
        id_palabra int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        texto nvarchar(200) NOT NULL UNIQUE
    );
END

IF OBJECT_ID('Traducciones', 'U') IS NULL
BEGIN
    CREATE TABLE Traducciones (
        id_traduccion int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        id_idioma int NOT NULL,
        id_palabra int NOT NULL,
        palabra_traducida nvarchar(max) NOT NULL,
        CONSTRAINT FK_Traducciones_Idiomas FOREIGN KEY (id_idioma)
            REFERENCES Idiomas(id_idioma) ON DELETE CASCADE,
        CONSTRAINT FK_Traducciones_Palabras FOREIGN KEY (id_palabra)
            REFERENCES Palabras(id_palabra) ON DELETE CASCADE,
        CONSTRAINT UQ_Traducciones_Idioma_Palabra UNIQUE (id_idioma, id_palabra)
    );
END

DECLARE @Seed TABLE (
    idioma nvarchar(100),
    clave nvarchar(200),
    texto nvarchar(max)
);

INSERT INTO @Seed (idioma, clave, texto) VALUES
('Espanol', 'FrmPrincipal.Text', 'Sistema'),
('Ingles', 'FrmPrincipal.Text', 'System'),
('Espanol', 'Menu.Usuario', 'Usuario'),
('Ingles', 'Menu.Usuario', 'User'),
('Espanol', 'Menu.UsuarioActual', 'Usuario: {0}'),
('Ingles', 'Menu.UsuarioActual', 'User: {0}'),
('Espanol', 'Menu.IniciarSesion', 'Iniciar sesion'),
('Ingles', 'Menu.IniciarSesion', 'Log in'),
('Espanol', 'Menu.CerrarSesion', 'Cerrar sesion'),
('Ingles', 'Menu.CerrarSesion', 'Log out'),
('Espanol', 'Menu.AdministrarUsuarios', 'Administrar usuarios'),
('Ingles', 'Menu.AdministrarUsuarios', 'Manage users'),
('Espanol', 'Menu.Idioma', 'Idioma'),
('Ingles', 'Menu.Idioma', 'Language'),
('Espanol', 'Menu.AdministrarTraducciones', 'Administrar traducciones'),
('Ingles', 'Menu.AdministrarTraducciones', 'Manage translations'),
('Espanol', 'Idioma.Espanol', 'Espanol'),
('Ingles', 'Idioma.Espanol', 'Spanish'),
('Espanol', 'Idioma.Ingles', 'Ingles'),
('Ingles', 'Idioma.Ingles', 'English'),
('Espanol', 'FrmLogin.Text', 'Iniciar sesion'),
('Ingles', 'FrmLogin.Text', 'Log in'),
('Espanol', 'Login.Titulo', 'Iniciar sesion'),
('Ingles', 'Login.Titulo', 'Log in'),
('Espanol', 'Campo.Username', 'Nombre de usuario'),
('Ingles', 'Campo.Username', 'Username'),
('Espanol', 'Campo.Password', 'Contrasena'),
('Ingles', 'Campo.Password', 'Password'),
('Espanol', 'FrmAdministrarUsuarios.Text', 'Administracion de usuarios'),
('Ingles', 'FrmAdministrarUsuarios.Text', 'User administration'),
('Espanol', 'AdministrarUsuarios.Titulo', 'Administracion de usuarios'),
('Ingles', 'AdministrarUsuarios.Titulo', 'User administration'),
('Espanol', 'AdministrarUsuarios.NuevoUsuario', 'Nuevo usuario'),
('Ingles', 'AdministrarUsuarios.NuevoUsuario', 'New user'),
('Espanol', 'AdministrarUsuarios.CrearUsuario', 'Crear usuario'),
('Ingles', 'AdministrarUsuarios.CrearUsuario', 'Create user'),
('Espanol', 'AdministrarUsuarios.EditarUsuario', 'Editar usuario'),
('Ingles', 'AdministrarUsuarios.EditarUsuario', 'Edit user'),
('Espanol', 'AdministrarUsuarios.EliminarUsuario', 'Eliminar usuario'),
('Ingles', 'AdministrarUsuarios.EliminarUsuario', 'Delete user'),
('Espanol', 'AdministrarUsuarios.UsuarioActual', 'Usuario: {0}'),
('Ingles', 'AdministrarUsuarios.UsuarioActual', 'User: {0}'),
('Espanol', 'AdministrarUsuarios.SesionIniciada', 'Sesion iniciada: {0}'),
('Ingles', 'AdministrarUsuarios.SesionIniciada', 'Session started: {0}'),
('Espanol', 'FrmAdministrarTraducciones.Text', 'Administracion de traducciones'),
('Ingles', 'FrmAdministrarTraducciones.Text', 'Translation administration'),
('Espanol', 'Traducciones.Titulo', 'Administracion de traducciones'),
('Ingles', 'Traducciones.Titulo', 'Translation administration'),
('Espanol', 'Traducciones.GestionTraducciones', 'Traducciones'),
('Ingles', 'Traducciones.GestionTraducciones', 'Translations'),
('Espanol', 'Traducciones.GestionIdiomas', 'Idiomas'),
('Ingles', 'Traducciones.GestionIdiomas', 'Languages'),
('Espanol', 'Traducciones.Clave', 'Clave'),
('Ingles', 'Traducciones.Clave', 'Key'),
('Espanol', 'Traducciones.Idioma', 'Idioma'),
('Ingles', 'Traducciones.Idioma', 'Language'),
('Espanol', 'Traducciones.Texto', 'Texto'),
('Ingles', 'Traducciones.Texto', 'Text'),
('Espanol', 'Traducciones.Crear', 'Crear traduccion'),
('Ingles', 'Traducciones.Crear', 'Create translation'),
('Espanol', 'Traducciones.Editar', 'Editar traduccion'),
('Ingles', 'Traducciones.Editar', 'Edit translation'),
('Espanol', 'Traducciones.Eliminar', 'Eliminar traduccion'),
('Ingles', 'Traducciones.Eliminar', 'Delete translation'),
('Espanol', 'Accion.Limpiar', 'Limpiar'),
('Ingles', 'Accion.Limpiar', 'Clear'),
('Espanol', 'Idiomas.Nombre', 'Nombre'),
('Ingles', 'Idiomas.Nombre', 'Name'),
('Espanol', 'Idiomas.Crear', 'Crear idioma'),
('Ingles', 'Idiomas.Crear', 'Create language'),
('Espanol', 'Idiomas.Editar', 'Editar idioma'),
('Ingles', 'Idiomas.Editar', 'Edit language'),
('Espanol', 'Idiomas.Eliminar', 'Eliminar idioma'),
('Ingles', 'Idiomas.Eliminar', 'Delete language'),
('Espanol', 'Columna.Id', 'Id'),
('Ingles', 'Columna.Id', 'Id'),
('Espanol', 'Columna.Username', 'Nombre de usuario'),
('Ingles', 'Columna.Username', 'Username'),
('Espanol', 'Columna.Password', 'Contrasena'),
('Ingles', 'Columna.Password', 'Password'),
('Espanol', 'Columna.IdTraduccion', 'Id'),
('Ingles', 'Columna.IdTraduccion', 'Id'),
('Espanol', 'Columna.Clave', 'Clave'),
('Ingles', 'Columna.Clave', 'Key'),
('Espanol', 'Columna.Idioma', 'Idioma'),
('Ingles', 'Columna.Idioma', 'Language'),
('Espanol', 'Columna.Texto', 'Texto'),
('Ingles', 'Columna.Texto', 'Text'),
('Espanol', 'Columna.Nombre', 'Nombre'),
('Ingles', 'Columna.Nombre', 'Name'),
('Espanol', 'Titulo.AccesoDenegado', 'Acceso denegado'),
('Ingles', 'Titulo.AccesoDenegado', 'Access denied'),
('Espanol', 'Titulo.Error', 'Error'),
('Ingles', 'Titulo.Error', 'Error'),
('Espanol', 'Titulo.Exito', 'Exito'),
('Ingles', 'Titulo.Exito', 'Success'),
('Espanol', 'Titulo.ConfirmarEliminacion', 'Confirmar eliminacion'),
('Ingles', 'Titulo.ConfirmarEliminacion', 'Confirm deletion'),
('Espanol', 'Titulo.ConfirmarEdicion', 'Confirmar edicion'),
('Ingles', 'Titulo.ConfirmarEdicion', 'Confirm edition'),
('Espanol', 'Titulo.UsuarioDefectoCreado', 'Usuario por defecto creado'),
('Ingles', 'Titulo.UsuarioDefectoCreado', 'Default user created'),
('Espanol', 'Mensaje.DebeIniciarSesion', 'Debes iniciar sesion para acceder a esta seccion.'),
('Ingles', 'Mensaje.DebeIniciarSesion', 'You must log in to access this section.'),
('Espanol', 'Mensaje.SinPermisos', 'No tenes permisos para acceder a esta seccion.'),
('Ingles', 'Mensaje.SinPermisos', 'You do not have permission to access this section.'),
('Espanol', 'Mensaje.UsuarioDefectoCreado', 'No hay usuarios registrados. Se creara un usuario por defecto con username ''admin'' y password ''123''.'),
('Ingles', 'Mensaje.UsuarioDefectoCreado', 'There are no registered users. A default user will be created with username ''admin'' and password ''123''.'),
('Espanol', 'Mensaje.ErrorIniciarSesion', 'Error al iniciar sesion: {0}'),
('Ingles', 'Mensaje.ErrorIniciarSesion', 'Login error: {0}'),
('Espanol', 'Mensaje.UsuarioCreado', 'Usuario creado exitosamente.'),
('Ingles', 'Mensaje.UsuarioCreado', 'User created successfully.'),
('Espanol', 'Mensaje.ErrorCrearUsuario', 'Error al crear usuario: {0}'),
('Ingles', 'Mensaje.ErrorCrearUsuario', 'Error creating user: {0}'),
('Espanol', 'Mensaje.NoEliminarPropio', 'No podes eliminarte a vos mismo.'),
('Ingles', 'Mensaje.NoEliminarPropio', 'You cannot delete yourself.'),
('Espanol', 'Mensaje.ConfirmarEliminarUsuario', 'Estas seguro de eliminar al usuario ''{0}''?'),
('Ingles', 'Mensaje.ConfirmarEliminarUsuario', 'Are you sure you want to delete user ''{0}''?'),
('Espanol', 'Mensaje.UsuarioEliminado', 'Usuario eliminado exitosamente.'),
('Ingles', 'Mensaje.UsuarioEliminado', 'User deleted successfully.'),
('Espanol', 'Mensaje.ErrorEliminarUsuario', 'Error al eliminar usuario: {0}'),
('Ingles', 'Mensaje.ErrorEliminarUsuario', 'Error deleting user: {0}'),
('Espanol', 'Mensaje.ConfirmarEditarUsuario', 'Estas seguro de editar al usuario ''{0}''?'),
('Ingles', 'Mensaje.ConfirmarEditarUsuario', 'Are you sure you want to edit user ''{0}''?'),
('Espanol', 'Mensaje.UsuarioEditado', 'Usuario editado exitosamente.'),
('Ingles', 'Mensaje.UsuarioEditado', 'User edited successfully.'),
('Espanol', 'Mensaje.ErrorEditarUsuario', 'Error al editar usuario: {0}'),
('Ingles', 'Mensaje.ErrorEditarUsuario', 'Error editing user: {0}'),
('Espanol', 'Mensaje.TraduccionCreada', 'Traduccion creada exitosamente.'),
('Ingles', 'Mensaje.TraduccionCreada', 'Translation created successfully.'),
('Espanol', 'Mensaje.TraduccionEditada', 'Traduccion editada exitosamente.'),
('Ingles', 'Mensaje.TraduccionEditada', 'Translation edited successfully.'),
('Espanol', 'Mensaje.TraduccionEliminada', 'Traduccion eliminada exitosamente.'),
('Ingles', 'Mensaje.TraduccionEliminada', 'Translation deleted successfully.'),
('Espanol', 'Mensaje.IdiomaCreado', 'Idioma creado exitosamente.'),
('Ingles', 'Mensaje.IdiomaCreado', 'Language created successfully.'),
('Espanol', 'Mensaje.IdiomaEditado', 'Idioma editado exitosamente.'),
('Ingles', 'Mensaje.IdiomaEditado', 'Language edited successfully.'),
('Espanol', 'Mensaje.IdiomaEliminado', 'Idioma eliminado exitosamente.'),
('Ingles', 'Mensaje.IdiomaEliminado', 'Language deleted successfully.'),
('Espanol', 'Mensaje.SeleccioneTraduccion', 'Seleccione una traduccion.'),
('Ingles', 'Mensaje.SeleccioneTraduccion', 'Select a translation.'),
('Espanol', 'Mensaje.SeleccioneIdioma', 'Seleccione un idioma.'),
('Ingles', 'Mensaje.SeleccioneIdioma', 'Select a language.'),
('Espanol', 'Mensaje.NoEliminarUltimoIdioma', 'No se puede eliminar el ultimo idioma.'),
('Ingles', 'Mensaje.NoEliminarUltimoIdioma', 'The last language cannot be deleted.'),
('Espanol', 'Mensaje.ConfirmarEliminarTraduccion', 'Estas seguro de eliminar la traduccion ''{0}''?'),
('Ingles', 'Mensaje.ConfirmarEliminarTraduccion', 'Are you sure you want to delete translation ''{0}''?'),
('Espanol', 'Mensaje.ConfirmarEliminarIdioma', 'Estas seguro de eliminar el idioma ''{0}''? Tambien se eliminaran sus traducciones.'),
('Ingles', 'Mensaje.ConfirmarEliminarIdioma', 'Are you sure you want to delete language ''{0}''? Its translations will also be deleted.'),
('Espanol', 'Mensaje.ErrorOperacion', 'Error al realizar la operacion: {0}'),
('Ingles', 'Mensaje.ErrorOperacion', 'Operation error: {0}');

INSERT INTO Idiomas (nombre)
SELECT DISTINCT s.idioma
FROM @Seed s
WHERE NOT EXISTS (SELECT 1 FROM Idiomas i WHERE i.nombre = s.idioma);

INSERT INTO Palabras (texto)
SELECT DISTINCT s.clave
FROM @Seed s
WHERE NOT EXISTS (SELECT 1 FROM Palabras p WHERE p.texto = s.clave);

INSERT INTO Traducciones (id_idioma, id_palabra, palabra_traducida)
SELECT i.id_idioma, p.id_palabra, s.texto
FROM @Seed s
INNER JOIN Idiomas i ON i.nombre = s.idioma
INNER JOIN Palabras p ON p.texto = s.clave
WHERE NOT EXISTS (
    SELECT 1
    FROM Traducciones t
    WHERE t.id_idioma = i.id_idioma
      AND t.id_palabra = p.id_palabra
);
