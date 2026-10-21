using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DOMAIN.Features.Repuestos;

namespace REPOSITORY.Features.Repuestos
{
    public class MovimientoStockRepository
    {
        private readonly SqlHelper _db;

        public MovimientoStockRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public MovimientoStockRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        public void Inicializar()
        {
            // Crea la tabla MovimientosStock de forma idempotente.
            // id_compra e id_reparacion quedan NULL sin FK (reservados para CP4 y reparaciones).
            // La FK de id_reparacion hacia Reparaciones se agrega en ReparacionRepository
            // con guarda para evitar dependencia de orden de inicializacion.
            string query = @"
                IF OBJECT_ID('MovimientosStock', 'U') IS NULL
                BEGIN
                    CREATE TABLE MovimientosStock (
                        id_movimiento int IDENTITY(1,1) NOT NULL PRIMARY KEY,
                        id_repuesto int NOT NULL,
                        fecha datetime NOT NULL CONSTRAINT DF_MovimientosStock_Fecha DEFAULT GETDATE(),
                        tipo nvarchar(30) NOT NULL,
                        cantidad int NOT NULL,
                        stock_anterior int NOT NULL,
                        stock_posterior int NOT NULL,
                        id_usuario int NOT NULL,
                        id_compra int NULL,
                        id_reparacion int NULL,
                        observacion nvarchar(max) NULL,
                        CONSTRAINT FK_MovimientosStock_Repuesto FOREIGN KEY (id_repuesto)
                            REFERENCES Repuestos(id_repuesto),
                        CONSTRAINT FK_MovimientosStock_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario)
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('MovimientosStock', 'id_repuesto') IS NULL
                        ALTER TABLE MovimientosStock ADD id_repuesto int NOT NULL CONSTRAINT DF_MovimientosStock_IdRepuesto DEFAULT 0;

                    IF COL_LENGTH('MovimientosStock', 'fecha') IS NULL
                        ALTER TABLE MovimientosStock ADD fecha datetime NOT NULL CONSTRAINT DF_MovimientosStock_Fecha DEFAULT GETDATE();

                    IF COL_LENGTH('MovimientosStock', 'tipo') IS NULL
                        ALTER TABLE MovimientosStock ADD tipo nvarchar(30) NOT NULL CONSTRAINT DF_MovimientosStock_Tipo DEFAULT '';

                    IF COL_LENGTH('MovimientosStock', 'cantidad') IS NULL
                        ALTER TABLE MovimientosStock ADD cantidad int NOT NULL CONSTRAINT DF_MovimientosStock_Cantidad DEFAULT 0;

                    IF COL_LENGTH('MovimientosStock', 'stock_anterior') IS NULL
                        ALTER TABLE MovimientosStock ADD stock_anterior int NOT NULL CONSTRAINT DF_MovimientosStock_StockAnterior DEFAULT 0;

                    IF COL_LENGTH('MovimientosStock', 'stock_posterior') IS NULL
                        ALTER TABLE MovimientosStock ADD stock_posterior int NOT NULL CONSTRAINT DF_MovimientosStock_StockPosterior DEFAULT 0;

                    IF COL_LENGTH('MovimientosStock', 'id_usuario') IS NULL
                        ALTER TABLE MovimientosStock ADD id_usuario int NOT NULL CONSTRAINT DF_MovimientosStock_IdUsuario DEFAULT 0;

                    IF COL_LENGTH('MovimientosStock', 'id_compra') IS NULL
                        ALTER TABLE MovimientosStock ADD id_compra int NULL;

                    IF COL_LENGTH('MovimientosStock', 'id_reparacion') IS NULL
                        ALTER TABLE MovimientosStock ADD id_reparacion int NULL;

                    IF COL_LENGTH('MovimientosStock', 'observacion') IS NULL
                        ALTER TABLE MovimientosStock ADD observacion nvarchar(max) NULL;

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_MovimientosStock_Repuesto'
                          AND parent_object_id = OBJECT_ID('MovimientosStock')
                    )
                    BEGIN
                        ALTER TABLE MovimientosStock WITH CHECK
                        ADD CONSTRAINT FK_MovimientosStock_Repuesto FOREIGN KEY (id_repuesto)
                            REFERENCES Repuestos(id_repuesto);
                    END

                    IF NOT EXISTS (
                        SELECT 1 FROM sys.foreign_keys
                        WHERE name = 'FK_MovimientosStock_Usuario'
                          AND parent_object_id = OBJECT_ID('MovimientosStock')
                    )
                    BEGIN
                        ALTER TABLE MovimientosStock WITH CHECK
                        ADD CONSTRAINT FK_MovimientosStock_Usuario FOREIGN KEY (id_usuario)
                            REFERENCES Usuarios(id_usuario);
                    END
                END

