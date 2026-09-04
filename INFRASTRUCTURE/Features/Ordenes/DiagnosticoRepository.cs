using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Ordenes;

namespace REPOSITORY.Features.Ordenes
{
    public class DiagnosticoRepository
    {
        private readonly SqlHelper _db;

        public DiagnosticoRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public DiagnosticoRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            string query = @"
                IF OBJECT_ID('Diagnosticos', 'U') IS NULL
                BEGIN
                    CREATE TABLE Diagnosticos (
                        id_diagnostico int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_orden int NOT NULL,
                        id_usuario_tecnico int NOT NULL,
                        fecha datetime NOT NULL CONSTRAINT DF_Diagnosticos_Fecha DEFAULT GETDATE(),
                        descripcion nvarchar(max) NOT NULL,
                        es_reparable bit NOT NULL CONSTRAINT DF_Diagnosticos_EsReparable DEFAULT 1,
                        tiempo_estimado_dias int NULL,
                        observaciones nvarchar(max) NULL,
                        CONSTRAINT FK_Diagnosticos_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden),
                        CONSTRAINT FK_Diagnosticos_Tecnico FOREIGN KEY (id_usuario_tecnico)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Diagnosticos', 'id_orden') IS NULL
                        ALTER TABLE Diagnosticos ADD id_orden int NOT NULL CONSTRAINT DF_Diagnosticos_IdOrden DEFAULT 0;

                    IF COL_LENGTH('Diagnosticos', 'id_usuario_tecnico') IS NULL
                        ALTER TABLE Diagnosticos ADD id_usuario_tecnico int NOT NULL CONSTRAINT DF_Diagnosticos_IdTecnico DEFAULT 0;

                    IF COL_LENGTH('Diagnosticos', 'fecha') IS NULL
                        ALTER TABLE Diagnosticos ADD fecha datetime NOT NULL CONSTRAINT DF_Diagnosticos_Fecha DEFAULT GETDATE();

                    IF COL_LENGTH('Diagnosticos', 'descripcion') IS NULL
                        ALTER TABLE Diagnosticos ADD descripcion nvarchar(max) NOT NULL CONSTRAINT DF_Diagnosticos_Descripcion DEFAULT '';

                    IF COL_LENGTH('Diagnosticos', 'es_reparable') IS NULL
                        ALTER TABLE Diagnosticos ADD es_reparable bit NOT NULL CONSTRAINT DF_Diagnosticos_EsReparable DEFAULT 1;

                    IF COL_LENGTH('Diagnosticos', 'tiempo_estimado_dias') IS NULL
                        ALTER TABLE Diagnosticos ADD tiempo_estimado_dias int NULL;

