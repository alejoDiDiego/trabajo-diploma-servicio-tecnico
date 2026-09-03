using DOMAIN.Exceptions;

namespace DOMAIN.Features.Equipos
{
    public class Equipo
    {
        public int Id { get; private set; }
        public int IdCliente { get; private set; }
        public int IdTipoEquipo { get; private set; }
        public int IdMarca { get; private set; }
        public string Modelo { get; private set; }
        public string NumeroSerie { get; private set; }
        public string Imei { get; private set; }
        public string Color { get; private set; }
        public string Observaciones { get; private set; }
        public bool Activo { get; private set; }

        private Equipo() { }

        public static Equipo CrearNuevo(int idCliente, int idTipoEquipo, int idMarca,
            string modelo, string numeroSerie, string imei, string color, string observaciones)
        {
            if (idCliente <= 0)
                throw new ReglaNegocioException("El cliente del equipo es obligatorio.");
            if (idTipoEquipo <= 0)
                throw new ReglaNegocioException("El tipo de equipo es obligatorio.");
            if (idMarca <= 0)
                throw new ReglaNegocioException("La marca del equipo es obligatoria.");

            return new Equipo
            {
                IdCliente = idCliente,
                IdTipoEquipo = idTipoEquipo,
                IdMarca = idMarca,
                Modelo = modelo == null ? "" : modelo.Trim(),
                NumeroSerie = numeroSerie == null ? "" : numeroSerie.Trim(),
                Imei = imei == null ? "" : imei.Trim(),
                Color = color == null ? "" : color.Trim(),
                Observaciones = observaciones == null ? "" : observaciones.Trim(),
                Activo = true
            };
        }

        public static Equipo CargarDesdeDB(int id, int idCliente, int idTipoEquipo, int idMarca,
            string modelo, string numeroSerie, string imei, string color, string observaciones, bool activo)
        {
            return new Equipo
            {
                Id = id,
                IdCliente = idCliente,
                IdTipoEquipo = idTipoEquipo,
                IdMarca = idMarca,
                Modelo = modelo ?? "",
                NumeroSerie = numeroSerie ?? "",
                Imei = imei ?? "",
                Color = color ?? "",
                Observaciones = observaciones ?? "",
                Activo = activo
            };
        }
    }
}
