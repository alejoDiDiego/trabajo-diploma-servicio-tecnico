using System;
using System.Collections.Generic;
using DOMAIN.Features.ControlCambios;
using REPOSITORY.Features.ControlCambios;
using REPOSITORY.Features.Idiomas;

namespace APPLICATION.Features.ControlCambios
{
    public class ControlCambioService
    {
        private readonly ControlCambioRepository _repository;
        private readonly IdiomaRepository _idiomaRepository;

        public ControlCambioService()
        {
            _repository = new ControlCambioRepository();
            _idiomaRepository = new IdiomaRepository();
        }

        public void Inicializar()
        {
            _repository.Inicializar();
        }

        public List<ControlCambio> Listar()
        {
            return _repository.Listar();
        }

        public void RegistrarCambio(string tablaAfectada, int idIdioma, int idPalabra,
            string claveRegistro, string campoModificado, string valorAnterior,
            string valorNuevo, string usuarioModifico, string tipoCambio)
        {
            ControlCambio cambio = ControlCambio.Crear(
                0, tablaAfectada, idIdioma, idPalabra, claveRegistro,
                campoModificado, valorAnterior, valorNuevo,
                usuarioModifico, DateTime.Now, tipoCambio
            );

            _repository.Insertar(cambio);
        }

        public void Restaurar(int idCambio)
        {
            ControlCambio cambio = _repository.ObtenerPorId(idCambio);

            if (cambio == null)
                throw new Exception("El cambio seleccionado no existe.");

            if (cambio.TipoCambio != "UPDATE")
                throw new Exception("Solo se pueden restaurar cambios de tipo UPDATE.");

            _idiomaRepository.GuardarTraduccion(cambio.IdIdioma, cambio.IdPalabra, cambio.ValorAnterior);

            string usuario = SERVICES.Auth.SessionManager.HaySesionActiva()
                ? SERVICES.Auth.SessionManager.ObtenerUsuarioActual().Username
                : "Sistema";

            RegistrarCambio(
                cambio.TablaAfectada,
                cambio.IdIdioma,
                cambio.IdPalabra,
                cambio.ClaveRegistro,
                cambio.CampoModificado,
                cambio.ValorNuevo,
                cambio.ValorAnterior,
                usuario,
                "UPDATE"
            );
        }
    }
}
