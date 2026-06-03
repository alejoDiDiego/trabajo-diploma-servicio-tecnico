using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DOMAIN.Exceptions;
using DOMAIN.Features.Permisos;

namespace REPOSITORY.Features.Permisos
{
    public class PermisoRepository
    {
        private readonly SqlHelper _db;

        public PermisoRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public PermisoRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            CrearTablaPermisos();
            AgregarPermisosPrueba();
            AgregarTraduccionesPermisos();
        }

        public List<PermisoComponent> ListarArbol()
        {
            string query = @"
                SELECT id_permiso, nombre, codigo, descripcion, es_familia, id_permiso_padre
                FROM Permisos
                ORDER BY id_permiso_padre, nombre;
            ";

            DataTable permisos = _db.ExecuteQuery(query);
            List<PermisoComponent> raices = new List<PermisoComponent>();
            List<PermisoComponent> padresActuales = new List<PermisoComponent>();

            foreach (DataRow fila in permisos.Rows)
            {
                if (fila["id_permiso_padre"] != DBNull.Value)
                    continue;

                PermisoComponent permiso = CrearComponente(fila);
                raices.Add(permiso);

                if (permiso.EsFamilia)
                    padresActuales.Add(permiso);
            }

            while (padresActuales.Count > 0)
            {
                List<PermisoComponent> proximosPadres = new List<PermisoComponent>();

                foreach (PermisoComponent padre in padresActuales)
                {
                    foreach (DataRow fila in permisos.Rows)
                    {
                        if (fila["id_permiso_padre"] == DBNull.Value)
                            continue;
                        if (Convert.ToInt32(fila["id_permiso_padre"]) != padre.Id)
                            continue;

                        PermisoComponent hijo = CrearComponente(fila);
                        padre.AgregarHijo(hijo);

                        if (hijo.EsFamilia)
                            proximosPadres.Add(hijo);
                    }
                }

                padresActuales = proximosPadres;
            }

            return raices.OrderBy(x => x.Nombre).ToList();
        }

        public PermisoComponent Agregar(string nombre, string codigo, string descripcion, bool esFamilia, int? idPadre)
        {
            string query = @"
                INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                VALUES (@Nombre, @Codigo, @Descripcion, @EsFamilia, @IdPadre);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@Codigo", codigo),
                new SqlParameter("@Descripcion", (object)descripcion ?? DBNull.Value),
                new SqlParameter("@EsFamilia", esFamilia),
                new SqlParameter("@IdPadre", idPadre.HasValue ? (object)idPadre.Value : DBNull.Value)
            };

