using System;
using DOMAIN.Exceptions;

namespace DOMAIN.Features.Ordenes
{
    public class OrdenServicio
    {
        public int Id { get; private set; }
        public int NumeroOrden { get; private set; }
        public int IdCliente { get; private set; }
        public int IdEquipo { get; private set; }
        public int? IdTecnicoAsignado { get; private set; }
        public int? IdOrdenOrigen { get; private set; }
        public string TipoOrden { get; private set; }
        public string Estado { get; private set; }
        public string Resultado { get; private set; }
        public DateTime FechaIngreso { get; private set; }
        public string ProblemaInformado { get; private set; }
        public string EstadoFisicoIngreso { get; private set; }
        public string AccesoriosIngreso { get; private set; }
        public string ObservacionesIngreso { get; private set; }
        public string ObservacionResultado { get; private set; }
        public int IdUsuarioAlta { get; private set; }

        private OrdenServicio() { }

        public static OrdenServicio CrearNuevo(int idCliente, int idEquipo, string problema,
            string estadoFisico, string accesorios, string observacionesIngreso, int idUsuarioAlta)
        {
            if (idCliente <= 0)
                throw new ReglaNegocioException("El cliente de la orden es obligatorio.");
            if (idEquipo <= 0)
                throw new ReglaNegocioException("El equipo de la orden es obligatorio.");
            if (string.IsNullOrWhiteSpace(problema))
                throw new ReglaNegocioException("El problema informado es obligatorio.");
            if (idUsuarioAlta <= 0)
                throw new ReglaNegocioException("El usuario de alta es obligatorio.");

            return new OrdenServicio
            {
                NumeroOrden = 0,
                IdCliente = idCliente,
                IdEquipo = idEquipo,
                IdTecnicoAsignado = null,
                IdOrdenOrigen = null,
                TipoOrden = Ordenes.TipoOrden.Normal,
                Estado = EstadoOrdenServicio.Recibido,
                Resultado = null,
                FechaIngreso = DateTime.Now,
                ProblemaInformado = problema.Trim(),
                EstadoFisicoIngreso = estadoFisico == null ? "" : estadoFisico.Trim(),
                AccesoriosIngreso = accesorios == null ? "" : accesorios.Trim(),
                ObservacionesIngreso = observacionesIngreso == null ? "" : observacionesIngreso.Trim(),
                ObservacionResultado = null,
                IdUsuarioAlta = idUsuarioAlta
            };
        }

        public static OrdenServicio CargarDesdeDB(int id, int numeroOrden, int idCliente, int idEquipo,
            int? idTecnicoAsignado, int? idOrdenOrigen, string tipoOrden, string estado, string resultado,
            DateTime fechaIngreso, string problemaInformado, string estadoFisicoIngreso,
            string accesoriosIngreso, string observacionesIngreso, string observacionResultado,
            int idUsuarioAlta)
        {
            return new OrdenServicio
            {
                Id = id,
                NumeroOrden = numeroOrden,
                IdCliente = idCliente,
                IdEquipo = idEquipo,
                IdTecnicoAsignado = idTecnicoAsignado,
                IdOrdenOrigen = idOrdenOrigen,
                TipoOrden = tipoOrden ?? Ordenes.TipoOrden.Normal,
                Estado = estado ?? EstadoOrdenServicio.Recibido,
                Resultado = resultado,
                FechaIngreso = fechaIngreso,
                ProblemaInformado = problemaInformado ?? "",
                EstadoFisicoIngreso = estadoFisicoIngreso ?? "",
                AccesoriosIngreso = accesoriosIngreso ?? "",
                ObservacionesIngreso = observacionesIngreso ?? "",
                ObservacionResultado = observacionResultado,
                IdUsuarioAlta = idUsuarioAlta
            };
        }

        public void AsignarTecnico(int idTecnico)
        {
            if (Estado == EstadoOrdenServicio.Entregado)
                throw new ReglaNegocioException("No se puede asignar tecnico a una orden entregada.");
            if (idTecnico <= 0)
                throw new ReglaNegocioException("El tecnico asignado es obligatorio.");

            IdTecnicoAsignado = idTecnico;
        }

        public void ActualizarRecepcion(string problema, string estadoFisico, string accesorios, string observacionesIngreso)
        {
            if (Estado == EstadoOrdenServicio.Entregado)
                throw new ReglaNegocioException("No se puede modificar la recepcion de una orden entregada.");
            if (string.IsNullOrWhiteSpace(problema))
                throw new ReglaNegocioException("El problema informado es obligatorio.");

            ProblemaInformado = problema.Trim();
            EstadoFisicoIngreso = estadoFisico == null ? "" : estadoFisico.Trim();
            AccesoriosIngreso = accesorios == null ? "" : accesorios.Trim();
            ObservacionesIngreso = observacionesIngreso == null ? "" : observacionesIngreso.Trim();
        }

