using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace REPOSITORY.Features.Integridad
{
    public class IntegridadRepository
    {
        private readonly SqlHelper _db;

        public IntegridadRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public IntegridadRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            string query = @"
                IF OBJECT_ID('DigitosVerticales', 'U') IS NULL
                BEGIN
                    CREATE TABLE DigitosVerticales (
                        id_dvv int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        nombre_tabla nvarchar(100) NOT NULL UNIQUE,
                        dvv nvarchar(100) NOT NULL,
                        fecha_calculo datetime NOT NULL
                    );
                END

                IF OBJECT_ID('Usuarios', 'U') IS NOT NULL
                BEGIN
                    IF COL_LENGTH('Usuarios', 'dvh') IS NULL
                        ALTER TABLE Usuarios ADD dvh nvarchar(100) NOT NULL DEFAULT '';
                END
            ";

            _db.ExecuteTransaction(query);
        }

        public void GuardarDVV(string nombreTabla, string dvv)
        {
            string query = @"
                IF EXISTS (SELECT 1 FROM DigitosVerticales WHERE nombre_tabla = @Tabla)
                    UPDATE DigitosVerticales
                    SET dvv = @DVV, fecha_calculo = GETDATE()
                    WHERE nombre_tabla = @Tabla
                ELSE
                    INSERT INTO DigitosVerticales (nombre_tabla, dvv, fecha_calculo)
                    VALUES (@Tabla, @DVV, GETDATE());
            ";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Tabla", nombreTabla),
                new SqlParameter("@DVV", dvv)
            };

            _db.ExecuteTransaction(query, parametros);
        }

        public string ObtenerDVV(string nombreTabla)
        {
            string query = "SELECT dvv FROM DigitosVerticales WHERE nombre_tabla = @Tabla";

            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@Tabla", nombreTabla)
            };

            DataTable dt = _db.ExecuteQuery(query, parametros);
            return dt.Rows.Count > 0 ? dt.Rows[0]["dvv"].ToString() : null;
        }
    }
}
