using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Usuarios;
using DOMAIN.Features.Usuarios.Exceptions;

namespace REPOSITORY.Features.Usuarios
{
    public class UsuarioRepository
    {
        private readonly SqlHelper _db;

        public UsuarioRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public UsuarioRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla Usuarios para que el login tenga datos base.
            string query = @"
                IF OBJECT_ID('Usuarios', 'U') IS NULL
                BEGIN
                    CREATE TABLE Usuarios (
                        id_usuario int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        username nvarchar(100) NOT NULL,
                        password nvarchar(max) NOT NULL,
                        activo BIT NOT NULL DEFAULT 1
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Usuarios', 'username') IS NULL
                        ALTER TABLE Usuarios ADD username nvarchar(100) NULL;

                    IF COL_LENGTH('Usuarios', 'password') IS NULL
                        ALTER TABLE Usuarios ADD password nvarchar(max) NULL;
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Usuarios_Username'
                      AND object_id = OBJECT_ID('Usuarios')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Usuarios_Username
                    ON Usuarios(username);
                END

                IF COL_LENGTH('Usuarios', 'dvh') IS NULL
                    ALTER TABLE Usuarios ADD dvh nvarchar(100) NOT NULL DEFAULT '';

                IF COL_LENGTH('Usuarios', 'activo') IS NULL
                    ALTER TABLE Usuarios ADD activo BIT NOT NULL DEFAULT 1;

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Usuario Agregar(Usuario usuario)
        {
            string query = @"
                INSERT INTO Usuarios (username, password, activo) VALUES (@Username, @Password, @Activo);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", usuario.Username),
                new SqlParameter("@Password", usuario.Password),
                new SqlParameter("@Activo", usuario.Activo)
            };

            try
            {
                int id = _db.ExecuteTransaction(query, sqlParameters);

                return Usuario.CargarDesdeDB(
                    id,
                    usuario.Username,
                    usuario.Password,
                    "",
                    usuario.Activo
                );
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new UsuarioYaExisteException();
            }
        }

        public void Eliminar(int id)
        {
            // Baja logica: nunca borra fisicamente, solo desactiva. El service recalcula DVH/DVV.
            CambiarEstado(id, false);
        }

        public void CambiarEstado(int id, bool activo)
        {
            string query = @"
                UPDATE Usuarios SET activo=@Activo WHERE id_usuario=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Activo", activo)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Usuario ObtenerPorUsername(string userName)
        {
            string query = @"
                SELECT * FROM Usuarios WHERE username=@Username
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", userName)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            DataRow fila = dt.Rows[0];

            return Usuario.CargarDesdeDB(
                Convert.ToInt32(fila["id_usuario"]),
                fila["username"].ToString(),
                fila["password"].ToString(),
                fila["dvh"].ToString(),
                LeerActivo(fila)
            );
        }

        public Usuario ObtenerPorId(int id)
        {
            string query = @"
                SELECT * FROM Usuarios WHERE id_usuario=@Id
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            DataRow fila = dt.Rows[0];

            return Usuario.CargarDesdeDB(
                Convert.ToInt32(fila["id_usuario"]),
                fila["username"].ToString(),
                fila["password"].ToString(),
                fila["dvh"].ToString(),
                LeerActivo(fila)
            );
        }

        public List<Usuario> Listar()
        {
            string query = @"
                SELECT * FROM Usuarios
            ";

            DataTable dt = _db.ExecuteQuery(query);

            List<Usuario> usuarios = new List<Usuario>();

            foreach (DataRow fila in dt.Rows)
            {
                Usuario usuario = Usuario.CargarDesdeDB(
                    Convert.ToInt32(fila["id_usuario"]),
                    fila["username"].ToString(),
                    fila["password"].ToString(),
                    fila["dvh"].ToString(),
                    LeerActivo(fila)
                );

                usuarios.Add(usuario);
            }

            return usuarios;
        }

        public void Modificar(Usuario usuario)
        {
            string query = @"
                UPDATE Usuarios SET username=@Username, password=@Password, dvh=@DVH, activo=@Activo WHERE id_usuario=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", usuario.Id),
                new SqlParameter("@Username", usuario.Username),
                new SqlParameter("@Password", usuario.Password),
                new SqlParameter("@DVH", usuario.DVH ?? ""),
                new SqlParameter("@Activo", usuario.Activo)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public List<UserDVH> ObtenerTodosDVH()
        {
            string query = "SELECT id_usuario, dvh FROM Usuarios ORDER BY id_usuario";

            DataTable dt = _db.ExecuteQuery(query);

            List<UserDVH> resultado = new List<UserDVH>();
            foreach (DataRow fila in dt.Rows)
            {
                resultado.Add(new UserDVH
                {
                    Id = Convert.ToInt32(fila["id_usuario"]),
                    DVH = fila["dvh"].ToString()
                });
            }

            return resultado;
        }

        public void ActualizarDVH(int id, string dvh)
        {
            string query = "UPDATE Usuarios SET dvh = @DVH WHERE id_usuario = @Id";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@DVH", dvh)
            };

            _db.ExecuteTransaction(query, parametros);
        }

        private static bool LeerActivo(DataRow fila)
        {
            // T1: tras el ALTER defensivo la columna siempre existe; si falta en algun SELECT legacy, asumir activo.
            if (!fila.Table.Columns.Contains("activo") || fila["activo"] == DBNull.Value)
                return true;

            object valor = fila["activo"];

            if (valor is bool)
                return (bool)valor;

            bool parsed;
            if (bool.TryParse(valor.ToString(), out parsed))
                return parsed;

            return valor.ToString() == "1";
        }
    }
}
