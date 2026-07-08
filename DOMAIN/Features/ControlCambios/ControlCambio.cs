using System;

namespace DOMAIN.Features.ControlCambios
{
    public class ControlCambio
    {
        public int Id { get; private set; }
        public string TablaAfectada { get; private set; }
        public int IdIdioma { get; private set; }
        public int IdPalabra { get; private set; }
        public string ClaveRegistro { get; private set; }
        public string CampoModificado { get; private set; }
        public string ValorAnterior { get; private set; }
        public string ValorNuevo { get; private set; }
        public string UsuarioModifico { get; private set; }
        public DateTime FechaCambio { get; private set; }
        public string TipoCambio { get; private set; }

        private ControlCambio() { }

        public static ControlCambio Crear(int id, string tablaAfectada, int idIdioma, int idPalabra,
            string claveRegistro, string campoModificado, string valorAnterior, string valorNuevo,
            string usuarioModifico, DateTime fechaCambio, string tipoCambio)
        {
            return new ControlCambio
            {
                Id = id,
                TablaAfectada = tablaAfectada,
                IdIdioma = idIdioma,
                IdPalabra = idPalabra,
                ClaveRegistro = claveRegistro,
                CampoModificado = campoModificado,
                ValorAnterior = valorAnterior,
                ValorNuevo = valorNuevo,
                UsuarioModifico = usuarioModifico,
                FechaCambio = fechaCambio,
                TipoCambio = tipoCambio
            };
        }
    }
}
