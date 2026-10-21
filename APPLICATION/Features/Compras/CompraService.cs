using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APPLICATION.Features.Bitacora;
using DOMAIN.Exceptions;
using DOMAIN.Features.Compras;
using DOMAIN.Features.Proveedores;
using DOMAIN.Features.Repuestos;
using REPOSITORY.Features.Compras;
using REPOSITORY.Features.Proveedores;
using REPOSITORY.Features.Repuestos;
using SERVICES.Auth;

namespace APPLICATION.Features.Compras
{
    public class CompraService
    {
        private readonly CompraRepository _compraRepository;
        private readonly ProveedorRepository _proveedorRepository;
        private readonly RepuestoRepository _repuestoRepository;

        public CompraService()
        {
            _compraRepository = new CompraRepository();
            _proveedorRepository = new ProveedorRepository();
            _repuestoRepository = new RepuestoRepository();
        }

        public void Inicializar()
        {
            _compraRepository.Inicializar();
        }

        public Compra CrearBorrador(int idProveedor, List<DetalleCompra> items, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Proveedor proveedor = _proveedorRepository.ObtenerPorId(idProveedor);

                if (proveedor == null)
                    throw new ReglaNegocioException("El proveedor seleccionado no existe.");

                if (!proveedor.Activo)
                    throw new ReglaNegocioException("El proveedor seleccionado esta inactivo.");

                List<DetalleCompra> validados = ValidarItems(items);

                Compra compraToSave = Compra.CrearBorrador(idProveedor, idUsuario, observaciones);

                Compra compraDb = _compraRepository.CrearBorradorConDetalle(compraToSave, validados);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Compra creada",
                    "id=" + compraDb.Id + " | id_proveedor=" + idProveedor, "COMPRAS");

                return compraDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear compra", ex);
            }
        }

        public void AgregarItem(int idCompra, int idRepuesto, int cantidad, decimal costoUnitario)
        {
            try
            {
                ObtenerIdUsuarioSesion();
                Compra compraDb = ObtenerCompraExistente(idCompra);

                if (compraDb.Estado != EstadoCompra.Borrador)
                    throw new ReglaNegocioException("Solo se puede modificar una compra en borrador.");

                Repuesto repuesto = _repuestoRepository.ObtenerPorId(idRepuesto);

                if (repuesto == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                if (!repuesto.Activo)
                    throw new ReglaNegocioException("El repuesto seleccionado esta inactivo.");

                DetalleCompra item = DetalleCompra.CrearNuevo(idCompra, idRepuesto, cantidad, costoUnitario);

                _compraRepository.AgregarItem(idCompra, item);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Compra item agregado",
                    "id_compra=" + idCompra + " | id_repuesto=" + idRepuesto + " | cantidad=" + cantidad, "COMPRAS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar item a la compra", ex);
            }
        }

        public void QuitarItem(int idCompra, int idDetalle)
        {
            try
            {
                ObtenerIdUsuarioSesion();
                Compra compraDb = ObtenerCompraExistente(idCompra);

                if (compraDb.Estado != EstadoCompra.Borrador)
                    throw new ReglaNegocioException("Solo se puede modificar una compra en borrador.");

                List<DetalleCompra> detalle = _compraRepository.ListarDetalle(idCompra);
                bool pertenece = false;

                foreach (DetalleCompra d in detalle)
                {
                    if (d.Id == idDetalle)
                    {
                        pertenece = true;
                        break;
                    }
                }

                if (!pertenece)
                    throw new ReglaNegocioException("El detalle seleccionado no pertenece a la compra.");

                _compraRepository.QuitarItem(idDetalle);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Compra item quitado",
                    "id_compra=" + idCompra + " | id_detalle=" + idDetalle, "COMPRAS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al quitar item de la compra", ex);
            }
        }

        public Compra Confirmar(int idCompra)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                Compra compraDb = ObtenerCompraExistente(idCompra);

                if (compraDb.Estado != EstadoCompra.Borrador)
                    throw new ReglaNegocioException("Solo se puede confirmar una compra en borrador.");

                Compra confirmada = _compraRepository.ConfirmarConStock(idCompra, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Compra confirmada",
                    "id=" + idCompra + " | total=" + confirmada.Total, "COMPRAS");

                return confirmada;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al confirmar compra", ex);
            }
        }

        public void Cancelar(int idCompra)
        {
            // Permiso COMPRAS_CANCELAR (mismo codigo cubre Cancelar borrador y Anular confirmada).
            try
            {
                ObtenerIdUsuarioSesion();
                Compra compraDb = ObtenerCompraExistente(idCompra);

                compraDb.CancelarBorrador();

                _compraRepository.CancelarBorrador(idCompra);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Compra cancelada", "id=" + idCompra, "COMPRAS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar compra", ex);
            }
        }

        public void Anular(int idCompra, string motivo)
        {
            // Permiso COMPRAS_CANCELAR (mismo codigo que Cancelar borrador, documentado).
            // Anula una Confirmada con motivo obligatorio: si el stock cubre la reversion
            // completa pasa a Cancelada + revierte stock con AjusteNegativo por item;
            // si no cubre, el repository hace THROW 50035 sin cambios (consumos posteriores).
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                if (string.IsNullOrWhiteSpace(motivo))
                    throw new ReglaNegocioException("El motivo de la anulacion es obligatorio.");

                Compra compraDb = ObtenerCompraExistente(idCompra);

                compraDb.AnularConfirmada(motivo.Trim());

                _compraRepository.AnularConfirmadaConStock(idCompra, compraDb.MotivoAnulacion, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Compra anulada",
                    "id=" + idCompra + " | motivo=" + compraDb.MotivoAnulacion, "COMPRAS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (SqlException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al anular compra", ex);
            }
        }

        public Compra ObtenerPorId(int id)
        {
            try
            {
                return ObtenerCompraExistente(id);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener compra", ex);
            }
        }

        public List<Compra> Listar()
        {
            try
            {
                return _compraRepository.Listar(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar compras", ex);
            }
        }

        public List<DetalleCompra> ListarDetalle(int idCompra)
        {
            try
            {
                ObtenerCompraExistente(idCompra);
                return _compraRepository.ListarDetalle(idCompra);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar detalle de la compra", ex);
            }
        }

        private List<DetalleCompra> ValidarItems(List<DetalleCompra> items)
        {
            if (items == null || items.Count == 0)
                throw new ReglaNegocioException("La compra debe tener al menos un item.");

            List<DetalleCompra> validados = new List<DetalleCompra>();

            foreach (DetalleCompra item in items)
            {
                Repuesto repuesto = _repuestoRepository.ObtenerPorId(item.IdRepuesto);

                if (repuesto == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                if (!repuesto.Activo)
                    throw new ReglaNegocioException("El repuesto seleccionado esta inactivo.");

                validados.Add(DetalleCompra.CrearNuevo(0, item.IdRepuesto, item.Cantidad, item.CostoUnitario));
            }

            return validados;
        }

        private Compra ObtenerCompraExistente(int id)
        {
            Compra compra = _compraRepository.ObtenerPorId(id);

            if (compra == null)
                throw new ReglaNegocioException("La compra seleccionada no existe.");

            return compra;
        }

        private int ObtenerIdUsuarioSesion()
        {
            if (!SessionManager.HaySesionActiva() || SessionManager.ObtenerUsuarioActual() == null)
                throw new ReglaNegocioException("Debe iniciar sesion para gestionar compras.");

            return SessionManager.ObtenerUsuarioActual().Id;
        }
    }
}
