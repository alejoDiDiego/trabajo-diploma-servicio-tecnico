using System;
using System.Collections.Generic;
using APPLICATION.Features.Bitacora;
using DOMAIN.Exceptions;
using DOMAIN.Features.Marcas;
using REPOSITORY.Features.Marcas;

namespace APPLICATION.Features.Marcas
{
    public class MarcaService
    {
        private readonly MarcaRepository _marcaRepository;

        public MarcaService()
        {
            _marcaRepository = new MarcaRepository();
        }

        public void Inicializar()
        {
            _marcaRepository.Inicializar();
        }

        public Marca Crear(string nombre)
        {
            try
            {
                Marca marcaToSave = Marca.CrearNuevo(nombre);

                Marca marcaDb = _marcaRepository.Agregar(marcaToSave);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Creacion de marca",
                    "id=" + marcaDb.Id + " | nombre=" + marcaDb.Nombre, "MARCAS");

                return marcaDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear marca", ex);
            }
        }

        public Marca Modificar(int id, string nombre)
        {
            try
            {
                Marca marcaDb = _marcaRepository.ObtenerPorId(id);

                if (marcaDb == null)
                    throw new ReglaNegocioException("La marca seleccionada no existe.");

                Marca validada = Marca.CrearNuevo(nombre);

                Marca actualizada = Marca.CargarDesdeDB(id, validada.Nombre, marcaDb.Activo);

                _marcaRepository.Modificar(actualizada);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Modificacion de marca",
                    "id=" + id + " | nombre=" + actualizada.Nombre, "MARCAS");

                return actualizada;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar marca", ex);
            }
        }

        public void Desactivar(int id)
        {
            try
            {
                Marca marcaDb = _marcaRepository.ObtenerPorId(id);

                if (marcaDb == null)
                    throw new ReglaNegocioException("La marca seleccionada no existe.");

                _marcaRepository.Desactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Desactivacion de marca", "id=" + id, "MARCAS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar marca", ex);
            }
        }

        public void Reactivar(int id)
        {
            try
            {
                Marca marcaDb = _marcaRepository.ObtenerPorId(id);

                if (marcaDb == null)
                    throw new ReglaNegocioException("La marca seleccionada no existe.");

                _marcaRepository.Reactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Reactivacion de marca", "id=" + id, "MARCAS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reactivar marca", ex);
            }
        }

        public List<Marca> Listar(bool incluirInactivos = false)
        {
            try
            {
                return _marcaRepository.Listar(incluirInactivos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar marcas", ex);
            }
        }

        public Marca ObtenerPorId(int id)
        {
            try
            {
                Marca marca = _marcaRepository.ObtenerPorId(id);

                if (marca == null)
                    throw new ReglaNegocioException("La marca seleccionada no existe.");

                return marca;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener marca", ex);
            }
        }
    }
}
