namespace DOMAIN.Features.Ordenes
{
    // Resultado final de una orden de servicio.
    // Reparado lo asigna FinalizarPruebas (todas aprobadas).
    // Valor reservado: GarantiaNoCubierta no se asigna desde ningun metodo.
    public static class ResultadoOrdenServicio
    {
        public const string Reparado = "Reparado";
        public const string NoReparable = "NoReparable";
        public const string PresupuestoRechazado = "PresupuestoRechazado";
        public const string Cancelado = "Cancelado";
        public const string GarantiaNoCubierta = "GarantiaNoCubierta";
    }
}
