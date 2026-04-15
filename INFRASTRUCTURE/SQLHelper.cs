using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE
{
    public class SqlHelper
    {
        private readonly string _cadenaConexion;

        public SqlHelper(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public int ExecuteScalar(string query, SqlParameter[] parametros = null)
        {
            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            using (SqlCommand cmd = new SqlCommand(query, conexion))
            {
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros);
                }

                conexion.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parametros = null)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            using (SqlCommand cmd = new SqlCommand(query, conexion))
            {
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros);
                }

                conexion.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    tabla.Load(reader);
                }
            }

            return tabla;
        }
    }
}
