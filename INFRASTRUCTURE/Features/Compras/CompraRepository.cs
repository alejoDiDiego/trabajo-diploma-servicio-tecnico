using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DOMAIN.Features.Compras;

namespace REPOSITORY.Features.Compras
{
    public class CompraRepository
    {
        private readonly SqlHelper _db;

        public CompraRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public CompraRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea Compras + CompraDetalle de forma idempotente.
            // La FK de MovimientosStock.id_compra hacia Compras se agrega con guarda
            // para evitar dependencia de orden de inicializacion.
            string query = @"
                IF OBJECT_ID('Compras', 'U') IS NULL
                BEGIN
                    CREATE TABLE Compras (
                        id_compra int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_proveedor int NOT NULL,
                        fecha datetime NOT NULL CONSTRAINT DF_Compras_Fecha DEFAULT GETDATE(),
                        estado nvarchar(20) NOT NULL CONSTRAINT DF_Compras_Estado DEFAULT 'Borrador',
                        total decimal(18,2) NOT NULL CONSTRAINT DF_Compras_Total DEFAULT 0,
                        id_usuario int NOT NULL,
                        observaciones nvarchar(max) NULL,
                        motivo_anulacion nvarchar(500) NULL,
                        CONSTRAINT FK_Compras_Proveedor FOREIGN KEY (id_proveedor)
                            REFERENCES Proveedores(id_proveedor),
                        CONSTRAINT FK_Compras_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Compras', 'id_proveedor') IS NULL
                        ALTER TABLE Compras ADD id_proveedor int NOT NULL CONSTRAINT DF_Compras_IdProveedor DEFAULT 0;

                    IF COL_LENGTH('Compras', 'fecha') IS NULL
                        ALTER TABLE Compras ADD fecha datetime NOT NULL CONSTRAINT DF_Compras_Fecha DEFAULT GETDATE();

                    IF COL_LENGTH('Compras', 'estado') IS NULL
                        ALTER TABLE Compras ADD estado nvarchar(20) NOT NULL CONSTRAINT DF_Compras_Estado DEFAULT 'Borrador';

                    IF COL_LENGTH('Compras', 'total') IS NULL
                        ALTER TABLE Compras ADD total decimal(18,2) NOT NULL CONSTRAINT DF_Compras_Total DEFAULT 0;

                    IF COL_LENGTH('Compras', 'id_usuario') IS NULL
                        ALTER TABLE Compras ADD id_usuario int NOT NULL CONSTRAINT DF_Compras_IdUsuario DEFAULT 0;

                    IF COL_LENGTH('Compras', 'observaciones') IS NULL
                        ALTER TABLE Compras ADD observaciones nvarchar(max) NULL;

                    IF COL_LENGTH('Compras', 'motivo_anulacion') IS NULL
                        ALTER TABLE Compras ADD motivo_anulacion nvarchar(500) NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Compras_Proveedor'
                          AND parent_object_id = OBJECT_ID('Compras')
                    )
                    BEGIN
                        ALTER TABLE Compras WITH CHECK
                        ADD CONSTRAINT FK_Compras_Proveedor FOREIGN KEY (id_proveedor)
                            REFERENCES Proveedores(id_proveedor);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_Compras_Usuario'
                          AND parent_object_id = OBJECT_ID('Compras')
                    )
                    BEGIN
                        ALTER TABLE Compras WITH CHECK
                        ADD CONSTRAINT FK_Compras_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                IF OBJECT_ID('CompraDetalle', 'U') IS NULL
                BEGIN
                    CREATE TABLE CompraDetalle (
                        id_detalle int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_compra int NOT NULL,
                        id_repuesto int NOT NULL,
                        cantidad int NOT NULL,
                        costo_unitario decimal(18,2) NOT NULL CONSTRAINT DF_CompraDetalle_Costo DEFAULT 0,
                        subtotal decimal(18,2) NOT NULL CONSTRAINT DF_CompraDetalle_Subtotal DEFAULT 0,
                        CONSTRAINT FK_CompraDetalle_Compra FOREIGN KEY (id_compra)
                            REFERENCES Compras(id_compra),
                        CONSTRAINT FK_CompraDetalle_Repuesto FOREIGN KEY (id_repuesto)
                            REFERENCES Repuestos(id_repuesto)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('CompraDetalle', 'id_compra') IS NULL
                        ALTER TABLE CompraDetalle ADD id_compra int NOT NULL CONSTRAINT DF_CompraDetalle_IdCompra DEFAULT 0;

                    IF COL_LENGTH('CompraDetalle', 'id_repuesto') IS NULL
                        ALTER TABLE CompraDetalle ADD id_repuesto int NOT NULL CONSTRAINT DF_CompraDetalle_IdRepuesto DEFAULT 0;

                    IF COL_LENGTH('CompraDetalle', 'cantidad') IS NULL
                        ALTER TABLE CompraDetalle ADD cantidad int NOT NULL CONSTRAINT DF_CompraDetalle_Cantidad DEFAULT 1;

                    IF COL_LENGTH('CompraDetalle', 'costo_unitario') IS NULL
                        ALTER TABLE CompraDetalle ADD costo_unitario decimal(18,2) NOT NULL CONSTRAINT DF_CompraDetalle_Costo DEFAULT 0;

                    IF COL_LENGTH('CompraDetalle', 'subtotal') IS NULL
                        ALTER TABLE CompraDetalle ADD subtotal decimal(18,2) NOT NULL CONSTRAINT DF_CompraDetalle_Subtotal DEFAULT 0;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_CompraDetalle_Compra'
                          AND parent_object_id = OBJECT_ID('CompraDetalle')
                    )
                    BEGIN
                        ALTER TABLE CompraDetalle WITH CHECK
                        ADD CONSTRAINT FK_CompraDetalle_Compra FOREIGN KEY (id_compra)
                            REFERENCES Compras(id_compra);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_CompraDetalle_Repuesto'
                          AND parent_object_id = OBJECT_ID('CompraDetalle')
                    )
                    BEGIN
                        ALTER TABLE CompraDetalle WITH CHECK
                        ADD CONSTRAINT FK_CompraDetalle_Repuesto FOREIGN KEY (id_repuesto)
                            REFERENCES Repuestos(id_repuesto);
                    END
                END

                IF OBJECT_ID('MovimientosStock', 'U') IS NOT NULL
                    AND OBJECT_ID('Compras', 'U') IS NOT NULL
                BEGIN
                    IF COL_LENGTH('MovimientosStock', 'id_compra') IS NULL
                        ALTER TABLE MovimientosStock ADD id_compra int NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_MovimientosStock_Compra'
                          AND parent_object_id = OBJECT_ID('MovimientosStock')
                    )
                    BEGIN
                        ALTER TABLE MovimientosStock WITH CHECK
                        ADD CONSTRAINT FK_MovimientosStock_Compra FOREIGN KEY (id_compra)
                            REFERENCES Compras(id_compra);
                    END
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Compra CrearBorradorConDetalle(Compra compra, List<DetalleCompra> items)
        {
            // Batch atomico: INSERT cabecera + N INSERT detalle + UPDATE total. Sin tocar stock.
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                INSERT INTO Compras (id_proveedor, fecha, estado, total, id_usuario, observaciones)
                VALUES (@IdProveedor, @Fecha, @Estado, 0, @IdUsuario, @Observaciones);

                DECLARE @C int = CAST(SCOPE_IDENTITY() AS int);
            ");

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdProveedor", compra.IdProveedor));
            parametros.Add(new SqlParameter("@Fecha", compra.Fecha));
            parametros.Add(new SqlParameter("@Estado", compra.Estado));
            parametros.Add(new SqlParameter("@IdUsuario", compra.IdUsuario));
            parametros.Add(new SqlParameter("@Observaciones", (object)compra.Observaciones ?? DBNull.Value));

            for (int i = 0; i < items.Count; i++)
            {
                sb.Append(@"
                INSERT INTO CompraDetalle (id_compra, id_repuesto, cantidad, costo_unitario, subtotal)
                VALUES (@C, @Rep" + i + ", @Cant" + i + ", @Costo" + i + ", @Sub" + i + ");");

                parametros.Add(new SqlParameter("@Rep" + i, items[i].IdRepuesto));
                parametros.Add(new SqlParameter("@Cant" + i, items[i].Cantidad));
                parametros.Add(new SqlParameter("@Costo" + i, items[i].CostoUnitario));
                parametros.Add(new SqlParameter("@Sub" + i, items[i].Subtotal));
            }

            sb.Append(@"
                UPDATE Compras
                SET total = (SELECT ISNULL(SUM(subtotal), 0) FROM CompraDetalle WHERE id_compra = @C)
                WHERE id_compra = @C;

                SELECT @C;
            ");

            int id = _db.ExecuteTransaction(sb.ToString(), parametros.ToArray());

            return ObtenerPorId(id);
        }

        public void AgregarItem(int idCompra, DetalleCompra item)
        {
            // Batch atomico: exige Borrador, INSERT detalle + recalculo de total.
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Compras WHERE id_compra = @IdCompra)
                    THROW 50030, 'La compra seleccionada no existe.', 1;

                IF ((SELECT estado FROM Compras WHERE id_compra = @IdCompra) <> 'Borrador')
                    THROW 50031, 'Solo se puede modificar una compra en borrador.', 1;

                INSERT INTO CompraDetalle (id_compra, id_repuesto, cantidad, costo_unitario, subtotal)
                VALUES (@IdCompra, @IdRepuesto, @Cantidad, @Costo, @Subtotal);

                UPDATE Compras
                SET total = (SELECT ISNULL(SUM(subtotal), 0) FROM CompraDetalle WHERE id_compra = @IdCompra)
                WHERE id_compra = @IdCompra;

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCompra", idCompra),
                new SqlParameter("@IdRepuesto", item.IdRepuesto),
                new SqlParameter("@Cantidad", item.Cantidad),
                new SqlParameter("@Costo", item.CostoUnitario),
                new SqlParameter("@Subtotal", item.Subtotal)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void QuitarItem(int idDetalle)
        {
            // Batch atomico: exige Borrador, DELETE detalle + recalculo de total.
            string query = @"
                DECLARE @Compra int;

                SELECT @Compra = id_compra FROM CompraDetalle WHERE id_detalle = @IdDetalle;

                IF (@Compra IS NULL)
                    THROW 50032, 'El detalle seleccionado no existe.', 1;

                IF ((SELECT estado FROM Compras WHERE id_compra = @Compra) <> 'Borrador')
                    THROW 50031, 'Solo se puede modificar una compra en borrador.', 1;

                DELETE FROM CompraDetalle WHERE id_detalle = @IdDetalle;

                UPDATE Compras
                SET total = (SELECT ISNULL(SUM(subtotal), 0) FROM CompraDetalle WHERE id_compra = @Compra)
                WHERE id_compra = @Compra;

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdDetalle", idDetalle)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Compra ConfirmarConStock(int idCompra, int idUsuario)
        {
            // Batch atomico: Borrador->Confirmada + N x (check existe + stock+= + movimiento Compra
            // con id_compra) + costo_actual = ultimo costo del detalle + SELECT id. THROW si no-Borrador.
            // DECISION: costo_actual del repuesto = ultimo costo de compra del detalle (simple,
            // sin promedio ponderado; documentado en informe CP4).
            string query = @"
                DECLARE @Estado nvarchar(20);

                SELECT @Estado = estado FROM Compras WITH (UPDLOCK, HOLDLOCK) WHERE id_compra = @IdCompra;

                IF (@Estado IS NULL)
                    THROW 50030, 'La compra seleccionada no existe.', 1;

                IF (@Estado <> 'Borrador')
                    THROW 50031, 'Solo se puede confirmar una compra en borrador.', 1;

                IF NOT EXISTS (SELECT 1 FROM CompraDetalle WHERE id_compra = @IdCompra)
                    THROW 50033, 'La compra debe tener al menos un item para confirmarse.', 1;

                UPDATE Compras SET estado = 'Confirmada' WHERE id_compra = @IdCompra;

                DECLARE @IdDetalle int;
                DECLARE @IdRepuesto int;
                DECLARE @Cantidad int;
                DECLARE @Costo decimal(18,2);
                DECLARE @Stock int;

                DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
                    SELECT id_detalle, id_repuesto, cantidad, costo_unitario
                    FROM CompraDetalle WITH (UPDLOCK, HOLDLOCK)
                    WHERE id_compra = @IdCompra
                    ORDER BY id_detalle;

                OPEN cur;
                FETCH NEXT FROM cur INTO @IdDetalle, @IdRepuesto, @Cantidad, @Costo;

                WHILE (@@FETCH_STATUS = 0)
                BEGIN
                    SELECT @Stock = stock_actual
                    FROM Repuestos WITH (UPDLOCK, HOLDLOCK)
                    WHERE id_repuesto = @IdRepuesto;

                    IF (@Stock IS NULL)
                    BEGIN
                        CLOSE cur;
                        DEALLOCATE cur;
                        THROW 50034, 'El repuesto seleccionado no existe.', 1;
                    END

                    UPDATE Repuestos
                    SET stock_actual = @Stock + @Cantidad,
                        costo_actual = @Costo
                    WHERE id_repuesto = @IdRepuesto;

                    INSERT INTO MovimientosStock (id_repuesto, fecha, tipo, cantidad,
                        stock_anterior, stock_posterior, id_usuario, id_compra,
                        id_reparacion, observacion)
                    VALUES (@IdRepuesto, GETDATE(), 'Compra', @Cantidad,
                        @Stock, @Stock + @Cantidad, @IdUsuario, @IdCompra, NULL, 'Compra confirmada');

                    FETCH NEXT FROM cur INTO @IdDetalle, @IdRepuesto, @Cantidad, @Costo;
                END

                CLOSE cur;
                DEALLOCATE cur;

                SELECT @IdCompra;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCompra", idCompra),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void CancelarBorrador(int idCompra)
        {
            // Batch atomico: exige Borrador, UPDATE estado->Cancelada.
            // Conserva cabecera + detalle para trazabilidad (filtrable por Cancelada). Sin tocar stock.
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM Compras WHERE id_compra = @IdCompra)
                    THROW 50030, 'La compra seleccionada no existe.', 1;

                IF ((SELECT estado FROM Compras WHERE id_compra = @IdCompra) <> 'Borrador')
                    THROW 50031, 'Solo se puede cancelar una compra en borrador.', 1;

                UPDATE Compras SET estado = 'Cancelada' WHERE id_compra = @IdCompra;

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCompra", idCompra)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void AnularConfirmadaConStock(int idCompra, string motivo, int idUsuario)
        {
            // Batch atomico: exige Confirmada + motivo no vacio; si el stock actual cubre
            // la reversion completa por repuesto (stock >= SUMA de cantidades del repuesto,
            // agregando renglones repetidos): UPDATE estado->Cancelada + motivo + por item
            // UPDATE stock-= + INSERT AjusteNegativo con obs 'Anulacion compra N'.
            // Si no cubre -> THROW 50035 sin cambios.
            string query = @"
                DECLARE @Estado nvarchar(20);

                SELECT @Estado = estado FROM Compras WITH (UPDLOCK, HOLDLOCK) WHERE id_compra = @IdCompra;

                IF (@Estado IS NULL)
                    THROW 50030, 'La compra seleccionada no existe.', 1;

                IF (@Estado <> 'Confirmada')
                    THROW 50031, 'Solo se puede anular una compra confirmada.', 1;

                IF (@Motivo IS NULL OR LTRIM(RTRIM(@Motivo)) = '')
                    THROW 50036, 'El motivo de la anulacion es obligatorio.', 1;

                IF NOT EXISTS (SELECT 1 FROM CompraDetalle WHERE id_compra = @IdCompra)
                    THROW 50033, 'La compra debe tener al menos un item para anularse.', 1;

                IF EXISTS (
                    SELECT 1
                    FROM (
                        SELECT id_repuesto, SUM(cantidad) AS total
                        FROM CompraDetalle
                        WHERE id_compra = @IdCompra
                        GROUP BY id_repuesto
                    ) d
                    INNER JOIN Repuestos r WITH (UPDLOCK, HOLDLOCK) ON r.id_repuesto = d.id_repuesto
                    WHERE r.stock_actual < d.total
                )
                    THROW 50035, 'No se puede anular la compra porque el stock actual no cubre la reversion. Los repuestos ya fueron consumidos parcial o totalmente.', 1;

                UPDATE Compras
                SET estado = 'Cancelada',
                    motivo_anulacion = @Motivo
                WHERE id_compra = @IdCompra;

                DECLARE @IdRepuesto int;
                DECLARE @Cantidad int;
                DECLARE @Stock int;

                DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
                    SELECT id_repuesto, cantidad
                    FROM CompraDetalle WITH (UPDLOCK, HOLDLOCK)
                    WHERE id_compra = @IdCompra
                    ORDER BY id_detalle;

                OPEN cur;
                FETCH NEXT FROM cur INTO @IdRepuesto, @Cantidad;

                WHILE (@@FETCH_STATUS = 0)
                BEGIN
                    SELECT @Stock = stock_actual
                    FROM Repuestos WITH (UPDLOCK, HOLDLOCK)
                    WHERE id_repuesto = @IdRepuesto;

                    IF (@Stock IS NULL)
                    BEGIN
                        CLOSE cur;
                        DEALLOCATE cur;
                        THROW 50034, 'El repuesto seleccionado no existe.', 1;
                    END

                    UPDATE Repuestos
                    SET stock_actual = @Stock - @Cantidad
                    WHERE id_repuesto = @IdRepuesto;

                    INSERT INTO MovimientosStock (id_repuesto, fecha, tipo, cantidad,
                        stock_anterior, stock_posterior, id_usuario, id_compra,
                        id_reparacion, observacion)
                    VALUES (@IdRepuesto, GETDATE(), 'AjusteNegativo', @Cantidad,
                        @Stock, @Stock - @Cantidad, @IdUsuario, @IdCompra, NULL,
                        'Anulacion compra ' + CAST(@IdCompra AS nvarchar(20)));

                    FETCH NEXT FROM cur INTO @IdRepuesto, @Cantidad;
                END

                CLOSE cur;
                DEALLOCATE cur;

                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCompra", idCompra),
                new SqlParameter("@Motivo", motivo),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Compra ObtenerPorId(int id)
        {
            // motivo_anulacion garantizado por Inicializar (misma idea que Presupuestos);
            // Mapear usa Columns.Contains como defensa adicional.
            string query = @"
                SELECT id_compra, id_proveedor, fecha, estado, total, id_usuario, observaciones, motivo_anulacion
                FROM Compras WHERE id_compra = @Id;
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

        public List<Compra> Listar(bool incluirNoBorrador = true)
        {
            string query = @"
                SELECT id_compra, id_proveedor, fecha, estado, total, id_usuario, observaciones, motivo_anulacion
                FROM Compras
                WHERE (@Todas = 1 OR estado = 'Borrador')
                ORDER BY id_compra;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Todas", incluirNoBorrador ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Compra> compras = new List<Compra>();

            foreach (DataRow fila in dt.Rows)
                compras.Add(Mapear(fila));

            return compras;
        }

        public List<DetalleCompra> ListarDetalle(int idCompra)
        {
            string query = @"
                SELECT id_detalle, id_compra, id_repuesto, cantidad, costo_unitario, subtotal
                FROM CompraDetalle
                WHERE id_compra = @IdCompra
                ORDER BY id_detalle;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdCompra", idCompra)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<DetalleCompra> items = new List<DetalleCompra>();

            foreach (DataRow fila in dt.Rows)
            {
                items.Add(DetalleCompra.CargarDesdeDB(
                    Convert.ToInt32(fila["id_detalle"]),
                    Convert.ToInt32(fila["id_compra"]),
                    Convert.ToInt32(fila["id_repuesto"]),
                    Convert.ToInt32(fila["cantidad"]),
                    Convert.ToDecimal(fila["costo_unitario"]),
                    Convert.ToDecimal(fila["subtotal"])
                ));
            }

            return items;
        }

        private Compra Mapear(DataRow fila)
        {
            return Compra.CargarDesdeDB(
                Convert.ToInt32(fila["id_compra"]),
                Convert.ToInt32(fila["id_proveedor"]),
                Convert.ToDateTime(fila["fecha"]),
                fila["estado"] == DBNull.Value ? null : fila["estado"].ToString(),
                Convert.ToDecimal(fila["total"]),
                Convert.ToInt32(fila["id_usuario"]),
                fila["observaciones"] == DBNull.Value ? "" : fila["observaciones"].ToString(),
                fila.Table.Columns.Contains("motivo_anulacion") && fila["motivo_anulacion"] != DBNull.Value ? fila["motivo_anulacion"].ToString() : null
            );
        }
    }
}
