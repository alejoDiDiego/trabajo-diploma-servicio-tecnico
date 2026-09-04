using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Ordenes
{
    public class Presupuesto
    {
        public int Id { get; private set; }
        public int IdOrden { get; private set; }
        public DateTime FechaEmision { get; private set; }
        public string Estado { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal Descuento { get; private set; }
        public decimal Total { get; private set; }
        public int DiasGarantia { get; private set; }
        public DateTime? FechaRespuesta { get; private set; }
        public string MedioRespuesta { get; private set; }
        public string MotivoRechazo { get; private set; }
        public string Observaciones { get; private set; }

        private Presupuesto() { }

        public static Presupuesto CrearNuevo(int idOrden, decimal subtotal, decimal descuento,
            int diasGarantia, string observaciones)
        {
            if (idOrden <= 0)
                throw new ReglaNegocioException("La orden del presupuesto es obligatoria.");
            if (subtotal < 0)
                throw new ReglaNegocioException("El subtotal no puede ser negativo.");
            if (descuento < 0)
                throw new ReglaNegocioException("El descuento no puede ser negativo.");
            if (descuento > subtotal)
                throw new ReglaNegocioException("El descuento no puede superar el subtotal.");
            if (diasGarantia < 0)
                throw new ReglaNegocioException("Los dias de garantia no pueden ser negativos.");

            return new Presupuesto
            {
                IdOrden = idOrden,
                FechaEmision = DateTime.Now,
                Estado = EstadoPresupuesto.Pendiente,
                Subtotal = subtotal,
                Descuento = descuento,
                Total = subtotal - descuento,
                DiasGarantia = diasGarantia,
                FechaRespuesta = null,
                MedioRespuesta = null,
                MotivoRechazo = null,
                Observaciones = observaciones == null ? "" : observaciones.Trim()
            };
        }

        public static Presupuesto CargarDesdeDB(int id, int idOrden, DateTime fechaEmision,
            string estado, decimal subtotal, decimal descuento, decimal total, int diasGarantia,
            DateTime? fechaRespuesta, string medioRespuesta, string motivoRechazo, string observaciones)
        {
            if (total != subtotal - descuento)
                throw new ReglaNegocioException("El total del presupuesto no coincide con subtotal menos descuento.");

            return new Presupuesto
            {
                Id = id,
                IdOrden = idOrden,
                FechaEmision = fechaEmision,
                Estado = estado ?? EstadoPresupuesto.Pendiente,
                Subtotal = subtotal,
                Descuento = descuento,
                Total = total,
                DiasGarantia = diasGarantia,
                FechaRespuesta = fechaRespuesta,
                MedioRespuesta = medioRespuesta,
                MotivoRechazo = motivoRechazo,
                Observaciones = observaciones ?? ""
            };
        }

        public void Aprobar(string medioRespuesta, string observaciones)
        {
            if (Estado != EstadoPresupuesto.Pendiente)
                throw new ReglaNegocioException("Solo se puede aprobar un presupuesto pendiente.");

            Estado = EstadoPresupuesto.Aprobado;
            FechaRespuesta = DateTime.Now;
            MedioRespuesta = medioRespuesta == null ? "" : medioRespuesta.Trim();
            Observaciones = observaciones == null ? Observaciones : observaciones.Trim();
        }

        public void Rechazar(string motivo, string medioRespuesta, string observaciones)
        {
            if (Estado != EstadoPresupuesto.Pendiente)
                throw new ReglaNegocioException("Solo se puede rechazar un presupuesto pendiente.");
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ReglaNegocioException("El motivo del rechazo es obligatorio.");

            Estado = EstadoPresupuesto.Rechazado;
            FechaRespuesta = DateTime.Now;
            MedioRespuesta = medioRespuesta == null ? "" : medioRespuesta.Trim();
            MotivoRechazo = motivo.Trim();
            Observaciones = observaciones == null ? Observaciones : observaciones.Trim();
        }
    }
}