        public void IniciarDiagnostico()
        {
            if (Estado != EstadoOrdenServicio.Recibido)
                throw new ReglaNegocioException("Solo se puede iniciar el diagnostico de una orden recibida.");
            if (!IdTecnicoAsignado.HasValue)
                throw new ReglaNegocioException("La orden debe tener un tecnico asignado para iniciar el diagnostico.");

            Estado = EstadoOrdenServicio.EnDiagnostico;
        }

        public void MarcarPendientePresupuesto()
        {
            if (Estado != EstadoOrdenServicio.EnDiagnostico)
                throw new ReglaNegocioException("Solo se puede pedir presupuesto de una orden en diagnostico.");

            Estado = EstadoOrdenServicio.PendientePresupuesto;
        }

        public void MarcarEsperandoRespuesta()
        {
            if (Estado != EstadoOrdenServicio.PendientePresupuesto)
                throw new ReglaNegocioException("Solo se puede emitir el presupuesto de una orden pendiente de presupuesto.");

            Estado = EstadoOrdenServicio.EsperandoRespuesta;
        }

        public void AutorizarReparacion()
        {
            if (Estado != EstadoOrdenServicio.EsperandoRespuesta)
                throw new ReglaNegocioException("Solo se puede autorizar la reparacion de una orden en espera de respuesta.");

            Estado = EstadoOrdenServicio.AutorizadoReparacion;
        }

        public void MarcarNoReparable(string motivo)
        {
            if (Estado != EstadoOrdenServicio.EnDiagnostico)
                throw new ReglaNegocioException("Solo se puede declarar no reparable una orden en diagnostico.");
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ReglaNegocioException("El motivo de no reparable es obligatorio.");

            Estado = EstadoOrdenServicio.ListoRetiro;
            Resultado = ResultadoOrdenServicio.NoReparable;
            ObservacionResultado = motivo.Trim();
        }

        public void MarcarPresupuestoRechazado(string motivo)
        {
            if (Estado != EstadoOrdenServicio.EsperandoRespuesta)
                throw new ReglaNegocioException("Solo se puede registrar el rechazo de una orden en espera de respuesta.");
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ReglaNegocioException("El motivo del rechazo es obligatorio.");

            Estado = EstadoOrdenServicio.ListoRetiro;
            Resultado = ResultadoOrdenServicio.PresupuestoRechazado;
            ObservacionResultado = motivo.Trim();
        }

        public void IniciarReparacion()
        {
            if (Estado != EstadoOrdenServicio.AutorizadoReparacion)
                throw new ReglaNegocioException("Solo se puede iniciar la reparacion de una orden autorizada.");

            Estado = EstadoOrdenServicio.EnReparacion;
        }

        public void FinalizarReparacion()
        {
            if (Estado != EstadoOrdenServicio.EnReparacion)
                throw new ReglaNegocioException("Solo se puede finalizar la reparacion de una orden en reparacion.");

            Estado = EstadoOrdenServicio.EnPruebas;
        }

        public void SolicitarAdicional()
        {
            if (Estado != EstadoOrdenServicio.EnReparacion
                && Estado != EstadoOrdenServicio.EnPruebas)
                throw new ReglaNegocioException("Solo se puede solicitar un presupuesto adicional de una orden en reparacion o en pruebas.");

            Estado = EstadoOrdenServicio.PendientePresupuesto;
        }

        public void RegistrarPruebaAprobada()
        {
            if (Estado != EstadoOrdenServicio.EnPruebas)
                throw new ReglaNegocioException("Solo se puede aprobar la prueba de una orden en pruebas.");

            Estado = EstadoOrdenServicio.ListoRetiro;
            Resultado = ResultadoOrdenServicio.Reparado;
        }

        public void RegistrarPruebaFallida()
        {
            if (Estado != EstadoOrdenServicio.EnPruebas)
                throw new ReglaNegocioException("Solo se puede registrar la prueba fallida de una orden en pruebas.");

            Estado = EstadoOrdenServicio.EnReparacion;
        }

        public void Cancelar(string motivo)
        {
            if (Estado != EstadoOrdenServicio.Recibido
                && Estado != EstadoOrdenServicio.EnDiagnostico
                && Estado != EstadoOrdenServicio.PendientePresupuesto
                && Estado != EstadoOrdenServicio.EsperandoRespuesta
                && Estado != EstadoOrdenServicio.AutorizadoReparacion
                && Estado != EstadoOrdenServicio.EnReparacion
                && Estado != EstadoOrdenServicio.EnPruebas)
                throw new ReglaNegocioException("La orden no se puede cancelar en su estado actual.");
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ReglaNegocioException("El motivo de la cancelacion es obligatorio.");

            Estado = EstadoOrdenServicio.ListoRetiro;
            Resultado = ResultadoOrdenServicio.Cancelado;
            ObservacionResultado = motivo.Trim();
        }

        public void Entregar()
        {
            if (Estado != EstadoOrdenServicio.ListoRetiro)
                throw new ReglaNegocioException("Solo se puede entregar una orden lista para retiro.");

            Estado = EstadoOrdenServicio.Entregado;
        }
    }
}
