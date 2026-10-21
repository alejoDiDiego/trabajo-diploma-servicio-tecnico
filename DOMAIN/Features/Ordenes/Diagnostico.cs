using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Ordenes
{
    public class Diagnostico
    {
        public int Id { get; private set; }
        public int IdOrden { get; private set; }
        public int IdUsuarioTecnico { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Descripcion { get; private set; }
        public bool EsReparable { get; private set; }
        public int? TiempoEstimadoDias { get; private set; }
        public string Observaciones { get; private set; }

        private Diagnostico() { }

        public static Diagnostico CrearNuevo(int idOrden, int idUsuarioTecnico, string descripcion,
            bool esReparable, int? tiempoEstimadoDias, string observaciones)
        {
            if (idOrden <= 0)
                throw new ReglaNegocioException("La orden del diagnostico es obligatoria.");
            if (idUsuarioTecnico <= 0)
                throw new ReglaNegocioException("El tecnico del diagnostico es obligatorio.");
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ReglaNegocioException("La descripcion del diagnostico es obligatoria.");
            if (tiempoEstimadoDias.HasValue && tiempoEstimadoDias.Value < 0)
                throw new ReglaNegocioException("El tiempo estimado no puede ser negativo.");

            return new Diagnostico
            {
                IdOrden = idOrden,
                IdUsuarioTecnico = idUsuarioTecnico,
                Fecha = DateTime.Now,
                Descripcion = descripcion.Trim(),
                EsReparable = esReparable,
                TiempoEstimadoDias = tiempoEstimadoDias,
                Observaciones = observaciones == null ? "" : observaciones.Trim()
            };
        }

        public static Diagnostico CargarDesdeDB(int id, int idOrden, int idUsuarioTecnico,
            DateTime fecha, string descripcion, bool esReparable, int? tiempoEstimadoDias,
            string observaciones)
        {
            return new Diagnostico
            {
                Id = id,
                IdOrden = idOrden,
                IdUsuarioTecnico = idUsuarioTecnico,
                Fecha = fecha,
                Descripcion = descripcion ?? "",
                EsReparable = esReparable,
                TiempoEstimadoDias = tiempoEstimadoDias,
                Observaciones = observaciones ?? ""
            };
        }
    }
}
