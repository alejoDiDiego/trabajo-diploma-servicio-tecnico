using System;
using System.Collections.Generic;
using APPLICATION.Features.Bitacora;
using DOMAIN.Exceptions;
using DOMAIN.Features.Proveedores;
using REPOSITORY.Features.Proveedores;

namespace APPLICATION.Features.Proveedores
{
    public class ProveedorService
    {
        private readonly ProveedorRepository _proveedorRepository;

        public ProveedorService()
        {
            _proveedorRepository = new ProveedorRepository();
        }

        public void Inicializar()
        {
            _proveedorRepository.Inicializar();
        }

        public Proveedor Crear(string razonSocial, string cuit, string telefono,
            string email, string direccion, string contacto)
        {
            try
            {
                Proveedor proveedorToSave = Proveedor.CrearNuevo(
                    razonSocial, cuit, telefono, email, direccion, contacto);

                Proveedor proveedorDb = _proveedorRepository.Agregar(proveedorToSave);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Creacion de proveedor",
                    "id=" + proveedorDb.Id + " | razon_social=" + proveedorDb.RazonSocial, "PROVEEDORES");

                return proveedorDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear proveedor", ex);
            }
        }

        public Proveedor Modificar(int id, string razonSocial, string cuit, string telefono,
            string email, string direccion, string contacto)
        {
            try
            {
                Proveedor proveedorDb = _proveedorRepository.ObtenerPorId(id);

                if (proveedorDb == null)
                    throw new ReglaNegocioException("El proveedor seleccionado no existe.");

                Proveedor validado = Proveedor.CrearNuevo(
                    razonSocial, cuit, telefono, email, direccion, contacto);

                Proveedor actualizado = Proveedor.CargarDesdeDB(id, validado.RazonSocial,
                    validado.Cuit, validado.Telefono, validado.Email, validado.Direccion,
                    validado.Contacto, proveedorDb.Activo);

                _proveedorRepository.Modificar(actualizado);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Modificacion de proveedor",
                    "id=" + id + " | razon_social=" + actualizado.RazonSocial, "PROVEEDORES");

                return actualizado;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar proveedor", ex);
            }
        }

        public void Desactivar(int id)
        {
            try
            {
                Proveedor proveedorDb = _proveedorRepository.ObtenerPorId(id);

                if (proveedorDb == null)
                    throw new ReglaNegocioException("El proveedor seleccionado no existe.");

                _proveedorRepository.Desactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Desactivacion de proveedor", "id=" + id, "PROVEEDORES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar proveedor", ex);
            }
        }

        public void Reactivar(int id)
        {
            try
            {
                Proveedor proveedorDb = _proveedorRepository.ObtenerPorId(id);

                if (proveedorDb == null)
                    throw new ReglaNegocioException("El proveedor seleccionado no existe.");

                _proveedorRepository.Reactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Reactivacion de proveedor", "id=" + id, "PROVEEDORES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reactivar proveedor", ex);
            }
        }

        public List<Proveedor> Listar(bool incluirInactivos = false)
        {
            try
            {
                return _proveedorRepository.Listar(incluirInactivos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar proveedores", ex);
            }
        }

        public Proveedor ObtenerPorId(int id)
        {
            try
            {
                Proveedor proveedor = _proveedorRepository.ObtenerPorId(id);

                if (proveedor == null)
                    throw new ReglaNegocioException("El proveedor seleccionado no existe.");

                return proveedor;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener proveedor", ex);
            }
        }
    }
}
