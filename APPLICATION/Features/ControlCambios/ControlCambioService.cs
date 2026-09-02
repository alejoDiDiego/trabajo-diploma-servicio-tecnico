using System;
using System.Collections.Generic;
using ABSTRACTIONS.Features.Idiomas;
using DOMAIN.Features.ControlCambios;
using DOMAIN.Features.Idiomas;
using REPOSITORY.Features.ControlCambios;
using REPOSITORY.Features.Idiomas;
using SERVICES.Auth;
using SERVICES.Idiomas;
using APPLICATION.Features.Bitacora;

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

            string usuario = SessionManager.HaySesionActiva()
                ? SessionManager.ObtenerUsuarioActual().Username
                : "Sistema";

            if (cambio.TablaAfectada == "Idiomas")
            {
                RestaurarIdioma(cambio, usuario);
            }
            else if (cambio.TablaAfectada == "Traducciones")
            {
                AsegurarIdiomaExiste(cambio, usuario);
                RestaurarTraduccion(cambio, usuario);
            }

            BitacoraService bitacoraService = new BitacoraService();
            string detalle = "tabla=" + cambio.TablaAfectada + " | tipo=" + cambio.TipoCambio
                + " | id_idioma=" + cambio.IdIdioma;
            bitacoraService.Registrar("Restauracion de cambio", detalle, "CONTROL_CAMBIOS");
        }

        private void RestaurarIdioma(ControlCambio cambio, string usuario)
        {
            if (cambio.TipoCambio == "DELETE")
            {
                _idiomaRepository.AgregarIdiomaConId(cambio.IdIdioma, cambio.ClaveRegistro);

                RegistrarCambio(
                    "Idiomas", cambio.IdIdioma, 0, cambio.ClaveRegistro,
                    "nombre", "", cambio.ClaveRegistro,
                    usuario, "INSERT"
                );

                IIdioma idiomaActual = SesionIdioma.GetInstance().idioma;
                SesionIdioma.GetInstance().ActualizarObservadores(idiomaActual);
            }
            else if (cambio.TipoCambio == "INSERT")
            {
                throw new Exception("No se puede restaurar la creacion de un idioma. Elimine el idioma manualmente si desea deshacerlo.");
            }
        }

        private void AsegurarIdiomaExiste(ControlCambio cambio, string usuario)
        {
            Idioma idioma = _idiomaRepository.ObtenerPorId(cambio.IdIdioma);
            if (idioma != null)
                return;

            ControlCambio deleteIdioma = _repository.ObtenerPorTablaYRegistro("Idiomas", cambio.IdIdioma, "DELETE");

            if (deleteIdioma == null)
                throw new Exception("No se puede restaurar: el idioma asociado no existe y no hay registro de su creacion.");

            RestaurarIdioma(deleteIdioma, usuario);
        }

        private void RestaurarTraduccion(ControlCambio cambio, string usuario)
        {
            if (cambio.TipoCambio == "UPDATE")
            {
                _idiomaRepository.GuardarTraduccion(cambio.IdIdioma, cambio.IdPalabra, cambio.ValorAnterior);

                RegistrarCambio(
                    cambio.TablaAfectada, cambio.IdIdioma, cambio.IdPalabra, cambio.ClaveRegistro,
                    cambio.CampoModificado, cambio.ValorNuevo, cambio.ValorAnterior,
                    usuario, "UPDATE"
                );
            }
            else if (cambio.TipoCambio == "INSERT")
            {
                _idiomaRepository.EliminarTraduccion(cambio.IdIdioma, cambio.IdPalabra);

                RegistrarCambio(
                    cambio.TablaAfectada, cambio.IdIdioma, cambio.IdPalabra, cambio.ClaveRegistro,
                    cambio.CampoModificado, cambio.ValorNuevo, cambio.ValorAnterior,
                    usuario, "DELETE"
                );
            }
            else if (cambio.TipoCambio == "DELETE")
            {
                _idiomaRepository.GuardarTraduccion(cambio.IdIdioma, cambio.IdPalabra, cambio.ValorAnterior);

                RegistrarCambio(
                    cambio.TablaAfectada, cambio.IdIdioma, cambio.IdPalabra, cambio.ClaveRegistro,
                    cambio.CampoModificado, cambio.ValorNuevo, cambio.ValorAnterior,
                    usuario, "INSERT"
                );
            }
        }
    }
}
