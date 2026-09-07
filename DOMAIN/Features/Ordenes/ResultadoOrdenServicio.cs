namespace DOMAIN.Features.Ordenes
{
    // Resultado final de una orden de servicio.
    // Reparado lo asigna FinalizarPruebas (todas aprobadas).
    // CP4: GarantiaNoCubierta lo asigna MarcarGarantiaNoCubierta (reingreso rechazado sin pago).
    public static class ResultadoOrdenServicio
    {
        public const string Reparado = "Reparado";
        public const string NoReparable = "NoReparable";
        public const string PresupuestoRechazado = "PresupuestoRechazado";
        public const string Cancelado = "Cancelado";
        public const string GarantiaNoCubierta = "GarantiaNoCubierta";
    }
}
