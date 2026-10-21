using DOMAIN.Exceptions;

namespace DOMAIN.Features.Compras
{
    public class DetalleCompra
    {
        public int Id { get; private set; }
        public int IdCompra { get; private set; }
        public int IdRepuesto { get; private set; }
        public int Cantidad { get; private set; }
        public decimal CostoUnitario { get; private set; }
        public decimal Subtotal { get; private set; }

        private DetalleCompra() { }

        public static DetalleCompra CrearNuevo(int idCompra, int idRepuesto, int cantidad, decimal costoUnitario)
        {
            // idCompra no se valida (igual que DetallePresupuesto): la UI arma los items
            // con id 0 antes de crear la cabecera; el repository los persiste con el id real.
            if (idRepuesto <= 0)
                throw new ReglaNegocioException("El repuesto del detalle es obligatorio.");
            if (cantidad <= 0)
                throw new ReglaNegocioException("La cantidad del detalle debe ser mayor a cero.");
            if (costoUnitario < 0)
                throw new ReglaNegocioException("El costo unitario no puede ser negativo.");

            return new DetalleCompra
            {
                IdCompra = idCompra,
                IdRepuesto = idRepuesto,
                Cantidad = cantidad,
                CostoUnitario = costoUnitario,
                Subtotal = cantidad * costoUnitario
            };
        }

        public static DetalleCompra CargarDesdeDB(int id, int idCompra, int idRepuesto,
            int cantidad, decimal costoUnitario, decimal subtotal)
        {
            return new DetalleCompra
            {
                Id = id,
                IdCompra = idCompra,
                IdRepuesto = idRepuesto,
                Cantidad = cantidad,
                CostoUnitario = costoUnitario,
                Subtotal = subtotal
            };
        }
    }
}
