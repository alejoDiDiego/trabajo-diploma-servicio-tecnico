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

            _db.ExecuteScalar(query, sqlParameters);
        }
    }
}
