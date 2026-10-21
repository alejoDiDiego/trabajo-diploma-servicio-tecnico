using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Ordenes
{
    public class Entrega
    {
        public int Id { get; private set; }
        public int IdOrden { get; private set; }
        public DateTime FechaEntrega { get; private set; }
        public int IdUsuario { get; private set; }
        public string EntregadoA { get; private set; }
        public string DocumentoReceptor { get; private set; }
        public string Observaciones { get; private set; }

        private Entrega() { }

        public static Entrega CrearNuevo(int idOrden, int idUsuario, string entregadoA,
            string documentoReceptor, string observaciones)
        {
            if (idOrden <= 0)
                throw new ReglaNegocioException("La orden de la entrega es obligatoria.");
            if (idUsuario <= 0)
                throw new ReglaNegocioException("El usuario de la entrega es obligatorio.");
            if (string.IsNullOrWhiteSpace(entregadoA))
                throw new ReglaNegocioException("El receptor de la entrega es obligatorio.");

            return new Entrega
            {
                IdOrden = idOrden,
                FechaEntrega = DateTime.Now,
                IdUsuario = idUsuario,
                EntregadoA = entregadoA.Trim(),
                DocumentoReceptor = documentoReceptor == null ? "" : documentoReceptor.Trim(),
                Observaciones = observaciones == null ? "" : observaciones.Trim()
            };
        }

        public static Entrega CargarDesdeDB(int id, int idOrden, DateTime fechaEntrega,
            int idUsuario, string entregadoA, string documentoReceptor, string observaciones)
        {
            return new Entrega
            {
                Id = id,
                IdOrden = idOrden,
                FechaEntrega = fechaEntrega,
                IdUsuario = idUsuario,
                EntregadoA = entregadoA ?? "",
                DocumentoReceptor = documentoReceptor ?? "",
                Observaciones = observaciones ?? ""
            };
        }
    }
}