                SELECT 0;
            ";

            _db.ExecuteTransaction(query);
        }

        public MovimientoStock Registrar(MovimientoStock movimiento)
        {
            string query = @"
                INSERT INTO MovimientosStock (id_repuesto, fecha, tipo, cantidad,
                    stock_anterior, stock_posterior, id_usuario, id_compra,
                    id_reparacion, observacion)
                VALUES (@IdRepuesto, @Fecha, @Tipo, @Cantidad,
                    @StockAnterior, @StockPosterior, @IdUsuario, @IdCompra,
                    @IdReparacion, @Observacion);
                SELECT CAST(SCOPE_IDENTITY() AS int);
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdRepuesto", movimiento.IdRepuesto),
                new SqlParameter("@Fecha", movimiento.Fecha),
                new SqlParameter("@Tipo", movimiento.Tipo),
                new SqlParameter("@Cantidad", movimiento.Cantidad),
                new SqlParameter("@StockAnterior", movimiento.StockAnterior),
                new SqlParameter("@StockPosterior", movimiento.StockPosterior),
                new SqlParameter("@IdUsuario", movimiento.IdUsuario),
                new SqlParameter("@IdCompra", movimiento.IdCompra.HasValue ? (object)movimiento.IdCompra.Value : DBNull.Value),
                new SqlParameter("@IdReparacion", movimiento.IdReparacion.HasValue ? (object)movimiento.IdReparacion.Value : DBNull.Value),
                new SqlParameter("@Observacion", (object)movimiento.Observacion ?? DBNull.Value)
            };

            int id = _db.ExecuteTransaction(query, sqlParameters);

            return ObtenerPorId(id);
        }

        public MovimientoStock ObtenerPorId(int id)
        {
            string query = @"
                SELECT id_movimiento, id_repuesto, fecha, tipo, cantidad,
                       stock_anterior, stock_posterior, id_usuario, id_compra,
                       id_reparacion, observacion
                FROM MovimientosStock WHERE id_movimiento = @Id;
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

        public List<MovimientoStock> ListarPorRepuesto(int idRepuesto)
        {
            return ListarFiltros(idRepuesto, null, null, null);
        }

        public List<MovimientoStock> ListarFiltros(int? idRepuesto, string tipo,
            DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT id_movimiento, id_repuesto, fecha, tipo, cantidad,
                       stock_anterior, stock_posterior, id_usuario, id_compra,
                       id_reparacion, observacion
                FROM MovimientosStock
                WHERE (@IdRepuesto IS NULL OR id_repuesto = @IdRepuesto)
                  AND (@Tipo IS NULL OR tipo = @Tipo)
                  AND (@Desde IS NULL OR fecha >= @Desde)
                  AND (@Hasta IS NULL OR fecha <= @Hasta)
                ORDER BY fecha, id_movimiento;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@IdRepuesto", idRepuesto.HasValue ? (object)idRepuesto.Value : DBNull.Value),
                new SqlParameter("@Tipo", (object)tipo ?? DBNull.Value),
                new SqlParameter("@Desde", desde.HasValue ? (object)desde.Value : DBNull.Value),
                new SqlParameter("@Hasta", hasta.HasValue ? (object)hasta.Value : DBNull.Value)
            };

            DataTable dt = _db.ExecuteQuery(query, sqlParameters);
            List<MovimientoStock> movimientos = new List<MovimientoStock>();

            foreach (DataRow fila in dt.Rows)
                movimientos.Add(Mapear(fila));

            return movimientos;
        }

        private MovimientoStock Mapear(DataRow fila)
        {
            return MovimientoStock.CargarDesdeDB(
                Convert.ToInt32(fila["id_movimiento"]),
                Convert.ToInt32(fila["id_repuesto"]),
                Convert.ToDateTime(fila["fecha"]),
                fila["tipo"] == DBNull.Value ? "" : fila["tipo"].ToString(),
                Convert.ToInt32(fila["cantidad"]),
                Convert.ToInt32(fila["stock_anterior"]),
                Convert.ToInt32(fila["stock_posterior"]),
                Convert.ToInt32(fila["id_usuario"]),
                fila["id_compra"] == DBNull.Value ? (int?)null : Convert.ToInt32(fila["id_compra"]),
                fila["id_reparacion"] == DBNull.Value ? (int?)null : Convert.ToInt32(fila["id_reparacion"]),
                fila["observacion"] == DBNull.Value ? "" : fila["observacion"].ToString()
            );
        }
    }
}
