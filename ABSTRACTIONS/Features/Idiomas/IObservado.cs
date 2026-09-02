using System.Collections.Generic;

namespace ABSTRACTIONS.Features.Idiomas
{
    public interface IObservado
    {
        IList<IObservador> ObservadoresRegistrados { get; }
        void RegistrarObservador(IObservador observador);
        void DesregistrarObservador(IObservador observador);
        void ActualizarObservadores(IIdioma idioma);
    }
}