                    IF COL_LENGTH('Diagnosticos', 'observaciones') IS NULL
                        ALTER TABLE Diagnosticos ADD observaciones nvarchar(max) NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Diagnosticos_Orden'
                          AND parent_object_id = OBJECT_ID('Diagnosticos')
                    )
                    BEGIN
                        ALTER TABLE Diagnosticos WITH CHECK
                        ADD CONSTRAINT FK_Diagnosticos_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Diagnosticos_Tecnico'
                          AND parent_object_id = OBJECT_ID('Diagnosticos')
                    )
                    BEGIN
                        ALTER TABLE Diagnosticos WITH CHECK
                        ADD CONSTRAINT FK_Diagnosticos_Tecnico FOREIGN KEY (id_usuario_tecnico)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Diagnostico_Orden'
                      AND object_id = OBJECT_ID('Diagnosticos')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Diagnostico_Orden
                    ON Diagnosticos(id_orden);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Diagnostico Crear(Diagnostico diagnostico)
        {
            string query = @"
                INSERT INTO Diagnosticos (id_orden, id_usuario_tecnico, fecha, descripcion, es_reparable, tiempo_estimado_dias, observaciones)
                VALUES (@IdOrden, @IdTecnico, @Fecha, @Descripcion, @EsReparable, @TiempoDias, @Observaciones);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", diagnostico.IdOrden),
                new SqlParameter("@IdTecnico", diagnostico.IdUsuarioTecnico),
                new SqlParameter("@Fecha", diagnostico.Fecha),
                new SqlParameter("@Descripcion", diagnostico.Descripcion),
                new SqlParameter("@EsReparable", diagnostico.EsReparable),
                new SqlParameter("@TiempoDias", diagnostico.TiempoEstimadoDias.HasValue ? (object)diagnostico.TiempoEstimadoDias.Value : DBNull.Value),
                new SqlParameter("@Observaciones", (object)diagnostico.Observaciones ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorOrden(diagnostico.IdOrden);
        }

        public Diagnostico CrearConTransicion(Diagnostico diagnostico, string estadoAnterior,
            string estadoNuevo, int idUsuario, string observacionHistorial, string resultado,
            string observacionResultado)
        {
            // Batch atomico: INSERT diagnostico + UPDATE orden + INSERT historial.
            string query = @"
                INSERT INTO Diagnosticos (id_orden, id_usuario_tecnico, fecha, descripcion, es_reparable, tiempo_estimado_dias, observaciones)
                VALUES (@IdOrden, @IdTecnico, @Fecha, @Descripcion, @EsReparable, @TiempoDias, @Observaciones);

                DECLARE @D int = CAST(SCOPE_IDENTITY() AS int);

                UPDATE OrdenesServicio
                SET estado = @EstadoNuevo,
                    resultado = COALESCE(@Resultado, resultado),
                    observacion_resultado = COALESCE(@ObservacionResultado, observacion_resultado)
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoAnterior, @EstadoNuevo, GETDATE(), @IdUsuario, @ObservacionHistorial);

                SELECT @D;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", diagnostico.IdOrden),
                new SqlParameter("@IdTecnico", diagnostico.IdUsuarioTecnico),
                new SqlParameter("@Fecha", diagnostico.Fecha),
                new SqlParameter("@Descripcion", diagnostico.Descripcion),
                new SqlParameter("@EsReparable", diagnostico.EsReparable),
                new SqlParameter("@TiempoDias", diagnostico.TiempoEstimadoDias.HasValue ? (object)diagnostico.TiempoEstimadoDias.Value : DBNull.Value),
                new SqlParameter("@Observaciones", (object)diagnostico.Observaciones ?? DBNull.Value),
                new SqlParameter("@EstadoAnterior", (object)estadoAnterior ?? DBNull.Value),
                new SqlParameter("@EstadoNuevo", estadoNuevo),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value),
                new SqlParameter("@Resultado", (object)resultado ?? DBNull.Value),
                new SqlParameter("@ObservacionResultado", (object)observacionResultado ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorOrden(diagnostico.IdOrden);
        }

        public Diagnostico ObtenerPorOrden(int idOrden)
        {
            string query = @"
                SELECT id_diagnostico, id_orden, id_usuario_tecnico, fecha, descripcion,
                       es_reparable, tiempo_estimado_dias, observaciones
                FROM Diagnosticos WHERE id_orden = @IdOrden;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            DataRow fila = dt.Rows[0];

            return Diagnostico.CargarDesdeDB(
                Convert.ToInt32(fila["id_diagnostico"]),
                Convert.ToInt32(fila["id_orden"]),
                Convert.ToInt32(fila["id_usuario_tecnico"]),
                Convert.ToDateTime(fila["fecha"]),
                fila["descripcion"] == DBNull.Value ? "" : fila["descripcion"].ToString(),
                Convert.ToBoolean(fila["es_reparable"]),
                fila["tiempo_estimado_dias"] == DBNull.Value ? (int?)null : Convert.ToInt32(fila["tiempo_estimado_dias"]),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString()
            );
        }
    }
}
