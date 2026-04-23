using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

        public int ExecuteTransaction(string query, SqlParameter[] parametros = null)
        {
            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                // Usamos using para la transacción también
                using (SqlTransaction trans = conexion.BeginTransaction())
                using (SqlCommand cmd = new SqlCommand(query, conexion, trans))
                {
                    try
                    {
                        if (parametros != null)
                        {
                            // Limpiamos por seguridad si el comando se reutilizara
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(parametros);
                        }

                        var result = cmd.ExecuteScalar();
                        trans.Commit(); // Si llega aquí, todo bien

                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                    catch (Exception)
                    {
                        // Si algo falla, el Rollback deshace los cambios
                        trans.Rollback();
                        throw; 
                    }
                }
            }
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
