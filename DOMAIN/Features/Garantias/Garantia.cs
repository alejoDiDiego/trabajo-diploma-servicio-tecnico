using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Garantias
{
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
}
