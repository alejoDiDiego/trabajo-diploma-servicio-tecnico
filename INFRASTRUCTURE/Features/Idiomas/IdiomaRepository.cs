using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Exceptions;
using DOMAIN.Features.Idiomas;

namespace REPOSITORY.Features.Idiomas
{
    public class IdiomaRepository
    {
        private readonly SqlHelper _db;

        public IdiomaRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public IdiomaRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            CrearTablas();
            SembrarDatosIniciales();
        }

        public List<Idioma> Listar()
        {
            string query = @"
                SELECT
                    i.id_idioma,
                    i.nombre,
                    t.id_idioma AS id_idioma_traduccion,
                    p.id_palabra,
                    p.texto AS clave,
                    t.palabra_traducida
                FROM Idiomas i
                LEFT JOIN Traducciones t ON t.id_idioma = i.id_idioma
                LEFT JOIN Palabras p ON p.id_palabra = t.id_palabra
                ORDER BY i.id_idioma, p.texto
            ";

            DataTable dt = _db.ExecuteQuery(query);
            Dictionary<int, Idioma> idiomas = new Dictionary<int, Idioma>();

            foreach (DataRow fila in dt.Rows)
            {
                int idIdioma = Convert.ToInt32(fila["id_idioma"]);

                if (!idiomas.ContainsKey(idIdioma))
                {
                    idiomas.Add(
                        idIdioma,
                        Idioma.Crear(idIdioma, fila["nombre"].ToString())
                    );
                }

                if (fila["id_idioma_traduccion"] == DBNull.Value)
                    continue;

                Palabra palabra = Palabra.Crear(
                    Convert.ToInt32(fila["id_palabra"]),
                    fila["clave"].ToString()
                );

                Traduccion traduccion = Traduccion.Crear(
                    idIdioma,
                    Convert.ToInt32(fila["id_palabra"]),
                    palabra,
                    fila["palabra_traducida"].ToString()
                );

                idiomas[idIdioma].AgregarTraduccion(traduccion);
            }

            return new List<Idioma>(idiomas.Values);
        }

        public Idioma ObtenerPorId(int id)
        {
            foreach (Idioma idioma in Listar())
            {
                if (idioma.Id == id)
                    return idioma;
            }

            return null;
        }

