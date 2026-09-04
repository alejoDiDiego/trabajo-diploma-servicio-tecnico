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

        public bool TienePermisoAsignado(int idUsuario, int idPermiso)
        {
            // Consulta la relacion directa; no expande familias ni permisos efectivos.
            string query = @"
                SELECT 1
                FROM UsuarioPermisos
                WHERE id_usuario=@IdUsuario
                  AND id_permiso=@IdPermiso;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@IdPermiso", idPermiso)
            };

            return _db.ExecuteQuery(query, sqlParameters).Rows.Count > 0;
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
                        id_usuario int NOT NULL,
                        id_permiso int NOT NULL,
                        CONSTRAINT PK_UsuarioPermisos PRIMARY KEY (id_usuario, id_permiso),
                        CONSTRAINT FK_UsuarioPermisos_Usuarios FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario) ON DELETE CASCADE,
                        CONSTRAINT FK_UsuarioPermisos_Permisos FOREIGN KEY (id_permiso)
                            REFERENCES Permisos(id_permiso) ON DELETE CASCADE
                    );
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
                ('lector', 'Lectura general'),
                ('encargado', 'Gestion clientes'),
                ('encargado', 'Gestion equipos'),
                ('encargado', 'Gestion catalogos'),
                ('encargado', 'Gestion ordenes');

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

                -- Simples directos: recepcionista/tecnico no usan familias para no heredar de mas;
                -- encargado solo suma BITACORA_VER simple (sus 4 familias ya calzan exacto arriba).
                CREATE TABLE #AsignacionesSimples (
                    username nvarchar(100),
                    codigo nvarchar(100)
                );

                INSERT INTO #AsignacionesSimples (username, codigo) VALUES
                ('recepcionista', 'CLIENTES_VER'),
                ('recepcionista', 'CLIENTES_CREAR'),
                ('recepcionista', 'CLIENTES_EDITAR'),
                ('recepcionista', 'EQUIPOS_VER'),
                ('recepcionista', 'EQUIPOS_CREAR'),
                ('recepcionista', 'EQUIPOS_EDITAR'),
                ('recepcionista', 'TIPOS_EQUIPO_VER'),
                ('recepcionista', 'MARCAS_VER'),
                ('recepcionista', 'ORDENES_VER'),
                ('recepcionista', 'ORDENES_CREAR'),
                ('recepcionista', 'ORDENES_EDITAR'),
                ('recepcionista', 'ORDENES_ENTREGAR'),
                ('tecnico', 'CLIENTES_VER'),
                ('tecnico', 'EQUIPOS_VER'),
                ('tecnico', 'TIPOS_EQUIPO_VER'),
                ('tecnico', 'MARCAS_VER'),
                ('tecnico', 'ORDENES_VER'),
                ('tecnico', 'ORDENES_EDITAR'),
                ('encargado', 'BITACORA_VER');

                INSERT INTO UsuarioPermisos (id_usuario, id_permiso)
                SELECT u.id_usuario, p.id_permiso
                FROM #AsignacionesSimples a
                INNER JOIN Usuarios u ON UPPER(u.username) = UPPER(a.username)
                INNER JOIN Permisos p ON p.codigo = a.codigo AND p.es_familia=0
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM UsuarioPermisos up
                    WHERE up.id_usuario = u.id_usuario
                      AND up.id_permiso = p.id_permiso
                );

                -- Familias de rol base: los 3 usuarios pasan de asignacion directa a su rol.
                CREATE TABLE #AsignacionesRol (
                    username nvarchar(100),
                    familia nvarchar(100)
                );

                INSERT INTO #AsignacionesRol (username, familia) VALUES
                ('recepcionista', 'Rol recepcionista'),
                ('tecnico', 'Rol tecnico'),
                ('encargado', 'Rol encargado');

                INSERT INTO UsuarioPermisos (id_usuario, id_permiso)
                SELECT u.id_usuario, p.id_permiso
                FROM #AsignacionesRol a
                INNER JOIN Usuarios u ON UPPER(u.username) = UPPER(a.username)
                INNER JOIN Permisos p ON UPPER(p.nombre) = UPPER(a.familia) AND p.es_familia=1
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM UsuarioPermisos up
                    WHERE up.id_usuario = u.id_usuario
                      AND up.id_permiso = p.id_permiso
                );

                -- Limpieza de asignaciones redundantes ahora cubiertas por el rol (solo estos 3 usuarios).
                DELETE up
                FROM UsuarioPermisos up
                INNER JOIN Usuarios u ON u.id_usuario = up.id_usuario
                INNER JOIN Permisos p ON p.id_permiso = up.id_permiso
                WHERE UPPER(u.username) = UPPER('recepcionista')
                  AND p.es_familia = 0
                  AND p.codigo IN ('CLIENTES_VER', 'CLIENTES_CREAR', 'CLIENTES_EDITAR', 'EQUIPOS_VER', 'EQUIPOS_CREAR', 'EQUIPOS_EDITAR', 'TIPOS_EQUIPO_VER', 'MARCAS_VER', 'ORDENES_VER', 'ORDENES_CREAR', 'ORDENES_EDITAR', 'ORDENES_ENTREGAR');

                DELETE up
                FROM UsuarioPermisos up
                INNER JOIN Usuarios u ON u.id_usuario = up.id_usuario
                INNER JOIN Permisos p ON p.id_permiso = up.id_permiso
                WHERE UPPER(u.username) = UPPER('tecnico')
                  AND p.es_familia = 0
                  AND p.codigo IN ('CLIENTES_VER', 'EQUIPOS_VER', 'TIPOS_EQUIPO_VER', 'MARCAS_VER', 'ORDENES_VER', 'ORDENES_EDITAR');

                DELETE up
                FROM UsuarioPermisos up
                INNER JOIN Usuarios u ON u.id_usuario = up.id_usuario
                INNER JOIN Permisos p ON p.id_permiso = up.id_permiso
                WHERE UPPER(u.username) = UPPER('encargado')
                  AND p.es_familia = 0
                  AND p.codigo IN ('BITACORA_VER');

                DELETE up
                FROM UsuarioPermisos up
                INNER JOIN Usuarios u ON u.id_usuario = up.id_usuario
                INNER JOIN Permisos p ON p.id_permiso = up.id_permiso
                WHERE UPPER(u.username) = UPPER('encargado')
                  AND p.es_familia = 1
                  AND UPPER(p.nombre) IN (UPPER('Gestion clientes'), UPPER('Gestion equipos'), UPPER('Gestion catalogos'), UPPER('Gestion ordenes'));

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
