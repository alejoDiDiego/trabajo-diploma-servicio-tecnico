namespace DOMAIN.Features.Ordenes
{
    // Estados de un presupuesto. CP2 emite directo en Pendiente;
    // Borrador queda reservado para uso futuro.
    public static class EstadoPresupuesto
    {
        public const string Borrador = "Borrador";
        public const string Pendiente = "Pendiente";
        public const string Aprobado = "Aprobado";
        public const string Rechazado = "Rechazado";
    }
}
