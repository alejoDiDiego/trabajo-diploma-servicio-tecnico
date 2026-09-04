using DOMAIN.Exceptions;

namespace DOMAIN.Features.Repuestos
{
    public class Repuesto
    {
        public int Id { get; private set; }
        public string Codigo { get; private set; }
        public string Descripcion { get; private set; }
        public int StockActual { get; private set; }
        public int StockMinimo { get; private set; }
        public decimal CostoActual { get; private set; }
        public decimal PrecioReferencia { get; private set; }
        public bool Activo { get; private set; }

        private Repuesto() { }

        public static Repuesto CrearNuevo(string codigo, string descripcion, int stockInicial,
            int stockMinimo, decimal costoActual, decimal precioReferencia)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ReglaNegocioException("El codigo del repuesto es obligatorio.");
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ReglaNegocioException("La descripcion del repuesto es obligatoria.");
            if (stockInicial < 0)
                throw new ReglaNegocioException("El stock inicial no puede ser negativo.");
            if (stockMinimo < 0)
                throw new ReglaNegocioException("El stock minimo no puede ser negativo.");
            if (costoActual < 0)
                throw new ReglaNegocioException("El costo actual no puede ser negativo.");
            if (precioReferencia < 0)
                throw new ReglaNegocioException("El precio de referencia no puede ser negativo.");

            return new Repuesto
            {
                Codigo = codigo.Trim(),
                Descripcion = descripcion.Trim(),
                StockActual = stockInicial,
                StockMinimo = stockMinimo,
                CostoActual = costoActual,
                PrecioReferencia = precioReferencia,
                Activo = true
            };
        }

        public static Repuesto CargarDesdeDB(int id, string codigo, string descripcion,
            int stockActual, int stockMinimo, decimal costoActual, decimal precioReferencia,
            bool activo)
        {
            return new Repuesto
            {
                Id = id,
                Codigo = codigo ?? "",
                Descripcion = descripcion ?? "",
                StockActual = stockActual,
                StockMinimo = stockMinimo,
                CostoActual = costoActual,
                PrecioReferencia = precioReferencia,
                Activo = activo
            };
        }
    }
}
