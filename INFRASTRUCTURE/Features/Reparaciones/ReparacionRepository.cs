using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Reparaciones;

namespace REPOSITORY.Features.Reparaciones
{
    public class ReparacionRepository
    {
        private readonly SqlHelper _db;

        public ReparacionRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public ReparacionRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // FIX migracion legacy (idem Presupuestos Error 207): SQL Server compila el batch
            // completo antes de ejecutarlo, asi que el bloque ELSE que referencia 'id_consumo'
            // fallaria en DBs legacy sin esa columna si va en el mismo batch que el ALTER ADD.
            // Se divide en 2 fases: fase 1 crea todo lo que NO referencia 'id_consumo'
            // (incluye ALTER ADD id_consumo + DROP PK compuesta + PK nueva + resto de columnas/FKs),
            // fase 2 crea el indice cuando la columna ya existe. 2x ExecuteTransaction por
            // ser lo mas simple/explicito frente a EXEC(sp_executesql).
            // F8: ReparacionRepuesto pasa a una fila por consumo (id_consumo IDENTITY PK).
            string fase1 = @"
                IF OBJECT_ID('Reparaciones', 'U') IS NULL
                BEGIN
                    CREATE TABLE Reparaciones (
                        id_reparacion int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_orden int NOT NULL,
                        numero_intervencion int NOT NULL,
                        id_usuario_tecnico int NOT NULL,
                        fecha_inicio datetime NOT NULL CONSTRAINT DF_Reparaciones_FechaInicio DEFAULT GETDATE(),
                        fecha_fin datetime NULL,
                        trabajo_realizado nvarchar(max) NULL,
                        observaciones nvarchar(max) NULL,
                        CONSTRAINT FK_Reparaciones_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden),
                        CONSTRAINT FK_Reparaciones_Tecnico FOREIGN KEY (id_usuario_tecnico)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Reparaciones', 'id_orden') IS NULL
                        ALTER TABLE Reparaciones ADD id_orden int NOT NULL CONSTRAINT DF_Reparaciones_IdOrden DEFAULT 0;

                    IF COL_LENGTH('Reparaciones', 'numero_intervencion') IS NULL
                        ALTER TABLE Reparaciones ADD numero_intervencion int NOT NULL CONSTRAINT DF_Reparaciones_Numero DEFAULT 1;

                    IF COL_LENGTH('Reparaciones', 'id_usuario_tecnico') IS NULL
                        ALTER TABLE Reparaciones ADD id_usuario_tecnico int NOT NULL CONSTRAINT DF_Reparaciones_IdTecnico DEFAULT 0;

                    IF COL_LENGTH('Reparaciones', 'fecha_inicio') IS NULL
                        ALTER TABLE Reparaciones ADD fecha_inicio datetime NOT NULL CONSTRAINT DF_Reparaciones_FechaInicio DEFAULT GETDATE();

                    IF COL_LENGTH('Reparaciones', 'fecha_fin') IS NULL
                        ALTER TABLE Reparaciones ADD fecha_fin datetime NULL;

                    IF COL_LENGTH('Reparaciones', 'trabajo_realizado') IS NULL
                        ALTER TABLE Reparaciones ADD trabajo_realizado nvarchar(max) NULL;

