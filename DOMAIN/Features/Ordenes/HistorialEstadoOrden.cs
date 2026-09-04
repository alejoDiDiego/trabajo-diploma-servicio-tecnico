using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Ordenes
{
    public class HistorialEstadoOrden
    {
        public int Id { get; private set; }
        public int IdOrden { get; private set; }
        public string EstadoAnterior { get; private set; }
        public string EstadoNuevo { get; private set; }
        public DateTime FechaHora { get; private set; }
        public int IdUsuario { get; private set; }
        public string Observacion { get; private set; }

        private HistorialEstadoOrden() { }

        public static HistorialEstadoOrden CrearNuevo(int idOrden, string estadoAnterior,
            string estadoNuevo, int idUsuario, string observacion)
        {
            if (idOrden <= 0)
                throw new ReglaNegocioException("La orden del historial es obligatoria.");
            if (string.IsNullOrWhiteSpace(estadoNuevo))
                throw new ReglaNegocioException("El estado nuevo del historial es obligatorio.");
            if (idUsuario <= 0)
                throw new ReglaNegocioException("El usuario del historial es obligatorio.");

            return new HistorialEstadoOrden
            {
                IdOrden = idOrden,
                EstadoAnterior = string.IsNullOrWhiteSpace(estadoAnterior) ? null : estadoAnterior.Trim(),
                EstadoNuevo = estadoNuevo.Trim(),
                FechaHora = DateTime.Now,
                IdUsuario = idUsuario,
                Observacion = observacion == null ? "" : observacion.Trim()
            };
        }

        public static HistorialEstadoOrden CargarDesdeDB(int id, int idOrden, string estadoAnterior,
            string estadoNuevo, DateTime fechaHora, int idUsuario, string observacion)
        {
            return new HistorialEstadoOrden
            {
                Id = id,
                IdOrden = idOrden,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = estadoNuevo ?? "",
                FechaHora = fechaHora,
                IdUsuario = idUsuario,
                Observacion = observacion ?? ""
            };
        }
    }
}
