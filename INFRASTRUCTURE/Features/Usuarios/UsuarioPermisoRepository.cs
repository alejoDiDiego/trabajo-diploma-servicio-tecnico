using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Exceptions;
using DOMAIN.Features.Permisos;

namespace REPOSITORY.Features.Usuarios
{
    public class UsuarioPermisoRepository
    {
        private readonly SqlHelper _db;

        public UsuarioPermisoRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public UsuarioPermisoRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla puente y carga relaciones iniciales de prueba.
            CrearTablaUsuarioPermisos();
            AgregarAsignacionesBase();
        }

        public List<PermisoComponent> ListarPermisosAsignados(int idUsuario)
        {
            // Componentes que ya estan vinculados directamente al usuario.
            string query = @"
                SELECT p.id_permiso, p.nombre, p.codigo, p.es_familia
                FROM UsuarioPermisos up
                INNER JOIN Permisos p ON p.id_permiso = up.id_permiso
                WHERE up.id_usuario=@IdUsuario
                  AND UPPER(p.nombre) <> UPPER(@NombreRaiz)
                ORDER BY p.es_familia DESC, p.nombre;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@NombreRaiz", REPOSITORY.Features.Permisos.PermisoRepository.NombreRaizSistema)
            };

            return CrearPermisos(_db.ExecuteQuery(query, sqlParameters));
        }

        public List<PermisoComponent> ListarPermisosDisponibles(int idUsuario)
        {
            // Componentes del catalogo que aun no tiene asignados directamente el usuario.
            string query = @"
                SELECT p.id_permiso, p.nombre, p.codigo, p.es_familia
                FROM Permisos p
                WHERE UPPER(p.nombre) <> UPPER(@NombreRaiz)
                  AND NOT EXISTS (
                      SELECT 1
                      FROM UsuarioPermisos up
                      WHERE up.id_usuario=@IdUsuario
                        AND up.id_permiso=p.id_permiso
                  )
                ORDER BY p.es_familia DESC, p.nombre;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@NombreRaiz", REPOSITORY.Features.Permisos.PermisoRepository.NombreRaizSistema)
            };

            return CrearPermisos(_db.ExecuteQuery(query, sqlParameters));
        }

        public void AsignarPermiso(int idUsuario, int idPermiso)
        {
            // Inserta la relacion usuario-permiso si ambos existen y no esta repetida.
            string query = @"
                DECLARE @Filas int = 0;

                IF EXISTS (SELECT 1 FROM Usuarios WHERE id_usuario=@IdUsuario)
                   AND EXISTS (SELECT 1 FROM Permisos WHERE id_permiso=@IdPermiso AND UPPER(nombre) <> UPPER(@NombreRaiz))
                   AND NOT EXISTS (
                       SELECT 1
                       FROM UsuarioPermisos
                       WHERE id_usuario=@IdUsuario
                         AND id_permiso=@IdPermiso
                   )
                BEGIN
                    INSERT INTO UsuarioPermisos (id_usuario, id_permiso)
                    VALUES (@IdUsuario, @IdPermiso);

                    SET @Filas = 1;
                END

                SELECT @Filas;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@IdPermiso", idPermiso),
                new SqlParameter("@NombreRaiz", REPOSITORY.Features.Permisos.PermisoRepository.NombreRaizSistema)
            };

            int filas = _db.ExecuteTransaction(query, sqlParameters);

            if (filas <= 0)
                throw new ReglaNegocioException("No se pudo asignar el permiso al usuario.");
        }

        public void QuitarPermiso(int idUsuario, int idPermiso)
        {
            // Elimina solo la asignacion; no borra usuarios ni permisos.
            string query = @"
                DELETE FROM UsuarioPermisos
                WHERE id_usuario=@IdUsuario
                  AND id_permiso=@IdPermiso;

                SELECT @@ROWCOUNT;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@IdPermiso", idPermiso)
            };

            int filas = _db.ExecuteTransaction(query, sqlParameters);

            if (filas <= 0)
                throw new ReglaNegocioException("El permiso no esta asignado al usuario.");
        }

        private void CrearTablaUsuarioPermisos()
        {
            // Tabla puente entre Usuarios y cualquier componente de Permisos.
            string query = @"
                IF OBJECT_ID('UsuarioPermisos', 'U') IS NULL
                BEGIN
                    CREATE TABLE UsuarioPermisos (
                        id_usuario_permiso int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_usuario int NOT NULL,
                        id_permiso int NOT NULL,
                        CONSTRAINT FK_UsuarioPermisos_Usuarios FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario) ON DELETE CASCADE,
                        CONSTRAINT FK_UsuarioPermisos_Permisos FOREIGN KEY (id_permiso)
                            REFERENCES Permisos(id_permiso) ON DELETE CASCADE
                    );
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_UsuarioPermisos_Usuario_Permiso'
                      AND object_id = OBJECT_ID('UsuarioPermisos')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_UsuarioPermisos_Usuario_Permiso
                    ON UsuarioPermisos(id_usuario, id_permiso);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        private void AgregarAsignacionesBase()
        {
            // Asignaciones iniciales para probar permisos desde el primer inicio.
            string query = @"
                CREATE TABLE #Asignaciones (
                    username nvarchar(100),
                    familia nvarchar(100)
                );

                INSERT INTO #Asignaciones (username, familia) VALUES
                ('admin', 'Administrador'),
                ('usuarios', 'Gestion usuarios'),
                ('permisos', 'Gestion permisos'),
                ('idiomas', 'Gestion idiomas'),
                ('idiomas', 'Gestion traducciones'),
                ('lector', 'Lectura general');

                INSERT INTO UsuarioPermisos (id_usuario, id_permiso)
                SELECT u.id_usuario, p.id_permiso
                FROM #Asignaciones a
                INNER JOIN Usuarios u ON UPPER(u.username) = UPPER(a.username)
                INNER JOIN Permisos p ON UPPER(p.nombre) = UPPER(a.familia) AND p.es_familia=1
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM UsuarioPermisos up
                    WHERE up.id_usuario = u.id_usuario
                      AND up.id_permiso = p.id_permiso
                );

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        private List<PermisoComponent> CrearPermisos(DataTable dt)
        {
            // Convierte filas de SQL en entidades de dominio.
            List<PermisoComponent> permisos = new List<PermisoComponent>();

            foreach (DataRow fila in dt.Rows)
            {
                int id = Convert.ToInt32(fila["id_permiso"]);
                string nombre = fila["nombre"].ToString();
                bool esFamilia = Convert.ToBoolean(fila["es_familia"]);

                if (esFamilia)
                {
                    permisos.Add(FamiliaPermiso.CargarDesdeDB(id, nombre));
                    continue;
                }

                string codigo = fila["codigo"] == DBNull.Value ? null : fila["codigo"].ToString();
                permisos.Add(PermisoSimple.CargarDesdeDB(id, nombre, codigo));
            }

            return permisos;
        }
    }
}
