using System;
using System.Collections.Generic;
using System.Data;
using DOMAIN.Exceptions;
using REPOSITORY.Features.Reportes;
using REPOSITORY.Features.Repuestos;

namespace APPLICATION.Features.Reportes
{
    // Backend solo-lectura para dashboard + reportes. Delega las consultas al
    // repositorio (DataTable, lo que devuelve ExecuteQuery) y calcula tasas y
    // promedios en C# con division-por-cero protegida. Sin escrituras: no hay
    // eventos de bitacora que registrar.
    public class ReporteService
    {
        private readonly ReporteRepository _reporteRepository;
        private readonly RepuestoRepository _repuestoRepository;

        public ReporteService()
        {
            _reporteRepository = new ReporteRepository();
            _repuestoRepository = new RepuestoRepository();
        }

        public DataTable ContarOrdenesPorEstado(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                return _reporteRepository.ContarOrdenesPorEstado(desde, hasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar ordenes por estado", ex);
            }
        }

        public DataTable ContarOrdenesPorResultado(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                return _reporteRepository.ContarOrdenesPorResultado(desde, hasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar ordenes por resultado", ex);
            }
        }

        public DataTable ReparacionesPorTecnico(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                return _reporteRepository.ReparacionesPorTecnico(desde, hasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar reparaciones por tecnico", ex);
            }
        }

        // Promedio en dias entre ingreso y entrega. 0 si no hay entregadas.
        public decimal TiempoPromedioIngresoEntrega(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                DataTable dt = _reporteRepository.TiempoPromedioIngresoEntrega(desde, hasta);

                if (dt.Rows.Count <= 0 || dt.Rows[0]["promedio_dias"] == DBNull.Value)
                    return 0;

                return Convert.ToDecimal(dt.Rows[0]["promedio_dias"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular el tiempo promedio de ingreso a entrega", ex);
            }
        }

        // Conteos por estado (Aprobado/Rechazado/Pendiente; sin Borrador/Anulado).
        public DataTable ContarPresupuestosPorEstado(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                return _reporteRepository.ContarPresupuestosPorEstado(desde, hasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar presupuestos por estado", ex);
            }
        }

        // Tasa de aprobacion = aprobados / (aprobados + rechazados). 0 si no hay decididos.
        // Los pendientes se informan pero no entran en la tasa.
        public decimal TasaAprobacionPresupuestos(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                DataTable dt = _reporteRepository.ContarPresupuestosPorEstado(desde, hasta);

                int aprobados = LeerCantidad(dt, "Aprobado");
                int rechazados = LeerCantidad(dt, "Rechazado");
                int decididos = aprobados + rechazados;

                if (decididos <= 0)
                    return 0;

                return (decimal)aprobados / decididos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular la tasa de aprobacion de presupuestos", ex);
            }
        }

        public DataTable RepuestosMasUtilizados(int top, DateTime? desde, DateTime? hasta)
        {
            try
            {
                if (top <= 0)
                    throw new ReglaNegocioException("El top de repuestos debe ser mayor a cero.");

                ValidarRango(desde, hasta);
                return _reporteRepository.RepuestosMasUtilizados(top, desde, hasta);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar repuestos mas utilizados", ex);
            }
        }

        public DataTable ComprasPorProveedor(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                return _reporteRepository.ComprasPorProveedor(desde, hasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al resumir compras por proveedor", ex);
            }
        }

        // Cantidad de reingresos en garantia (ordenes Tipo=Garantia).
        public int ContarReingresosGarantia(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                DataTable dt = _reporteRepository.ContarReingresosGarantia(desde, hasta);

                if (dt.Rows.Count <= 0 || dt.Rows[0]["cantidad"] == DBNull.Value)
                    return 0;

                return Convert.ToInt32(dt.Rows[0]["cantidad"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar reingresos en garantia", ex);
            }
        }

        public DataTable EvaluacionesPorEstado(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                return _reporteRepository.EvaluacionesPorEstado(desde, hasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar evaluaciones de garantia por estado", ex);
            }
        }

        // Tasa de aceptacion de garantia = aceptadas / (aceptadas + rechazadas). 0 si vacio.
        public decimal TasaAceptacionGarantia(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                DataTable dt = _reporteRepository.EvaluacionesPorEstado(desde, hasta);

                int aceptadas = LeerCantidad(dt, "Aceptada");
                int rechazadas = LeerCantidad(dt, "Rechazada");
                int decididas = aceptadas + rechazadas;

                if (decididas <= 0)
                    return 0;

                return (decimal)aceptadas / decididas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular la tasa de aceptacion de garantia", ex);
            }
        }

        // Monto total de presupuestos aprobados (nunca facturacion/ingresos/ventas:
        // el sistema no factura, los presupuestos aprobados son compromiso, no cobro).
        public decimal MontoPresupuestosAprobados(DateTime? desde, DateTime? hasta)
        {
            try
            {
                ValidarRango(desde, hasta);
                DataTable dt = _reporteRepository.MontoPresupuestosAprobados(desde, hasta);

                if (dt.Rows.Count <= 0 || dt.Rows[0]["monto"] == DBNull.Value)
                    return 0;

                return Convert.ToDecimal(dt.Rows[0]["monto"]);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular el monto de presupuestos aprobados", ex);
            }
        }

        // Reutiliza RepuestoRepository.Listar(): sin query nueva.
        // Criterio igual al de la UI (FrmRepuestos): stock_actual <= stock_minimo.
        public List<DOMAIN.Features.Repuestos.Repuesto> RepuestosBajoMinimo()
        {
            try
            {
                List<DOMAIN.Features.Repuestos.Repuesto> todos = _repuestoRepository.Listar(false);
                List<DOMAIN.Features.Repuestos.Repuesto> filtrados = new List<DOMAIN.Features.Repuestos.Repuesto>();

                foreach (DOMAIN.Features.Repuestos.Repuesto repuesto in todos)
                {
                    if (repuesto.StockActual <= repuesto.StockMinimo)
                        filtrados.Add(repuesto);
                }

                return filtrados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar repuestos bajo minimo", ex);
            }
        }

        // 6 KPIs en 1 fila: abiertas_total, esperando_respuesta, en_reparacion
        // (EnReparacion + EnPruebas), listas_retiro, garantias_abiertas, bajo_minimo.
        public DataTable DashboardResumen()
        {
            try
            {
                return _reporteRepository.DashboardResumen();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el resumen del dashboard", ex);
            }
        }

        private void ValidarRango(DateTime? desde, DateTime? hasta)
        {
            if (desde.HasValue && hasta.HasValue && desde.Value > hasta.Value)
                throw new ReglaNegocioException("La fecha desde no puede ser posterior a la fecha hasta.");
        }

        // DataTable con columnas estado+cantidad. Si un estado no aparece, cuenta 0.
        private int LeerCantidad(DataTable dt, string estado)
        {
            foreach (DataRow fila in dt.Rows)
            {
                object valor = fila["estado"];

                if (valor != DBNull.Value && string.Equals(valor.ToString(), estado, StringComparison.OrdinalIgnoreCase))
                    return Convert.ToInt32(fila["cantidad"]);
            }

            return 0;
        }
    }
}
