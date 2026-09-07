using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Ordenes;

namespace REPOSITORY.Features.Ordenes
{
    public class EntregaRepository
    {
        private readonly SqlHelper _db;

        public EntregaRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public EntregaRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            string query = @"
                IF OBJECT_ID('Entregas', 'U') IS NULL
                BEGIN
                    CREATE TABLE Entregas (
                        id_entrega int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_orden int NOT NULL,
                        fecha_entrega datetime NOT NULL CONSTRAINT DF_Entregas_FechaEntrega DEFAULT GETDATE(),
                        id_usuario int NOT NULL,
                        entregado_a nvarchar(200) NOT NULL,
                        documento_receptor nvarchar(100) NULL,
                        observaciones nvarchar(max) NULL,
                        CONSTRAINT FK_Entregas_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden),
                        CONSTRAINT FK_Entregas_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Entregas', 'id_orden') IS NULL
                        ALTER TABLE Entregas ADD id_orden int NOT NULL CONSTRAINT DF_Entregas_IdOrden DEFAULT 0;

                    IF COL_LENGTH('Entregas', 'fecha_entrega') IS NULL
                        ALTER TABLE Entregas ADD fecha_entrega datetime NOT NULL CONSTRAINT DF_Entregas_FechaEntrega DEFAULT GETDATE();

                    IF COL_LENGTH('Entregas', 'id_usuario') IS NULL
                        ALTER TABLE Entregas ADD id_usuario int NOT NULL CONSTRAINT DF_Entregas_IdUsuario DEFAULT 0;

                    IF COL_LENGTH('Entregas', 'entregado_a') IS NULL
                        ALTER TABLE Entregas ADD entregado_a nvarchar(200) NOT NULL CONSTRAINT DF_Entregas_EntregadoA DEFAULT '';

                    IF COL_LENGTH('Entregas', 'documento_receptor') IS NULL
                        ALTER TABLE Entregas ADD documento_receptor nvarchar(100) NULL;