            try
            {
                int id = _db.ExecuteTransaction(query, sqlParameters);

                if (esFamilia)
                    return FamiliaPermiso.CargarDesdeDB(id, nombre, codigo, descripcion);

                return PermisoSimple.CargarDesdeDB(id, nombre, codigo, descripcion);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new ReglaNegocioException("Ya existe un permiso con ese codigo.");
            }
        }

        public void Modificar(int id, string nombre, string codigo, string descripcion)
        {
            string query = @"
                UPDATE Permisos
                SET nombre=@Nombre, codigo=@Codigo, descripcion=@Descripcion
                WHERE id_permiso=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombre", nombre),
                new SqlParameter("@Codigo", codigo),
                new SqlParameter("@Descripcion", (object)descripcion ?? DBNull.Value)
            };

            try
            {
                _db.ExecuteTransaction(query, sqlParameters);
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new ReglaNegocioException("Ya existe un permiso con ese codigo.");
            }
        }

        public void Eliminar(int id)
        {
            if (TieneHijos(id))
                throw new ReglaNegocioException("No se puede eliminar una familia con permisos hijos.");

            string query = @"
                DELETE FROM Permisos WHERE id_permiso=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Mover(int idPermiso, int? idPadre)
        {
            string query = @"
                UPDATE Permisos
                SET id_permiso_padre=@IdPadre
                WHERE id_permiso=@IdPermiso;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPermiso", idPermiso),
                new SqlParameter("@IdPadre", idPadre.HasValue ? (object)idPadre.Value : DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public PermisoComponent ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_permiso, nombre, codigo, descripcion, es_familia, id_permiso_padre
                FROM Permisos
                WHERE id_permiso=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return CrearComponente(dt.Rows[0]);
        }

        private void CrearTablaPermisos()
        {
            string query = @"
                IF OBJECT_ID('Permisos', 'U') IS NULL
                BEGIN
                    CREATE TABLE Permisos (
                        id_permiso int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        nombre nvarchar(100) NOT NULL,
                        codigo nvarchar(100) NOT NULL UNIQUE,
                        descripcion nvarchar(500) NULL,
                        es_familia bit NOT NULL,
                        id_permiso_padre int NULL,
                        CONSTRAINT FK_Permisos_Padre FOREIGN KEY (id_permiso_padre)
                            REFERENCES Permisos(id_permiso)
                    );
                END
            ";

            _db.ExecuteTransaction(query);
        }

        private void AgregarTraduccionesPermisos()
        {
            string query = @"
                IF OBJECT_ID('Idiomas', 'U') IS NOT NULL
                   AND OBJECT_ID('Palabras', 'U') IS NOT NULL
                   AND OBJECT_ID('Traducciones', 'U') IS NOT NULL
                BEGIN
                    DECLARE @Seed TABLE (
                        idioma nvarchar(100),
                        clave nvarchar(200),
                        texto nvarchar(max)
                    );

                    INSERT INTO @Seed (idioma, clave, texto) VALUES
                    ('Espanol', 'Menu.AdministrarPermisos', 'Administrar permisos'),
                    ('Ingles', 'Menu.AdministrarPermisos', 'Manage permissions'),
                    ('Espanol', 'FrmAdministrarPermisos.Text', 'Administracion de permisos'),
                    ('Ingles', 'FrmAdministrarPermisos.Text', 'Permission administration'),
                    ('Espanol', 'Permisos.Titulo', 'Administracion de permisos'),
                    ('Ingles', 'Permisos.Titulo', 'Permission administration'),
                    ('Espanol', 'Permisos.Arbol', 'Arbol de permisos'),
                    ('Ingles', 'Permisos.Arbol', 'Permission tree'),
                    ('Espanol', 'Permisos.Detalle', 'Detalle'),
                    ('Ingles', 'Permisos.Detalle', 'Detail'),
                    ('Espanol', 'Permisos.Nombre', 'Nombre'),
                    ('Ingles', 'Permisos.Nombre', 'Name'),
                    ('Espanol', 'Permisos.Codigo', 'Codigo'),
                    ('Ingles', 'Permisos.Codigo', 'Code'),
                    ('Espanol', 'Permisos.Descripcion', 'Descripcion'),
                    ('Ingles', 'Permisos.Descripcion', 'Description'),
                    ('Espanol', 'Permisos.Tipo', 'Tipo'),
                    ('Ingles', 'Permisos.Tipo', 'Type'),
                    ('Espanol', 'Permisos.TipoPermiso', 'Permiso simple'),
                    ('Ingles', 'Permisos.TipoPermiso', 'Simple permission'),
                    ('Espanol', 'Permisos.TipoFamilia', 'Familia'),
                    ('Ingles', 'Permisos.TipoFamilia', 'Family'),
                    ('Espanol', 'Permisos.Padre', 'Padre'),
                    ('Ingles', 'Permisos.Padre', 'Parent'),
                    ('Espanol', 'Permisos.Raiz', 'Raiz'),
                    ('Ingles', 'Permisos.Raiz', 'Root'),
                    ('Espanol', 'Permisos.Crear', 'Crear'),
                    ('Ingles', 'Permisos.Crear', 'Create'),
                    ('Espanol', 'Permisos.Editar', 'Editar'),
                    ('Ingles', 'Permisos.Editar', 'Edit'),
                    ('Espanol', 'Permisos.Eliminar', 'Eliminar'),
                    ('Ingles', 'Permisos.Eliminar', 'Delete'),
                    ('Espanol', 'Permisos.Mover', 'Mover'),
                    ('Ingles', 'Permisos.Mover', 'Move'),
                    ('Espanol', 'Permisos.Limpiar', 'Limpiar'),
                    ('Ingles', 'Permisos.Limpiar', 'Clear'),
                    ('Espanol', 'Mensaje.PermisoCreado', 'Permiso creado exitosamente.'),
                    ('Ingles', 'Mensaje.PermisoCreado', 'Permission created successfully.'),
                    ('Espanol', 'Mensaje.PermisoEditado', 'Permiso editado exitosamente.'),
                    ('Ingles', 'Mensaje.PermisoEditado', 'Permission edited successfully.'),
                    ('Espanol', 'Mensaje.PermisoEliminado', 'Permiso eliminado exitosamente.'),
                    ('Ingles', 'Mensaje.PermisoEliminado', 'Permission deleted successfully.'),
                    ('Espanol', 'Mensaje.PermisoMovido', 'Permiso movido exitosamente.'),
                    ('Ingles', 'Mensaje.PermisoMovido', 'Permission moved successfully.'),
                    ('Espanol', 'Mensaje.SeleccionePermiso', 'Seleccione un permiso.'),
                    ('Ingles', 'Mensaje.SeleccionePermiso', 'Select a permission.'),
                    ('Espanol', 'Mensaje.ConfirmarEliminarPermiso', 'Estas seguro de eliminar el permiso ''{0}''?'),
                    ('Ingles', 'Mensaje.ConfirmarEliminarPermiso', 'Are you sure you want to delete permission ''{0}''?'),
                    ('Espanol', 'Mensaje.ErrorPermiso', 'Error al gestionar permisos: {0}'),
                    ('Ingles', 'Mensaje.ErrorPermiso', 'Permission management error: {0}'),
                    ('Espanol', 'Mensaje.DropInvalido', 'No se puede mover el permiso al destino seleccionado.'),
                    ('Ingles', 'Mensaje.DropInvalido', 'The permission cannot be moved to the selected target.');

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
                END
            ";

            _db.ExecuteTransaction(query);
        }

        private void AgregarPermisosPrueba()
        {
            string query = @"
                DECLARE @IdAdministracion int;
                DECLARE @IdUsuarios int;
                DECLARE @IdIdiomas int;
                DECLARE @IdPermisos int;

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'FAM_ADMINISTRACION')
                BEGIN
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Administracion', 'FAM_ADMINISTRACION', 'Permisos administrativos del sistema.', 1, NULL);
                END

                SELECT @IdAdministracion = id_permiso
                FROM Permisos
                WHERE codigo = 'FAM_ADMINISTRACION';

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'FAM_USUARIOS')
                BEGIN
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Usuarios', 'FAM_USUARIOS', 'Permisos para gestionar usuarios.', 1, @IdAdministracion);
                END

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'FAM_IDIOMAS')
                BEGIN
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Idiomas', 'FAM_IDIOMAS', 'Permisos para gestionar idiomas y traducciones.', 1, @IdAdministracion);
                END

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'FAM_PERMISOS')
                BEGIN
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Permisos', 'FAM_PERMISOS', 'Permisos para gestionar permisos y familias.', 1, @IdAdministracion);
                END

                SELECT @IdUsuarios = id_permiso FROM Permisos WHERE codigo = 'FAM_USUARIOS';
                SELECT @IdIdiomas = id_permiso FROM Permisos WHERE codigo = 'FAM_IDIOMAS';
                SELECT @IdPermisos = id_permiso FROM Permisos WHERE codigo = 'FAM_PERMISOS';

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'USUARIOS_ADMINISTRAR')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Administrar usuarios', 'USUARIOS_ADMINISTRAR', 'Acceso al formulario de administracion de usuarios.', 0, @IdUsuarios);

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'USUARIOS_CREAR')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Crear usuarios', 'USUARIOS_CREAR', 'Permite crear nuevos usuarios.', 0, @IdUsuarios);

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'USUARIOS_EDITAR')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Editar usuarios', 'USUARIOS_EDITAR', 'Permite modificar usuarios existentes.', 0, @IdUsuarios);

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'USUARIOS_ELIMINAR')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Eliminar usuarios', 'USUARIOS_ELIMINAR', 'Permite eliminar usuarios existentes.', 0, @IdUsuarios);

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'TRADUCCIONES_ADMINISTRAR')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Administrar traducciones', 'TRADUCCIONES_ADMINISTRAR', 'Acceso al formulario de traducciones.', 0, @IdIdiomas);

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'IDIOMAS_CAMBIAR')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Cambiar idioma', 'IDIOMAS_CAMBIAR', 'Permite cambiar el idioma actual del sistema.', 0, @IdIdiomas);

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'PERMISOS_ADMINISTRAR')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Administrar permisos', 'PERMISOS_ADMINISTRAR', 'Acceso al formulario de administracion de permisos.', 0, @IdPermisos);

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'PERMISOS_CREAR')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Crear permisos', 'PERMISOS_CREAR', 'Permite crear permisos simples y familias.', 0, @IdPermisos);

                IF NOT EXISTS (SELECT 1 FROM Permisos WHERE codigo = 'PERMISOS_MOVER')
                    INSERT INTO Permisos (nombre, codigo, descripcion, es_familia, id_permiso_padre)
                    VALUES ('Mover permisos', 'PERMISOS_MOVER', 'Permite mover permisos entre familias.', 0, @IdPermisos);
            ";

            _db.ExecuteTransaction(query);
        }

        private bool TieneHijos(int id)
        {
            string query = @"
                SELECT COUNT(1)
                FROM Permisos
                WHERE id_permiso_padre=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            return _db.ExecuteTransaction(query, sqlParameters) > 0;
        }

        private PermisoComponent CrearComponente(DataRow fila)
        {
            int id = Convert.ToInt32(fila["id_permiso"]);
            string nombre = fila["nombre"].ToString();
            string codigo = fila["codigo"].ToString();
            string descripcion = fila["descripcion"] == DBNull.Value ? string.Empty : fila["descripcion"].ToString();
            bool esFamilia = Convert.ToBoolean(fila["es_familia"]);

            if (esFamilia)
                return FamiliaPermiso.CargarDesdeDB(id, nombre, codigo, descripcion);

            return PermisoSimple.CargarDesdeDB(id, nombre, codigo, descripcion);
        }
    }
}
