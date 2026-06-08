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

        public List<FamiliaPermiso> ListarFamiliasAsignadas(int idUsuario)
        {
            // Familias que ya estan vinculadas al usuario.
            string query = @"
                SELECT p.id_permiso, p.nombre, p.codigo, p.es_familia
                FROM UsuarioPermisos up
                INNER JOIN Permisos p ON p.id_permiso = up.id_permiso_familia
                WHERE up.id_usuario=@IdUsuario
                  AND p.es_familia=1
                ORDER BY p.nombre;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario)
            };

            return CrearFamilias(_db.ExecuteQuery(query, sqlParameters));
        }

        public List<FamiliaPermiso> ListarFamiliasDisponibles(int idUsuario)
        {
            // Familias del catalogo que aun no tiene asignadas el usuario.
            string query = @"
                SELECT p.id_permiso, p.nombre, p.codigo, p.es_familia
                FROM Permisos p
                WHERE p.es_familia=1
                  AND NOT EXISTS (
                      SELECT 1
                      FROM UsuarioPermisos up
                      WHERE up.id_usuario=@IdUsuario
                        AND up.id_permiso_familia=p.id_permiso
                  )
                ORDER BY p.nombre;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario)
            };

            return CrearFamilias(_db.ExecuteQuery(query, sqlParameters));
        }

        public void AsignarFamilia(int idUsuario, int idFamilia)
        {
            // Inserta la relacion usuario-familia si ambos existen y no esta repetida.
            string query = @"
                DECLARE @Filas int = 0;

                IF EXISTS (SELECT 1 FROM Usuarios WHERE id_usuario=@IdUsuario)
                   AND EXISTS (SELECT 1 FROM Permisos WHERE id_permiso=@IdFamilia AND es_familia=1)
                   AND NOT EXISTS (
                       SELECT 1
                       FROM UsuarioPermisos
                       WHERE id_usuario=@IdUsuario
                         AND id_permiso_familia=@IdFamilia
                   )
                BEGIN
                    INSERT INTO UsuarioPermisos (id_usuario, id_permiso_familia)
                    VALUES (@IdUsuario, @IdFamilia);

                    SET @Filas = 1;
                END

                SELECT @Filas;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@IdFamilia", idFamilia)
            };

            int filas = _db.ExecuteTransaction(query, sqlParameters);

            if (filas <= 0)
                throw new ReglaNegocioException("No se pudo asignar la familia al usuario.");
        }

        public void QuitarFamilia(int idUsuario, int idFamilia)
        {
            // Elimina solo la asignacion; no borra usuarios ni familias.
            string query = @"
                DELETE FROM UsuarioPermisos
                WHERE id_usuario=@IdUsuario
                  AND id_permiso_familia=@IdFamilia;

                SELECT @@ROWCOUNT;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@IdFamilia", idFamilia)
            };

            int filas = _db.ExecuteTransaction(query, sqlParameters);

            if (filas <= 0)
                throw new ReglaNegocioException("La familia no esta asignada al usuario.");
        }

        private void CrearTablaUsuarioPermisos()
        {
            // Tabla puente entre Usuarios y familias de Permisos.
            string query = @"
                IF OBJECT_ID('UsuarioPermisos', 'U') IS NULL
                BEGIN
                    CREATE TABLE UsuarioPermisos (
                        id_usuario_permiso int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_usuario int NOT NULL,
                        id_permiso_familia int NOT NULL,
                        CONSTRAINT FK_UsuarioPermisos_Usuarios FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario) ON DELETE CASCADE,
                        CONSTRAINT FK_UsuarioPermisos_Permisos FOREIGN KEY (id_permiso_familia)
                            REFERENCES Permisos(id_permiso) ON DELETE CASCADE
                    );
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_UsuarioPermisos_Usuario_Familia'
                      AND object_id = OBJECT_ID('UsuarioPermisos')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_UsuarioPermisos_Usuario_Familia
                    ON UsuarioPermisos(id_usuario, id_permiso_familia);
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

                INSERT INTO UsuarioPermisos (id_usuario, id_permiso_familia)
                SELECT u.id_usuario, p.id_permiso
                FROM #Asignaciones a
                INNER JOIN Usuarios u ON UPPER(u.username) = UPPER(a.username)
                INNER JOIN Permisos p ON UPPER(p.nombre) = UPPER(a.familia) AND p.es_familia=1
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM UsuarioPermisos up
                    WHERE up.id_usuario = u.id_usuario
                      AND up.id_permiso_familia = p.id_permiso
                );

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        private List<FamiliaPermiso> CrearFamilias(DataTable dt)
        {
            // Convierte filas de SQL en entidades de dominio.
            List<FamiliaPermiso> familias = new List<FamiliaPermiso>();

            foreach (DataRow fila in dt.Rows)
            {
                familias.Add(FamiliaPermiso.CargarDesdeDB(
                    Convert.ToInt32(fila["id_permiso"]),
                    fila["nombre"].ToString()
                ));
            }

            return familias;
        }
    }
}
