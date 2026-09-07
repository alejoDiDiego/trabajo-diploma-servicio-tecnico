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
            // FIX migracion legacy (Error 207): SQL Server compila el batch completo antes de
            // ejecutarlo, asi que el CREATE UNIQUE INDEX filtrado WHERE tipo fallaba en DBs sin
            // la columna 'tipo'. Se divide en 2 batches/compilaciones separadas: fase 1 crea/migra
            // todo lo que NO referencia 'tipo' (incluye ALTER ADD tipo + DROP old UX), fase 2 crea
            // el indice filtrado cuando la columna ya existe. Se eligio 2x ExecuteTransaction por
            // ser lo mas simple/explicito frente a EXEC(sp_executesql).
            string fase1 = @"
                IF OBJECT_ID('Presupuestos', 'U') IS NULL
                BEGIN
                    CREATE TABLE Presupuestos (
                        id_presupuesto int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_orden int NOT NULL,
                        tipo nvarchar(20) NOT NULL CONSTRAINT DF_Presupuestos_Tipo DEFAULT 'Original',
                        fecha_emision datetime NOT NULL CONSTRAINT DF_Presupuestos_FechaEmision DEFAULT GETDATE(),
                        estado nvarchar(20) NOT NULL CONSTRAINT DF_Presupuestos_Estado DEFAULT 'Pendiente',
                        subtotal decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Subtotal DEFAULT 0,
                        descuento decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Descuento DEFAULT 0,
                        total decimal(18,2) NOT NULL CONSTRAINT DF_Presupuestos_Total DEFAULT 0,
                        dias_garantia int NOT NULL CONSTRAINT DF_Presupuestos_DiasGarantia DEFAULT 0,
                        fecha_respuesta datetime NULL,
                        medio_respuesta nvarchar(100) NULL,
                        motivo_rechazo nvarchar(max) NULL,
                        motivo_anulacion nvarchar(500) NULL,
                        observaciones nvarchar(max) NULL,
                        CONSTRAINT FK_Presupuestos_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Presupuestos', 'id_orden') IS NULL
                        ALTER TABLE Presupuestos ADD id_orden int NOT NULL CONSTRAINT DF_Presupuestos_IdOrden DEFAULT 0;

                    IF COL_LENGTH('Presupuestos', 'tipo') IS NULL
                        ALTER TABLE Presupuestos ADD tipo nvarchar(20) NOT NULL CONSTRAINT DF_Presupuestos_Tipo DEFAULT 'Original';

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

                    IF COL_LENGTH('Presupuestos', 'motivo_anulacion') IS NULL
                        ALTER TABLE Presupuestos ADD motivo_anulacion nvarchar(500) NULL;

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

                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Presupuesto_Orden'
                      AND object_id = OBJECT_ID('Presupuestos')
                )
                BEGIN
                    DROP INDEX UX_Presupuesto_Orden ON Presupuestos;
                END

                IF EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Presupuesto_Original'
                      AND object_id = OBJECT_ID('Presupuestos')
                )
                BEGIN
                    DROP INDEX UX_Presupuesto_Original ON Presupuestos;
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
                        -- CP4: la FK de id_repuesto hacia Repuestos se agrega en RepuestoRepository
                        -- con guarda (igual que FK_MovimientosStock_Reparacion en ReparacionRepository):
                        -- Ordenes se inicializa ANTES que Repuestos y en BD fresca Repuestos aun no existe.
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

                    -- CP4: infraestructura id_repuesto -> Repuestos (sin habilitar en Emitir:
                    -- la validacion sigue siendo solo ManoObra/Servicio).
                    -- La FK se crea en RepuestoRepository.Inicializar con guarda (corre despues,
                    -- cuando Repuestos ya existe); aqui no se crea para no fallar en BD fresca
                    -- donde Ordenes se inicializa antes que Repuestos.
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(fase1);

            string fase2 = @"
                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Presupuesto_Original'
                      AND object_id = OBJECT_ID('Presupuestos')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Presupuesto_Original
                    ON Presupuestos(id_orden) WHERE tipo = 'Original' AND estado <> 'Anulado';
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(fase2);
        }

        public Presupuesto EmitirConDetalle(Presupuesto presupuesto, List<DetallePresupuesto> items,
            string estadoOrdenAnterior, string estadoOrdenNuevo, int idUsuario, string observacionHistorial)
        {
            // Batch atomico: INSERT presupuesto + N INSERT detalle + UPDATE orden + INSERT historial.
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                INSERT INTO Presupuestos (id_orden, tipo, fecha_emision, estado, subtotal, descuento, total,
                    dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, observaciones)
                VALUES (@IdOrden, @Tipo, @FechaEmision, @Estado, @Subtotal, @Descuento, @Total,
                    @DiasGarantia, NULL, NULL, NULL, @Observaciones);

                DECLARE @P int = CAST(SCOPE_IDENTITY() AS int);
            ");

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdOrden", presupuesto.IdOrden));
            parametros.Add(new SqlParameter("@Tipo", presupuesto.Tipo));
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
                SELECT id_presupuesto, id_orden, tipo, fecha_emision, estado, subtotal, descuento, total,
                       dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, motivo_anulacion, observaciones
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
            // Compatibilidad: con presupuestos 1:N devuelve el Original activo (los anulados son historial).
            string query = @"
                SELECT id_presupuesto, id_orden, tipo, fecha_emision, estado, subtotal, descuento, total,
                       dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, motivo_anulacion, observaciones
                FROM Presupuestos WHERE id_orden = @IdOrden AND tipo = 'Original' AND estado <> 'Anulado';
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

        public Presupuesto ObtenerOriginal(int idOrden)
        {
            // Devuelve el Original activo. Los anulados quedan como historial (ver ListarPorOrden).
            string query = @"
                SELECT id_presupuesto, id_orden, tipo, fecha_emision, estado, subtotal, descuento, total,
                       dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, motivo_anulacion, observaciones
                FROM Presupuestos WHERE id_orden = @IdOrden AND tipo = 'Original' AND estado <> 'Anulado';
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

        public List<Presupuesto> ListarAdicionales(int idOrden)
        {
            string query = @"
                SELECT id_presupuesto, id_orden, tipo, fecha_emision, estado, subtotal, descuento, total,
                       dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, motivo_anulacion, observaciones
                FROM Presupuestos
                WHERE id_orden = @IdOrden AND tipo = 'Adicional'
                ORDER BY id_presupuesto;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Presupuesto> adicionales = new List<Presupuesto>();

            foreach (DataRow fila in dt.Rows)
                adicionales.Add(Mapear(fila));

            return adicionales;
        }

        public bool ExisteAdicionalPendiente(int idOrden)
        {
            string query = @"
                SELECT COUNT(1)
                FROM Presupuestos
                WHERE id_orden = @IdOrden
                  AND tipo = 'Adicional'
                  AND estado IN ('Borrador', 'Pendiente');
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            return _db.ExecuteTransaction(query, sqlParameters) > 0;
        }

        public decimal CalcularMontoAutorizado(int idOrden)
        {
            string query = @"
                SELECT ISNULL(SUM(total), 0)
                FROM Presupuestos
                WHERE id_orden = @IdOrden AND estado = 'Aprobado';
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0 || dt.Rows[0][0] == DBNull.Value)
                return 0;

            return Convert.ToDecimal(dt.Rows[0][0]);
        }

        public Presupuesto EmitirAdicional(Presupuesto presupuesto, List<DetallePresupuesto> items, int idUsuario)
        {
            // Emision atomica como el Original: INSERT adicional + N detalle + orden a EsperandoRespuesta + historial.
            return EmitirConDetalle(presupuesto, items, "PendientePresupuesto", "EsperandoRespuesta",
                idUsuario, "Presupuesto adicional emitido");
        }

        public void AprobarAdicionalConTransicion(int idPresupuesto, int idUsuario,
            string medioRespuesta, string observaciones, string estadoDestino)
        {
            // Batch atomico: adicional->Aprobado + orden EsperandoRespuesta->origen + historial.
            // Origen EnReparacion/EnPruebas vuelve a EnReparacion; origen Autorizado vuelve a Autorizado.
            string query = @"
                DECLARE @Orden int;

                SELECT @Orden = id_orden FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto;

                UPDATE Presupuestos
                SET estado = 'Aprobado',
                    fecha_respuesta = GETDATE(),
                    medio_respuesta = @Medio,
                    observaciones = COALESCE(@Observaciones, observaciones)
                WHERE id_presupuesto = @IdPresupuesto;

                UPDATE OrdenesServicio
                SET estado = @EstadoDestino
                WHERE id_orden = @Orden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@Orden, 'EsperandoRespuesta', @EstadoDestino, GETDATE(), @IdUsuario, 'Presupuesto adicional aprobado');

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Medio", (object)medioRespuesta ?? DBNull.Value),
                new SqlParameter("@Observaciones", (object)observaciones ?? DBNull.Value),
                new SqlParameter("@EstadoDestino", estadoDestino)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void RechazarAdicionalConTransicion(int idPresupuesto, int idUsuario, string motivo,
            string medioRespuesta, string observaciones, string resultadoOrden,
            string observacionResultado, string observacionHistorial)
        {
            // Batch atomico: adicional->Rechazado + cierra reparacion abierta + orden->ListoRetiro + historial.
            string query = @"
                DECLARE @Orden int;

                SELECT @Orden = id_orden FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto;

                UPDATE Presupuestos
                SET estado = 'Rechazado',
                    fecha_respuesta = GETDATE(),
                    medio_respuesta = @Medio,
                    motivo_rechazo = @Motivo,
                    observaciones = COALESCE(@Observaciones, observaciones)
                WHERE id_presupuesto = @IdPresupuesto;

                -- F9: preservar observaciones existentes de la reparacion abierta
                -- (no sobrescribir con el motivo del rechazo).
                UPDATE Reparaciones
                SET fecha_fin = GETDATE()
                WHERE id_orden = @Orden AND fecha_fin IS NULL;

                UPDATE OrdenesServicio
                SET estado = 'ListoRetiro',
                    resultado = @ResultadoOrden,
                    observacion_resultado = @ObservacionResultado
                WHERE id_orden = @Orden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@Orden, 'EsperandoRespuesta', 'ListoRetiro', GETDATE(), @IdUsuario, @ObservacionHistorial);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto),
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

        public Presupuesto CrearBorrador(Presupuesto presupuesto, List<DetallePresupuesto> items)
        {
            // Batch atomico SIN transicion de orden: INSERT borrador + N INSERT detalle.
            // No referencia motivo_anulacion (queda NULL por defecto).
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                INSERT INTO Presupuestos (id_orden, tipo, fecha_emision, estado, subtotal, descuento, total,
                    dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, observaciones)
                VALUES (@IdOrden, @Tipo, @FechaEmision, @Estado, @Subtotal, @Descuento, @Total,
                    @DiasGarantia, NULL, NULL, NULL, @Observaciones);

                DECLARE @P int = CAST(SCOPE_IDENTITY() AS int);
            ");

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdOrden", presupuesto.IdOrden));
            parametros.Add(new SqlParameter("@Tipo", presupuesto.Tipo));
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
                SELECT @P;
            ");

            int id = _db.ExecuteTransaction(sb.ToString(), parametros.ToArray());

            return ObtenerPorId(id);
        }

        public void PublicarBorrador(int idPresupuesto, int idUsuario)
        {
            // Batch atomico: borrador->Pendiente + orden->EsperandoRespuesta + historial.
            string query = @"
                DECLARE @Orden int;
                DECLARE @Ant nvarchar(50);

                SELECT @Orden = p.id_orden, @Ant = o.estado
                FROM Presupuestos p
                INNER JOIN OrdenesServicio o ON o.id_orden = p.id_orden
                WHERE p.id_presupuesto = @IdPresupuesto;

                IF (@Orden IS NULL)
                    THROW 50010, 'El presupuesto seleccionado no existe.', 1;

                IF ((SELECT estado FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto) <> 'Borrador')
                    THROW 50011, 'Solo se puede publicar un presupuesto en borrador.', 1;

                UPDATE Presupuestos
                SET estado = 'Pendiente'
                WHERE id_presupuesto = @IdPresupuesto;

                UPDATE OrdenesServicio
                SET estado = 'EsperandoRespuesta'
                WHERE id_orden = @Orden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@Orden, @Ant, 'EsperandoRespuesta', GETDATE(), @IdUsuario, 'Borrador de presupuesto publicado');

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void EliminarBorrador(int idPresupuesto)
        {
            // Batch atomico: exige Borrador y elimina fisico (detalle + cabecera).
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto)
                    THROW 50010, 'El presupuesto seleccionado no existe.', 1;

                IF ((SELECT estado FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto) <> 'Borrador')
                    THROW 50012, 'Solo se puede eliminar un presupuesto en borrador.', 1;

                DELETE FROM PresupuestoDetalle WHERE id_presupuesto = @IdPresupuesto;

                DELETE FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto;

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void AnularConTransicion(int idPresupuesto, string motivo, string estadoOrdenAnterior,
            string nuevoEstadoOrden, string resultadoOrden, string observacionResultado,
            string observacionHistorial, bool cerrarAbierta, int idUsuario)
        {
            // Batch atomico: budget->Anulado+motivo (+ orden/historial/cierre segun parametros).
            // Si nuevoEstadoOrden es NULL no se toca la orden ni se registra historial.
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto)
                    THROW 50010, 'El presupuesto seleccionado no existe.', 1;

                IF ((SELECT estado FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto) = 'Anulado')
                    THROW 50013, 'El presupuesto ya se encuentra anulado.', 1;

                UPDATE Presupuestos
                SET estado = 'Anulado',
                    motivo_anulacion = @Motivo
                WHERE id_presupuesto = @IdPresupuesto;

                DECLARE @Orden int;

                SELECT @Orden = id_orden FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto;

                IF (@CerrarAbierta = 1)
                BEGIN
                    UPDATE Reparaciones
                    SET fecha_fin = GETDATE(),
                        observaciones = @Motivo
                    WHERE id_orden = @Orden AND fecha_fin IS NULL;
                END

                IF (@NuevoEstadoOrden IS NOT NULL)
                BEGIN
                    UPDATE OrdenesServicio
                    SET estado = @NuevoEstadoOrden,
                        resultado = COALESCE(@ResultadoOrden, resultado),
                        observacion_resultado = COALESCE(@ObservacionResultado, observacion_resultado)
                    WHERE id_orden = @Orden;

                    INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                    VALUES (@Orden, @EstadoOrdenAnterior, @NuevoEstadoOrden, GETDATE(), @IdUsuario, @ObservacionHistorial);
                END

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto),
                new SqlParameter("@Motivo", motivo),
                new SqlParameter("@EstadoOrdenAnterior", (object)estadoOrdenAnterior ?? DBNull.Value),
                new SqlParameter("@NuevoEstadoOrden", (object)nuevoEstadoOrden ?? DBNull.Value),
                new SqlParameter("@ResultadoOrden", (object)resultadoOrden ?? DBNull.Value),
                new SqlParameter("@ObservacionResultado", (object)observacionResultado ?? DBNull.Value),
                new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value),
                new SqlParameter("@CerrarAbierta", cerrarAbierta ? 1 : 0),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void AnularRechazadoConTransicion(int idPresupuesto, string motivo,
            string estadoOrdenAnterior, string nuevoEstadoOrden, string observacionResultadoCombinada,
            string observacionHistorial, int idUsuario)
        {
            // F5: batch atomico especifico para anular un Rechazado con transicion coherente:
            // presupuesto->Anulado + orden ListoRetiro/PresupuestoRechazado->PendientePresupuesto +
            // resultado NULL + historial. F9: observacion_resultado se COMBINA en C# antes de
            // llamar (existente + " | " + motivo anulacion), el batch solo la escribe.
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto)
                    THROW 50010, 'El presupuesto seleccionado no existe.', 1;

                IF ((SELECT estado FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto) <> 'Rechazado')
                    THROW 50015, 'Solo se puede anular con transicion un presupuesto rechazado.', 1;

                DECLARE @Orden int;
                DECLARE @EstadoOrden nvarchar(50);
                DECLARE @ResultadoOrden nvarchar(50);

                SELECT @Orden = id_orden FROM Presupuestos WHERE id_presupuesto = @IdPresupuesto;

                SELECT @EstadoOrden = estado, @ResultadoOrden = resultado
                FROM OrdenesServicio WHERE id_orden = @Orden;

                IF (@EstadoOrden <> 'ListoRetiro' OR @ResultadoOrden <> 'PresupuestoRechazado')
                    THROW 50016, 'No se puede anular el presupuesto rechazado en el estado actual de la orden.', 1;

                IF EXISTS (SELECT 1 FROM Entregas WHERE id_orden = @Orden)
                    THROW 50017, 'No se puede anular el presupuesto rechazado porque la orden ya fue entregada.', 1;

                UPDATE Presupuestos
                SET estado = 'Anulado',
                    motivo_anulacion = @Motivo
                WHERE id_presupuesto = @IdPresupuesto;

                UPDATE OrdenesServicio
                SET estado = @NuevoEstadoOrden,
                    resultado = NULL,
                    observacion_resultado = @ObservacionResultado
                WHERE id_orden = @Orden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@Orden, @EstadoOrdenAnterior, @NuevoEstadoOrden, GETDATE(), @IdUsuario, @ObservacionHistorial);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdPresupuesto", idPresupuesto),
                new SqlParameter("@Motivo", motivo),
                new SqlParameter("@EstadoOrdenAnterior", (object)estadoOrdenAnterior ?? DBNull.Value),
                new SqlParameter("@NuevoEstadoOrden", nuevoEstadoOrden),
                new SqlParameter("@ObservacionResultado", (object)observacionResultadoCombinada ?? DBNull.Value),
                new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void CancelarSolicitudAdicional(int idOrden, string estadoOrigen, string estadoAnterior,
            int idUsuario, string observacionHistorial)
        {
            // Batch atomico: elimina borradores adicionales fisicos + orden->origen + historial.
            // Solo bloquea un adicional Pendiente del ciclo actual: los Aprobado/Rechazado
            // pertenecen siempre a ciclos anteriores (aprobar/rechazar saca a la orden de
            // PendientePresupuesto) y no deben impedir cancelar la solicitud vigente.
            string query = @"
                IF EXISTS (SELECT 1 FROM Presupuestos
                           WHERE id_orden = @IdOrden AND tipo = 'Adicional'
                             AND estado = 'Pendiente')
                    THROW 50014, 'La orden tiene un presupuesto adicional pendiente que impide cancelar la solicitud.', 1;

                DELETE d
                FROM PresupuestoDetalle d
                INNER JOIN Presupuestos p ON p.id_presupuesto = d.id_presupuesto
                WHERE p.id_orden = @IdOrden AND p.tipo = 'Adicional' AND p.estado = 'Borrador';

                DELETE FROM Presupuestos
                WHERE id_orden = @IdOrden AND tipo = 'Adicional' AND estado = 'Borrador';

                UPDATE OrdenesServicio
                SET estado = @EstadoOrigen
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo, fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoAnterior, @EstadoOrigen, GETDATE(), @IdUsuario, @ObservacionHistorial);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden),
                new SqlParameter("@EstadoOrigen", estadoOrigen),
                new SqlParameter("@EstadoAnterior", (object)estadoAnterior ?? DBNull.Value),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@ObservacionHistorial", (object)observacionHistorial ?? DBNull.Value)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public bool ExisteOriginalActivo(int idOrden)
        {
            string query = @"
                SELECT COUNT(1)
                FROM Presupuestos
                WHERE id_orden = @IdOrden
                  AND tipo = 'Original'
                  AND estado <> 'Anulado';
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            return _db.ExecuteTransaction(query, sqlParameters) > 0;
        }

        public List<Presupuesto> ListarPorOrden(int idOrden)
        {
            string query = @"
                SELECT id_presupuesto, id_orden, tipo, fecha_emision, estado, subtotal, descuento, total,
                       dias_garantia, fecha_respuesta, medio_respuesta, motivo_rechazo, motivo_anulacion, observaciones
                FROM Presupuestos
                WHERE id_orden = @IdOrden
                ORDER BY id_presupuesto;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Presupuesto> presupuestos = new List<Presupuesto>();

            foreach (DataRow fila in dt.Rows)
                presupuestos.Add(Mapear(fila));

            return presupuestos;
        }

        public bool HasActividadPosterior(int idPresupuesto)
        {
            // Actividad operativa posterior a la aprobacion (fecha_respuesta):
            // reparaciones iniciadas despues, movimientos de sus reparaciones o pruebas posteriores.
            // Las 4 ramas de aprobacion fijan fecha_respuesta = GETDATE(), nunca es NULL en aprobados.
            // SqlHelper.AddRange adhiere los SqlParameter al comando: no se puede reutilizar
            // el mismo array en 3 ExecuteTransaction (Error "already contained"). Se crea uno nuevo por consulta.
            SqlParameter[] P1() { return new SqlParameter[] { new SqlParameter("@Id", idPresupuesto) }; }

            string reparaciones = @"
                SELECT COUNT(1)
                FROM Reparaciones r
                INNER JOIN Presupuestos p ON p.id_orden = r.id_orden
                WHERE p.id_presupuesto = @Id
                  AND p.fecha_respuesta IS NOT NULL
                  AND r.fecha_inicio > p.fecha_respuesta;
            ";

            if (_db.ExecuteTransaction(reparaciones, P1()) > 0)
                return true;

            string movimientos = @"
                SELECT COUNT(1)
                FROM MovimientosStock m
                INNER JOIN Reparaciones r ON r.id_reparacion = m.id_reparacion
                INNER JOIN Presupuestos p ON p.id_orden = r.id_orden
                WHERE p.id_presupuesto = @Id
                  AND p.fecha_respuesta IS NOT NULL
                  AND m.fecha > p.fecha_respuesta;
            ";

            if (_db.ExecuteTransaction(movimientos, P1()) > 0)
                return true;

            string pruebas = @"
                SELECT COUNT(1)
                FROM Pruebas pr
                INNER JOIN Reparaciones r ON r.id_reparacion = pr.id_reparacion
                INNER JOIN Presupuestos p ON p.id_orden = r.id_orden
                WHERE p.id_presupuesto = @Id
                  AND p.fecha_respuesta IS NOT NULL
                  AND pr.fecha > p.fecha_respuesta;
            ";

            return _db.ExecuteTransaction(pruebas, P1()) > 0;
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
                fila["tipo"] == DBNull.Value ? TipoPresupuesto.Original : fila["tipo"].ToString(),
                Convert.ToDateTime(fila["fecha_emision"]),
                fila["estado"] == DBNull.Value ? null : fila["estado"].ToString(),
                Convert.ToDecimal(fila["subtotal"]),
                Convert.ToDecimal(fila["descuento"]),
                Convert.ToDecimal(fila["total"]),
                Convert.ToInt32(fila["dias_garantia"]),
                fila["fecha_respuesta"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(fila["fecha_respuesta"]),
                fila["medio_respuesta"] == DBNull.Value ? null : fila["medio_respuesta"].ToString(),
                fila["motivo_rechazo"] == DBNull.Value ? null : fila["motivo_rechazo"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString(),
                fila.Table.Columns.Contains("motivo_anulacion") && fila["motivo_anulacion"] != DBNull.Value ? fila["motivo_anulacion"].ToString() : null
            );
        }
    }
}
