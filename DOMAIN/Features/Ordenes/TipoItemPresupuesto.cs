namespace DOMAIN.Features.Ordenes
{
    // Tipos de item permitidos en el detalle del presupuesto.
    // CP4: Repuesto existe como constante solo para infraestructura de
    // PresupuestoDetalle.id_repuesto (FK). Emitir sigue validando solo
    // ManoObra/Servicio; Repuesto NO habilitado en emision.
    public static class TipoItemPresupuesto
    {
        public const string ManoObra = "ManoObra";
        public const string Servicio = "Servicio";
        public const string Repuesto = "Repuesto";
    }
}
