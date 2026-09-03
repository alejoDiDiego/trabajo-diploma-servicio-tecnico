using System;
using System.Collections.Generic;
using APPLICATION.Features.Bitacora;
using DOMAIN.Exceptions;
using DOMAIN.Features.Clientes;
using REPOSITORY.Features.Clientes;

namespace APPLICATION.Features.Clientes
{
    public class ClienteService
    {
        private readonly ClienteRepository _clienteRepository;

        public ClienteService()
        {
            _clienteRepository = new ClienteRepository();
        }

        public void Inicializar()
        {
            _clienteRepository.Inicializar();
        }

        public Cliente Crear(string nombre, string apellido, string documento,
            string telefono, string email, string direccion, string observaciones)
        {
            try
            {
                Cliente clienteToSave = Cliente.CrearNuevo(
                    nombre, apellido, documento, telefono, email, direccion, observaciones);

                Cliente clienteDb = _clienteRepository.Agregar(clienteToSave);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Creacion de cliente",
                    "id=" + clienteDb.Id + " | documento=" + clienteDb.Documento, "CLIENTES");

                return clienteDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear cliente", ex);
            }
        }

        public Cliente Modificar(int id, string nombre, string apellido, string documento,
            string telefono, string email, string direccion, string observaciones)
        {
            try
            {
                Cliente clienteDb = _clienteRepository.ObtenerPorId(id);

                if (clienteDb == null)
                    throw new ReglaNegocioException("El cliente seleccionado no existe.");

                Cliente clienteToUpdate = Cliente.CrearNuevo(
                    nombre, apellido, documento, telefono, email, direccion, observaciones);

                Cliente actualizado = Cliente.CargarDesdeDB(
                    clienteDb.Id,
                    clienteToUpdate.Nombre,
                    clienteToUpdate.Apellido,
                    clienteToUpdate.Documento,
                    clienteToUpdate.Telefono,
                    clienteToUpdate.Email,
                    clienteToUpdate.Direccion,
                    clienteToUpdate.Observaciones,
                    clienteDb.Activo,
                    clienteDb.FechaAlta
                );

                _clienteRepository.Modificar(actualizado);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Modificacion de cliente",
                    "id=" + id + " | documento=" + actualizado.Documento, "CLIENTES");

                return actualizado;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar cliente", ex);
            }
        }

        public void Desactivar(int id)
        {
            try
            {
                Cliente clienteDb = _clienteRepository.ObtenerPorId(id);

                if (clienteDb == null)
                    throw new ReglaNegocioException("El cliente seleccionado no existe.");

                _clienteRepository.Desactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Desactivacion de cliente", "id=" + id, "CLIENTES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar cliente", ex);
            }
        }

        public void Reactivar(int id)
        {
            try
            {
                Cliente clienteDb = _clienteRepository.ObtenerPorId(id);

                if (clienteDb == null)
                    throw new ReglaNegocioException("El cliente seleccionado no existe.");

                _clienteRepository.Reactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Reactivacion de cliente", "id=" + id, "CLIENTES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reactivar cliente", ex);
            }
        }

        public List<Cliente> Listar(bool incluirInactivos = false)
        {
            try
            {
                return _clienteRepository.Listar(incluirInactivos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar clientes", ex);
            }
        }

        public Cliente ObtenerPorId(int id)
        {
            try
            {
                Cliente cliente = _clienteRepository.ObtenerPorId(id);

                if (cliente == null)
                    throw new ReglaNegocioException("El cliente seleccionado no existe.");

                return cliente;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener cliente", ex);
            }
        }
    }
}
