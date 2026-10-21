using DOMAIN.Exceptions;

namespace DOMAIN.Features.Proveedores
{
    public class Proveedor
    {
        public int Id { get; private set; }
        public string RazonSocial { get; private set; }
        public string Cuit { get; private set; }
        public string Telefono { get; private set; }
        public string Email { get; private set; }
        public string Direccion { get; private set; }
        public string Contacto { get; private set; }
        public bool Activo { get; private set; }

        private Proveedor() { }

        public static Proveedor CrearNuevo(string razonSocial, string cuit,
            string telefono, string email, string direccion, string contacto)
        {
            if (string.IsNullOrWhiteSpace(razonSocial))
                throw new ReglaNegocioException("La razon social del proveedor es obligatoria.");

            return new Proveedor
            {
                RazonSocial = razonSocial.Trim(),
                Cuit = cuit == null ? "" : cuit.Trim(),
                Telefono = telefono == null ? "" : telefono.Trim(),
                Email = email == null ? "" : email.Trim(),
                Direccion = direccion == null ? "" : direccion.Trim(),
                Contacto = contacto == null ? "" : contacto.Trim(),
                Activo = true
            };
        }

        public static Proveedor CargarDesdeDB(int id, string razonSocial, string cuit,
            string telefono, string email, string direccion, string contacto, bool activo)
        {
            return new Proveedor
            {
                Id = id,
                RazonSocial = razonSocial ?? "",
                Cuit = cuit ?? "",
                Telefono = telefono ?? "",
                Email = email ?? "",
                Direccion = direccion ?? "",
                Contacto = contacto ?? "",
                Activo = activo
            };
        }
    }
}
