using DOMAIN.Exceptions;

namespace DOMAIN.Features.Ordenes
{
    public class DetallePresupuesto
    {
        public int Id { get; private set; }
        public int IdPresupuesto { get; private set; }
        public int? IdRepuesto { get; private set; }
        public string TipoItem { get; private set; }
        public string Descripcion { get; private set; }
        public int Cantidad { get; private set; }
        public decimal PrecioUnitario { get; private set; }
        public decimal Subtotal { get; private set; }

        private DetallePresupuesto() { }

        public static DetallePresupuesto CrearNuevo(int idPresupuesto, string tipoItem,
            string descripcion, int cantidad, decimal precioUnitario)
        {
            if (tipoItem != TipoItemPresupuesto.ManoObra && tipoItem != TipoItemPresupuesto.Servicio)
                throw new ReglaNegocioException("El tipo de item solo puede ser mano de obra o servicio.");
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ReglaNegocioException("La descripcion del item es obligatoria.");
            if (cantidad <= 0)
                throw new ReglaNegocioException("La cantidad del item debe ser mayor a cero.");
            if (precioUnitario < 0)
                throw new ReglaNegocioException("El precio unitario no puede ser negativo.");

            return new DetallePresupuesto
            {
                IdPresupuesto = idPresupuesto,
                IdRepuesto = null,
                TipoItem = tipoItem,
                Descripcion = descripcion.Trim(),
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario,
                Subtotal = cantidad * precioUnitario
            };
        }

        public static DetallePresupuesto CargarDesdeDB(int id, int idPresupuesto, int? idRepuesto,
            string tipoItem, string descripcion, int cantidad, decimal precioUnitario, decimal subtotal)
        {
            return new DetallePresupuesto
            {
                Id = id,
                IdPresupuesto = idPresupuesto,
                IdRepuesto = idRepuesto,
                TipoItem = tipoItem ?? "",
                Descripcion = descripcion ?? "",
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario,
                Subtotal = subtotal
            };
        }
    }
}
