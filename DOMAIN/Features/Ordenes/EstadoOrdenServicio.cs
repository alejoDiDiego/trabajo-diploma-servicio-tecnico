namespace DOMAIN.Features.Ordenes
{
    // Estados del ciclo de vida de una orden de servicio.
    // Reservado para checkpoints futuros: PendienteEvaluacionGarantia no se alcanza desde ningun metodo.
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
