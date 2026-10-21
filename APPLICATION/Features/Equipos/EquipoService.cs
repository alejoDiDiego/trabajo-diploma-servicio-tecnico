using System;
using System.Collections.Generic;
using APPLICATION.Features.Bitacora;
using DOMAIN.Exceptions;
using DOMAIN.Features.Clientes;
using DOMAIN.Features.Equipos;
using DOMAIN.Features.Marcas;
using DOMAIN.Features.TiposEquipo;
using REPOSITORY.Features.Clientes;
using REPOSITORY.Features.Equipos;
using REPOSITORY.Features.Marcas;
using REPOSITORY.Features.TiposEquipo;

namespace APPLICATION.Features.Equipos
{
    public class EquipoService
    {
        private readonly EquipoRepository _equipoRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly TipoEquipoRepository _tipoEquipoRepository;
        private readonly MarcaRepository _marcaRepository;

        public EquipoService()
        {
            _equipoRepository = new EquipoRepository();
            _clienteRepository = new ClienteRepository();
            _tipoEquipoRepository = new TipoEquipoRepository();
            _marcaRepository = new MarcaRepository();
        }

        public void Inicializar()
        {
            _equipoRepository.Inicializar();
        }

        public Equipo Crear(int idCliente, int idTipoEquipo, int idMarca,
            string modelo, string numeroSerie, string imei, string color, string observaciones)
        {
            try
            {
                Equipo equipoToSave = Equipo.CrearNuevo(
                    idCliente, idTipoEquipo, idMarca, modelo, numeroSerie, imei, color, observaciones);

                ValidarReferenciasExistentesYActivas(
                    equipoToSave.IdCliente, equipoToSave.IdTipoEquipo, equipoToSave.IdMarca);

                Equipo equipoDb = _equipoRepository.Agregar(equipoToSave);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Creacion de equipo",
                    "id=" + equipoDb.Id + " | id_cliente=" + equipoDb.IdCliente, "EQUIPOS");

                return equipoDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear equipo", ex);
            }
        }

        public Equipo Modificar(int id, int idCliente, int idTipoEquipo, int idMarca,
            string modelo, string numeroSerie, string imei, string color, string observaciones)
        {
            try
            {
                Equipo equipoDb = _equipoRepository.ObtenerPorId(id);

                if (equipoDb == null)
                    throw new ReglaNegocioException("El equipo seleccionado no existe.");

                Equipo validado = Equipo.CrearNuevo(
                    idCliente, idTipoEquipo, idMarca, modelo, numeroSerie, imei, color, observaciones);

                ValidarReferenciasExistentesYActivas(
                    validado.IdCliente, validado.IdTipoEquipo, validado.IdMarca);

                Equipo actualizado = Equipo.CargarDesdeDB(
                    equipoDb.Id,
                    validado.IdCliente,
                    validado.IdTipoEquipo,
                    validado.IdMarca,
                    validado.Modelo,
                    validado.NumeroSerie,
                    validado.Imei,
                    validado.Color,
                    validado.Observaciones,
                    equipoDb.Activo
                );

                _equipoRepository.Modificar(actualizado);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Modificacion de equipo",
                    "id=" + id + " | id_cliente=" + actualizado.IdCliente, "EQUIPOS");

                return actualizado;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar equipo", ex);
            }
        }

        public void Desactivar(int id)
        {
            try
            {
                Equipo equipoDb = _equipoRepository.ObtenerPorId(id);

                if (equipoDb == null)
                    throw new ReglaNegocioException("El equipo seleccionado no existe.");

                _equipoRepository.Desactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Desactivacion de equipo", "id=" + id, "EQUIPOS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar equipo", ex);
            }
        }

        public void Reactivar(int id)
        {
            try
            {
                Equipo equipoDb = _equipoRepository.ObtenerPorId(id);

                if (equipoDb == null)
                    throw new ReglaNegocioException("El equipo seleccionado no existe.");

                _equipoRepository.Reactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Reactivacion de equipo", "id=" + id, "EQUIPOS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reactivar equipo", ex);
            }
        }

        public List<Equipo> Listar(bool incluirInactivos = false)
        {
            try
            {
                return _equipoRepository.Listar(incluirInactivos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar equipos", ex);
            }
        }

        public List<Equipo> ListarPorCliente(int idCliente, bool incluirInactivos = false)
        {
            try
            {
                return _equipoRepository.ListarPorCliente(idCliente, incluirInactivos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar equipos del cliente", ex);
            }
        }

        public Equipo ObtenerPorId(int id)
        {
            try
            {
                Equipo equipo = _equipoRepository.ObtenerPorId(id);

                if (equipo == null)
                    throw new ReglaNegocioException("El equipo seleccionado no existe.");

                return equipo;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener equipo", ex);
            }
        }

        private void ValidarReferenciasExistentesYActivas(int idCliente, int idTipoEquipo, int idMarca)
        {
            // La existencia y el estado de las FK se valida en el service, no en la entidad.
            Cliente cliente = _clienteRepository.ObtenerPorId(idCliente);

            if (cliente == null)
                throw new ReglaNegocioException("El cliente seleccionado no existe.");

            if (!cliente.Activo)
                throw new ReglaNegocioException("El cliente seleccionado esta inactivo.");

            TipoEquipo tipo = _tipoEquipoRepository.ObtenerPorId(idTipoEquipo);

            if (tipo == null)
                throw new ReglaNegocioException("El tipo de equipo seleccionado no existe.");

            if (!tipo.Activo)
                throw new ReglaNegocioException("El tipo de equipo seleccionado esta inactivo.");

            Marca marca = _marcaRepository.ObtenerPorId(idMarca);

            if (marca == null)
                throw new ReglaNegocioException("La marca seleccionada no existe.");

            if (!marca.Activo)
                throw new ReglaNegocioException("La marca seleccionada esta inactiva.");
        }
    }
}
