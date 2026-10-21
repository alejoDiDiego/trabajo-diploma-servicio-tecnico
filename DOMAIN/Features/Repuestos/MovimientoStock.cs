using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Repuestos
{
    public class MovimientoStock
    {
        public int Id { get; private set; }
        public int IdRepuesto { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Tipo { get; private set; }
        public int Cantidad { get; private set; }
        public int StockAnterior { get; private set; }
        public int StockPosterior { get; private set; }
        public int IdUsuario { get; private set; }
        public int? IdCompra { get; private set; }
        public int? IdReparacion { get; private set; }
        public string Observacion { get; private set; }

        private MovimientoStock() { }

        public static MovimientoStock CrearNuevo(int idRepuesto, string tipo, int cantidad,
            int stockAnterior, int stockPosterior, int idUsuario, string observacion,
            int? idCompra = null, int? idReparacion = null)
        {
            if (idRepuesto <= 0)
                throw new ReglaNegocioException("El repuesto del movimiento es obligatorio.");
            if (string.IsNullOrWhiteSpace(tipo))
                throw new ReglaNegocioException("El tipo del movimiento es obligatorio.");
            if (tipo != TipoMovimientoStock.Compra
                && tipo != TipoMovimientoStock.ConsumoReparacion
                && tipo != TipoMovimientoStock.DevolucionConsumo
                && tipo != TipoMovimientoStock.AjustePositivo
                && tipo != TipoMovimientoStock.AjusteNegativo)
                throw new ReglaNegocioException("El tipo de movimiento no es valido.");
            if (cantidad <= 0)
                throw new ReglaNegocioException("La cantidad del movimiento debe ser mayor a cero.");
            if (stockAnterior < 0)
                throw new ReglaNegocioException("El stock anterior no puede ser negativo.");
            if (stockPosterior < 0)
                throw new ReglaNegocioException("El stock posterior no puede ser negativo.");
            if (idUsuario <= 0)
                throw new ReglaNegocioException("El usuario del movimiento es obligatorio.");
            if ((tipo == TipoMovimientoStock.AjustePositivo || tipo == TipoMovimientoStock.AjusteNegativo)
                && string.IsNullOrWhiteSpace(observacion))
                throw new ReglaNegocioException("La observacion es obligatoria para los ajustes de stock.");

            return new MovimientoStock
            {
                IdRepuesto = idRepuesto,
                Fecha = DateTime.Now,
                Tipo = tipo,
                Cantidad = cantidad,
                StockAnterior = stockAnterior,
                StockPosterior = stockPosterior,
                IdUsuario = idUsuario,
                IdCompra = idCompra,
                IdReparacion = idReparacion,
                Observacion = observacion == null ? "" : observacion.Trim()
            };
        }

        public static MovimientoStock CargarDesdeDB(int id, int idRepuesto, DateTime fecha,
            string tipo, int cantidad, int stockAnterior, int stockPosterior, int idUsuario,
            int? idCompra, int? idReparacion, string observacion)
        {
            return new MovimientoStock
            {
                Id = id,
                IdRepuesto = idRepuesto,
                Fecha = fecha,
                Tipo = tipo ?? "",
                Cantidad = cantidad,
                StockAnterior = stockAnterior,
                StockPosterior = stockPosterior,
                IdUsuario = idUsuario,
                IdCompra = idCompra,
                IdReparacion = idReparacion,
                Observacion = observacion ?? ""
            };
        }
    }
}
