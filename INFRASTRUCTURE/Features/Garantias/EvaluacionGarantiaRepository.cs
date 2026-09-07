using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Garantias;

namespace REPOSITORY.Features.Garantias
{
    public class EvaluacionGarantiaRepository
    {
        private readonly SqlHelper _db;

        public EvaluacionGarantiaRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public EvaluacionGarantiaRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea EvaluacionesGarantia de forma idempotente. UNIQUE en id_orden_reingreso:
            // un reingreso se evalua una sola vez.
            string query = @"
                IF OBJECT_ID('EvaluacionesGarantia', 'U') IS NULL
                BEGIN
                    CREATE TABLE EvaluacionesGarantia (
                        id_evaluacion int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_orden_reingreso int NOT NULL,
                        id_garantia int NOT NULL,
                        estado nvarchar(20) NOT NULL CONSTRAINT DF_EvaluacionesGarantia_Estado DEFAULT 'Pendiente',
                        fecha datetime NOT NULL CONSTRAINT DF_EvaluacionesGarantia_Fecha DEFAULT GETDATE(),
                        id_usuario int NOT NULL,
                        motivo nvarchar(max) NULL,
                        observaciones nvarchar(max) NULL,
                        CONSTRAINT FK_EvaluacionesGarantia_Reingreso FOREIGN KEY (id_orden_reingreso)
                            REFERENCES OrdenesServicio(id_orden),
                        CONSTRAINT FK_EvaluacionesGarantia_Garantia FOREIGN KEY (id_garantia)
                            REFERENCES Garantias(id_garantia),
                        CONSTRAINT FK_EvaluacionesGarantia_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('EvaluacionesGarantia', 'id_orden_reingreso') IS NULL
                        ALTER TABLE EvaluacionesGarantia ADD id_orden_reingreso int NOT NULL CONSTRAINT DF_EvaluacionesGarantia_IdReingreso DEFAULT 0;

                    IF COL_LENGTH('EvaluacionesGarantia', 'id_garantia') IS NULL
                        ALTER TABLE EvaluacionesGarantia ADD id_garantia int NOT NULL CONSTRAINT DF_EvaluacionesGarantia_IdGarantia DEFAULT 0;

                    IF COL_LENGTH('EvaluacionesGarantia', 'estado') IS NULL
                        ALTER TABLE EvaluacionesGarantia ADD estado nvarchar(20) NOT NULL CONSTRAINT DF_EvaluacionesGarantia_Estado DEFAULT 'Pendiente';

                    IF COL_LENGTH('EvaluacionesGarantia', 'fecha') IS NULL
                        ALTER TABLE EvaluacionesGarantia ADD fecha datetime NOT NULL CONSTRAINT DF_EvaluacionesGarantia_Fecha DEFAULT GETDATE();

                    IF COL_LENGTH('EvaluacionesGarantia', 'id_usuario') IS NULL
                        ALTER TABLE EvaluacionesGarantia ADD id_usuario int NOT NULL CONSTRAINT DF_EvaluacionesGarantia_IdUsuario DEFAULT 0;

                    IF COL_LENGTH('EvaluacionesGarantia', 'motivo') IS NULL
                        ALTER TABLE EvaluacionesGarantia ADD motivo nvarchar(max) NULL;

                    IF COL_LENGTH('EvaluacionesGarantia', 'observaciones') IS NULL
                        ALTER TABLE EvaluacionesGarantia ADD observaciones nvarchar(max) NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_EvaluacionesGarantia_Reingreso'
                          AND parent_object_id = OBJECT_ID('EvaluacionesGarantia')
                    )
                    BEGIN
                        ALTER TABLE EvaluacionesGarantia WITH CHECK
                        ADD CONSTRAINT FK_EvaluacionesGarantia_Reingreso FOREIGN KEY (id_orden_reingreso)
                            REFERENCES OrdenesServicio(id_orden);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_EvaluacionesGarantia_Garantia'
                          AND parent_object_id = OBJECT_ID('EvaluacionesGarantia')
                    )
                    BEGIN
                        ALTER TABLE EvaluacionesGarantia WITH CHECK
                        ADD CONSTRAINT FK_EvaluacionesGarantia_Garantia FOREIGN KEY (id_garantia)
                            REFERENCES Garantias(id_garantia);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_EvaluacionesGarantia_Usuario'
                          AND parent_object_id = OBJECT_ID('EvaluacionesGarantia')
                    )
                    BEGIN
                        ALTER TABLE EvaluacionesGarantia WITH CHECK
                        ADD CONSTRAINT FK_EvaluacionesGarantia_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_EvaluacionesGarantia_Reingreso'
                      AND object_id = OBJECT_ID('EvaluacionesGarantia')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_EvaluacionesGarantia_Reingreso
                    ON EvaluacionesGarantia(id_orden_reingreso);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public EvaluacionGarantia Crear(EvaluacionGarantia evaluacion)
        {
            string query = @"
                INSERT INTO EvaluacionesGarantia (id_orden_reingreso, id_garantia, estado, fecha, id_usuario, motivo, observaciones)
                VALUES (@IdReingreso, @IdGarantia, @Estado, @Fecha, @IdUsuario, @Motivo, @Observaciones);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdReingreso", evaluacion.IdOrdenReingreso),
                new SqlParameter("@IdGarantia", evaluacion.IdGarantia),
                new SqlParameter("@Estado", evaluacion.Estado),
                new SqlParameter("@Fecha", evaluacion.Fecha),
                new SqlParameter("@IdUsuario", evaluacion.IdUsuario),
                new SqlParameter("@Motivo", (object)evaluacion.Motivo ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)evaluacion.Observaciones ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void Evaluar(int id, string estado, string motivo, string observaciones)
        {
            // Batch atomico: exige Pendiente, fija estado final + motivo.
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM EvaluacionesGarantia WHERE id_evaluacion = @Id)
                    THROW 50050, 'La evaluacion seleccionada no existe.', 1;

                IF ((SELECT estado FROM EvaluacionesGarantia WHERE id_evaluacion = @Id) <> 'Pendiente')
                    THROW 50051, 'Solo se puede evaluar una evaluacion pendiente.', 1;

                UPDATE EvaluacionesGarantia
                SET estado = @Estado,
                    fecha = GETDATE(),
                    motivo = @Motivo,
                    observaciones = @Observaciones
                WHERE id_evaluacion = @Id;

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Estado", estado),
                new SqlParameter("@Motivo", (object)motivo ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)observaciones ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public EvaluacionGarantia ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_evaluacion, id_orden_reingreso, id_garantia, estado, fecha, id_usuario, motivo, observaciones
                FROM EvaluacionesGarantia WHERE id_evaluacion = @Id;
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

        public EvaluacionGarantia ObtenerPorReingreso(int idOrdenReingreso)
        {
            string query = @"
                SELECT id_evaluacion, id_orden_reingreso, id_garantia, estado, fecha, id_usuario, motivo, observaciones
                FROM EvaluacionesGarantia WHERE id_orden_reingreso = @IdReingreso;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdReingreso", idOrdenReingreso)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return Mapear(dt.Rows[0]);
        }

        private EvaluacionGarantia Mapear(DataRow fila)
        {
            return EvaluacionGarantia.CargarDesdeDB(
                Convert.ToInt32(fila["id_evaluacion"]),
                Convert.ToInt32(fila["id_orden_reingreso"]),
                Convert.ToInt32(fila["id_garantia"]),
                fila["estado"] == DBNull.Value ? null : fila["estado"].ToString(),
                Convert.ToDateTime(fila["fecha"]),
                Convert.ToInt32(fila["id_usuario"]),
                fila["motivo"] == DBNull.Value ? "" : fila["motivo"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString()
            );
        }
    }
}
