using System;
using System.Collections.Generic;
using APPLICATION.Features.Bitacora;
using DOMAIN.Exceptions;
using DOMAIN.Features.Repuestos;
using REPOSITORY.Features.Repuestos;
using SERVICES.Auth;

namespace APPLICATION.Features.Repuestos
{
    public class RepuestoService
    {
        private readonly RepuestoRepository _repuestoRepository;
        private readonly MovimientoStockRepository _movimientoRepository;

        public RepuestoService()
        {
            _repuestoRepository = new RepuestoRepository();
            _movimientoRepository = new MovimientoStockRepository();
        }

        public void Inicializar()
        {
            _repuestoRepository.Inicializar();
            _movimientoRepository.Inicializar();
        }

        public Repuesto Crear(string codigo, string descripcion, int stockInicial,
            int stockMinimo, decimal costoActual, decimal precioReferencia)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Repuesto repuestoToSave = Repuesto.CrearNuevo(codigo, descripcion,
                    stockInicial, stockMinimo, costoActual, precioReferencia);

                Repuesto repuestoDb = _repuestoRepository.Crear(repuestoToSave, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Creacion de repuesto",
                    "id=" + repuestoDb.Id + " | codigo=" + repuestoDb.Codigo, "REPUESTOS");

                return repuestoDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear repuesto", ex);
            }
        }

        public Repuesto Modificar(int id, string codigo, string descripcion,
            int stockMinimo, decimal costoActual, decimal precioReferencia)
        {
            try
            {
                Repuesto repuestoDb = _repuestoRepository.ObtenerPorId(id);

                if (repuestoDb == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                Repuesto validado = Repuesto.CrearNuevo(codigo, descripcion,
                    repuestoDb.StockActual, stockMinimo, costoActual, precioReferencia);

                Repuesto actualizado = Repuesto.CargarDesdeDB(id, validado.Codigo,
                    validado.Descripcion, repuestoDb.StockActual, validado.StockMinimo,
                    validado.CostoActual, validado.PrecioReferencia, repuestoDb.Activo);

                _repuestoRepository.Modificar(actualizado);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Modificacion de repuesto",
                    "id=" + id + " | codigo=" + actualizado.Codigo, "REPUESTOS");

                return actualizado;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar repuesto", ex);
            }
        }

        public void AjustarStock(int idRepuesto, int cantidadFirmada, string motivo, int? idUsuario = null)
        {
            try
            {
                if (cantidadFirmada == 0)
                    throw new ReglaNegocioException("La cantidad del ajuste no puede ser cero.");
                if (string.IsNullOrWhiteSpace(motivo))
                    throw new ReglaNegocioException("El motivo del ajuste es obligatorio.");

                Repuesto repuestoDb = _repuestoRepository.ObtenerPorId(idRepuesto);

                if (repuestoDb == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                if (repuestoDb.StockActual + cantidadFirmada < 0)
                    throw new ReglaNegocioException("Stock insuficiente para el ajuste.");

                int idUsuarioAjuste = idUsuario.HasValue ? idUsuario.Value : ObtenerIdUsuarioSesion();

                _repuestoRepository.AjustarStock(idRepuesto, cantidadFirmada, motivo.Trim(), idUsuarioAjuste);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Ajuste de stock",
                    "id_repuesto=" + idRepuesto + " | cantidad=" + cantidadFirmada + " | motivo=" + motivo.Trim(), "REPUESTOS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ajustar stock", ex);
            }
        }

        public void Desactivar(int id)
        {
            try
            {
                Repuesto repuestoDb = _repuestoRepository.ObtenerPorId(id);

                if (repuestoDb == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                _repuestoRepository.Desactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Desactivacion de repuesto", "id=" + id, "REPUESTOS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar repuesto", ex);
            }
        }

        public void Reactivar(int id)
        {
            try
            {
                Repuesto repuestoDb = _repuestoRepository.ObtenerPorId(id);

                if (repuestoDb == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                _repuestoRepository.Reactivar(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Reactivacion de repuesto", "id=" + id, "REPUESTOS");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reactivar repuesto", ex);
            }
        }

        public List<Repuesto> Listar(bool incluirInactivos = false)
        {
            try
            {
                return _repuestoRepository.Listar(incluirInactivos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar repuestos", ex);
            }
        }

        public Repuesto ObtenerPorId(int id)
        {
            try
            {
                Repuesto repuesto = _repuestoRepository.ObtenerPorId(id);

                if (repuesto == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                return repuesto;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener repuesto", ex);
            }
        }

        public Repuesto ObtenerPorCodigo(string codigo)
        {
            try
            {
                Repuesto repuesto = _repuestoRepository.ObtenerPorCodigo(codigo);

                if (repuesto == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                return repuesto;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener repuesto", ex);
            }
        }

        public List<MovimientoStock> ListarMovimientos(int idRepuesto)
        {
            try
            {
                return _movimientoRepository.ListarPorRepuesto(idRepuesto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar movimientos de stock", ex);
            }
        }

        public List<MovimientoStock> ListarMovimientosFiltros(int? idRepuesto, string tipo,
            DateTime? desde, DateTime? hasta)
        {
            try
            {
                return _movimientoRepository.ListarFiltros(idRepuesto, tipo, desde, hasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar movimientos de stock", ex);
            }
        }

        private int ObtenerIdUsuarioSesion()
        {
            if (!SessionManager.HaySesionActiva() || SessionManager.ObtenerUsuarioActual() == null)
                throw new ReglaNegocioException("Debe iniciar sesion para gestionar repuestos.");

            return SessionManager.ObtenerUsuarioActual().Id;
        }
    }
}
