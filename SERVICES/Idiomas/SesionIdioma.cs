using System.Collections.Generic;
using ABSTRACTIONS.Features.Idiomas;

namespace SERVICES.Idiomas
{
    public class SesionIdioma : IObservado
    {
        public IIdioma idioma { get; set; }
        public IList<IObservador> ObservadoresRegistrados { get; private set; }
        private static SesionIdioma sesionIdioma;

        private SesionIdioma()
        {
            ObservadoresRegistrados = new List<IObservador>();
        }

        public static SesionIdioma GetInstance()
        {
            if (sesionIdioma == null)
                sesionIdioma = new SesionIdioma();

            return sesionIdioma;
        }

        public void CambiarIdioma(IIdioma idiomaSeleccionado)
        {
            idioma = idiomaSeleccionado;
            ActualizarObservadores(idiomaSeleccionado);
        }

        public void ActualizarObservadores(IIdioma idiomaSeleccionado)
        {
            foreach (IObservador observador in ObservadoresRegistrados)
                observador.Actualizar(idiomaSeleccionado);
        }

        public void RegistrarObservador(IObservador observador)
        {
            if (observador == null)
                return;

            if (!ObservadoresRegistrados.Contains(observador))
                ObservadoresRegistrados.Add(observador);
        }

        public void DesregistrarObservador(IObservador observador)
        {
            if (observador == null)
                return;

            if (ObservadoresRegistrados.Contains(observador))
                ObservadoresRegistrados.Remove(observador);
        }
    }
}
