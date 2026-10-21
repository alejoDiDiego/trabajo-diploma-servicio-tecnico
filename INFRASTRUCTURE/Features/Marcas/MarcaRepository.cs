using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Marcas;

namespace REPOSITORY.Features.Marcas
{
    public class MarcaRepository
    {
        private readonly SqlHelper _db;

        public MarcaRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public MarcaRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla Marcas de forma idempotente (IF OBJECT_ID + ALTER defensivos).
            string query = @"
                IF OBJECT_ID('Marcas', 'U') IS NULL
                BEGIN
                    CREATE TABLE Marcas (
                        id_marca int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        nombre nvarchar(100) NOT NULL,
                        activo bit NOT NULL CONSTRAINT DF_Marcas_Activo DEFAULT 1
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Marcas', 'nombre') IS NULL
                        ALTER TABLE Marcas ADD nombre nvarchar(100) NOT NULL CONSTRAINT DF_Marcas_Nombre DEFAULT '';

                    IF COL_LENGTH('Marcas', 'activo') IS NULL
                        ALTER TABLE Marcas ADD activo bit NOT NULL CONSTRAINT DF_Marcas_Activo DEFAULT 1;
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Marcas_Nombre'
                      AND object_id = OBJECT_ID('Marcas')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Marcas_Nombre
                    ON Marcas(nombre);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Marca Agregar(Marca marca)
        {
            string query = @"
                INSERT INTO Marcas (nombre, activo) VALUES (@Nombre, 1);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", marca.Nombre)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void Modificar(Marca marca)
        {
            string query = @"
                UPDATE Marcas SET nombre=@Nombre WHERE id_marca=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", marca.Id),
                new SqlParameter("@Nombre", marca.Nombre)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Desactivar(int id)
        {
            string query = @"
                UPDATE Marcas SET activo=0 WHERE id_marca=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Reactivar(int id)
        {
            string query = @"
                UPDATE Marcas SET activo=1 WHERE id_marca=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Marca ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_marca, nombre, activo
                FROM Marcas WHERE id_marca=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return Mapear(dt.Rows[0]);
        }

        public List<Marca> Listar(bool incluirInactivos = false)
        {
            string query = @"
                SELECT id_marca, nombre, activo
                FROM Marcas
                WHERE (@IncluirInactivos = 1 OR activo = 1)
                ORDER BY nombre;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IncluirInactivos", incluirInactivos ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Marca> marcas = new List<Marca>();

            foreach (DataRow fila in dt.Rows)
                marcas.Add(Mapear(fila));

            return marcas;
        }

        private Marca Mapear(DataRow fila)
        {
            return Marca.CargarDesdeDB(
                Convert.ToInt32(fila["id_marca"]),
                fila["nombre"] == DBNull.Value ? "" : fila["nombre"].ToString(),
                Convert.ToBoolean(fila["activo"])
            );
        }
    }
}
