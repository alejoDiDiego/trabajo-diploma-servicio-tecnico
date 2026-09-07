using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Garantias
{
    public class EvaluacionGarantia
    {
        public int Id { get; private set; }
        public int IdOrdenReingreso { get; private set; }
        public int IdGarantia { get; private set; }
        public string Estado { get; private set; }
        public DateTime Fecha { get; private set; }
        public int IdUsuario { get; private set; }
        public string Motivo { get; private set; }
        public string Observaciones { get; private set; }

        private EvaluacionGarantia() { }

        public static EvaluacionGarantia CrearPendiente(int idOrdenReingreso, int idGarantia, int idUsuario)
        {
            if (idOrdenReingreso <= 0)
                throw new ReglaNegocioException("La orden de reingreso de la evaluacion es obligatoria.");
            if (idGarantia <= 0)
                throw new ReglaNegocioException("La garantia de la evaluacion es obligatoria.");
            if (idUsuario <= 0)
                throw new ReglaNegocioException("El usuario de la evaluacion es obligatorio.");

            return new EvaluacionGarantia
            {
                IdOrdenReingreso = idOrdenReingreso,
                IdGarantia = idGarantia,
                Estado = EstadoEvaluacionGarantia.Pendiente,
                Fecha = DateTime.Now,
                IdUsuario = idUsuario,
                Motivo = "",
                Observaciones = ""
            };
        }

        public static EvaluacionGarantia CargarDesdeDB(int id, int idOrdenReingreso, int idGarantia,
            string estado, DateTime fecha, int idUsuario, string motivo, string observaciones)
        {
            return new EvaluacionGarantia
            {
                Id = id,
                IdOrdenReingreso = idOrdenReingreso,
                IdGarantia = idGarantia,
                Estado = estado ?? EstadoEvaluacionGarantia.Pendiente,
                Fecha = fecha,
                IdUsuario = idUsuario,
                Motivo = motivo ?? "",
                Observaciones = observaciones ?? ""
            };
        }
    }
}