        public Idioma ObtenerPorNombre(string nombre)
        {
            foreach (Idioma idioma in Listar())
            {
                if (string.Equals(idioma.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                    return idioma;
            }

            return null;
        }

        public List<TraduccionEditable> ListarTraduccionesPorIdioma(int idIdioma)
        {
            string query = @"
                SELECT
                    p.id_palabra,
                    p.texto AS clave,
                    ISNULL(t.palabra_traducida, '') AS palabra_traducida
                FROM Palabras p
                LEFT JOIN Traducciones t ON t.id_palabra = p.id_palabra AND t.id_idioma = @IdIdioma
                ORDER BY p.texto
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdIdioma", idIdioma)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<TraduccionEditable> traducciones = new List<TraduccionEditable>();

            foreach (DataRow fila in dt.Rows)
            {
                // Palabras es el catalogo de claves mantenido por desarrolladores.
                traducciones.Add(new TraduccionEditable
                {
                    IdPalabra = Convert.ToInt32(fila["id_palabra"]),
                    Clave = fila["clave"].ToString(),
                    Texto = fila["palabra_traducida"].ToString()
                });
            }

            return traducciones;
        }

        public Idioma AgregarIdioma(string nombre)
        {
            Idioma idioma = Idioma.Crear(0, nombre);

            string query = @"
                INSERT INTO Idiomas (nombre) VALUES (@Nombre);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", idioma.Nombre)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);
            return Idioma.Crear(id, idioma.Nombre);
        }

        public void ModificarIdioma(int id, string nombre)
        {
            Idioma idioma = Idioma.Crear(id, nombre);

            string query = @"
                UPDATE Idiomas SET nombre=@Nombre WHERE id_idioma=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idioma.Id),
                new SqlParameter("@Nombre", idioma.Nombre)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void EliminarIdioma(int id)
        {
            string query = @"
                DELETE FROM Idiomas WHERE id_idioma=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public TraduccionEditable ObtenerTraduccionEditable(int idIdioma, int idPalabra)
        {
            string query = @"
                SELECT
                    p.id_palabra,
                    p.texto AS clave,
                    ISNULL(t.palabra_traducida, '') AS palabra_traducida
                FROM Palabras p
                LEFT JOIN Traducciones t ON t.id_palabra = p.id_palabra AND t.id_idioma = @IdIdioma
                WHERE p.id_palabra = @IdPalabra
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdIdioma", idIdioma),
                new SqlParameter("@IdPalabra", idPalabra)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count == 0)
                return null;

            DataRow fila = dt.Rows[0];
            return new TraduccionEditable
            {
                IdPalabra = Convert.ToInt32(fila["id_palabra"]),
                Clave = fila["clave"].ToString(),
                Texto = fila["palabra_traducida"].ToString()
            };
        }

        public void AgregarIdiomaConId(int id, string nombre)
        {
            string query = @"
                SET IDENTITY_INSERT Idiomas ON;
                INSERT INTO Idiomas (id_idioma, nombre) VALUES (@Id, @Nombre);
                SET IDENTITY_INSERT Idiomas OFF;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombre", nombre)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void EliminarTraduccion(int idIdioma, int idPalabra)
        {
            string query = @"
                DELETE FROM Traducciones WHERE id_idioma=@IdIdioma AND id_palabra=@IdPalabra;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdIdioma", idIdioma),
                new SqlParameter("@IdPalabra", idPalabra)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void GuardarTraduccion(int idIdioma, int idPalabra, string texto)
        {
            if (string.IsNullOrEmpty(texto))
                throw new ReglaNegocioException("La traduccion es obligatoria.");

            string query = @"
                IF EXISTS (
                    SELECT 1 FROM Traducciones WHERE id_idioma=@IdIdioma AND id_palabra=@IdPalabra
                )
                BEGIN
                    UPDATE Traducciones
                    SET palabra_traducida=@Texto
                    WHERE id_idioma=@IdIdioma AND id_palabra=@IdPalabra;
                END
                ELSE
                BEGIN
                    INSERT INTO Traducciones (id_idioma, id_palabra, palabra_traducida)
                    VALUES (@IdIdioma, @IdPalabra, @Texto);
                END

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdIdioma", idIdioma),
                new SqlParameter("@IdPalabra", idPalabra),
                new SqlParameter("@Texto", texto)
            };

            // La UI solo guarda textos sobre claves existentes; nunca crea nuevas claves.
            _db.ExecuteTransaction(query, sqlParameters);
        }

        private void CrearTablas()
        {
            string query = @"
                IF OBJECT_ID('Idiomas', 'U') IS NULL
                BEGIN
                    CREATE TABLE Idiomas (
                        id_idioma int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        nombre nvarchar(100) NOT NULL UNIQUE,
                        CONSTRAINT CK_Idiomas_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(nombre))) > 0)
                    );
                END

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.check_constraints
                    WHERE name = 'CK_Idiomas_Nombre_NoVacio'
                      AND parent_object_id = OBJECT_ID('Idiomas')
                )
                BEGIN
                    ALTER TABLE Idiomas WITH CHECK
                    ADD CONSTRAINT CK_Idiomas_Nombre_NoVacio
                    CHECK (LEN(LTRIM(RTRIM(nombre))) > 0);
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
                        id_idioma int NOT NULL,
                        id_palabra int NOT NULL,
                        palabra_traducida nvarchar(max) NOT NULL,
                        CONSTRAINT PK_Traducciones PRIMARY KEY (id_idioma, id_palabra),
                        CONSTRAINT FK_Traducciones_Idiomas FOREIGN KEY (id_idioma)
                            REFERENCES Idiomas(id_idioma) ON DELETE CASCADE,
                        CONSTRAINT FK_Traducciones_Palabras FOREIGN KEY (id_palabra)
                            REFERENCES Palabras(id_palabra) ON DELETE CASCADE
                    );
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        private void SembrarDatosIniciales()
        {
            AgregarSeed("Espanol", "FrmPrincipal.Text", "Sistema");
            AgregarSeed("Ingles", "FrmPrincipal.Text", "System");
            AgregarSeed("Espanol", "Menu.Usuario", "Usuario");
            AgregarSeed("Ingles", "Menu.Usuario", "User");
            AgregarSeed("Espanol", "Menu.UsuarioActual", "Usuario: {0}");
            AgregarSeed("Ingles", "Menu.UsuarioActual", "User: {0}");
            AgregarSeed("Espanol", "Menu.IniciarSesion", "Iniciar sesion");
            AgregarSeed("Ingles", "Menu.IniciarSesion", "Log in");
            AgregarSeed("Espanol", "Menu.CerrarSesion", "Cerrar sesion");
            AgregarSeed("Ingles", "Menu.CerrarSesion", "Log out");
            AgregarSeed("Espanol", "Menu.AdministrarUsuarios", "Administrar usuarios");
            AgregarSeed("Ingles", "Menu.AdministrarUsuarios", "Manage users");
            AgregarSeed("Espanol", "Menu.Idioma", "Idioma");
            AgregarSeed("Ingles", "Menu.Idioma", "Language");
            AgregarSeed("Espanol", "Menu.AdministrarTraducciones", "Administrar traducciones");
            AgregarSeed("Ingles", "Menu.AdministrarTraducciones", "Manage translations");
            AgregarSeed("Espanol", "Idioma.Espanol", "Espanol");
            AgregarSeed("Ingles", "Idioma.Espanol", "Spanish");
            AgregarSeed("Espanol", "Idioma.Ingles", "Ingles");
            AgregarSeed("Ingles", "Idioma.Ingles", "English");

            AgregarSeed("Espanol", "FrmLogin.Text", "Iniciar sesion");
            AgregarSeed("Ingles", "FrmLogin.Text", "Log in");
            AgregarSeed("Espanol", "Login.Titulo", "Iniciar sesion");
            AgregarSeed("Ingles", "Login.Titulo", "Log in");
            AgregarSeed("Espanol", "Campo.Username", "Nombre de usuario");
            AgregarSeed("Ingles", "Campo.Username", "Username");
            AgregarSeed("Espanol", "Campo.Password", "Contrasena");
            AgregarSeed("Ingles", "Campo.Password", "Password");

            AgregarSeed("Espanol", "FrmAdministrarUsuarios.Text", "Administracion de usuarios");
            AgregarSeed("Ingles", "FrmAdministrarUsuarios.Text", "User administration");
            AgregarSeed("Espanol", "AdministrarUsuarios.Titulo", "Administracion de usuarios");
            AgregarSeed("Ingles", "AdministrarUsuarios.Titulo", "User administration");
            AgregarSeed("Espanol", "AdministrarUsuarios.NuevoUsuario", "Nuevo usuario");
            AgregarSeed("Ingles", "AdministrarUsuarios.NuevoUsuario", "New user");
            AgregarSeed("Espanol", "AdministrarUsuarios.CrearUsuario", "Crear usuario");
            AgregarSeed("Ingles", "AdministrarUsuarios.CrearUsuario", "Create user");
            AgregarSeed("Espanol", "AdministrarUsuarios.EditarUsuario", "Editar usuario");
            AgregarSeed("Ingles", "AdministrarUsuarios.EditarUsuario", "Edit user");
            AgregarSeed("Espanol", "AdministrarUsuarios.EliminarUsuario", "Eliminar usuario");
            AgregarSeed("Ingles", "AdministrarUsuarios.EliminarUsuario", "Delete user");
            AgregarSeed("Espanol", "AdministrarUsuarios.UsuarioActual", "Usuario: {0}");
            AgregarSeed("Ingles", "AdministrarUsuarios.UsuarioActual", "User: {0}");
            AgregarSeed("Espanol", "AdministrarUsuarios.SesionIniciada", "Sesion iniciada: {0}");
            AgregarSeed("Ingles", "AdministrarUsuarios.SesionIniciada", "Session started: {0}");

            AgregarSeed("Espanol", "FrmAdministrarTraducciones.Text", "Administracion de traducciones");
            AgregarSeed("Ingles", "FrmAdministrarTraducciones.Text", "Translation administration");
            AgregarSeed("Espanol", "Traducciones.Titulo", "Administracion de traducciones");
            AgregarSeed("Ingles", "Traducciones.Titulo", "Translation administration");
            AgregarSeed("Espanol", "Traducciones.GestionTraducciones", "Traducciones");
            AgregarSeed("Ingles", "Traducciones.GestionTraducciones", "Translations");
            AgregarSeed("Espanol", "Traducciones.GestionIdiomas", "Idiomas");
            AgregarSeed("Ingles", "Traducciones.GestionIdiomas", "Languages");
            AgregarSeed("Espanol", "Traducciones.Clave", "Clave");
            AgregarSeed("Ingles", "Traducciones.Clave", "Key");
            AgregarSeed("Espanol", "Traducciones.Idioma", "Idioma");
            AgregarSeed("Ingles", "Traducciones.Idioma", "Language");
            AgregarSeed("Espanol", "Traducciones.Texto", "Texto");
            AgregarSeed("Ingles", "Traducciones.Texto", "Text");
            AgregarSeed("Espanol", "Traducciones.Crear", "Crear traduccion");
            AgregarSeed("Ingles", "Traducciones.Crear", "Create translation");
            AgregarSeed("Espanol", "Traducciones.Editar", "Editar traduccion");
            AgregarSeed("Ingles", "Traducciones.Editar", "Edit translation");
            AgregarSeed("Espanol", "Traducciones.Eliminar", "Eliminar traduccion");
            AgregarSeed("Ingles", "Traducciones.Eliminar", "Delete translation");
            AgregarSeed("Espanol", "Accion.Limpiar", "Limpiar");
            AgregarSeed("Ingles", "Accion.Limpiar", "Clear");
            AgregarSeed("Espanol", "Idiomas.Nombre", "Nombre");
            AgregarSeed("Ingles", "Idiomas.Nombre", "Name");
            AgregarSeed("Espanol", "Idiomas.Crear", "Crear idioma");
            AgregarSeed("Ingles", "Idiomas.Crear", "Create language");
            AgregarSeed("Espanol", "Idiomas.Editar", "Editar idioma");
            AgregarSeed("Ingles", "Idiomas.Editar", "Edit language");
            AgregarSeed("Espanol", "Idiomas.Eliminar", "Eliminar idioma");
            AgregarSeed("Ingles", "Idiomas.Eliminar", "Delete language");

            AgregarSeed("Espanol", "Columna.Id", "Id");
            AgregarSeed("Ingles", "Columna.Id", "Id");
            AgregarSeed("Espanol", "Columna.Username", "Nombre de usuario");
            AgregarSeed("Ingles", "Columna.Username", "Username");
            AgregarSeed("Espanol", "Columna.Password", "Contrasena");
            AgregarSeed("Ingles", "Columna.Password", "Password");
            AgregarSeed("Espanol", "Columna.IdTraduccion", "Id");
            AgregarSeed("Ingles", "Columna.IdTraduccion", "Id");
            AgregarSeed("Espanol", "Columna.Clave", "Clave");
            AgregarSeed("Ingles", "Columna.Clave", "Key");
            AgregarSeed("Espanol", "Columna.Idioma", "Idioma");
            AgregarSeed("Ingles", "Columna.Idioma", "Language");
            AgregarSeed("Espanol", "Columna.Texto", "Texto");
            AgregarSeed("Ingles", "Columna.Texto", "Text");
            AgregarSeed("Espanol", "Columna.Nombre", "Nombre");
            AgregarSeed("Ingles", "Columna.Nombre", "Name");

            AgregarSeed("Espanol", "Titulo.AccesoDenegado", "Acceso denegado");
            AgregarSeed("Ingles", "Titulo.AccesoDenegado", "Access denied");
            AgregarSeed("Espanol", "Titulo.Error", "Error");
            AgregarSeed("Ingles", "Titulo.Error", "Error");
            AgregarSeed("Espanol", "Titulo.Exito", "Exito");
            AgregarSeed("Ingles", "Titulo.Exito", "Success");
            AgregarSeed("Espanol", "Titulo.ConfirmarEliminacion", "Confirmar eliminacion");
            AgregarSeed("Ingles", "Titulo.ConfirmarEliminacion", "Confirm deletion");
            AgregarSeed("Espanol", "Titulo.ConfirmarEdicion", "Confirmar edicion");
            AgregarSeed("Ingles", "Titulo.ConfirmarEdicion", "Confirm edition");
            AgregarSeed("Espanol", "Titulo.UsuarioDefectoCreado", "Usuario por defecto creado");
            AgregarSeed("Ingles", "Titulo.UsuarioDefectoCreado", "Default user created");

            AgregarSeed("Espanol", "Mensaje.DebeIniciarSesion", "Debes iniciar sesion para acceder a esta seccion.");
            AgregarSeed("Ingles", "Mensaje.DebeIniciarSesion", "You must log in to access this section.");
            AgregarSeed("Espanol", "Mensaje.SinPermisos", "No tenes permisos para acceder a esta seccion.");
            AgregarSeed("Ingles", "Mensaje.SinPermisos", "You do not have permission to access this section.");
            AgregarSeed("Espanol", "Mensaje.UsuarioDefectoCreado", "No hay usuarios registrados. Se creara un usuario por defecto con username 'admin' y password '123'.");
            AgregarSeed("Ingles", "Mensaje.UsuarioDefectoCreado", "There are no registered users. A default user will be created with username 'admin' and password '123'.");
            AgregarSeed("Espanol", "Mensaje.ErrorIniciarSesion", "Error al iniciar sesion: {0}");
            AgregarSeed("Ingles", "Mensaje.ErrorIniciarSesion", "Login error: {0}");
            AgregarSeed("Espanol", "Mensaje.UsuarioCreado", "Usuario creado exitosamente.");
            AgregarSeed("Ingles", "Mensaje.UsuarioCreado", "User created successfully.");
            AgregarSeed("Espanol", "Mensaje.ErrorCrearUsuario", "Error al crear usuario: {0}");
            AgregarSeed("Ingles", "Mensaje.ErrorCrearUsuario", "Error creating user: {0}");
            AgregarSeed("Espanol", "Mensaje.NoEliminarPropio", "No podes eliminarte a vos mismo.");
            AgregarSeed("Ingles", "Mensaje.NoEliminarPropio", "You cannot delete yourself.");
            AgregarSeed("Espanol", "Mensaje.ConfirmarEliminarUsuario", "Estas seguro de eliminar al usuario '{0}'?");
            AgregarSeed("Ingles", "Mensaje.ConfirmarEliminarUsuario", "Are you sure you want to delete user '{0}'?");
            AgregarSeed("Espanol", "Mensaje.UsuarioEliminado", "Usuario eliminado exitosamente.");
            AgregarSeed("Ingles", "Mensaje.UsuarioEliminado", "User deleted successfully.");
            AgregarSeed("Espanol", "Mensaje.ErrorEliminarUsuario", "Error al eliminar usuario: {0}");
            AgregarSeed("Ingles", "Mensaje.ErrorEliminarUsuario", "Error deleting user: {0}");
            AgregarSeed("Espanol", "Mensaje.ConfirmarEditarUsuario", "Estas seguro de editar al usuario '{0}'?");
            AgregarSeed("Ingles", "Mensaje.ConfirmarEditarUsuario", "Are you sure you want to edit user '{0}'?");
            AgregarSeed("Espanol", "Mensaje.UsuarioEditado", "Usuario editado exitosamente.");
            AgregarSeed("Ingles", "Mensaje.UsuarioEditado", "User edited successfully.");
            AgregarSeed("Espanol", "Mensaje.ErrorEditarUsuario", "Error al editar usuario: {0}");
            AgregarSeed("Ingles", "Mensaje.ErrorEditarUsuario", "Error editing user: {0}");
            AgregarSeed("Espanol", "Mensaje.TraduccionCreada", "Traduccion creada exitosamente.");
            AgregarSeed("Ingles", "Mensaje.TraduccionCreada", "Translation created successfully.");
            AgregarSeed("Espanol", "Mensaje.TraduccionEditada", "Traduccion editada exitosamente.");
            AgregarSeed("Ingles", "Mensaje.TraduccionEditada", "Translation edited successfully.");
            AgregarSeed("Espanol", "Mensaje.TraduccionEliminada", "Traduccion eliminada exitosamente.");
            AgregarSeed("Ingles", "Mensaje.TraduccionEliminada", "Translation deleted successfully.");
            AgregarSeed("Espanol", "Mensaje.IdiomaCreado", "Idioma creado exitosamente.");
            AgregarSeed("Ingles", "Mensaje.IdiomaCreado", "Language created successfully.");
            AgregarSeed("Espanol", "Mensaje.NombreIdiomaObligatorio", "El nombre del idioma es obligatorio.");
            AgregarSeed("Ingles", "Mensaje.NombreIdiomaObligatorio", "The language name is required.");
            AgregarSeed("Espanol", "Mensaje.IdiomaEditado", "Idioma editado exitosamente.");
            AgregarSeed("Ingles", "Mensaje.IdiomaEditado", "Language edited successfully.");
            AgregarSeed("Espanol", "Mensaje.IdiomaEliminado", "Idioma eliminado exitosamente.");
            AgregarSeed("Ingles", "Mensaje.IdiomaEliminado", "Language deleted successfully.");
            AgregarSeed("Espanol", "Mensaje.SeleccioneTraduccion", "Seleccione una traduccion.");
            AgregarSeed("Ingles", "Mensaje.SeleccioneTraduccion", "Select a translation.");
            AgregarSeed("Espanol", "Mensaje.SeleccioneIdioma", "Seleccione un idioma.");
            AgregarSeed("Ingles", "Mensaje.SeleccioneIdioma", "Select a language.");
            AgregarSeed("Espanol", "Mensaje.NoEliminarUltimoIdioma", "No se puede eliminar el ultimo idioma.");
            AgregarSeed("Ingles", "Mensaje.NoEliminarUltimoIdioma", "The last language cannot be deleted.");
            AgregarSeed("Espanol", "Mensaje.ConfirmarEliminarTraduccion", "Estas seguro de eliminar la traduccion '{0}'?");
            AgregarSeed("Ingles", "Mensaje.ConfirmarEliminarTraduccion", "Are you sure you want to delete translation '{0}'?");
            AgregarSeed("Espanol", "Mensaje.ConfirmarEliminarIdioma", "Estas seguro de eliminar el idioma '{0}'? Tambien se eliminaran sus traducciones.");
            AgregarSeed("Ingles", "Mensaje.ConfirmarEliminarIdioma", "Are you sure you want to delete language '{0}'? Its translations will also be deleted.");
            AgregarSeed("Espanol", "Mensaje.ErrorOperacion", "Error al realizar la operacion: {0}");
            AgregarSeed("Ingles", "Mensaje.ErrorOperacion", "Operation error: {0}");
            AgregarSeed("Espanol", "Mensaje.OperacionExitosa", "Operacion realizada exitosamente.");
            AgregarSeed("Ingles", "Mensaje.OperacionExitosa", "Operation completed successfully.");
            AgregarSeed("Espanol", "Mensaje.SeleccioneRegistro", "Seleccione un registro.");
            AgregarSeed("Ingles", "Mensaje.SeleccioneRegistro", "Select a record.");
            AgregarSeed("Espanol", "Mensaje.ConfirmarDesactivar", "Confirma que desea desactivar el registro seleccionado?");
            AgregarSeed("Ingles", "Mensaje.ConfirmarDesactivar", "Are you sure you want to deactivate the selected record?");
            AgregarSeed("Espanol", "Mensaje.ConfirmarReactivar", "Confirma que desea reactivar el registro seleccionado?");
            AgregarSeed("Ingles", "Mensaje.ConfirmarReactivar", "Are you sure you want to reactivate the selected record?");
            AgregarSeed("Espanol", "Mensaje.ClienteCamposObligatorios", "Nombre, apellido y documento son obligatorios.");
            AgregarSeed("Ingles", "Mensaje.ClienteCamposObligatorios", "First name, last name and document are required.");
            AgregarSeed("Espanol", "Mensaje.EquipoCamposObligatorios", "Cliente, tipo y marca son obligatorios.");
            AgregarSeed("Ingles", "Mensaje.EquipoCamposObligatorios", "Customer, type and brand are required.");
            AgregarSeed("Espanol", "Mensaje.NombreObligatorio", "El nombre es obligatorio.");
            AgregarSeed("Ingles", "Mensaje.NombreObligatorio", "The name is required.");

            AgregarSeed("Espanol", "Titulo.ConfirmarDesactivacion", "Confirmar desactivacion");
            AgregarSeed("Ingles", "Titulo.ConfirmarDesactivacion", "Confirm deactivation");
            AgregarSeed("Espanol", "Titulo.ConfirmarReactivacion", "Confirmar reactivacion");
            AgregarSeed("Ingles", "Titulo.ConfirmarReactivacion", "Confirm reactivation");

            AgregarSeed("Espanol", "Menu.Gestion", "Gestion");
            AgregarSeed("Ingles", "Menu.Gestion", "Management");
            AgregarSeed("Espanol", "Menu.Clientes", "Clientes");
            AgregarSeed("Ingles", "Menu.Clientes", "Customers");
            AgregarSeed("Espanol", "Menu.Equipos", "Equipos");
            AgregarSeed("Ingles", "Menu.Equipos", "Devices");
            AgregarSeed("Espanol", "Menu.Catalogos", "Catalogos");
            AgregarSeed("Ingles", "Menu.Catalogos", "Catalogs");
            AgregarSeed("Espanol", "Menu.TiposEquipo", "Tipos de equipo");
            AgregarSeed("Ingles", "Menu.TiposEquipo", "Device types");
            AgregarSeed("Espanol", "Menu.Marcas", "Marcas");
            AgregarSeed("Ingles", "Menu.Marcas", "Brands");

            AgregarSeed("Espanol", "FrmClientes.Text", "Administracion de clientes");
            AgregarSeed("Ingles", "FrmClientes.Text", "Customer administration");
            AgregarSeed("Espanol", "Clientes.Titulo", "Administracion de clientes");
            AgregarSeed("Ingles", "Clientes.Titulo", "Customer administration");
            AgregarSeed("Espanol", "Clientes.FiltroNombre", "Nombre:");
            AgregarSeed("Ingles", "Clientes.FiltroNombre", "First name:");
            AgregarSeed("Espanol", "Clientes.FiltroApellido", "Apellido:");
            AgregarSeed("Ingles", "Clientes.FiltroApellido", "Last name:");
            AgregarSeed("Espanol", "Clientes.FiltroDocumento", "Documento:");
            AgregarSeed("Ingles", "Clientes.FiltroDocumento", "Document:");
            AgregarSeed("Espanol", "Clientes.VerInactivos", "Ver inactivos");
            AgregarSeed("Ingles", "Clientes.VerInactivos", "Show inactive");
            AgregarSeed("Espanol", "Clientes.Crear", "Crear");
            AgregarSeed("Ingles", "Clientes.Crear", "Create");
            AgregarSeed("Espanol", "Clientes.Editar", "Editar");
            AgregarSeed("Ingles", "Clientes.Editar", "Edit");
            AgregarSeed("Espanol", "Clientes.Desactivar", "Desactivar");
            AgregarSeed("Ingles", "Clientes.Desactivar", "Deactivate");
            AgregarSeed("Espanol", "Clientes.Reactivar", "Reactivar");
            AgregarSeed("Ingles", "Clientes.Reactivar", "Reactivate");

            AgregarSeed("Espanol", "ClienteEditar.TituloNuevo", "Nuevo cliente");
            AgregarSeed("Ingles", "ClienteEditar.TituloNuevo", "New customer");
            AgregarSeed("Espanol", "ClienteEditar.TituloEditar", "Editar cliente");
            AgregarSeed("Ingles", "ClienteEditar.TituloEditar", "Edit customer");

            AgregarSeed("Espanol", "FrmEquipos.Text", "Administracion de equipos");
            AgregarSeed("Ingles", "FrmEquipos.Text", "Device administration");
            AgregarSeed("Espanol", "Equipos.Titulo", "Administracion de equipos");
            AgregarSeed("Ingles", "Equipos.Titulo", "Device administration");
            AgregarSeed("Espanol", "Equipos.FiltroCliente", "Cliente:");
            AgregarSeed("Ingles", "Equipos.FiltroCliente", "Customer:");
            AgregarSeed("Espanol", "Equipos.FiltroTexto", "Modelo / Serie:");
            AgregarSeed("Ingles", "Equipos.FiltroTexto", "Model / Serial:");
            AgregarSeed("Espanol", "Equipos.TodosClientes", "Todos");
            AgregarSeed("Ingles", "Equipos.TodosClientes", "All");
            AgregarSeed("Espanol", "Equipos.VerInactivos", "Ver inactivos");
            AgregarSeed("Ingles", "Equipos.VerInactivos", "Show inactive");
            AgregarSeed("Espanol", "Equipos.Crear", "Crear");
            AgregarSeed("Ingles", "Equipos.Crear", "Create");
            AgregarSeed("Espanol", "Equipos.Editar", "Editar");
            AgregarSeed("Ingles", "Equipos.Editar", "Edit");
            AgregarSeed("Espanol", "Equipos.Desactivar", "Desactivar");
            AgregarSeed("Ingles", "Equipos.Desactivar", "Deactivate");
            AgregarSeed("Espanol", "Equipos.Reactivar", "Reactivar");
            AgregarSeed("Ingles", "Equipos.Reactivar", "Reactivate");

            AgregarSeed("Espanol", "EquipoEditar.TituloNuevo", "Nuevo equipo");
            AgregarSeed("Ingles", "EquipoEditar.TituloNuevo", "New device");
            AgregarSeed("Espanol", "EquipoEditar.TituloEditar", "Editar equipo");
            AgregarSeed("Ingles", "EquipoEditar.TituloEditar", "Edit device");

            AgregarSeed("Espanol", "FrmTiposEquipo.Text", "Tipos de equipo");
            AgregarSeed("Ingles", "FrmTiposEquipo.Text", "Device types");
            AgregarSeed("Espanol", "TiposEquipo.Titulo", "Tipos de equipo");
            AgregarSeed("Ingles", "TiposEquipo.Titulo", "Device types");
            AgregarSeed("Espanol", "FrmMarcas.Text", "Marcas");
            AgregarSeed("Ingles", "FrmMarcas.Text", "Brands");
            AgregarSeed("Espanol", "Marcas.Titulo", "Marcas");
            AgregarSeed("Ingles", "Marcas.Titulo", "Brands");
            AgregarSeed("Espanol", "Catalogos.FiltroNombre", "Nombre:");
            AgregarSeed("Ingles", "Catalogos.FiltroNombre", "Name:");
            AgregarSeed("Espanol", "Catalogos.VerInactivos", "Ver inactivos");
            AgregarSeed("Ingles", "Catalogos.VerInactivos", "Show inactive");
            AgregarSeed("Espanol", "Catalogos.Crear", "Crear");
            AgregarSeed("Ingles", "Catalogos.Crear", "Create");
            AgregarSeed("Espanol", "Catalogos.Editar", "Editar");
            AgregarSeed("Ingles", "Catalogos.Editar", "Edit");
            AgregarSeed("Espanol", "Catalogos.Desactivar", "Desactivar");
            AgregarSeed("Ingles", "Catalogos.Desactivar", "Deactivate");
            AgregarSeed("Espanol", "Catalogos.Reactivar", "Reactivar");
            AgregarSeed("Ingles", "Catalogos.Reactivar", "Reactivate");
            AgregarSeed("Espanol", "Catalogos.Nombre", "Nombre:");
            AgregarSeed("Ingles", "Catalogos.Nombre", "Name:");
            AgregarSeed("Espanol", "Catalogos.EditarNombre", "Editar nombre");
            AgregarSeed("Ingles", "Catalogos.EditarNombre", "Edit name");

            AgregarSeed("Espanol", "Campo.Nombre", "Nombre:");
            AgregarSeed("Ingles", "Campo.Nombre", "First name:");
            AgregarSeed("Espanol", "Campo.Apellido", "Apellido:");
            AgregarSeed("Ingles", "Campo.Apellido", "Last name:");
            AgregarSeed("Espanol", "Campo.Documento", "Documento:");
            AgregarSeed("Ingles", "Campo.Documento", "Document:");
            AgregarSeed("Espanol", "Campo.Telefono", "Telefono:");
            AgregarSeed("Ingles", "Campo.Telefono", "Phone:");
            AgregarSeed("Espanol", "Campo.Email", "Email:");
            AgregarSeed("Ingles", "Campo.Email", "Email:");
            AgregarSeed("Espanol", "Campo.Direccion", "Direccion:");
            AgregarSeed("Ingles", "Campo.Direccion", "Address:");
            AgregarSeed("Espanol", "Campo.Observaciones", "Observaciones:");
            AgregarSeed("Ingles", "Campo.Observaciones", "Notes:");
            AgregarSeed("Espanol", "Campo.Cliente", "Cliente:");
            AgregarSeed("Ingles", "Campo.Cliente", "Customer:");
            AgregarSeed("Espanol", "Campo.TipoEquipo", "Tipo:");
            AgregarSeed("Ingles", "Campo.TipoEquipo", "Type:");
            AgregarSeed("Espanol", "Campo.Marca", "Marca:");
            AgregarSeed("Ingles", "Campo.Marca", "Brand:");
            AgregarSeed("Espanol", "Campo.Modelo", "Modelo:");
            AgregarSeed("Ingles", "Campo.Modelo", "Model:");
            AgregarSeed("Espanol", "Campo.NumeroSerie", "Numero de serie:");
            AgregarSeed("Ingles", "Campo.NumeroSerie", "Serial number:");
            AgregarSeed("Espanol", "Campo.Imei", "Imei:");
            AgregarSeed("Ingles", "Campo.Imei", "IMEI:");
            AgregarSeed("Espanol", "Campo.Color", "Color:");
            AgregarSeed("Ingles", "Campo.Color", "Color:");

            AgregarSeed("Espanol", "Accion.Aceptar", "Aceptar");
            AgregarSeed("Ingles", "Accion.Aceptar", "Accept");
            AgregarSeed("Espanol", "Accion.Cancelar", "Cancelar");
            AgregarSeed("Ingles", "Accion.Cancelar", "Cancel");

            AgregarSeed("Espanol", "Columna.Nombre", "Nombre");
            AgregarSeed("Ingles", "Columna.Nombre", "Name");
            AgregarSeed("Espanol", "Columna.Apellido", "Apellido");
            AgregarSeed("Ingles", "Columna.Apellido", "Last name");
            AgregarSeed("Espanol", "Columna.Documento", "Documento");
            AgregarSeed("Ingles", "Columna.Documento", "Document");
            AgregarSeed("Espanol", "Columna.Telefono", "Telefono");
            AgregarSeed("Ingles", "Columna.Telefono", "Phone");
            AgregarSeed("Espanol", "Columna.Email", "Email");
            AgregarSeed("Ingles", "Columna.Email", "Email");
            AgregarSeed("Espanol", "Columna.Activo", "Activo");
            AgregarSeed("Ingles", "Columna.Activo", "Active");
            AgregarSeed("Espanol", "Columna.Cliente", "Cliente");
            AgregarSeed("Ingles", "Columna.Cliente", "Customer");
            AgregarSeed("Espanol", "Columna.TipoEquipo", "Tipo");
            AgregarSeed("Ingles", "Columna.TipoEquipo", "Type");
            AgregarSeed("Espanol", "Columna.Marca", "Marca");
            AgregarSeed("Ingles", "Columna.Marca", "Brand");
            AgregarSeed("Espanol", "Columna.Modelo", "Modelo");
            AgregarSeed("Ingles", "Columna.Modelo", "Model");
            AgregarSeed("Espanol", "Columna.NumeroSerie", "Nro. serie");
            AgregarSeed("Ingles", "Columna.NumeroSerie", "Serial no.");
            AgregarSeed("Espanol", "Columna.Imei", "Imei");
            AgregarSeed("Ingles", "Columna.Imei", "IMEI");
            AgregarSeed("Espanol", "Columna.Color", "Color");
            AgregarSeed("Ingles", "Columna.Color", "Color");
        }

        private void AgregarSeed(string idioma, string clave, string texto)
        {
            string query = @"
                DECLARE @IdIdioma int;
                DECLARE @IdPalabra int;

                SELECT @IdIdioma = id_idioma FROM Idiomas WHERE nombre=@Idioma;

                IF @IdIdioma IS NULL
                BEGIN
                    INSERT INTO Idiomas (nombre) VALUES (@Idioma);
                    SET @IdIdioma = CAST(SCOPE_IDENTITY() AS int);
                END

                SELECT @IdPalabra = id_palabra FROM Palabras WHERE texto=@Clave;

                IF @IdPalabra IS NULL
                BEGIN
                    INSERT INTO Palabras (texto) VALUES (@Clave);
                    SET @IdPalabra = CAST(SCOPE_IDENTITY() AS int);
                END

                IF NOT EXISTS (
                    SELECT 1 FROM Traducciones WHERE id_idioma=@IdIdioma AND id_palabra=@IdPalabra
                )
                BEGIN
                    INSERT INTO Traducciones (id_idioma, id_palabra, palabra_traducida)
                    VALUES (@IdIdioma, @IdPalabra, @Texto);
                END

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Idioma", idioma),
                new SqlParameter("@Clave", clave),
                new SqlParameter("@Texto", texto)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }
    }
}
