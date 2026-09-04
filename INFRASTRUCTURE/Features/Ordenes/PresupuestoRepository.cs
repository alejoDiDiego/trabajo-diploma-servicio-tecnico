using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DOMAIN.Features.Ordenes;

namespace REPOSITORY.Features.Ordenes
{
    public class PresupuestoRepository
    {
        private readonly SqlHelper _db;

        public PresupuestoRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public PresupuestoRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            string query = @"
                IF OBJECT_ID('Presupuestos', 'U') IS NULL
                BEGIN
                    CREATE TABLE Presupuestos (
                        id_presupuesto int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_orden int NOT NULL,
                        fecha_emision datetime NOT NULL CONSTRAINT DF_Presupuestos_FechaEmision DEFAULT GETDATE(),
                        estado nvarchar(20) NOT NULL CONSTRAINT DF_Presupuestos_Estado DEFAULT 'Pendiente',
                        subtotal decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Subtotal DEFAULT 0,
                        descuento decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Descuento DEFAULT 0,
                        total decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Total DEFAULT 0,
                        dias_garantia int NOT NULL CONSTRAINT DF_Presupuestos_DiasGarantia DEFAULT 0,
                        fecha_respuesta datetime NULL,
                        medio_respuesta nvarchar(100) NULL,
                        motivo_rechazo nvarchar(max) NULL,
                        observaciones nvarchar(max) NULL,
                        CONSTRAINT FK_Presupuestos_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Presupuestos', 'id_orden') IS NULL
                        ALTER TABLE Presupuestos ADD id_orden int NOT NULL CONSTRAINT DF_Presupuestos_IdOrden DEFAULT 0;

                    IF COL_LENGTH('Presupuestos', 'fecha_emision') IS NULL
                        ALTER TABLE Presupuestos ADD fecha_emision datetime NOT NULL CONSTRAINT DF_Presupuestos_FechaEmision DEFAULT GETDATE();

                    IF COL_LENGTH('Presupuestos', 'estado') IS NULL
                        ALTER TABLE Presupuestos ADD estado nvarchar(20) NOT NULL CONSTRAINT DF_Presupuestos_Estado DEFAULT 'Pendiente';

                    IF COL_LENGTH('Presupuestos', 'subtotal') IS NULL
                        ALTER TABLE Presupuestos ADD subtotal decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Subtotal DEFAULT 0;

                    IF COL_LENGTH('Presupuestos', 'descuento') IS NULL
                        ALTER TABLE Presupuestos ADD descuento decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Descuento DEFAULT 0;

                    IF COL_LENGTH('Presupuestos', 'total') IS NULL
                        ALTER TABLE Presupuestos ADD total decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Total DEFAULT 0;

                    IF COL_LENGTH('Presupuestos', 'dias_garantia') IS NULL
                        ALTER TABLE Presupuestos ADD dias_garantia int NOT NULL CONSTRAINT DF_Presupuestos_DiasGarantia DEFAULT 0;

                    IF COL_LENGTH('Presupuestos', 'fecha_respuesta') IS NULL
                        ALTER TABLE Presupuestos ADD fecha_respuesta datetime NULL;

                    IF COL_LENGTH('Presupuestos', 'medio_respuesta') IS NULL
                        ALTER TABLE Presupuestos ADD medio_respuesta nvarchar(100) NULL;

                    IF COL_LENGTH('Presupuestos', 'motivo_rechazo') IS NULL
                        ALTER TABLE Presupuestos ADD motivo_rechazo nvarchar(max) NULL;

                    IF COL_LENGTH('Presupuestos', 'observaciones') IS NULL
                        ALTER TABLE Presupuestos ADD observaciones nvarchar(max) NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Presupuestos_Orden'
                          AND parent_object_id = OBJECT_ID('Presupuestos')
                    )
                    BEGIN
                        ALTER TABLE Presupuestos WITH CHECK
                        ADD CONSTRAINT FK_Presupuestos_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden);
                    END
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Presupuesto_Orden'
                      AND object_id = OBJECT_ID('Presupuestos')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Presupuesto_Orden
                    ON Presupuestos(id_orden);
                END

