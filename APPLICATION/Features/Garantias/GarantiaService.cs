using System;
using APPLICATION.Features.Bitacora;
using DOMAIN.Exceptions;
using DOMAIN.Features.Garantias;
using REPOSITORY.Features.Garantias;
using SERVICES.Auth;

namespace APPLICATION.Features.Garantias
{
    public class GarantiaService
    {
        private readonly GarantiaRepository _garantiaRepository;
        private readonly EvaluacionGarantiaRepository _evaluacionRepository;

        public GarantiaService()
        {
            _garantiaRepository = new GarantiaRepository();
            _evaluacionRepository = new EvaluacionGarantiaRepository();
        }

        public void Inicializar()
        {
            _garantiaRepository.Inicializar();
            _evaluacionRepository.Inicializar();
        }

        public Garantia Crear(int idOrdenOriginal, DateTime fechaInicio, DateTime fechaFin, string observaciones)
        {
            try
            {
                ObtenerIdUsuarioSesion();

                Garantia garantiaToSave = Garantia.CrearNuevo(
                    idOrdenOriginal, fechaInicio, fechaFin, observaciones);

                Garantia garantiaDb = _garantiaRepository.Crear(garantiaToSave);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Garantia creada",
                    "id_orden=" + idOrdenOriginal + " | id_garantia=" + garantiaDb.Id, "ORDENES");

                return garantiaDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear garantia", ex);
            }
        }

        public Garantia ObtenerPorOrden(int idOrdenOriginal)
        {
            try
            {
                Garantia garantia = _garantiaRepository.ObtenerPorOrden(idOrdenOriginal);

                if (garantia == null)
                    throw new ReglaNegocioException("La orden no tiene una garantia registrada.");

                return garantia;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener garantia", ex);
            }
        }

        public Garantia ObtenerVigente(int idOrdenOriginal)
        {
            try
            {
                Garantia garantia = _garantiaRepository.ObtenerVigente(idOrdenOriginal);

                if (garantia == null)
                    throw new ReglaNegocioException("La garantia de la orden no esta vigente.");

                return garantia;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener garantia vigente", ex);
            }
        }

        public void Anular(int id)
        {
            try
            {
                ObtenerIdUsuarioSesion();

                Garantia garantia = _garantiaRepository.ObtenerPorId(id);

                if (garantia == null)
                    throw new ReglaNegocioException("La garantia seleccionada no existe.");

                _garantiaRepository.Anular(id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Garantia anulada", "id=" + id, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al anular garantia", ex);
            }
        }

        public EvaluacionGarantia ObtenerEvaluacionPorReingreso(int idOrdenReingreso)
        {
            try
            {
                EvaluacionGarantia evaluacion = _evaluacionRepository.ObtenerPorReingreso(idOrdenReingreso);

                if (evaluacion == null)
                    throw new ReglaNegocioException("El reingreso no tiene una evaluacion registrada.");

                return evaluacion;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener evaluacion de garantia", ex);
            }
        }

        private int ObtenerIdUsuarioSesion()
        {
            if (!SessionManager.HaySesionActiva() || SessionManager.ObtenerUsuarioActual() == null)
                throw new ReglaNegocioException("Debe iniciar sesion para gestionar garantias.");

            return SessionManager.ObtenerUsuarioActual().Id;
        }
    }
}
