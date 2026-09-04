namespace DOMAIN.Features.Ordenes
{
    // Estados del ciclo de vida de una orden de servicio (CP2).
    // Valores existentes pero SIN transiciones en CP2 (reservados para checkpoints futuros):
    // EnReparacion, EnPruebas y PendienteEvaluacionGarantia no se alcanzan desde ningun metodo.
    public static class EstadoOrdenServicio
    {
        public const string Recibido = "Recibido";
        public const string EnDiagnostico = "EnDiagnostico";
        public const string PendientePresupuesto = "PendientePresupuesto";
        public const string EsperandoRespuesta = "EsperandoRespuesta";
        public const string AutorizadoReparacion = "AutorizadoReparacion";
        public const string EnReparacion = "EnReparacion";
        public const string EnPruebas = "EnPruebas";
        public const string PendienteEvaluacionGarantia = "PendienteEvaluacionGarantia";
        public const string ListoRetiro = "ListoRetiro";
        public const string Entregado = "Entregado";
    }
}