                IF OBJECT_ID('PresupuestoDetalle', 'U') IS NULL
                BEGIN
                    CREATE TABLE PresupuestoDetalle (
                        id_detalle int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_presupuesto int NOT NULL,
                        id_repuesto int NULL,
                        tipo_item nvarchar(20) NOT NULL,
                        descripcion nvarchar(max) NOT NULL,
                        cantidad int NOT NULL,
                        precio_unitario decimal(18,2) NOT NULL,
                        subtotal decimal(18,2) NOT NULL,
                        CONSTRAINT FK_PresupuestoDetalle_Presupuesto FOREIGN KEY (id_presupuesto)
                            REFERENCES Presupuestos(id_presupuesto)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('PresupuestoDetalle', 'id_presupuesto') IS NULL
                        ALTER TABLE PresupuestoDetalle ADD id_presupuesto int NOT NULL CONSTRAINT DF_PresupuestoDetalle_IdPresupuesto DEFAULT 0;

                    IF COL_LENGTH('PresupuestoDetalle', 'id_repuesto') IS NULL
                        ALTER TABLE PresupuestoDetalle ADD id_repuesto int NULL;

                    IF COL_LENGTH('PresupuestoDetalle', 'tipo_item') IS NULL
                        ALTER TABLE PresupuestoDetalle ADD tipo_item nvarchar(20) NOT NULL CONSTRAINT DF_PresupuestoDetalle_Tipo DEFAULT '';

                    IF COL_LENGTH('PresupuestoDetalle', 'descripcion') IS NULL
                        ALTER TABLE PresupuestoDetalle ADD descripcion nvarchar(max) NOT NULL CONSTRAINT DF_PresupuestoDetalle_Descripcion DEFAULT '';

                    IF COL_LENGTH('PresupuestoDetalle', 'cantidad') IS NULL
                        ALTER TABLE PresupuestoDetalle ADD cantidad int NOT NULL CONSTRAINT DF_PresupuestoDetalle_Cantidad DEFAULT 1;

                    IF COL_LENGTH('PresupuestoDetalle', 'precio_unitario') IS NULL
                        ALTER TABLE PresupuestoDetalle ADD precio_unitario decimal(18,2) NOT NULL CONSTRAINT DF_PresupuestoDetalle_Precio DEFAULT 0;

                    IF COL_LENGTH('PresupuestoDetalle', 'subtotal') IS NULL
                        ALTER TABLE PresupuestoDetalle ADD subtotal decimal(18,2) NOT NULL CONSTRAINT DF_PresupuestoDetalle_Subtotal DEFAULT 0;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_PresupuestoDetalle_Presupuesto'
                          AND parent_object_id = OBJECT_ID('PresupuestoDetalle')
                    )
                    BEGIN
                        ALTER TABLE PresupuestoDetalle WITH CHECK
                        ADD CONSTRAINT FK_PresupuestoDetalle_Presupuesto FOREIGN KEY (id_presupuesto)
                            REFERENCES Presupuestos(id_presupuesto);
                    END
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Presupuesto EmitirConDetalle(Presupuesto presupuesto, List<DetallePresupuesto> items,
            string estadoOrdenAnterior, string estadoOrdenNuevo, int idUsuario, string observacionHistorial)
        {
            // Batch atomico: INSERT presupuesto + N INSERT detalle + UPDATE orden + INSERT historial.
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                INSERT INTO Presupuestos (id_orden, fecha_emision, estado, subtotal, descuento, total,
                    dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, observaciones)
                VALUES (@IdOrden, @FechaEmision, @Estado, @Subtotal, @Descuento, @Total,
                    @DiasGarantia, NULL, NULL, NULL, @Observaciones);

                DECLARE @P int = CAST(SCOPE_IDENTITY() AS int);
            ");

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdOrden", presupuesto.IdOrden));
            parametros.Add(new SqlParameter("@FechaEmision", presupuesto.FechaEmision));
            parametros.Add(new SqlParameter("@Estado", presupuesto.Estado));
            parametros.Add(new SqlParameter("@Subtotal", presupuesto.Subtotal));
            parametros.Add(new SqlParameter("@Descuento", presupuesto.Descuento));
            parametros.Add(new SqlParameter("@Total", presupuesto.Total));
            parametros.Add(new SqlParameter("@DiasGarantia", presupuesto.DiasGarantia));
            parametros.Add(new SqlParameter("@Observaciones", (object)presupuesto.Observaciones ?? DBNull.Value));

            for (int i = 0; i < items.Count; i++)
            {
                sb.Append(@"
                INSERT INTO PresupuestoDetalle (id_presupuesto, id_repuesto, tipo_item, descripcion, cantidad, precio_unitario, subtotal)
                VALUES (@P, NULL, @Tipo" + i + ", @Desc" + i + ", @Cant" + i + ", @Precio" + i + ", @Sub" + i + ");");

                parametros.Add(new SqlParameter("@Tipo" + i, items[i].TipoItem));
                parametros.Add(new SqlParameter("@Desc" + i, items[i].Descripcion));
                parametros.Add(new SqlParameter("@Cant" + i, items[i].Cantidad));
                parametros.Add(new SqlParameter("@Precio" + i, items[i].PrecioUnitario));
                parametros.Add(new SqlParameter("@Sub" + i, items[i].Subtotal));
            }

            sb.Append(@"
                UPDATE OrdenesServicio
                SET estado = @EstadoOrdenNuevo
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoOrdenAnterior, @EstadoOrdenNuevo, GETDATE(), @IdUsuario, @ObservacionHistorial);

                SELECT @P;
            ");

            parametros.Add(new SqlParameter("@EstadoOrdenNuevo", estadoOrdenNuevo));
            parametros.Add(new SqlParameter("@EstadoOrdenAnterior", (object)estadoOrdenAnterior ?? DBNull.Value));
            parametros.Add(new SqlParameter("@IdUsuario", idUsuario));
            parametros.Add(new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value));

            int id = _db.ExecuteTransaction(sb.ToString(), parametros.ToArray());

            return ObtenerPorId(id);
        }

