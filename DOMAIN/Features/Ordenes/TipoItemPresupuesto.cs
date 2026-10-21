namespace DOMAIN.Features.Ordenes
{
    // Tipos de item admitidos en el detalle del presupuesto.
    // Repuesto es una linea comercial cotizada: referencia un repuesto del catalogo
    // (PresupuestoDetalle.id_repuesto, FK creada con guarda en RepuestoRepository)
    // y NO reserva ni mueve stock. El stock solo cambia por el consumo explicito de
    // una reparacion (ConsumirRepuesto), que guarda su propio costo.
    public static class TipoItemPresupuesto
    {
        public const string ManoObra = "ManoObra";
        public const string Servicio = "Servicio";
        public const string Repuesto = "Repuesto";
    }
}
