using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Garantias
{
    // Cardinalidad de la garantia: la COBERTURA es 0..1 por orden original (una sola fila en
    // Garantias, unica por UX_Garantias_OrdenOriginal e incluida la anulada), mientras que los
    // reingresos son VISITAS 0..N de esa misma cobertura: cada reingreso es una orden nueva de
    // tipo Garantia enlazada por OrdenesServicio.id_orden_origen. Varios reingresos por garantia
    // son validos, pero secuenciales: solo uno puede estar abierto a la vez.
    public class Garantia
    {
        public int Id { get; private set; }
        public int IdOrdenOriginal { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime FechaFin { get; private set; }
        public string Observaciones { get; private set; }
        public bool Anulada { get; private set; }

        private Garantia() { }

        public static Garantia CrearNuevo(int idOrdenOriginal, DateTime fechaInicio,
            DateTime fechaFin, string observaciones)
        {
            if (idOrdenOriginal <= 0)
                throw new ReglaNegocioException("La orden original de la garantia es obligatoria.");
            if (fechaFin < fechaInicio)
                throw new ReglaNegocioException("La fecha de fin no puede ser anterior a la fecha de inicio.");

            return new Garantia
            {
                IdOrdenOriginal = idOrdenOriginal,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Observaciones = observaciones == null ? "" : observaciones.Trim(),
                Anulada = false
            };
        }

        public static Garantia CargarDesdeDB(int id, int idOrdenOriginal, DateTime fechaInicio,
            DateTime fechaFin, string observaciones, bool anulada)
        {
            return new Garantia
            {
                Id = id,
                IdOrdenOriginal = idOrdenOriginal,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Observaciones = observaciones ?? "",
                Anulada = anulada
            };
        }
    }

    // Se intento crear (o reabrir) un reingreso de garantia mientras la orden original ya tiene
    // otro reingreso abierto. La UI la reconoce por tipo de excepcion (nunca por el texto del
    // mensaje) y traduce ClaveMensaje; hereda de ReglaNegocioException para no alterar los
    // manejos existentes.
    public class ReingresoGarantiaAbiertoException : ReglaNegocioException
    {
        public string ClaveMensaje { get; private set; }

        public ReingresoGarantiaAbiertoException(string claveMensaje, string mensaje)
            : base(mensaje)
        {
            ClaveMensaje = claveMensaje;
        }
    }
}
