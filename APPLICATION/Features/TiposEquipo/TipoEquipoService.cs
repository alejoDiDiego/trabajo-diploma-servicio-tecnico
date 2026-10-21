using System;
using System.Collections.Generic;
using APPLICATION.Features.Bitacora;
using DOMAIN.Exceptions;
using DOMAIN.Features.TiposEquipo;
using REPOSITORY.Features.TiposEquipo;

namespace APPLICATION.Features.TiposEquipo
{
    public class TipoEquipoService
    {
        private readonly TipoEquipoRepository _tipoEquipoRepository;

        public TipoEquipoService()
        {
            _tipoEquipoRepository = new TipoEquipoRepository();
        }

        public void Inicializar()
        {
            _tipoEquipoRepository.Inicializar();
        }

        public TipoEquipo Crear(string nombre)
        {
            try
            {
                TipoEquipo tipoToSave = TipoEquipo.CrearNuevo(nombre);

                TipoEquipo tipoDb = _tipoEquipoRepository.Agregar(tipoToSave);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Creacion de tipo de equipo",
                    "id=" + tipoDb.Id + " | nombre=" + tipoDb.Nombre, "TIPOS_EQUIPO");

                return tipoDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear tipo de equipo", ex);
            }
        }

        public TipoEquipo Modificar(int id, string nombre)
        {
            try
            {
                TipoEquipo tipoDb = _tipoEquipoRepository.ObtenerPorId(id);

                if (tipoDb == null)
                    throw new ReglaNegocioException("El tipo de equipo seleccionado no existe.");

                TipoEquipo validado = TipoEquipo.CrearNuevo(nombre);

                TipoEquipo actualizado = TipoEquipo.CargarDesdeDB(id, validado.Nombre, tipoDb.Activo);

                _tipoEquipoRepository.Modificar(actualizado);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Modificacion de tipo de equipo",
                    "id=" + id + " | nombre=" + actualizado.Nombre, "TIPOS_EQUIPO");

                return actualizado;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar tipo de equipo", ex);
            }
        }

        public void Desactivar(int id)
        {
            try
            {
                TipoEquipo tipoDb = _tipoEquipoRepository.ObtenerPorId(id);

                if (tipoDb == null)
                    throw new ReglaNegocioException("El tipo de equipo seleccionado no existe.");

                _tipoEquipoRepository.Desactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Desactivacion de tipo de equipo", "id=" + id, "TIPOS_EQUIPO");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar tipo de equipo", ex);
            }
        }

        public void Reactivar(int id)
        {
            try
            {
                TipoEquipo tipoDb = _tipoEquipoRepository.ObtenerPorId(id);

                if (tipoDb == null)
                    throw new ReglaNegocioException("El tipo de equipo seleccionado no existe.");

                _tipoEquipoRepository.Reactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Reactivacion de tipo de equipo", "id=" + id, "TIPOS_EQUIPO");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reactivar tipo de equipo", ex);
            }
        }

        public List<TipoEquipo> Listar(bool incluirInactivos = false)
        {
            try
            {
                return _tipoEquipoRepository.Listar(incluirInactivos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar tipos de equipo", ex);
            }
        }

        public TipoEquipo ObtenerPorId(int id)
        {
            try
            {
                TipoEquipo tipo = _tipoEquipoRepository.ObtenerPorId(id);

                if (tipo == null)
                    throw new ReglaNegocioException("El tipo de equipo seleccionado no existe.");

                return tipo;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tipo de equipo", ex);
            }
        }
    }
}