                    IF COL_LENGTH('Reparaciones', 'observaciones') IS NULL
                        ALTER TABLE Reparaciones ADD observaciones nvarchar(max) NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Reparaciones_Orden'
                          AND parent_object_id = OBJECT_ID('Reparaciones')
                    )
                    BEGIN
                        ALTER TABLE Reparaciones WITH CHECK
                        ADD CONSTRAINT FK_Reparaciones_Orden FOREIGN KEY (id_orden)
                            REFERENCES OrdenesServicio(id_orden);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Reparaciones_Tecnico'
                          AND parent_object_id = OBJECT_ID('Reparaciones')
                    )
                    BEGIN
                        ALTER TABLE Reparaciones WITH CHECK
                        ADD CONSTRAINT FK_Reparaciones_Tecnico FOREIGN KEY (id_usuario_tecnico)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Reparaciones_Orden_Nro'
                      AND object_id = OBJECT_ID('Reparaciones')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Reparaciones_Orden_Nro
                    ON Reparaciones(id_orden, numero_intervencion);
                END

                IF OBJECT_ID('ReparacionRepuesto', 'U') IS NULL
                BEGIN
                    -- F8: una fila por consumo (id_consumo IDENTITY PK) + indice normal
                    -- en (id_reparacion, id_repuesto). Filas existentes (una por par)
                    -- siguen validas tras la migracion.
                    CREATE TABLE ReparacionRepuesto (
                        id_consumo int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_reparacion int NOT NULL,
                        id_repuesto int NOT NULL,
                        cantidad int NOT NULL,
                        costo_unitario decimal(18,2) NOT NULL CONSTRAINT DF_ReparacionRepuesto_Costo DEFAULT 0,
                        CONSTRAINT FK_ReparacionRepuesto_Reparacion FOREIGN KEY (id_reparacion)
                            REFERENCES Reparaciones(id_reparacion),
                        CONSTRAINT FK_ReparacionRepuesto_Repuesto FOREIGN KEY (id_repuesto)
                            REFERENCES Repuestos(id_repuesto)
                    );
                END
                ELSE
                BEGIN
                    -- F8 migracion idempotente PK compuesta -> id_consumo IDENTITY PK.
                    -- NOTA compile-time (Error 207): todo lo que referencia 'id_consumo'
                    -- (DROP PK por nombre + ADD PK + SELECT del cursor/lecturas) debe ir en
                    -- fase 2 (EXEC separado tras el ALTER ADD); fase 1 solo agrega la columna
                    -- y migra lo que NO nombra 'id_consumo'. Segunda pasada idempotente.
                    IF COL_LENGTH('ReparacionRepuesto', 'id_consumo') IS NULL
                        ALTER TABLE ReparacionRepuesto ADD id_consumo int IDENTITY(1,1) NOT NULL;

                    IF COL_LENGTH('ReparacionRepuesto', 'cantidad') IS NULL
                        ALTER TABLE ReparacionRepuesto ADD cantidad int NOT NULL CONSTRAINT DF_ReparacionRepuesto_Cantidad DEFAULT 1;

                    IF COL_LENGTH('ReparacionRepuesto', 'costo_unitario') IS NULL
                        ALTER TABLE ReparacionRepuesto ADD costo_unitario decimal(18,2) NOT NULL CONSTRAINT DF_ReparacionRepuesto_Costo DEFAULT 0;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_ReparacionRepuesto_Reparacion'
                          AND parent_object_id = OBJECT_ID('ReparacionRepuesto')
                    )
                    BEGIN
                        ALTER TABLE ReparacionRepuesto WITH CHECK
                        ADD CONSTRAINT FK_ReparacionRepuesto_Reparacion FOREIGN KEY (id_reparacion)
                            REFERENCES Reparaciones(id_reparacion);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_ReparacionRepuesto_Repuesto'
                          AND parent_object_id = OBJECT_ID('ReparacionRepuesto')
                    )
                    BEGIN
                        ALTER TABLE ReparacionRepuesto WITH CHECK
                        ADD CONSTRAINT FK_ReparacionRepuesto_Repuesto FOREIGN KEY (id_repuesto)
                            REFERENCES Repuestos(id_repuesto);
                    END
                END

                IF OBJECT_ID('Pruebas', 'U') IS NULL
                BEGIN
                    CREATE TABLE Pruebas (
                        id_prueba int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_reparacion int NOT NULL,
                        id_usuario_tecnico int NOT NULL,
                        fecha datetime NOT NULL CONSTRAINT DF_Pruebas_Fecha DEFAULT GETDATE(),
                        descripcion nvarchar(max) NOT NULL,
                        resultado nvarchar(30) NOT NULL,
                        observaciones nvarchar(max) NULL,
                        motivo_anulacion nvarchar(500) NULL,
                        fecha_anulacion datetime NULL,
                        id_usuario_anulacion int NULL,
                        CONSTRAINT FK_Pruebas_Reparacion FOREIGN KEY (id_reparacion)
                            REFERENCES Reparaciones(id_reparacion),
                        CONSTRAINT FK_Pruebas_Tecnico FOREIGN KEY (id_usuario_tecnico)
                            REFERENCES Usuarios(id_usuario),
                        CONSTRAINT FK_Pruebas_UsuarioAnulacion FOREIGN KEY (id_usuario_anulacion)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Pruebas', 'id_reparacion') IS NULL
                        ALTER TABLE Pruebas ADD id_reparacion int NOT NULL CONSTRAINT DF_Pruebas_IdReparacion DEFAULT 0;

                    IF COL_LENGTH('Pruebas', 'id_usuario_tecnico') IS NULL
                        ALTER TABLE Pruebas ADD id_usuario_tecnico int NOT NULL CONSTRAINT DF_Pruebas_IdTecnico DEFAULT 0;

                    IF COL_LENGTH('Pruebas', 'fecha') IS NULL
                        ALTER TABLE Pruebas ADD fecha datetime NOT NULL CONSTRAINT DF_Pruebas_Fecha DEFAULT GETDATE();

                    IF COL_LENGTH('Pruebas', 'descripcion') IS NULL
                        ALTER TABLE Pruebas ADD descripcion nvarchar(max) NOT NULL CONSTRAINT DF_Pruebas_Descripcion DEFAULT '';

                    IF COL_LENGTH('Pruebas', 'resultado') IS NULL
                        ALTER TABLE Pruebas ADD resultado nvarchar(30) NOT NULL CONSTRAINT DF_Pruebas_Resultado DEFAULT '';

                    IF COL_LENGTH('Pruebas', 'observaciones') IS NULL
                        ALTER TABLE Pruebas ADD observaciones nvarchar(max) NULL;

                    IF COL_LENGTH('Pruebas', 'motivo_anulacion') IS NULL
                        ALTER TABLE Pruebas ADD motivo_anulacion nvarchar(500) NULL;

                    IF COL_LENGTH('Pruebas', 'fecha_anulacion') IS NULL
                        ALTER TABLE Pruebas ADD fecha_anulacion datetime NULL;

                    IF COL_LENGTH('Pruebas', 'id_usuario_anulacion') IS NULL
                        ALTER TABLE Pruebas ADD id_usuario_anulacion int NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Pruebas_Reparacion'
                          AND parent_object_id = OBJECT_ID('Pruebas')
                    )
                    BEGIN
                        ALTER TABLE Pruebas WITH CHECK
                        ADD CONSTRAINT FK_Pruebas_Reparacion FOREIGN KEY (id_reparacion)
                            REFERENCES Reparaciones(id_reparacion);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Pruebas_Tecnico'
                          AND parent_object_id = OBJECT_ID('Pruebas')
                    )
                    BEGIN
                        ALTER TABLE Pruebas WITH CHECK
                        ADD CONSTRAINT FK_Pruebas_Tecnico FOREIGN KEY (id_usuario_tecnico)
                            REFERENCES Usuarios(id_usuario);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Pruebas_UsuarioAnulacion'
                          AND parent_object_id = OBJECT_ID('Pruebas')
                    )
                    BEGIN
                        ALTER TABLE Pruebas WITH CHECK
                        ADD CONSTRAINT FK_Pruebas_UsuarioAnulacion FOREIGN KEY (id_usuario_anulacion)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                IF OBJECT_ID('MovimientosStock', 'U') IS NOT NULL
                    AND OBJECT_ID('Reparaciones', 'U') IS NOT NULL
                BEGIN
                    IF COL_LENGTH('MovimientosStock', 'id_reparacion') IS NULL
                        ALTER TABLE MovimientosStock ADD id_reparacion int NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_MovimientosStock_Reparacion'
                          AND parent_object_id = OBJECT_ID('MovimientosStock')
                    )
                    BEGIN
                        ALTER TABLE MovimientosStock WITH CHECK
                        ADD CONSTRAINT FK_MovimientosStock_Reparacion FOREIGN KEY (id_reparacion)
                            REFERENCES Reparaciones(id_reparacion);
                    END
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(fase1);

            string fase2 = @"
                -- F8 fase 2 (compilacion separada: 'id_consumo' ya existe tras fase 1).
                -- (1) soltar PK compuesta legacy si sigue, (2) PK en id_consumo si falta,
                -- (3) indice normal. Todo con guarda: segunda pasada no-op.
                IF EXISTS (
                    SELECT 1 FROM sys.key_constraints
                    WHERE name = 'PK_ReparacionRepuesto'
                      AND parent_object_id = OBJECT_ID('ReparacionRepuesto')
                      AND type = 'PK'
                      AND OBJECT_NAME(parent_object_id) = 'ReparacionRepuesto'
                )
                BEGIN
                    DECLARE @Cols int;

                    SELECT @Cols = COUNT(1)
                    FROM sys.index_columns ic
                    INNER JOIN sys.key_constraints kc ON kc.parent_object_id = ic.object_id
                        AND kc.unique_index_id = ic.index_id
                    WHERE kc.name = 'PK_ReparacionRepuesto'
                      AND kc.parent_object_id = OBJECT_ID('ReparacionRepuesto');

                    IF (@Cols > 1)
                        ALTER TABLE ReparacionRepuesto DROP CONSTRAINT PK_ReparacionRepuesto;
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.key_constraints
                    WHERE parent_object_id = OBJECT_ID('ReparacionRepuesto')
                      AND type = 'PK'
                )
                BEGIN
                    ALTER TABLE ReparacionRepuesto ADD CONSTRAINT PK_ReparacionRepuesto PRIMARY KEY (id_consumo);
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'IX_ReparacionRepuesto_Rep_Rep'
                      AND object_id = OBJECT_ID('ReparacionRepuesto')
                )
                BEGIN
                    CREATE INDEX IX_ReparacionRepuesto_Rep_Rep
                    ON ReparacionRepuesto(id_reparacion, id_repuesto);
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(fase2);
        }

        public int IniciarReparacion(int idOrden, int idTecnico, int idUsuario)
        {
            // Batch atomico: INSERT reparacion N+1 + UPDATE orden Autorizado->EnReparacion + historial.
            string query = @"
                DECLARE @N int;

                SELECT @N = ISNULL(MAX(numero_intervencion), 0) + 1
                FROM Reparaciones WITH (UPDLOCK, HOLDLOCK)
                WHERE id_orden = @IdOrden;

                INSERT INTO Reparaciones (id_orden, numero_intervencion, id_usuario_tecnico,
                    fecha_inicio, fecha_fin, trabajo_realizado, observaciones)
                VALUES (@IdOrden, @N, @IdTecnico, GETDATE(), NULL, NULL, NULL);

                DECLARE @R int = CAST(SCOPE_IDENTITY() AS int);

                UPDATE OrdenesServicio
                SET estado = @EstadoNuevo
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo,
                    fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoAnterior, @EstadoNuevo, GETDATE(), @IdUsuario, @Observacion);

                SELECT @R;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden),
                new SqlParameter("@IdTecnico", idTecnico),
                new SqlParameter("@EstadoAnterior", "AutorizadoReparacion"),
                new SqlParameter("@EstadoNuevo", "EnReparacion"),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Observacion", "Reparacion iniciada")
            };

            return _db.ExecuteTransaction(query, sqlParameters);
        }

        public int IniciarIntervencionAdicional(int idOrden, int idTecnico, int idUsuario)
        {
            // Batch atomico sin cambio de estado ni historial (la orden ya esta EnReparacion).
            string query = @"
                DECLARE @N int;

                SELECT @N = ISNULL(MAX(numero_intervencion), 0) + 1
                FROM Reparaciones WITH (UPDLOCK, HOLDLOCK)
                WHERE id_orden = @IdOrden;

                INSERT INTO Reparaciones (id_orden, numero_intervencion, id_usuario_tecnico,
                    fecha_inicio, fecha_fin, trabajo_realizado, observaciones)
                VALUES (@IdOrden, @N, @IdTecnico, GETDATE(), NULL, NULL, NULL);

                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden),
                new SqlParameter("@IdTecnico", idTecnico)
            };

            return _db.ExecuteTransaction(query, sqlParameters);
        }

        public void ConsumirRepuesto(int idReparacion, int idRepuesto, int cantidad,
            decimal costoUnitario, int idUsuario, string observacion)
        {
            // F8: batch atomico con FILA SEPARADA por consumo (sin upsert cantidad+=):
            // cada consumo inserta una fila con el costo vigente. Valida stock,
            // inserta consumo, descuenta stock y registra movimiento.
            string query = @"
                DECLARE @Stock int;

                SELECT @Stock = stock_actual
                FROM Repuestos WITH (UPDLOCK, HOLDLOCK)
                WHERE id_repuesto = @IdRepuesto;

                IF (@Stock IS NULL)
                    THROW 50001, 'El repuesto seleccionado no existe.', 1;

                IF (@Stock < @Cantidad)
                    THROW 50002, 'Stock insuficiente para el consumo.', 1;

                INSERT INTO ReparacionRepuesto (id_reparacion, id_repuesto, cantidad, costo_unitario)
                VALUES (@IdReparacion, @IdRepuesto, @Cantidad, @Costo);

                UPDATE Repuestos
                SET stock_actual = @Stock - @Cantidad
                WHERE id_repuesto = @IdRepuesto;

                INSERT INTO MovimientosStock (id_repuesto, fecha, tipo, cantidad,
                    stock_anterior, stock_posterior, id_usuario, id_compra,
                    id_reparacion, observacion)
                VALUES (@IdRepuesto, GETDATE(), 'ConsumoReparacion', @Cantidad,
                    @Stock, @Stock - @Cantidad, @IdUsuario, NULL, @IdReparacion, @Observacion);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdReparacion", idReparacion),
                new SqlParameter("@IdRepuesto", idRepuesto),
                new SqlParameter("@Cantidad", cantidad),
                new SqlParameter("@Costo", costoUnitario),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Observacion", observacion)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void DevolverConsumo(int idReparacion, int idRepuesto, int cantidad, int idUsuario)
        {
            // F8: batch atomico con devolucion FIFO contra consumos concretos (por id_consumo,
            // los mas viejos primero) hasta cubrir la cantidad, con sus movimientos
            // correspondientes (una fila DevolucionConsumo por cada fila consumida tocada).
            string query = @"
                DECLARE @Total int;

                SELECT @Total = ISNULL(SUM(cantidad), 0)
                FROM ReparacionRepuesto WITH (UPDLOCK, HOLDLOCK)
                WHERE id_reparacion = @IdReparacion AND id_repuesto = @IdRepuesto;

                IF (@Total <= 0)
                    THROW 50003, 'La reparacion no tiene consumo registrado del repuesto.', 1;

                IF (@Cantidad > @Total)
                    THROW 50004, 'La cantidad a devolver supera la consumida.', 1;

                DECLARE @Resto int = @Cantidad;
                DECLARE @IdConsumo int;
                DECLARE @CantFila int;
                DECLARE @Dev int;
                DECLARE @StockFila int;

                DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
                    SELECT id_consumo, cantidad
                    FROM ReparacionRepuesto WITH (UPDLOCK, HOLDLOCK)
                    WHERE id_reparacion = @IdReparacion AND id_repuesto = @IdRepuesto
                    ORDER BY id_consumo;

                OPEN cur;
                FETCH NEXT FROM cur INTO @IdConsumo, @CantFila;

                WHILE (@@FETCH_STATUS = 0 AND @Resto > 0)
                BEGIN
                    SET @Dev = CASE WHEN @CantFila <= @Resto THEN @CantFila ELSE @Resto END;

                    IF (@Dev = @CantFila)
                    BEGIN
                        DELETE FROM ReparacionRepuesto WHERE id_consumo = @IdConsumo;
                    END
                    ELSE
                    BEGIN
                        UPDATE ReparacionRepuesto
                        SET cantidad = cantidad - @Dev
                        WHERE id_consumo = @IdConsumo;
                    END

                    SELECT @StockFila = stock_actual
                    FROM Repuestos WITH (UPDLOCK, HOLDLOCK)
                    WHERE id_repuesto = @IdRepuesto;

                    IF (@StockFila IS NULL)
                    BEGIN
                        CLOSE cur;
                        DEALLOCATE cur;
                        THROW 50001, 'El repuesto seleccionado no existe.', 1;
                    END

                    UPDATE Repuestos
                    SET stock_actual = @StockFila + @Dev
                    WHERE id_repuesto = @IdRepuesto;

                    INSERT INTO MovimientosStock (id_repuesto, fecha, tipo, cantidad,
                        stock_anterior, stock_posterior, id_usuario, id_compra,
                        id_reparacion, observacion)
                    VALUES (@IdRepuesto, GETDATE(), 'DevolucionConsumo', @Dev,
                        @StockFila, @StockFila + @Dev, @IdUsuario, NULL, @IdReparacion, @Observacion);

                    SET @Resto = @Resto - @Dev;

                    FETCH NEXT FROM cur INTO @IdConsumo, @CantFila;
                END

                CLOSE cur;
                DEALLOCATE cur;

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdReparacion", idReparacion),
                new SqlParameter("@IdRepuesto", idRepuesto),
                new SqlParameter("@Cantidad", cantidad),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Observacion", "Devolucion de consumo")
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void FinalizarReparacion(int idReparacion, string trabajoRealizado,
            string observaciones, int idUsuario)
        {
            // Batch atomico: UPDATE reparacion + UPDATE orden EnReparacion->EnPruebas + historial.
            string query = @"
                UPDATE Reparaciones
                SET fecha_fin = GETDATE(),
                    trabajo_realizado = @Trabajo,
                    observaciones = @Observaciones
                WHERE id_reparacion = @Id;

                DECLARE @Orden int;

                SELECT @Orden = id_orden FROM Reparaciones WHERE id_reparacion = @Id;

                UPDATE OrdenesServicio
                SET estado = @EstadoNuevo
                WHERE id_orden = @Orden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo,
                    fecha_hora, id_usuario, observacion)
                VALUES (@Orden, @EstadoAnterior, @EstadoNuevo, GETDATE(), @IdUsuario, @ObservacionHistorial);

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idReparacion),
                new SqlParameter("@Trabajo", trabajoRealizado),
                new SqlParameter("@Observaciones", (object)observaciones ?? DBNull.Value),
                new SqlParameter("@EstadoAnterior", "EnReparacion"),
                new SqlParameter("@EstadoNuevo", "EnPruebas"),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@ObservacionHistorial", "Reparacion finalizada")
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public int RegistrarPrueba(int idReparacion, int idTecnico, string descripcion,
            string resultado, string observaciones)
        {
            // Batch atomico INSERT-only: persiste la prueba sin transicion de orden.
            // La decision (ListoRetiro/EnReparacion) la toma FinalizarPruebas.
            string query = @"
                INSERT INTO Pruebas (id_reparacion, id_usuario_tecnico, fecha,
                    descripcion, resultado, observaciones)
                VALUES (@IdReparacion, @IdTecnico, GETDATE(),
                    @Descripcion, @Resultado, @Observaciones);

                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdReparacion", idReparacion),
                new SqlParameter("@IdTecnico", idTecnico),
                new SqlParameter("@Descripcion", descripcion),
                new SqlParameter("@Resultado", resultado),
                new SqlParameter("@Observaciones", (object)observaciones ?? DBNull.Value)
            };

            return _db.ExecuteTransaction(query, sqlParameters);
        }

        public void FinalizarPruebas(int idOrden, bool todasAprobadas, int idUsuario)
        {
            // Batch atomico: UPDATE orden + historial. La evaluacion previa la hace el service.
            // Todas aprobadas: EnPruebas->ListoRetiro(Reparado). Con revision: EnPruebas->EnReparacion(resultado NULL).
            string query = @"
                IF (@TodasAprobadas = 1)
                BEGIN
                    UPDATE OrdenesServicio
                    SET estado = 'ListoRetiro',
                        resultado = 'Reparado'
                    WHERE id_orden = @IdOrden;

                    INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo,
                        fecha_hora, id_usuario, observacion)
                    VALUES (@IdOrden, 'EnPruebas', 'ListoRetiro', GETDATE(), @IdUsuario, 'Pruebas finalizadas: todas aprobadas');
                END
                ELSE
                BEGIN
                    UPDATE OrdenesServicio
                    SET estado = 'EnReparacion',
                        resultado = NULL
                    WHERE id_orden = @IdOrden;

                    INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo,
                        fecha_hora, id_usuario, observacion)
                    VALUES (@IdOrden, 'EnPruebas', 'EnReparacion', GETDATE(), @IdUsuario, 'Pruebas finalizadas: requiere revision');
                END

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden),
                new SqlParameter("@TodasAprobadas", todasAprobadas ? 1 : 0),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void AnularPrueba(int idPrueba, string motivo, int idUsuario)
        {
            // Batch atomico SIN cambio de estado: solo marca la prueba como anulada.
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Pruebas WHERE id_prueba = @Id)
                    THROW 50020, 'La prueba seleccionada no existe.', 1;

                IF ((SELECT resultado FROM Pruebas WHERE id_prueba = @Id) = 'Anulada')
                    THROW 50021, 'La prueba ya se encuentra anulada.', 1;

                UPDATE Pruebas
                SET resultado = 'Anulada',
                    motivo_anulacion = @Motivo,
                    fecha_anulacion = GETDATE(),
                    id_usuario_anulacion = @IdUsuario
                WHERE id_prueba = @Id;

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idPrueba),
                new SqlParameter("@Motivo", motivo),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void CerrarAbiertaYCancelar(int idOrden, string motivo, int idUsuario)
        {
            // F2: sin llamantes validos (CancelarOrden ya no cierra reparaciones porque solo
            // admite hasta AutorizadoReparacion). Se conserva el batch por riesgo y se aplica
            // F9: preservar observaciones existentes (solo completa NULL/vacio con el motivo,
            // sin sobrescribir). Batch atomico: cierra reparacion abierta + orden->ListoRetiro(Cancelado) + historial.
            // F9: observacion_resultado tambien preserva combinando (existente + " | " + motivo).
            string query = @"
                UPDATE Reparaciones
                SET fecha_fin = GETDATE(),
                    observaciones = CASE WHEN observaciones IS NULL OR LTRIM(RTRIM(observaciones)) = '' THEN @Motivo ELSE observaciones END
                WHERE id_orden = @IdOrden AND fecha_fin IS NULL;

                DECLARE @ObsPrevia nvarchar(max);

                SELECT @ObsPrevia = observacion_resultado FROM OrdenesServicio WHERE id_orden = @IdOrden;

                UPDATE OrdenesServicio
                SET estado = 'ListoRetiro',
                    resultado = 'Cancelado',
                    observacion_resultado = CASE WHEN @ObsPrevia IS NULL OR LTRIM(RTRIM(@ObsPrevia)) = '' THEN @Motivo ELSE @ObsPrevia + ' | ' + @Motivo END
                WHERE id_orden = @IdOrden;

                INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo,
                    fecha_hora, id_usuario, observacion)
                VALUES (@IdOrden, @EstadoAnterior, 'ListoRetiro', GETDATE(), @IdUsuario, 'Orden cancelada');

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden),
                new SqlParameter("@Motivo", motivo),
                new SqlParameter("@EstadoAnterior", (object)ObtenerEstadoOrdenParaHistorial(idOrden) ?? DBNull.Value),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Reparacion ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_reparacion, id_orden, numero_intervencion, id_usuario_tecnico,
                       fecha_inicio, fecha_fin, trabajo_realizado, observaciones
                FROM Reparaciones WHERE id_reparacion = @Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return MapearReparacion(dt.Rows[0]);
        }

        public Reparacion ObtenerAbierta(int idOrden)
        {
            string query = @"
                SELECT id_reparacion, id_orden, numero_intervencion, id_usuario_tecnico,
                       fecha_inicio, fecha_fin, trabajo_realizado, observaciones
                FROM Reparaciones
                WHERE id_orden = @IdOrden AND fecha_fin IS NULL;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return MapearReparacion(dt.Rows[0]);
        }

        public List<Reparacion> ListarPorOrden(int idOrden)
        {
            string query = @"
                SELECT id_reparacion, id_orden, numero_intervencion, id_usuario_tecnico,
                       fecha_inicio, fecha_fin, trabajo_realizado, observaciones
                FROM Reparaciones
                WHERE id_orden = @IdOrden
                ORDER BY numero_intervencion;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Reparacion> reparaciones = new List<Reparacion>();

            foreach (DataRow fila in dt.Rows)
                reparaciones.Add(MapearReparacion(fila));

            return reparaciones;
        }

        public List<ReparacionRepuesto> ListarConsumidos(int idReparacion)
        {
            // F8: una fila por consumo (orden FIFO por id_consumo, igual que la devolucion).
            // NOTA compile-time (Error 207): el SELECT nombra 'id_consumo' directo; en DBs
            // legacy sin la columna fallaria al compilar. Pero Inicializar() fase 1/2 corre
            // en Program.cs ANTES que cualquier lectura, asi que para cuando este metodo
            // se ejecuta la columna ya existe. Segunda pasada idempotente.
            string query = @"
                SELECT id_consumo, id_reparacion, id_repuesto, cantidad, costo_unitario
                FROM ReparacionRepuesto
                WHERE id_reparacion = @IdReparacion
                ORDER BY id_consumo;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdReparacion", idReparacion)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<ReparacionRepuesto> consumidos = new List<ReparacionRepuesto>();

            foreach (DataRow fila in dt.Rows)
            {
                consumidos.Add(ReparacionRepuesto.CargarDesdeDB(
                    Convert.ToInt32(fila["id_reparacion"]),
                    Convert.ToInt32(fila["id_repuesto"]),
                    Convert.ToInt32(fila["cantidad"]),
                    Convert.ToDecimal(fila["costo_unitario"]),
                    fila["id_consumo"] == DBNull.Value ? 0 : Convert.ToInt32(fila["id_consumo"])
                ));
            }

            return consumidos;
        }

        public List<Prueba> ListarPruebas(int idReparacion)
        {
            string query = @"
                SELECT id_prueba, id_reparacion, id_usuario_tecnico, fecha,
                       descripcion, resultado, observaciones, motivo_anulacion,
                       fecha_anulacion, id_usuario_anulacion
                FROM Pruebas
                WHERE id_reparacion = @IdReparacion
                ORDER BY fecha, id_prueba;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdReparacion", idReparacion)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Prueba> pruebas = new List<Prueba>();

            foreach (DataRow fila in dt.Rows)
            {
                pruebas.Add(MapearPrueba(fila));
            }

            return pruebas;
        }

        public Prueba ObtenerPruebaPorId(int idPrueba)
        {
            string query = @"
                SELECT id_prueba, id_reparacion, id_usuario_tecnico, fecha,
                       descripcion, resultado, observaciones, motivo_anulacion,
                       fecha_anulacion, id_usuario_anulacion
                FROM Pruebas WHERE id_prueba = @Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idPrueba)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return MapearPrueba(dt.Rows[0]);
        }

        private Prueba MapearPrueba(DataRow fila)
        {
            return Prueba.CargarDesdeDB(
                Convert.ToInt32(fila["id_prueba"]),
                Convert.ToInt32(fila["id_reparacion"]),
                Convert.ToInt32(fila["id_usuario_tecnico"]),
                Convert.ToDateTime(fila["fecha"]),
                fila["descripcion"] == DBNull.Value ? "" : fila["descripcion"].ToString(),
                fila["resultado"] == DBNull.Value ? "" : fila["resultado"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString(),
                fila.Table.Columns.Contains("motivo_anulacion") && fila["motivo_anulacion"] != DBNull.Value ? fila["motivo_anulacion"].ToString() : null,
                fila.Table.Columns.Contains("fecha_anulacion") && fila["fecha_anulacion"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(fila["fecha_anulacion"]) : null,
                fila.Table.Columns.Contains("id_usuario_anulacion") && fila["id_usuario_anulacion"] != DBNull.Value ? (int?)Convert.ToInt32(fila["id_usuario_anulacion"]) : null
            );
        }
        private string ObtenerEstadoOrdenParaHistorial(int idOrden)
        {
            string query = @"
                SELECT estado FROM OrdenesServicio WHERE id_orden = @IdOrden;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdOrden", idOrden)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return dt.Rows[0]["estado"] == DBNull.Value ? null : dt.Rows[0]["estado"].ToString();
        }

        private Reparacion MapearReparacion(DataRow fila)
        {
            return Reparacion.CargarDesdeDB(
                Convert.ToInt32(fila["id_reparacion"]),
                Convert.ToInt32(fila["id_orden"]),
                Convert.ToInt32(fila["numero_intervencion"]),
                Convert.ToInt32(fila["id_usuario_tecnico"]),
                Convert.ToDateTime(fila["fecha_inicio"]),
                fila["fecha_fin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(fila["fecha_fin"]),
                fila["trabajo_realizado"] == DBNull.Value ? null : fila["trabajo_realizado"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString()
            );
        }
    }
}
