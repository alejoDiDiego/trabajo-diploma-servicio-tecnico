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
            if (string.IsNullOrWhiteSpace(telefono))
                throw new ReglaNegocioException("El telefono es obligatorio.");

            return new Cliente
            {
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Documento = documento.Trim(),
                Telefono = telefono.Trim(),
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

    // Excepcion de identidad: el documento propuesto ya pertenece a otro cliente
    // (activo o inactivo). La UI lo muestra con Mensaje.ClienteDocumentoDuplicado*.
    public class ClienteDocumentoDuplicadoException : ReglaNegocioException
    {
        public string Documento { get; private set; }
        public int IdClienteExistente { get; private set; }
        public string NombreExistente { get; private set; }
        public string ApellidoExistente { get; private set; }
        public bool ActivoExistente { get; private set; }

        // Respaldo del indice unico: conflicto sin la fila propietaria localizada.
        public ClienteDocumentoDuplicadoException(string documento)
            : this(0, "", "", documento, true)
        {
        }

        public ClienteDocumentoDuplicadoException(int idClienteExistente, string nombreExistente,
            string apellidoExistente, string documento, bool activoExistente)
            : base(ConstruirMensaje(idClienteExistente, nombreExistente, apellidoExistente, documento))
        {
            IdClienteExistente = idClienteExistente;
            NombreExistente = nombreExistente == null ? "" : nombreExistente;
            ApellidoExistente = apellidoExistente == null ? "" : apellidoExistente;
            Documento = documento == null ? "" : documento;
            ActivoExistente = activoExistente;
        }

        // true cuando el mensaje puede nombrar al cliente que ya tiene el documento.
        public bool TienePropietario
        {
            get { return IdClienteExistente > 0; }
        }

        public string NombrePropietario
        {
            get { return (NombreExistente + " " + ApellidoExistente).Trim(); }
        }

        private static string ConstruirMensaje(int idClienteExistente, string nombreExistente,
            string apellidoExistente, string documento)
        {
            string documentoTexto = documento == null ? "" : documento;

            if (idClienteExistente > 0)
            {
                string propietario = ((nombreExistente == null ? "" : nombreExistente) + " "
                    + (apellidoExistente == null ? "" : apellidoExistente)).Trim();

                return "El documento '" + documentoTexto + "' ya pertenece a " + propietario
                    + " (ID " + idClienteExistente + "). Edite el registro existente.";
            }

            return "El documento '" + documentoTexto + "' ya esta registrado por otro cliente. "
                + "Edite el registro existente.";
        }
    }
}
