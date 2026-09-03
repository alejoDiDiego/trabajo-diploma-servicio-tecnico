using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Clientes
{
    public class Cliente
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Documento { get; private set; }
        public string Telefono { get; private set; }
        public string Email { get; private set; }
        public string Direccion { get; private set; }
        public string Observaciones { get; private set; }
        public bool Activo { get; private set; }
        public DateTime FechaAlta { get; private set; }

        private Cliente() { }

        public static Cliente CrearNuevo(string nombre, string apellido, string documento,
            string telefono, string email, string direccion, string observaciones)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ReglaNegocioException("El nombre del cliente es obligatorio.");
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ReglaNegocioException("El apellido del cliente es obligatorio.");
            if (string.IsNullOrWhiteSpace(documento))
                throw new ReglaNegocioException("El documento del cliente es obligatorio.");

            return new Cliente
            {
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Documento = documento.Trim(),
                Telefono = telefono == null ? "" : telefono.Trim(),
                Email = email == null ? "" : email.Trim(),
                Direccion = direccion == null ? "" : direccion.Trim(),
                Observaciones = observaciones == null ? "" : observaciones.Trim(),
                Activo = true,
                FechaAlta = DateTime.Now
            };
        }

        public static Cliente CargarDesdeDB(int id, string nombre, string apellido, string documento,
            string telefono, string email, string direccion, string observaciones, bool activo, DateTime fechaAlta)
        {
            return new Cliente
            {
                Id = id,
                Nombre = nombre ?? "",
                Apellido = apellido ?? "",
                Documento = documento ?? "",
                Telefono = telefono ?? "",
                Email = email ?? "",
                Direccion = direccion ?? "",
                Observaciones = observaciones ?? "",
                Activo = activo,
                FechaAlta = fechaAlta
            };
        }
    }
}
