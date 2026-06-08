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
                        password nvarchar(max) NOT NULL
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

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Usuario Agregar(Usuario usuario)
        {
            string query = @"
                INSERT INTO Usuarios (username, password) VALUES (@Username, @Password);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", usuario.Username),
                new SqlParameter("@Password", usuario.Password)
            };

            try
            {
                int id = _db.ExecuteTransaction(query, sqlParameters);

                return Usuario.CargarDesdeDB(
                    id,
                    usuario.Username,
                    usuario.Password
                );
            }
            catch (SqlException ex) when (ex.Number == 2601 || ex.Number == 2627)
            {
                throw new UsuarioYaExisteException();
            }
        }

        public void Eliminar(int id)
        {
            string query = @"
                DELETE FROM Usuarios WHERE id_usuario=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
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
                fila["password"].ToString()
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
                fila["password"].ToString()
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
                    fila["password"].ToString()
                );

                usuarios.Add(usuario);
            }

            return usuarios;
        }

        public void Modificar(Usuario usuario)
        {
            string query = @"
                UPDATE Usuarios SET username=@Username, password=@Password WHERE id_usuario=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", usuario.Id),
                new SqlParameter("@Username", usuario.Username),
                new SqlParameter("@Password", usuario.Password)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }
    }
}
