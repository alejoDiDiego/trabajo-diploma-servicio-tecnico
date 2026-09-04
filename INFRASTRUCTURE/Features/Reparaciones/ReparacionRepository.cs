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
            string query = @"
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
                    CREATE TABLE ReparacionRepuesto (
                        id_reparacion int NOT NULL,
                        id_repuesto int NOT NULL,
                        cantidad int NOT NULL,
                        costo_unitario decimal(18,2) NOT NULL CONSTRAINT DF_ReparacionRepuesto_Costo DEFAULT 0,
                        CONSTRAINT PK_ReparacionRepuesto PRIMARY KEY (id_reparacion, id_repuesto),
                        CONSTRAINT FK_ReparacionRepuesto_Reparacion FOREIGN KEY (id_reparacion)
                            REFERENCES Reparaciones(id_reparacion),
                        CONSTRAINT FK_ReparacionRepuesto_Repuesto FOREIGN KEY (id_repuesto)
                            REFERENCES Repuestos(id_repuesto)
                    );
                END
                ELSE
                BEGIN
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
                        CONSTRAINT FK_Pruebas_Reparacion FOREIGN KEY (id_reparacion)
                            REFERENCES Reparaciones(id_reparacion),
                        CONSTRAINT FK_Pruebas_Tecnico FOREIGN KEY (id_usuario_tecnico)
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

            _db.ExecuteTransaction(query);
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
            // Batch atomico: valida stock, acumula consumo, descuenta stock y registra movimiento.
            string query = @"
                DECLARE @Stock int;

                SELECT @Stock = stock_actual
                FROM Repuestos WITH (UPDLOCK, HOLDLOCK)
                WHERE id_repuesto = @IdRepuesto;

                IF (@Stock IS NULL)
                    THROW 50001, 'El repuesto seleccionado no existe.', 1;

                IF (@Stock < @Cantidad)
                    THROW 50002, 'Stock insuficiente para el consumo.', 1;

                IF EXISTS (SELECT 1 FROM ReparacionRepuesto
                           WHERE id_reparacion = @IdReparacion AND id_repuesto = @IdRepuesto)
                BEGIN
                    UPDATE ReparacionRepuesto
                    SET cantidad = cantidad + @Cantidad
                    WHERE id_reparacion = @IdReparacion AND id_repuesto = @IdRepuesto;
                END
                ELSE
                BEGIN
                    INSERT INTO ReparacionRepuesto (id_reparacion, id_repuesto, cantidad, costo_unitario)
                    VALUES (@IdReparacion, @IdRepuesto, @Cantidad, @Costo);
                END

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
            string resultado, string observaciones, int idUsuario, bool aprobada)
        {
            // Batch atomico: INSERT prueba + transicion EnPruebas->ListoRetiro(Reparado) o ->EnReparacion + historial.
            string query = @"
                INSERT INTO Pruebas (id_reparacion, id_usuario_tecnico, fecha,
                    descripcion, resultado, observaciones)
                VALUES (@IdReparacion, @IdTecnico, GETDATE(),
                    @Descripcion, @Resultado, @Observaciones);

                DECLARE @P int = CAST(SCOPE_IDENTITY() AS int);

                DECLARE @Orden int;

                SELECT @Orden = id_orden FROM Reparaciones WHERE id_reparacion = @IdReparacion;

                IF (@Aprobada = 1)
                BEGIN
                    UPDATE OrdenesServicio
                    SET estado = 'ListoRetiro',
                        resultado = 'Reparado'
                    WHERE id_orden = @Orden;

                    INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo,
                        fecha_hora, id_usuario, observacion)
                    VALUES (@Orden, 'EnPruebas', 'ListoRetiro', GETDATE(), @IdUsuario, 'Prueba aprobada');
                END
                ELSE
                BEGIN
                    UPDATE OrdenesServicio
                    SET estado = 'EnReparacion'
                    WHERE id_orden = @Orden;

                    INSERT INTO HistorialOrdenes (id_orden, estado_anterior, estado_nuevo,
                        fecha_hora, id_usuario, observacion)
                    VALUES (@Orden, 'EnPruebas', 'EnReparacion', GETDATE(), @IdUsuario, 'Prueba fallida');
                END

                SELECT @P;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdReparacion", idReparacion),
                new SqlParameter("@IdTecnico", idTecnico),
                new SqlParameter("@Descripcion", descripcion),
                new SqlParameter("@Resultado", resultado),
                new SqlParameter("@Observaciones", (object)observaciones ?? DBNull.Value),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Aprobada", aprobada ? 1 : 0)
            };

            return _db.ExecuteTransaction(query, sqlParameters);
        }

        public void CerrarAbiertaYCancelar(int idOrden, string motivo, int idUsuario)
        {
            // Batch atomico: cierra reparacion abierta + orden->ListoRetiro(Cancelado) + historial.
            string query = @"
                UPDATE Reparaciones
                SET fecha_fin = GETDATE(),
                    observaciones = @Motivo
                WHERE id_orden = @IdOrden AND fecha_fin IS NULL;

                UPDATE OrdenesServicio
                SET estado = 'ListoRetiro',
                    resultado = 'Cancelado',
                    observacion_resultado = @Motivo
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
            string query = @"
                SELECT id_reparacion, id_repuesto, cantidad, costo_unitario
                FROM ReparacionRepuesto
                WHERE id_reparacion = @IdReparacion
                ORDER BY id_repuesto;
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
                    Convert.ToDecimal(fila["costo_unitario"])
                ));
            }

            return consumidos;
        }

        public List<Prueba> ListarPruebas(int idReparacion)
        {
            string query = @"
                SELECT id_prueba, id_reparacion, id_usuario_tecnico, fecha,
                       descripcion, resultado, observaciones
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
                pruebas.Add(Prueba.CargarDesdeDB(
                    Convert.ToInt32(fila["id_prueba"]),
                    Convert.ToInt32(fila["id_reparacion"]),
                    Convert.ToInt32(fila["id_usuario_tecnico"]),
                    Convert.ToDateTime(fila["fecha"]),
                    fila["descripcion"] == DBNull.Value ? "" : fila["descripcion"].ToString(),
                    fila["resultado"] == DBNull.Value ? "" : fila["resultado"].ToString(),
                    fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString()
                ));
            }

            return pruebas;
        }

        public Prueba ObtenerPruebaPorId(int idPrueba)
        {
            string query = @"
                SELECT id_prueba, id_reparacion, id_usuario_tecnico, fecha,
                       descripcion, resultado, observaciones
                FROM Pruebas WHERE id_prueba = @Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idPrueba)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            DataRow fila = dt.Rows[0];

            return Prueba.CargarDesdeDB(
                Convert.ToInt32(fila["id_prueba"]),
                Convert.ToInt32(fila["id_reparacion"]),
                Convert.ToInt32(fila["id_usuario_tecnico"]),
                Convert.ToDateTime(fila["fecha"]),
                fila["descripcion"] == DBNull.Value ? "" : fila["descripcion"].ToString(),
                fila["resultado"] == DBNull.Value ? "" : fila["resultado"].ToString(),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString()
            );
        }

        public bool EsUltimaFinalizada(int idReparacion)
        {
            string query = @"
                SELECT COUNT(1)
                FROM Reparaciones r
                WHERE r.id_reparacion = @Id
                  AND r.fecha_fin IS NOT NULL
                  AND r.numero_intervencion = (
                      SELECT MAX(numero_intervencion)
                      FROM Reparaciones
                      WHERE id_orden = r.id_orden
                  );
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idReparacion)
            };

            return _db.ExecuteTransaction(query, sqlParameters) > 0;
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
