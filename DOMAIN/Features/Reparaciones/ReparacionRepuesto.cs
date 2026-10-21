using DOMAIN.Exceptions;

namespace DOMAIN.Features.Reparaciones
{
    public class ReparacionRepuesto
    {
        // F8: una fila por consumo (id_consumo IDENTITY PK) para conservar el costo
        // vigente de cada consumo; (id_reparacion, id_repuesto) queda como indice normal.
        public int IdConsumo { get; private set; }
        public int IdReparacion { get; private set; }
        public int IdRepuesto { get; private set; }
        public int Cantidad { get; private set; }
        public decimal CostoUnitario { get; private set; }

        private ReparacionRepuesto() { }

        public static ReparacionRepuesto CrearNuevo(int idReparacion, int idRepuesto,
            int cantidad, decimal costoUnitario)
        {
            if (idReparacion <= 0)
                throw new ReglaNegocioException("La reparacion del consumo es obligatoria.");
            if (idRepuesto <= 0)
                throw new ReglaNegocioException("El repuesto del consumo es obligatorio.");
            if (cantidad <= 0)
                throw new ReglaNegocioException("La cantidad consumida debe ser mayor a cero.");
            if (costoUnitario < 0)
                throw new ReglaNegocioException("El costo unitario no puede ser negativo.");

            return new ReparacionRepuesto
            {
                IdReparacion = idReparacion,
                IdRepuesto = idRepuesto,
                Cantidad = cantidad,
                CostoUnitario = costoUnitario
            };
        }

        public static ReparacionRepuesto CargarDesdeDB(int idReparacion, int idRepuesto,
            int cantidad, decimal costoUnitario, int idConsumo = 0)
        {
            return new ReparacionRepuesto
            {
                IdConsumo = idConsumo,
                IdReparacion = idReparacion,
                IdRepuesto = idRepuesto,
                Cantidad = cantidad,
                CostoUnitario = costoUnitario
            };
        }
    }
}
