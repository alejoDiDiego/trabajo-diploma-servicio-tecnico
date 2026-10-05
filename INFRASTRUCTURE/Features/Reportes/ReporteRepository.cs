using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace REPOSITORY.Features.Reportes
{
    // Solo lectura para dashboard + reportes (spec §17.2). Sin escrituras,
    // sin transacciones y sin objetos nuevos en la DB: solo SELECTs.
    // Por eso no hay Inicializar(): no hay nada que crear.
    public class ReporteRepository
    {
        private readonly SqlHelper _db;

        public ReporteRepository()
            : this(ConfigurationManager.ConnectionStrings["UrlDB"].ConnectionString)
        {
        }

        public ReporteRepository(string cadenaConexion)
        {
            _db = new SqlHelper(cadenaConexion);
        }

        // 1. Ordenes abiertas agrupadas por estado (excluye Entregado).
        public DataTable ContarOrdenesPorEstado(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT estado, COUNT(1) AS cantidad
                FROM OrdenesServicio
                WHERE estado <> 'Entregado'
                  AND (@Desde IS NULL OR fecha_ingreso >= @Desde)
                  AND (@Hasta IS NULL OR fecha_ingreso < DATEADD(day, 1, @Hasta))
                GROUP BY estado
                ORDER BY estado;
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 2. Resultado final de las ordenes listas para retiro (solo con valor).
        public DataTable ContarOrdenesPorResultado(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT resultado, COUNT(1) AS cantidad
                FROM OrdenesServicio
                WHERE estado = 'ListoRetiro'
                  AND resultado IS NOT NULL
                  AND LTRIM(RTRIM(resultado)) <> ''
                  AND (@Desde IS NULL OR fecha_ingreso >= @Desde)
                  AND (@Hasta IS NULL OR fecha_ingreso < DATEADD(day, 1, @Hasta))
                GROUP BY resultado
                ORDER BY resultado;
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 3. Intervenciones de reparacion por tecnico (username o 'Sin asignar').
        public DataTable ReparacionesPorTecnico(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT COALESCE(u.username, 'Sin asignar') AS tecnico, COUNT(1) AS cantidad
                FROM Reparaciones r
                LEFT JOIN Usuarios u ON u.id_usuario = r.id_usuario_tecnico
                WHERE (@Desde IS NULL OR r.fecha_inicio >= @Desde)
                  AND (@Hasta IS NULL OR r.fecha_inicio < DATEADD(day, 1, @Hasta))
                GROUP BY COALESCE(u.username, 'Sin asignar')
                ORDER BY cantidad DESC, tecnico;
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 4. Promedio en dias entre ingreso y entrega (solo Entregado). 0 si vacio.
        // El CAST a float es obligatorio: AVG(int) trunca a entero en SQL Server.
        public DataTable TiempoPromedioIngresoEntrega(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT ISNULL(AVG(CAST(DATEDIFF(day, o.fecha_ingreso, e.fecha_entrega) AS float)), 0) AS promedio_dias
                FROM OrdenesServicio o
                INNER JOIN Entregas e ON e.id_orden = o.id_orden
                WHERE o.estado = 'Entregado'
                  AND (@Desde IS NULL OR o.fecha_ingreso >= @Desde)
                  AND (@Hasta IS NULL OR o.fecha_ingreso < DATEADD(day, 1, @Hasta));
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 5. Conteos de presupuestos por estado (excluye Borrador/Anulado).
        // La tasa se calcula en ReporteService (C#) con division-por-cero protegida.
        public DataTable ContarPresupuestosPorEstado(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT estado, COUNT(1) AS cantidad
                FROM Presupuestos
                WHERE estado IN ('Aprobado', 'Rechazado', 'Pendiente')
                  AND (@Desde IS NULL OR fecha_emision >= @Desde)
                  AND (@Hasta IS NULL OR fecha_emision < DATEADD(day, 1, @Hasta))
                GROUP BY estado
                ORDER BY estado;
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 6. Repuestos mas consumidos en reparaciones (TOP N por cantidad total).
        public DataTable RepuestosMasUtilizados(int top, DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT TOP (@Top) r.codigo AS codigo, r.descripcion AS descripcion,
                       SUM(rr.cantidad) AS cantidad
                FROM ReparacionRepuesto rr
                INNER JOIN Reparaciones rep ON rep.id_reparacion = rr.id_reparacion
                INNER JOIN Repuestos r ON r.id_repuesto = rr.id_repuesto
                WHERE (@Desde IS NULL OR rep.fecha_inicio >= @Desde)
                  AND (@Hasta IS NULL OR rep.fecha_inicio < DATEADD(day, 1, @Hasta))
                GROUP BY r.codigo, r.descripcion
                ORDER BY SUM(rr.cantidad) DESC, r.codigo;
            ";

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@Top", top),
                new SqlParameter("@Desde", desde.HasValue ? (object)desde.Value : DBNull.Value),
                new SqlParameter("@Hasta", hasta.HasValue ? (object)hasta.Value : DBNull.Value)
            };

            return _db.ExecuteQuery(query, sqlParameters);
        }

        // 7. Compras confirmadas por proveedor (cantidad de compras + monto total).
        public DataTable ComprasPorProveedor(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT p.razon_social AS proveedor, COUNT(1) AS cantidad,
                       ISNULL(SUM(c.total), 0) AS monto
                FROM Compras c
                INNER JOIN Proveedores p ON p.id_proveedor = c.id_proveedor
                WHERE c.estado = 'Confirmada'
                  AND (@Desde IS NULL OR c.fecha >= @Desde)
                  AND (@Hasta IS NULL OR c.fecha < DATEADD(day, 1, @Hasta))
                GROUP BY p.razon_social
                ORDER BY p.razon_social;
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 8a. Cantidad de reingresos en garantia (ordenes Tipo=Garantia). 1 fila.
        public DataTable ContarReingresosGarantia(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT COUNT(1) AS cantidad
                FROM OrdenesServicio
                WHERE tipo_orden = 'Garantia'
                  AND (@Desde IS NULL OR fecha_ingreso >= @Desde)
                  AND (@Hasta IS NULL OR fecha_ingreso < DATEADD(day, 1, @Hasta));
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 8b. Evaluaciones de garantia agrupadas por estado.
        // La tasa de aceptacion se calcula en ReporteService (C#).
        public DataTable EvaluacionesPorEstado(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT estado, COUNT(1) AS cantidad
                FROM EvaluacionesGarantia
                WHERE (@Desde IS NULL OR fecha >= @Desde)
                  AND (@Hasta IS NULL OR fecha < DATEADD(day, 1, @Hasta))
                GROUP BY estado
                ORDER BY estado;
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 9. Monto total de presupuestos aprobados (global: todos los tipos).
        // Se dice "monto de presupuestos aprobados", nunca facturacion/ingresos/ventas.
        public DataTable MontoPresupuestosAprobados(DateTime? desde, DateTime? hasta)
        {
            string query = @"
                SELECT ISNULL(SUM(total), 0) AS monto
                FROM Presupuestos
                WHERE estado = 'Aprobado'
                  AND (@Desde IS NULL OR fecha_emision >= @Desde)
                  AND (@Hasta IS NULL OR fecha_emision < DATEADD(day, 1, @Hasta));
            ";

            return _db.ExecuteQuery(query, CrearParametrosRango(desde, hasta));
        }

        // 10. Repuestos bajo minimo: SIN query nueva. El service reutiliza
        // RepuestoRepository.Listar() y filtra en C# (stock_actual <= stock_minimo).

        // 11. Resumen del dashboard: 6 KPIs en 1 sola query (1 roundtrip, 1 fila).
        // en_reparacion = EnReparacion + EnPruebas (trabajo en curso real, no Autorizado).
        // garantias_abiertas = reingresos Tipo=Garantia aun no entregados.
        // bajo_minimo = activos con stock_actual <= stock_minimo (igual que la UI).
        public DataTable DashboardResumen()
        {
            string query = @"
                SELECT
                    (SELECT COUNT(1) FROM OrdenesServicio WHERE estado <> 'Entregado') AS abiertas_total,
                    (SELECT COUNT(1) FROM OrdenesServicio WHERE estado = 'EsperandoRespuesta') AS esperando_respuesta,
                    (SELECT COUNT(1) FROM OrdenesServicio WHERE estado IN ('EnReparacion', 'EnPruebas')) AS en_reparacion,
                    (SELECT COUNT(1) FROM OrdenesServicio WHERE estado = 'ListoRetiro') AS listas_retiro,
                    (SELECT COUNT(1) FROM OrdenesServicio WHERE tipo_orden = 'Garantia' AND estado <> 'Entregado') AS garantias_abiertas,
                    (SELECT COUNT(1) FROM Repuestos WHERE activo = 1 AND stock_actual <= stock_minimo) AS bajo_minimo;
            ";

            return _db.ExecuteQuery(query);
        }

        private SqlParameter[] CrearParametrosRango(DateTime? desde, DateTime? hasta)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@Desde", desde.HasValue ? (object)desde.Value : DBNull.Value),
                new SqlParameter("@Hasta", hasta.HasValue ? (object)hasta.Value : DBNull.Value)
            };
        }
    }
}
