namespace DOMAIN.Features.Ordenes
{
    // Estados de un presupuesto. CP2 emite directo en Pendiente;
    // Borrador es la via alternativa (se elimina fisico, nunca se anula).
    // Anulado es terminal: conserva historial y libera el cupo de Original activo.
    public static class EstadoPresupuesto
    {
        public const string Borrador = "Borrador";
        public const string Pendiente = "Pendiente";
        public const string Aprobado = "Aprobado";
        public const string Rechazado = "Rechazado";
        public const string Anulado = "Anulado";
    }
}
