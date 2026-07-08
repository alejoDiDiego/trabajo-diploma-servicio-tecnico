using System;

namespace DOMAIN.Features.Bitacora
{
    public class EntradaBitacora
    {
        public int Id { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Usuario { get; private set; }
        public string Actividad { get; private set; }
        public string Detalle { get; private set; }
        public string TipoActividad { get; private set; }

        private EntradaBitacora() { }

        public static EntradaBitacora Crear(int id, DateTime fecha, string usuario, string actividad,
            string detalle, string tipoActividad)
        {
            return new EntradaBitacora
            {
                Id = id,
                Fecha = fecha,
                Usuario = usuario,
                Actividad = actividad,
                Detalle = detalle ?? "",
                TipoActividad = tipoActividad
            };
        }
    }
}
