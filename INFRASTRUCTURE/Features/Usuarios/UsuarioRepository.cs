using APPLICATION.Features.Usuarios.DTOs;
using APPLICATION.Features.Usuarios.Interfaces;
using DOMAIN.Features.Usuarios;
using DOMAIN.Features.Usuarios.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Features.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SqlHelper _db;

        public UsuarioRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public Usuario Agregar(Usuario u)
        {
            string query = @"
                INSERT INTO Usuarios (username, password) VALUES (@Username, @Password);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Username", u.Username),
                new SqlParameter("@Password", u.Password)
            };

            try
            {
                int id = _db.ExecuteScalar(query, sqlParameters);

                return Usuario.CargarDesdeDB(
                    id,
                    u.Username,
                    u.Password
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

            _db.ExecuteScalar(query, sqlParameters);
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
                throw new Exception("No hay usuarios");

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

            if (dt.Rows.Count <= 0)
                throw new Exception("No hay usuarios");

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


    }
}
