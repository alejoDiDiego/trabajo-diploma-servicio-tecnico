using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Garantias;

namespace REPOSITORY.Features.Garantias
{
    public class GarantiaRepository
    {
        private readonly SqlHelper _db;

        public GarantiaRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public GarantiaRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea Garantias de forma idempotente. UNIQUE en id_orden_original: una orden genera como maximo una garantia.
            string query = @"
                IF OBJECT_ID('Garantias', 'U') IS NULL
                BEGIN
                    CREATE TABLE Garantias (
                        id_garantia int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_orden_original int NOT NULL,
                        fecha_inicio datetime NOT NULL,
                        fecha_fin datetime NOT NULL,
                        observaciones nvarchar(max) NULL,
                        anulada bit NOT NULL CONSTRAINT DF_Garantias_Anulada DEFAULT 0,
                        CONSTRAINT FK_Garantias_OrdenOriginal FOREIGN KEY (id_orden_original)
                            REFERENCES OrdenesServicio(id_orden)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Garantias', 'id_orden_original') IS NULL
                        ALTER TABLE Garantias ADD id_orden_original int NOT NULL CONSTRAINT DF_Garantias_IdOrden DEFAULT 0;

                    IF COL_LENGTH('Garantias', 'fecha_inicio') IS NULL
                        ALTER TABLE Garantias ADD fecha_inicio datetime NOT NULL CONSTRAINT DF_Garantias_FechaInicio DEFAULT GETDATE();

                    IF COL_LENGTH('Garantias', 'fecha_fin') IS NULL
                        ALTER TABLE Garantias ADD fecha_fin datetime NOT NULL CONSTRAINT DF_Garantias_FechaFin DEFAULT GETDATE();

                    IF COL_LENGTH('Garantias', 'observaciones') IS NULL
                        ALTER TABLE Garantias ADD observaciones nvarchar(max) NULL;

                    IF COL_LENGTH('Garantias', 'anulada') IS NULL
                        ALTER TABLE Garantias ADD anulada bit NOT NULL CONSTRAINT DF_Garantias_Anulada DEFAULT 0;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Garantias_OrdenOriginal'
                          AND parent_object_id = OBJECT_ID('Garantias')
                    )
                    BEGIN
                        ALTER TABLE Garantias WITH CHECK
                        ADD CONSTRAINT FK_Garantias_OrdenOriginal FOREIGN KEY (id_orden_original)
                            REFERENCES OrdenesServicio(id_orden);
                    END
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Garantias_OrdenOriginal'
                      AND object_id = OBJECT_ID('Garantias')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Garantias_OrdenOriginal
                    ON Garantias(id_orden_original);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Garantia Crear(Garantia garantia)
        {
            // Batch atomico: THROW si ya existe garantia para la orden + INSERT + SELECT id.
            string query = @"
                IF EXISTS (SELECT 1 FROM Garantias WHERE id_orden_original = @IdOrden)
                    THROW 50040, 'La orden ya tiene una garantia registrada.', 1;

                INSERT INTO Garantias (id_orden_original, fecha_inicio, fecha_fin, observaciones, anulada)
                VALUES (@IdOrden, @FechaInicio, @FechaFin, @Observaciones, 0);

                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", garantia.IdOrdenOriginal),
                new SqlParameter("@FechaInicio", garantia.FechaInicio),
                new SqlParameter("@FechaFin", garantia.FechaFin),
                new SqlParameter("@Observaciones", (object)garantia.Observaciones ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public Garantia ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_garantia, id_orden_original, fecha_inicio, fecha_fin, observaciones, anulada
                FROM Garantias WHERE id_garantia = @Id;
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

        public Garantia ObtenerPorOrden(int idOrdenOriginal)
        {
            string query = @"
                SELECT id_garantia, id_orden_original, fecha_inicio, fecha_fin, observaciones, anulada
                FROM Garantias WHERE id_orden_original = @IdOrden;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrdenOriginal)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return Mapear(dt.Rows[0]);
        }

        public Garantia ObtenerVigente(int idOrdenOriginal)
        {
            // Vigente: GETDATE() <= fecha_fin y no anulada.
            string query = @"
                SELECT id_garantia, id_orden_original, fecha_inicio, fecha_fin, observaciones, anulada
                FROM Garantias
                WHERE id_orden_original = @IdOrden
                  AND anulada = 0
                  AND GETDATE() <= fecha_fin;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrdenOriginal)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return Mapear(dt.Rows[0]);
        }

        public void Anular(int id)
        {
            string query = @"
                UPDATE Garantias SET anulada = 1 WHERE id_garantia = @Id;
                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        private Garantia Mapear(DataRow fila)
        {
            return Garantia.CargarDesdeDB(
                Convert.ToInt32(fila["id_garantia"]),
                Convert.ToInt32(fila["id_orden_original"]),
                Convert.ToDateTime(fila["fecha_inicio"]),
                Convert.ToDateTime(fila["fecha_fin"]),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString(),
                Convert.ToBoolean(fila["anulada"])
            );
        }
    }
}
