namespace DOMAIN.Features.Ordenes
{
    // Tipo de orden. CP2 siempre crea Normal; CP4 crea Garantia en reingresos.
    public static class TipoOrden
    {
        public const string Normal = "Normal";
        public const string Garantia = "Garantia";
    }
}
