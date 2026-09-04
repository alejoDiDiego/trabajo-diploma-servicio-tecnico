using DOMAIN.Exceptions;

namespace DOMAIN.Features.Reparaciones
{
    public class ReparacionRepuesto
    {
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
            int cantidad, decimal costoUnitario)
        {
            return new ReparacionRepuesto
            {
                IdReparacion = idReparacion,
                IdRepuesto = idRepuesto,
                Cantidad = cantidad,
                CostoUnitario = costoUnitario
            };
        }
    }
}
