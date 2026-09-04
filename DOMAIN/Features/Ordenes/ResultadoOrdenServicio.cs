namespace DOMAIN.Features.Ordenes
{
    // Resultado final de una orden de servicio.
    // Valores existentes pero NO alcanzables en CP2 (reservados):
    // Reparado y GarantiaNoCubierta no se asignan desde ningun metodo.
    public static class ResultadoOrdenServicio
    {
        public const string Reparado = "Reparado";
        public const string NoReparable = "NoReparable";
        public const string PresupuestoRechazado = "PresupuestoRechazado";
        public const string Cancelado = "Cancelado";
        public const string GarantiaNoCubierta = "GarantiaNoCubierta";
    }
}
