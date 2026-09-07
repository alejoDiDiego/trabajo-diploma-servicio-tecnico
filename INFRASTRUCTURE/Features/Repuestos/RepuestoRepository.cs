using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Repuestos;

namespace REPOSITORY.Features.Repuestos
{
    public class RepuestoRepository
    {
        private readonly SqlHelper _db;

        public RepuestoRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public RepuestoRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla Repuestos de forma idempotente (IF OBJECT_ID + ALTER defensivos).
            string query = @"
                IF OBJECT_ID('Repuestos', 'U') IS NULL
                BEGIN
                    CREATE TABLE Repuestos (
                        id_repuesto int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        codigo nvarchar(50) NOT NULL,
                        descripcion nvarchar(200) NOT NULL,
                        stock_actual int NOT NULL CONSTRAINT DF_Repuestos_StockActual DEFAULT 0,
                        stock_minimo int NOT NULL CONSTRAINT DF_Repuestos_StockMinimo DEFAULT 0,
                        costo_actual decimal(18,2) NOT NULL CONSTRAINT DF_Repuestos_CostoActual DEFAULT 0,
                        precio_referencia decimal(18,2) NOT NULL CONSTRAINT DF_Repuestos_PrecioReferencia DEFAULT 0,
                        activo bit NOT NULL CONSTRAINT DF_Repuestos_Activo DEFAULT 1
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('Repuestos', 'codigo') IS NULL
                        ALTER TABLE Repuestos ADD codigo nvarchar(50) NOT NULL CONSTRAINT DF_Repuestos_Codigo DEFAULT '';

                    IF COL_LENGTH('Repuestos', 'descripcion') IS NULL
                        ALTER TABLE Repuestos ADD descripcion nvarchar(200) NOT NULL CONSTRAINT DF_Repuestos_Descripcion DEFAULT '';

                    IF COL_LENGTH('Repuestos', 'stock_actual') IS NULL
                        ALTER TABLE Repuestos ADD stock_actual int NOT NULL CONSTRAINT DF_Repuestos_StockActual DEFAULT 0;

                    IF COL_LENGTH('Repuestos', 'stock_minimo') IS NULL
                        ALTER TABLE Repuestos ADD stock_minimo int NOT NULL CONSTRAINT DF_Repuestos_StockMinimo DEFAULT 0;

                    IF COL_LENGTH('Repuestos', 'costo_actual') IS NULL
                        ALTER TABLE Repuestos ADD costo_actual decimal(18,2) NOT NULL CONSTRAINT DF_Repuestos_CostoActual DEFAULT 0;

                    IF COL_LENGTH('Repuestos', 'precio_referencia') IS NULL
                        ALTER TABLE Repuestos ADD precio_referencia decimal(18,2) NOT NULL CONSTRAINT DF_Repuestos_PrecioReferencia DEFAULT 0;

                    IF COL_LENGTH('Repuestos', 'activo') IS NULL
                        ALTER TABLE Repuestos ADD activo bit NOT NULL CONSTRAINT DF_Repuestos_Activo DEFAULT 1;
                END

                IF NOT EXISTS (
                    SELECT 1 FROM sys.indexes
                    WHERE name = 'UX_Repuestos_Codigo'
                      AND object_id = OBJECT_ID('Repuestos')
                )
                BEGIN
                    CREATE UNIQUE INDEX UX_Repuestos_Codigo
                    ON Repuestos(codigo);
                END

                -- CP4: infraestructura id_repuesto de PresupuestoDetalle -> Repuestos
                -- (guarda por orden de init: PresupuestoDetalle puede no existir aun).
                -- Solo infraestructura: Emitir sigue validando solo ManoObra/Servicio.
                IF OBJECT_ID('PresupuestoDetalle', 'U') IS NOT NULL
                    AND COL_LENGTH('PresupuestoDetalle', 'id_repuesto') IS NOT NULL
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_PresupuestoDetalle_Repuesto'
                          AND parent_object_id = OBJECT_ID('PresupuestoDetalle')
                    )
                    BEGIN
                        ALTER TABLE PresupuestoDetalle WITH CHECK
                        ADD CONSTRAINT FK_PresupuestoDetalle_Repuesto FOREIGN KEY (id_repuesto)
                            REFERENCES Repuestos(id_repuesto);
                    END
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public Repuesto Crear(Repuesto repuesto, int idUsuario)
        {
            // Batch atomico: INSERT repuesto + INSERT movimiento de stock inicial (si aplica).
            string query = @"
                INSERT INTO Repuestos (codigo, descripcion, stock_actual, stock_minimo,
                    costo_actual, precio_referencia, activo)
                VALUES (@Codigo, @Descripcion, @StockActual, @StockMinimo,
                    @CostoActual, @PrecioReferencia, 1);

                DECLARE @Id int = CAST(SCOPE_IDENTITY() AS int);

                IF (@StockActual > 0)
                BEGIN
                    INSERT INTO MovimientosStock (id_repuesto, fecha, tipo, cantidad,
                        stock_anterior, stock_posterior, id_usuario, id_compra,
                        id_reparacion, observacion)
                    VALUES (@Id, GETDATE(), 'AjustePositivo', @StockActual,
                        0, @StockActual, @IdUsuario, NULL, NULL, 'Stock inicial');
                END

                SELECT @Id;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Codigo", repuesto.Codigo),
                new SqlParameter("@Descripcion", repuesto.Descripcion),
                new SqlParameter("@StockActual", repuesto.StockActual),
                new SqlParameter("@StockMinimo", repuesto.StockMinimo),
                new SqlParameter("@CostoActual", repuesto.CostoActual),
                new SqlParameter("@PrecioReferencia", repuesto.PrecioReferencia),
                new SqlParameter("@IdUsuario", idUsuario)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public void Modificar(Repuesto repuesto)
        {
            // El stock NO se modifica por aqui; solo datos maestros.
            string query = @"
                UPDATE Repuestos
                SET codigo = @Codigo,
                    descripcion = @Descripcion,
                    stock_minimo = @StockMinimo,
                    costo_actual = @CostoActual,
                    precio_referencia = @PrecioReferencia
                WHERE id_repuesto = @Id;
                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", repuesto.Id),
                new SqlParameter("@Codigo", repuesto.Codigo),
                new SqlParameter("@Descripcion", repuesto.Descripcion),
                new SqlParameter("@StockMinimo", repuesto.StockMinimo),
                new SqlParameter("@CostoActual", repuesto.CostoActual),
                new SqlParameter("@PrecioReferencia", repuesto.PrecioReferencia)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void AjustarStock(int idRepuesto, int cantidadFirmada, string motivo, int idUsuario)
        {
            // Batch atomico: UPDATE stock + INSERT movimiento de ajuste.
            string query = @"
                DECLARE @Ant int;

                SELECT @Ant = stock_actual
                FROM Repuestos WITH (UPDLOCK, HOLDLOCK)
                WHERE id_repuesto = @Id;

                IF (@Ant IS NULL)
                    THROW 50001, 'El repuesto seleccionado no existe.', 1;

                DECLARE @Post int = @Ant + @Delta;

                IF (@Post < 0)
                    THROW 50002, 'Stock insuficiente para el ajuste.', 1;

                UPDATE Repuestos
                SET stock_actual = @Post
                WHERE id_repuesto = @Id;

                INSERT INTO MovimientosStock (id_repuesto, fecha, tipo, cantidad,
                    stock_anterior, stock_posterior, id_usuario, id_compra,
                    id_reparacion, observacion)
                VALUES (@Id, GETDATE(), @Tipo, @CantidadAbs,
                    @Ant, @Post, @IdUsuario, NULL, NULL, @Motivo);

                SELECT 0;
            ";

            string tipo = cantidadFirmada > 0
                ? TipoMovimientoStock.AjustePositivo
                : TipoMovimientoStock.AjusteNegativo;

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", idRepuesto),
                new SqlParameter("@Delta", cantidadFirmada),
                new SqlParameter("@Tipo", tipo),
                new SqlParameter("@CantidadAbs", Math.Abs(cantidadFirmada)),
                new SqlParameter("@IdUsuario", idUsuario),
                new SqlParameter("@Motivo", motivo)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public void Desactivar(int id)
        {
            string query = @"
                UPDATE Repuestos SET activo = 0 WHERE id_repuesto = @Id;
                SELECT 0;
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
                UPDATE Repuestos SET activo = 1 WHERE id_repuesto = @Id;
                SELECT 0;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Id", id)
            };

            _db.ExecuteTransaction(query, sqlParameters);
        }

        public Repuesto ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_repuesto, codigo, descripcion, stock_actual, stock_minimo,
                       costo_actual, precio_referencia, activo
                FROM Repuestos WHERE id_repuesto = @Id;
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

        public Repuesto ObtenerPorCodigo(string codigo)
        {
            string query = @"
                SELECT id_repuesto, codigo, descripcion, stock_actual, stock_minimo,
                       costo_actual, precio_referencia, activo
                FROM Repuestos WHERE codigo = @Codigo;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Codigo", codigo)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);

            if (dt.Rows.Count <= 0)
                return null;

            return Mapear(dt.Rows[0]);
        }

        public List<Repuesto> Listar(bool incluirInactivos = false)
        {
            string query = @"
                SELECT id_repuesto, codigo, descripcion, stock_actual, stock_minimo,
                       costo_actual, precio_referencia, activo
                FROM Repuestos
                WHERE (@IncluirInactivos = 1 OR activo = 1)
                ORDER BY codigo;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IncluirInactivos", incluirInactivos ? 1 : 0)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<Repuesto> repuestos = new List<Repuesto>();

            foreach (DataRow fila in dt.Rows)
                repuestos.Add(Mapear(fila));

            return repuestos;
        }

        private Repuesto Mapear(DataRow fila)
        {
            return Repuesto.CargarDesdeDB(
                Convert.ToInt32(fila["id_repuesto"]),
                fila["codigo"] == DBNull.Value ? "" : fila["codigo"].ToString(),
                fila["descripcion"] == DBNull.Value ? "" : fila["descripcion"].ToString(),
                Convert.ToInt32(fila["stock_actual"]),
                Convert.ToInt32(fila["stock_minimo"]),
                Convert.ToDecimal(fila["costo_actual"]),
                Convert.ToDecimal(fila["precio_referencia"]),
                Convert.ToBoolean(fila["activo"])
            );
        }
    }
}
