namespace DOMAIN.Features.Garantias
{
    // Estados de la evaluacion de un reingreso de garantia.
    // Aceptada cierra la EVALUACION (la garantia cubre la falla) pero no la visita: el reingreso
    // sigue abierto hasta que su orden se entrega. Rechazada con continuidad de pago o con retiro
    // cierra la evaluacion y habilita el cierre del reingreso. No se agregan estados nuevos.
    public static class EstadoEvaluacionGarantia
    {
        public const string Pendiente = "Pendiente";
        public const string Aceptada = "Aceptada";
        public const string Rechazada = "Rechazada";
    }
}