                    IF COL_LENGTH('Entregas', 'observaciones') IS NULL
                        ALTER TABLE Entregas ADD observaciones nvarchar(max) NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Entregas_Orden'
                          AND parent_object_id = OBJECT_ID('Entregas')
                    )
                    BEGIN
                        ALTER TABLE Entregas WITH CHECK
                        ADD CONSTRAINT FK_Entregas_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Entregas_Usuario'
                          AND parent_object_id = OBJECT_ID('Entregas')
                    )
                    BEGIN
                        ALTER TABLE Entregas WITH CHECK
                        ADD CONSTRAINT FK_Entregas_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Entrega_Orden'
                      AND object_id = OBJECT_ID('Entregas')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Entrega_Orden
                    ON Entregas(id_orden);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Entrega CrearConTransicion(Entrega entrega, string estadoAnterior, string estadoNuevo,
            int idUsuarioHistorial, string observacionHistorial)
        {
            // Batch atomico: INSERT entrega + UPDATE orden a Entregado + INSERT historial
            // + INSERT garantia condicional (solo Normal + Reparado + Original aprobado con DiasGarantia>0).
            // DECISION CP4: DiasGarantia se toma del presupuesto Original aprobado (no MAX con adicionales);
            // FechaInicio=fecha_entrega, FechaFin=DATEADD(day, dias, fecha_entrega). Si no cumple, sin garantia (sin error).
            string query = @"
                INSERT INTO Entregas (id_orden, fecha_entrega, id_usuario, entregado_a, documento_receptor, observaciones)
                VALUES (@IdOrden, @FechaEntrega, @IdUsuario, @EntregadoA, @Documento, @Observaciones);

                DECLARE @E int = CAST(SCOPE_IDENTITY() AS int);

                UPDATE OrdenesServicio
                SET estado = @EstadoNuevo
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoAnterior, @EstadoNuevo, GETDATE(), @IdUsuarioHistorial, @ObservacionHistorial);

                DECLARE @Tipo nvarchar(20);
                DECLARE @Resultado nvarchar(50);
                DECLARE @Dias int;

                SELECT @Tipo = tipo_orden, @Resultado = resultado
                FROM OrdenesServicio WHERE id_orden = @IdOrden;

                SELECT @Dias = dias_garantia
                FROM Presupuestos
                WHERE id_orden = @IdOrden AND tipo = 'Original' AND estado = 'Aprobado';

                -- Garantia automatica (CP4): solo si la tabla Garantias ya existe
                -- (GarantiaService.Inicializar corre despues de Ordenes en Program.cs).
                -- En BD fresca sin Garantias: entrega normal sin garantia (sin error).
                IF (OBJECT_ID('Garantias', 'U') IS NOT NULL
                    AND @Tipo = 'Normal' AND @Resultado = 'Reparado'
                    AND @Dias IS NOT NULL AND @Dias > 0)
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM Garantias WHERE id_orden_original = @IdOrden)
                    BEGIN
                        INSERT INTO Garantias (id_orden_original, fecha_inicio, fecha_fin, observaciones, anulada)
                        VALUES (@IdOrden, @FechaEntrega, DATEADD(day, @Dias, @FechaEntrega), 'Garantia automatica por entrega', 0);
                    END
                END

                SELECT @E;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", entrega.IdOrden),
                new SqlParameter("@FechaEntrega", entrega.FechaEntrega),
                new SqlParameter("@IdUsuario", entrega.IdUsuario),
                new SqlParameter("@EntregadoA", entrega.EntregadoA),
                new SqlParameter("@Documento", (object)entrega.DocumentoReceptor ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)entrega.Observaciones ?? DBNull.Value),
                new SqlParameter("@EstadoAnterior", (object)estadoAnterior ?? DBNull.Value),
                new SqlParameter("@EstadoNuevo", estadoNuevo),
                new SqlParameter("@IdUsuarioHistorial", idUsuarioHistorial),
                new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorOrden(entrega.IdOrden);
        }

        public void CancelarEntregaConTransicion(int idOrden, string estadoAnterior, string estadoNuevo,
            bool limpiarResultado, int idUsuarioHistorial, string observacionHistorial)
        {
            // Batch atomico: DELETE fisico entrega + UPDATE orden + INSERT historial.
            // El DELETE fisico es obligatorio: la UNIQUE UX_Entrega_Orden impediria re-entregar.
            // Si el destino es EnPruebas (era Reparado) el resultado se lleva a NULL explicito;
            // en otro caso el resultado se conserva intacto.
            string query = @"
                DELETE FROM Entregas WHERE id_orden = @IdOrden;

                UPDATE OrdenesServicio
                SET estado = @EstadoNuevo,
                    resultado = CASE WHEN @LimpiarResultado = 1 THEN NULL ELSE resultado END
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoAnterior, @EstadoNuevo, GETDATE(), @IdUsuarioHistorial, @ObservacionHistorial);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden),
                new SqlParameter("@EstadoAnterior", (object)estadoAnterior ?? DBNull.Value),
                new SqlParameter("@EstadoNuevo", estadoNuevo),
                new SqlParameter("@LimpiarResultado", limpiarResultado ? 1 : 0),
                new SqlParameter("@IdUsuarioHistorial", idUsuarioHistorial),
                new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Entrega ObtenerPorOrden(int idOrden)
        {
            string query = @"
                SELECT id_entrega, id_orden, fecha_entrega, id_usuario, entregado_a,
                       documento_receptor, observaciones
                FROM Entregas WHERE id_orden = @IdOrden;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            DataRow fila = dt.Rows[0];

            return Entrega.CargarDesdeDB(
                Convert.ToInt32(fila["id_entrega"]),
                Convert.ToInt32(fila["id_orden"]),
                Convert.ToDateTime(fila["fecha_entrega"]),
                Convert.ToInt32(fila["id_usuario"]),
                fila["entregado_a"] == DBNull.Value ? "" : fila["entregado_a"].ToString(),
                fila["documento_receptor"] == DBNull.Value ? "" : fila["documento_receptor"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString()
            );
        }
    }
}
