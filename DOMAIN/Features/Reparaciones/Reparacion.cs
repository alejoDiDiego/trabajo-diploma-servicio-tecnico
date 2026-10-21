using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Reparaciones
{
    public class Reparacion
    {
        public int Id { get; private set; }
        public int IdOrden { get; private set; }
        public int NumeroIntervencion { get; private set; }
        public int IdUsuarioTecnico { get; private set; }
        public DateTime FechaInicio { get; private set; }
        public DateTime? FechaFin { get; private set; }
        public string TrabajoRealizado { get; private set; }
        public string Observaciones { get; private set; }

        private Reparacion() { }

        public static Reparacion CrearNuevo(int idOrden, int numeroIntervencion, int idUsuarioTecnico)
        {
            if (idOrden <= 0)
                throw new ReglaNegocioException("La orden de la reparacion es obligatoria.");
            if (numeroIntervencion < 1)
                throw new ReglaNegocioException("El numero de intervencion debe ser mayor a cero.");
            if (idUsuarioTecnico <= 0)
                throw new ReglaNegocioException("El tecnico de la reparacion es obligatorio.");

            return new Reparacion
            {
                IdOrden = idOrden,
                NumeroIntervencion = numeroIntervencion,
                IdUsuarioTecnico = idUsuarioTecnico,
                FechaInicio = DateTime.Now,
                FechaFin = null,
                TrabajoRealizado = null,
                Observaciones = ""
            };
        }

        public static Reparacion CargarDesdeDB(int id, int idOrden, int numeroIntervencion,
            int idUsuarioTecnico, DateTime fechaInicio, DateTime? fechaFin,
            string trabajoRealizado, string observaciones)
        {
            return new Reparacion
            {
                Id = id,
                IdOrden = idOrden,
                NumeroIntervencion = numeroIntervencion,
                IdUsuarioTecnico = idUsuarioTecnico,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                TrabajoRealizado = trabajoRealizado,
                Observaciones = observaciones ?? ""
            };
        }

        public void Finalizar(string trabajoRealizado, string observaciones)
        {
            if (FechaFin.HasValue)
                throw new ReglaNegocioException("La reparacion ya fue finalizada.");
            if (string.IsNullOrWhiteSpace(trabajoRealizado))
                throw new ReglaNegocioException("El trabajo realizado es obligatorio para finalizar la reparacion.");

            FechaFin = DateTime.Now;
            TrabajoRealizado = trabajoRealizado.Trim();
            Observaciones = observaciones == null ? "" : observaciones.Trim();
        }
    }
}
