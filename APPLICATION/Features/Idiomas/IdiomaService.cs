using System;
using System.Collections.Generic;
using System.Linq;
using ABSTRACTIONS.Features.Idiomas;
using APPLICATION.Features.ControlCambios;
using DOMAIN.Features.Idiomas;
using REPOSITORY.Features.Idiomas;
using SERVICES.Auth;

namespace APPLICATION.Features.Idiomas
{
    public class IdiomaService
    {
        private readonly IdiomaRepository _idiomaRepository;

        public IdiomaService()
        {
            _idiomaRepository = new IdiomaRepository();
        }

        public void Inicializar()
        {
            _idiomaRepository.Inicializar();
        }

        public List<Idioma> Listar()
        {
            return _idiomaRepository.Listar();
        }

        public Idioma ObtenerPorId(int id)
        {
            return _idiomaRepository.ObtenerPorId(id);
        }

        public Idioma ObtenerPorNombre(string nombre)
        {
            return _idiomaRepository.ObtenerPorNombre(nombre);
        }

        public Idioma ObtenerIdiomaPorDefecto()
        {
            Idioma idioma = ObtenerPorNombre("Espanol");

            if (idioma != null)
                return idioma;

            return Listar().FirstOrDefault();
        }

        public List<TraduccionEditable> ListarTraduccionesPorIdioma(int idIdioma)
        {
            return _idiomaRepository.ListarTraduccionesPorIdioma(idIdioma);
        }

        public Idioma CrearIdioma(string nombre)
        {
            Idioma idioma = Idioma.Crear(0, nombre);
            return _idiomaRepository.AgregarIdioma(idioma.Nombre);
        }

        public void ModificarIdioma(int id, string nombre)
        {
            Idioma idioma = Idioma.Crear(id, nombre);
            _idiomaRepository.ModificarIdioma(idioma.Id, idioma.Nombre);
        }

        public void EliminarIdioma(int id)
        {
            _idiomaRepository.EliminarIdioma(id);
        }

        public void GuardarTraduccion(int idIdioma, int idPalabra, string texto, bool registrarCambio = true)
        {
            string valorAnterior = "";
            string clave = "";

            if (registrarCambio)
            {
                var traducciones = _idiomaRepository.ListarTraduccionesPorIdioma(idIdioma);
                var vieja = traducciones.FirstOrDefault(t => t.IdPalabra == idPalabra);
                if (vieja != null)
                {
                    valorAnterior = vieja.Texto;
                    clave = vieja.Clave;
                }
            }

            _idiomaRepository.GuardarTraduccion(idIdioma, idPalabra, texto);

            if (registrarCambio && texto != valorAnterior)
            {
                string usuario = SessionManager.HaySesionActiva()
                    ? SessionManager.ObtenerUsuarioActual().Username
                    : "Sistema";

                string tipo = string.IsNullOrEmpty(valorAnterior) ? "INSERT" : "UPDATE";

                ControlCambioService controlCambioService = new ControlCambioService();
                controlCambioService.RegistrarCambio(
                    "Traducciones", idIdioma, idPalabra, clave,
                    "palabra_traducida", valorAnterior, texto,
                    usuario, tipo
                );
            }
        }
    }
}
