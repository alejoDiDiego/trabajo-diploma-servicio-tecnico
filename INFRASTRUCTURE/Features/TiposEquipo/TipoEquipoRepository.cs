using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.TiposEquipo;

namespace REPOSITORY.Features.TiposEquipo
{
    public class TipoEquipoRepository
    {
        private readonly SqlHelper _db;

        public TipoEquipoRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public TipoEquipoRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla TiposEquipo de forma idempotente (IF OBJECT_ID + ALTER defensivos).
            string query = @"
                IF OBJECT_ID('TiposEquipo', 'U') IS NULL
                BEGIN
                    CREATE TABLE TiposEquipo (
                        id_tipo_equipo int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        nombre nvarchar(100) NOT NULL,
                        activo bit NOT NULL CONSTRAINT DF_TiposEquipo_Activo DEFAULT 1
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('TiposEquipo', 'nombre') IS NULL
                        ALTER TABLE TiposEquipo ADD nombre nvarchar(100) NOT NULL CONSTRAINT DF_TiposEquipo_Nombre DEFAULT '';

                    IF COL_LENGTH('TiposEquipo', 'activo') IS NULL
                        ALTER TABLE TiposEquipo ADD activo bit NOT NULL CONSTRAINT DF_TiposEquipo_Activo DEFAULT 1;
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_TiposEquipo_Nombre'
                      AND object_id = OBJECT_ID('TiposEquipo')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_TiposEquipo_Nombre
                    ON TiposEquipo(nombre);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public TipoEquipo Agregar(TipoEquipo tipo)
        {
            string query = @"
                INSERT INTO TiposEquipo (nombre, activo) VALUES (@Nombre, 1);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Nombre", tipo.Nombre)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void Modificar(TipoEquipo tipo)
        {
            string query = @"
                UPDATE TiposEquipo SET nombre=@Nombre WHERE id_tipo_equipo=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", tipo.Id),
                new SqlParameter("@Nombre", tipo.Nombre)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Desactivar(int id)
        {
            string query = @"
                UPDATE TiposEquipo SET activo=0 WHERE id_tipo_equipo=@Id;
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
                UPDATE TiposEquipo SET activo=1 WHERE id_tipo_equipo=@Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public TipoEquipo ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_tipo_equipo, nombre, activo
                FROM TiposEquipo WHERE id_tipo_equipo=@Id;
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

        public List<TipoEquipo> Listar(bool incluirInactivos = false)
        {
            string query = @"
                SELECT id_tipo_equipo, nombre, activo
                FROM TiposEquipo
                WHERE (@IncluirInactivos = 1 OR activo = 1)
                ORDER BY nombre;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IncluirInactivos", incluirInactivos ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<TipoEquipo> tipos = new List<TipoEquipo>();

            foreach (DataRow fila in dt.Rows)
                tipos.Add(Mapear(fila));

            return tipos;
        }

        private TipoEquipo Mapear(DataRow fila)
        {
            return TipoEquipo.CargarDesdeDB(
                Convert.ToInt32(fila["id_tipo_equipo"]),
                fila["nombre"] == DBNull.Value ? "" : fila["nombre"].ToString(),
                Convert.ToBoolean(fila["activo"])
            );
        }
    }
}
