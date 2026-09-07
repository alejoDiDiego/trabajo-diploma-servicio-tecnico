using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Compras
{
    public class Compra
    {
        public int Id { get; private set; }
        public int IdProveedor { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Estado { get; private set; }
        public decimal Total { get; private set; }
        public int IdUsuario { get; private set; }
        public string Observaciones { get; private set; }

        private Compra() { }

        public static Compra CrearBorrador(int idProveedor, int idUsuario, string observaciones)
        {
            if (idProveedor <= 0)
                throw new ReglaNegocioException("El proveedor de la compra es obligatorio.");
            if (idUsuario <= 0)
                throw new ReglaNegocioException("El usuario de la compra es obligatorio.");

            return new Compra
            {
                IdProveedor = idProveedor,
                Fecha = DateTime.Now,
                Estado = EstadoCompra.Borrador,
                Total = 0,
                IdUsuario = idUsuario,
                Observaciones = observaciones == null ? "" : observaciones.Trim()
            };
        }

        public static Compra CargarDesdeDB(int id, int idProveedor, DateTime fecha,
            string estado, decimal total, int idUsuario, string observaciones)
        {
            return new Compra
            {
                Id = id,
                IdProveedor = idProveedor,
                Fecha = fecha,
                Estado = estado ?? EstadoCompra.Borrador,
                Total = total,
                IdUsuario = idUsuario,
                Observaciones = observaciones ?? ""
            };
        }
    }
}
