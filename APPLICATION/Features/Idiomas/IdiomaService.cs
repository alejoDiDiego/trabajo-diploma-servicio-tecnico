using System.Collections.Generic;
using System.Linq;
using DOMAIN.Features.Idiomas;
using REPOSITORY.Features.Idiomas;

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

        public List<TraduccionItem> ListarTraducciones()
        {
            return _idiomaRepository.ListarTraducciones();
        }

        public Idioma CrearIdioma(string nombre)
        {
            return _idiomaRepository.AgregarIdioma(nombre);
        }

        public void ModificarIdioma(int id, string nombre)
        {
            _idiomaRepository.ModificarIdioma(id, nombre);
        }

        public void EliminarIdioma(int id)
        {
            _idiomaRepository.EliminarIdioma(id);
        }

        public TraduccionItem CrearTraduccion(int idIdioma, string clave, string texto)
        {
            return _idiomaRepository.AgregarTraduccion(idIdioma, clave, texto);
        }

        public void ModificarTraduccion(int idTraduccion, int idIdioma, string texto)
        {
            _idiomaRepository.ModificarTraduccion(idTraduccion, idIdioma, texto);
        }

        public void EliminarTraduccion(int idTraduccion)
        {
            _idiomaRepository.EliminarTraduccion(idTraduccion);
        }
    }
}
