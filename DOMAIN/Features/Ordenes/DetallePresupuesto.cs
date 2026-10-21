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

        // Importe maximo representable en PresupuestoDetalle (decimal(18,2)): un valor mayor
        // se rechaza en vez de dejar que SQL redondee o desborde el snapshot economico.
        private const decimal ImporteMaximoRepresentable = 9999999999999999.99m;

        public static DetallePresupuesto CrearNuevo(int idPresupuesto, string tipoItem,
            string descripcion, int cantidad, decimal precioUnitario, int? idRepuesto = null)
        {
            ValidarReglas(tipoItem, idRepuesto, descripcion, cantidad, precioUnitario);

            return new DetallePresupuesto
            {
                IdPresupuesto = idPresupuesto,
                IdRepuesto = tipoItem == TipoItemPresupuesto.Repuesto ? idRepuesto : null,
                TipoItem = tipoItem,
                Descripcion = descripcion.Trim(),
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario,
                Subtotal = cantidad * precioUnitario
            };
        }

        // Revalida el item antes de persistirlo. Cubre tambien objetos hidratados con
        // CargarDesdeDB (borrador o presupuesto ya guardado) para no escribir detalles mal
        // formados ni corregir economicos persistidos.
        public void ValidarParaGuardar()
        {
            ValidarReglas(TipoItem, IdRepuesto, Descripcion, Cantidad, PrecioUnitario);

            if (Subtotal != Cantidad * PrecioUnitario)
                throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoDetalleInvalido",
                    "El subtotal del item no coincide con cantidad por precio unitario.");
        }

        private static void ValidarReglas(string tipoItem, int? idRepuesto, string descripcion,
            int cantidad, decimal precioUnitario)
        {
            if (tipoItem != TipoItemPresupuesto.ManoObra
                && tipoItem != TipoItemPresupuesto.Servicio
                && tipoItem != TipoItemPresupuesto.Repuesto)
                throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoDetalleInvalido",
                    "El tipo de item del presupuesto no es valido.");

            if (tipoItem == TipoItemPresupuesto.Repuesto)
            {
                if (!idRepuesto.HasValue || idRepuesto.Value <= 0)
                    throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoRepuestoObligatorio",
                        "El item de repuesto requiere un repuesto seleccionado.");
            }
            else if (idRepuesto.HasValue)
            {
                throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoDetalleInvalido",
                    "Solo un item de repuesto puede referenciar un repuesto.");
            }

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoDetalleInvalido",
                    "La descripcion del item es obligatoria.");

            if (cantidad <= 0)
                throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoCantidadInvalida",
                    "La cantidad del item debe ser mayor a cero.");

            if (precioUnitario < 0)
                throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoPrecioInvalido",
                    "El precio unitario no puede ser negativo.");

            if (decimal.Round(precioUnitario, 2) != precioUnitario)
                throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoPrecioInvalido",
                    "El precio unitario debe tener hasta dos decimales.");

            if (precioUnitario > ImporteMaximoRepresentable
                || (decimal)cantidad * precioUnitario > ImporteMaximoRepresentable)
                throw new DetallePresupuestoInvalidoException("Mensaje.PresupuestoPrecioInvalido",
                    "El importe del item excede el maximo representable.");
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

    // Detalle de presupuesto invalido. La UI lo reconoce por tipo de excepcion (nunca por
    // texto) y traduce ClaveMensaje; hereda de ReglaNegocioException para no alterar los
    // manejos existentes.
    public class DetallePresupuestoInvalidoException : ReglaNegocioException
    {
        public string ClaveMensaje { get; private set; }

        public DetallePresupuestoInvalidoException(string claveMensaje, string mensaje)
            : base(mensaje)
        {
            ClaveMensaje = claveMensaje;
        }
    }
}