        public void AprobarConTransicion(int idPresupuesto, int idOrden, string estadoOrdenAnterior,
            string estadoOrdenNuevo, int idUsuario, string medioRespuesta, string observaciones,
            string observacionHistorial)
        {
            // Batch atomico: UPDATE presupuesto + UPDATE orden + INSERT historial.
            string query = @"
                UPDATE Presupuestos
                SET estado = 'Aprobado',
                    fecha_respuesta = GETDATE(),
                    medio_respuesta = @Medio,
                    observaciones = COALESCE(@Observaciones, observaciones)
                WHERE id_presupuesto = @IdPresupuesto;

                UPDATE OrdenesServicio
                SET estado = @EstadoOrdenNuevo
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoOrdenAnterior, @EstadoOrdenNuevo, GETDATE(), @IdUsuario, @ObservacionHistorial);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto),
                new SqlParameter("@IdOrden", idOrden),
                new SqlParameter("@EstadoOrdenAnterior", (object)estadoOrdenAnterior ?? DBNull.Value),
                new SqlParameter("@EstadoOrdenNuevo", estadoOrdenNuevo),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Medio", (object)medioRespuesta ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)observaciones ?? DBNull.Value),
                new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void RechazarConTransicion(int idPresupuesto, int idOrden, string estadoOrdenAnterior,
            string estadoOrdenNuevo, int idUsuario, string motivo, string medioRespuesta,
            string observaciones, string resultadoOrden, string observacionResultado,
            string observacionHistorial)
        {
            // Batch atomico: UPDATE presupuesto + UPDATE orden + INSERT historial.
            string query = @"
                UPDATE Presupuestos
                SET estado = 'Rechazado',
                    fecha_respuesta = GETDATE(),
                    medio_respuesta = @Medio,
                    motivo_rechazo = @Motivo,
                    observaciones = COALESCE(@Observaciones, observaciones)
                WHERE id_presupuesto = @IdPresupuesto;

                UPDATE OrdenesServicio
                SET estado = @EstadoOrdenNuevo,
                    resultado = @ResultadoOrden,
                    observacion_resultado = @ObservacionResultado
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoOrdenAnterior, @EstadoOrdenNuevo, GETDATE(), @IdUsuario, @ObservacionHistorial);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto),
                new SqlParameter("@IdOrden", idOrden),
                new SqlParameter("@EstadoOrdenAnterior", (object)estadoOrdenAnterior ?? DBNull.Value),
                new SqlParameter("@EstadoOrdenNuevo", estadoOrdenNuevo),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Motivo", motivo),
                new SqlParameter("@Medio", (object)medioRespuesta ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)observaciones ?? DBNull.Value),
                new SqlParameter("@ResultadoOrden", resultadoOrden),
                new SqlParameter("@ObservacionResultado", (object)observacionResultado ?? DBNull.Value),
                new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Presupuesto ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_presupuesto, id_orden, fecha_emision, estado, subtotal, descuento, total,
                       dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, observaciones
                FROM Presupuestos WHERE id_presupuesto = @Id;
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

        public Presupuesto ObtenerPorOrden(int idOrden)
        {
            string query = @"
                SELECT id_presupuesto, id_orden, fecha_emision, estado, subtotal, descuento, total,
                       dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, observaciones
                FROM Presupuestos WHERE id_orden = @IdOrden;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return Mapear(dt.Rows[0]);
        }

        public List<DetallePresupuesto> ListarDetalle(int idPresupuesto)
        {
            string query = @"
                SELECT id_detalle, id_presupuesto, id_repuesto, tipo_item, descripcion,
                       cantidad, precio_unitario, subtotal
                FROM PresupuestoDetalle
                WHERE id_presupuesto = @IdPresupuesto
                ORDER BY id_detalle;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<DetallePresupuesto> items = new List<DetallePresupuesto>();

            foreach (DataRow fila in dt.Rows)
            {
                items.Add(DetallePresupuesto.CargarDesdeDB(
                    Convert.ToInt32(fila["id_detalle"]),
                    Convert.ToInt32(fila["id_presupuesto"]),
                    fila["id_repuesto"] == DBNull.Value ? (int?)null : Convert.ToInt32(fila["id_repuesto"]),
                    fila["tipo_item"] == DBNull.Value ? "" : fila["tipo_item"].ToString(),
                    fila["descripcion"] == DBNull.Value ? "" : fila["descripcion"].ToString(),
                    Convert.ToInt32(fila["cantidad"]),
                    Convert.ToDecimal(fila["precio_unitario"]),
                    Convert.ToDecimal(fila["subtotal"])
                ));
            }

            return items;
        }

        private Presupuesto Mapear(DataRow fila)
        {
            return Presupuesto.CargarDesdeDB(
                Convert.ToInt32(fila["id_presupuesto"]),
                Convert.ToInt32(fila["id_orden"]),
                Convert.ToDateTime(fila["fecha_emision"]),
                fila["estado"] == DBNull.Value ? null : fila["estado"].ToString(),
                Convert.ToDecimal(fila["subtotal"]),
                Convert.ToDecimal(fila["descuento"]),
                Convert.ToDecimal(fila["total"]),
                Convert.ToInt32(fila["dias_garantia"]),
                fila["fecha_respuesta"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(fila["fecha_respuesta"]),
                fila["medio_respuesta"] == DBNull.Value ? null : fila["medio_respuesta"].ToString(),
                fila["motivo_rechazo"] == DBNull.Value ? null : fila["motivo_rechazo"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString()
            );
        }
    }
}
