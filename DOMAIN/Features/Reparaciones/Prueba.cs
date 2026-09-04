using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Reparaciones
{
    public class Prueba
    {
        public int Id { get; private set; }
        public int IdReparacion { get; private set; }
        public int IdUsuarioTecnico { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Descripcion { get; private set; }
        public string Resultado { get; private set; }
        public string Observaciones { get; private set; }

        private Prueba() { }

        public static Prueba CrearNuevo(int idReparacion, int idUsuarioTecnico,
            string descripcion, string resultado, string observaciones)
        {
            if (idReparacion <= 0)
                throw new ReglaNegocioException("La reparacion de la prueba es obligatoria.");
            if (idUsuarioTecnico <= 0)
                throw new ReglaNegocioException("El tecnico de la prueba es obligatorio.");
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ReglaNegocioException("La descripcion de la prueba es obligatoria.");
            if (resultado != ResultadoPrueba.Aprobada && resultado != ResultadoPrueba.RequiereRevision)
                throw new ReglaNegocioException("El resultado de la prueba no es valido.");

            return new Prueba
            {
                IdReparacion = idReparacion,
                IdUsuarioTecnico = idUsuarioTecnico,
                Fecha = DateTime.Now,
                Descripcion = descripcion.Trim(),
                Resultado = resultado,
                Observaciones = observaciones == null ? "" : observaciones.Trim()
            };
        }

        public static Prueba CargarDesdeDB(int id, int idReparacion, int idUsuarioTecnico,
            DateTime fecha, string descripcion, string resultado, string observaciones)
        {
            return new Prueba
            {
                Id = id,
                IdReparacion = idReparacion,
                IdUsuarioTecnico = idUsuarioTecnico,
                Fecha = fecha,
                Descripcion = descripcion ?? "",
                Resultado = resultado ?? "",
                Observaciones = observaciones ?? ""
            };
        }
    }
}
