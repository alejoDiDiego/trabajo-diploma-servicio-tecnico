using System;
using System.Collections.Generic;
using ABSTRACTIONS.Features.Permisos;
using APPLICATION.Features.Bitacora;
using APPLICATION.Features.Usuarios;
using DOMAIN.Exceptions;
using DOMAIN.Features.Clientes;
using DOMAIN.Features.Equipos;
using DOMAIN.Features.Ordenes;
using DOMAIN.Features.Permisos;
using DOMAIN.Features.Reparaciones;
using DOMAIN.Features.Repuestos;
using DOMAIN.Features.Usuarios;
using REPOSITORY.Features.Clientes;
using REPOSITORY.Features.Equipos;
using REPOSITORY.Features.Ordenes;
using REPOSITORY.Features.Reparaciones;
using REPOSITORY.Features.Repuestos;
using REPOSITORY.Features.Usuarios;
using SERVICES.Auth;

namespace APPLICATION.Features.Ordenes
{
    public class OrdenServicioService
    {
        private readonly OrdenServicioRepository _ordenRepository;
        private readonly HistorialEstadoOrdenRepository _historialRepository;
        private readonly DiagnosticoRepository _diagnosticoRepository;
        private readonly PresupuestoRepository _presupuestoRepository;
        private readonly EntregaRepository _entregaRepository;
        private readonly ClienteRepository _clienteRepository;
        private readonly EquipoRepository _equipoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly UsuarioPermisoService _usuarioPermisoService;
        private readonly ReparacionRepository _reparacionRepository;
        private readonly RepuestoRepository _repuestoRepository;

        public OrdenServicioService()
        {
            _ordenRepository = new OrdenServicioRepository();
            _historialRepository = new HistorialEstadoOrdenRepository();
            _diagnosticoRepository = new DiagnosticoRepository();
            _presupuestoRepository = new PresupuestoRepository();
            _entregaRepository = new EntregaRepository();
            _clienteRepository = new ClienteRepository();
            _equipoRepository = new EquipoRepository();
            _usuarioRepository = new UsuarioRepository();
            _usuarioPermisoService = new UsuarioPermisoService();
            _reparacionRepository = new ReparacionRepository();
            _repuestoRepository = new RepuestoRepository();
        }

        public void Inicializar()
        {
            _ordenRepository.Inicializar();
            _historialRepository.Inicializar();
            _diagnosticoRepository.Inicializar();
            _presupuestoRepository.Inicializar();
            _entregaRepository.Inicializar();
        }

        public void InicializarReparaciones()
        {
            _reparacionRepository.Inicializar();
        }

        public OrdenServicio CrearOrden(int idCliente, int idEquipo, string problema,
            string estadoFisico, string accesorios, string observacionesIngreso)
        {
            try
            {
                int idUsuarioAlta = ObtenerIdUsuarioSesion();

                Cliente cliente = _clienteRepository.ObtenerPorId(idCliente);

                if (cliente == null)
                    throw new ReglaNegocioException("El cliente seleccionado no existe.");

                if (!cliente.Activo)
                    throw new ReglaNegocioException("El cliente seleccionado esta inactivo.");

                Equipo equipo = _equipoRepository.ObtenerPorId(idEquipo);

                if (equipo == null)
                    throw new ReglaNegocioException("El equipo seleccionado no existe.");

                if (!equipo.Activo)
                    throw new ReglaNegocioException("El equipo seleccionado esta inactivo.");

                if (equipo.IdCliente != idCliente)
                    throw new ReglaNegocioException("El equipo no pertenece al cliente seleccionado.");

                OrdenServicio ordenToSave = OrdenServicio.CrearNuevo(
                    idCliente, idEquipo, problema, estadoFisico, accesorios,
                    observacionesIngreso, idUsuarioAlta);

                OrdenServicio ordenDb = _ordenRepository.CrearConHistorial(
                    ordenToSave, idUsuarioAlta, "Orden creada");

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Orden creada",
                    "id=" + ordenDb.Id + " | numero=" + ordenDb.NumeroOrden + " | id_cliente=" + idCliente, "ORDENES");

                return ordenDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear orden", ex);
            }
        }

        public void AsignarTecnico(int idOrden, int idTecnico)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                Usuario tecnico = ValidarTecnicoElegible(idTecnico);

                ordenDb.AsignarTecnico(tecnico.Id);
                _ordenRepository.AsignarTecnico(ordenDb.Id, tecnico.Id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Tecnico asignado",
                    "id_orden=" + idOrden + " | id_tecnico=" + tecnico.Id, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al asignar tecnico", ex);
            }
        }

        public void IniciarDiagnostico(int idOrden)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);
                string estadoAnterior = ordenDb.Estado;

                ordenDb.IniciarDiagnostico();

                _ordenRepository.CambiarEstadoConHistorial(ordenDb.Id, estadoAnterior, ordenDb.Estado,
                    idUsuario, "Diagnostico iniciado", null, null, null);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Diagnostico iniciado", "id_orden=" + idOrden, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar diagnostico", ex);
            }
        }

        public Diagnostico FinalizarDiagnostico(int idOrden, string descripcion, bool esReparable,
            int? tiempoEstimadoDias, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.EnDiagnostico)
                    throw new ReglaNegocioException("Solo se puede finalizar el diagnostico de una orden en diagnostico.");

                if (!ordenDb.IdTecnicoAsignado.HasValue)
                    throw new ReglaNegocioException("La orden debe tener un tecnico asignado para finalizar el diagnostico.");

                if (_diagnosticoRepository.ObtenerPorOrden(idOrden) != null)
                    throw new ReglaNegocioException("La orden ya tiene un diagnostico registrado.");

                string estadoAnterior = ordenDb.Estado;

                Diagnostico diagnosticoToSave = Diagnostico.CrearNuevo(idOrden,
                    ordenDb.IdTecnicoAsignado.Value, descripcion, esReparable,
                    tiempoEstimadoDias, observaciones);

                string resultado = null;
                string observacionResultado = null;

                if (esReparable)
                {
                    ordenDb.MarcarPendientePresupuesto();
                }
                else
                {
                    ordenDb.MarcarNoReparable(descripcion);
                    resultado = ordenDb.Resultado;
                    observacionResultado = ordenDb.ObservacionResultado;
                }

                Diagnostico diagnosticoDb = _diagnosticoRepository.CrearConTransicion(
                    diagnosticoToSave, estadoAnterior, ordenDb.Estado, idUsuario,
                    esReparable ? "Diagnostico reparable" : "Diagnostico no reparable",
                    resultado, observacionResultado);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Diagnostico finalizado",
                    "id_orden=" + idOrden + " | reparable=" + esReparable, "ORDENES");

                return diagnosticoDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al finalizar diagnostico", ex);
            }
        }

        public Presupuesto EmitirPresupuesto(int idOrden, List<DetallePresupuesto> items,
            decimal descuento, int diasGarantia, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.PendientePresupuesto)
                    throw new ReglaNegocioException("Solo se puede emitir el presupuesto de una orden pendiente de presupuesto.");

                Diagnostico diagnostico = _diagnosticoRepository.ObtenerPorOrden(idOrden);

                if (diagnostico == null || !diagnostico.EsReparable)
                    throw new ReglaNegocioException("La orden debe tener un diagnostico reparable para emitir el presupuesto.");

                if (_presupuestoRepository.ObtenerOriginal(idOrden) != null)
                    throw new ReglaNegocioException("La orden ya tiene un presupuesto original emitido. Debe emitir un presupuesto adicional.");

                decimal subtotal = ValidarItemsYCalcularSubtotal(items);

                string estadoAnterior = ordenDb.Estado;

                Presupuesto presupuestoToSave = Presupuesto.CrearNuevo(
                    idOrden, subtotal, descuento, diasGarantia, observaciones, TipoPresupuesto.Original);

                ordenDb.MarcarEsperandoRespuesta();

                Presupuesto presupuestoDb = _presupuestoRepository.EmitirConDetalle(
                    presupuestoToSave, items, estadoAnterior, ordenDb.Estado,
                    idUsuario, "Presupuesto emitido");

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Presupuesto emitido",
                    "id_orden=" + idOrden + " | total=" + presupuestoDb.Total, "ORDENES");

                return presupuestoDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al emitir presupuesto", ex);
            }
        }

        public void RegistrarAprobacionPresupuesto(int idOrden, string medioRespuesta, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                Presupuesto presupuestoDb = _presupuestoRepository.ObtenerPorOrden(idOrden);

                if (presupuestoDb == null)
                    throw new ReglaNegocioException("La orden no tiene un presupuesto emitido.");

                if (presupuestoDb.Tipo != TipoPresupuesto.Original)
                    throw new ReglaNegocioException("Solo se puede aprobar el presupuesto original por este medio.");

                string estadoAnterior = ordenDb.Estado;

                presupuestoDb.Aprobar(medioRespuesta, observaciones);
                ordenDb.AutorizarReparacion();

                _presupuestoRepository.AprobarConTransicion(presupuestoDb.Id, idOrden,
                    estadoAnterior, ordenDb.Estado, idUsuario, presupuestoDb.MedioRespuesta,
                    observaciones, "Presupuesto aprobado");

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Presupuesto aprobado", "id_orden=" + idOrden, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al aprobar presupuesto", ex);
            }
        }

        public void RegistrarRechazoPresupuesto(int idOrden, string motivo, string medioRespuesta, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                Presupuesto presupuestoDb = _presupuestoRepository.ObtenerPorOrden(idOrden);

                if (presupuestoDb == null)
                    throw new ReglaNegocioException("La orden no tiene un presupuesto emitido.");

                string estadoAnterior = ordenDb.Estado;

                presupuestoDb.Rechazar(motivo, medioRespuesta, observaciones);
                ordenDb.MarcarPresupuestoRechazado(motivo);

                _presupuestoRepository.RechazarConTransicion(presupuestoDb.Id, idOrden,
                    estadoAnterior, ordenDb.Estado, idUsuario, presupuestoDb.MotivoRechazo,
                    presupuestoDb.MedioRespuesta, observaciones, ordenDb.Resultado,
                    ordenDb.ObservacionResultado, "Presupuesto rechazado");

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Presupuesto rechazado", "id_orden=" + idOrden, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al rechazar presupuesto", ex);
            }
        }

        public void SolicitarAdicional(int idOrden, string motivo)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                if (string.IsNullOrWhiteSpace(motivo))
                    throw new ReglaNegocioException("El motivo de la solicitud es obligatorio.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                Presupuesto original = _presupuestoRepository.ObtenerOriginal(idOrden);

                if (original == null || original.Estado != EstadoPresupuesto.Aprobado)
                    throw new ReglaNegocioException("La orden debe tener el presupuesto original aprobado para solicitar un adicional.");

                if (_presupuestoRepository.ExisteAdicionalPendiente(idOrden))
                    throw new ReglaNegocioException("La orden ya tiene un presupuesto adicional pendiente de respuesta.");

                string estadoAnterior = ordenDb.Estado;

                ordenDb.SolicitarAdicional();

                _ordenRepository.CambiarEstadoConHistorial(ordenDb.Id, estadoAnterior, ordenDb.Estado,
                    idUsuario, motivo.Trim(), null, null, null);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("ADICIONAL_SOLICITADO", "id_orden=" + idOrden, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al solicitar presupuesto adicional", ex);
            }
        }

        public Presupuesto EmitirAdicional(int idOrden, List<DetallePresupuesto> items,
            decimal descuento, int diasGarantia, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.PendientePresupuesto)
                    throw new ReglaNegocioException("Solo se puede emitir el presupuesto adicional de una orden pendiente de presupuesto.");

                Presupuesto original = _presupuestoRepository.ObtenerOriginal(idOrden);

                if (original == null || original.Estado != EstadoPresupuesto.Aprobado)
                    throw new ReglaNegocioException("La orden debe tener el presupuesto original aprobado para emitir un adicional.");

                if (_presupuestoRepository.ExisteAdicionalPendiente(idOrden))
                    throw new ReglaNegocioException("La orden ya tiene un presupuesto adicional pendiente de respuesta.");

                decimal subtotal = ValidarItemsYCalcularSubtotal(items);

                Presupuesto presupuestoToSave = Presupuesto.CrearNuevo(
                    idOrden, subtotal, descuento, diasGarantia, observaciones, TipoPresupuesto.Adicional);

                Presupuesto presupuestoDb = _presupuestoRepository.EmitirAdicional(
                    presupuestoToSave, items, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("ADICIONAL_EMITIDO",
                    "id_orden=" + idOrden + " | id_presupuesto=" + presupuestoDb.Id + " | total=" + presupuestoDb.Total, "ORDENES");

                return presupuestoDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al emitir presupuesto adicional", ex);
            }
        }

        public void RegistrarAprobacionAdicional(int idPresupuesto, string medioRespuesta, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Presupuesto presupuestoDb = _presupuestoRepository.ObtenerPorId(idPresupuesto);

                if (presupuestoDb == null)
                    throw new ReglaNegocioException("El presupuesto seleccionado no existe.");

                if (presupuestoDb.Tipo != TipoPresupuesto.Adicional)
                    throw new ReglaNegocioException("Solo se puede aprobar un presupuesto adicional por este medio.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(presupuestoDb.IdOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.EsperandoRespuesta)
                    throw new ReglaNegocioException("Solo se puede aprobar el adicional de una orden en espera de respuesta.");

                string estadoOrigen;

                if (!TryObtenerOrigenSolicitud(ordenDb.Id, out estadoOrigen))
                    throw new ReglaNegocioException("La orden no registra una solicitud de presupuesto adicional para aprobar.");

                presupuestoDb.Aprobar(medioRespuesta, observaciones);

                string estadoDestino = estadoOrigen == EstadoOrdenServicio.AutorizadoReparacion
                    ? EstadoOrdenServicio.AutorizadoReparacion
                    : EstadoOrdenServicio.EnReparacion;

                if (estadoDestino == EstadoOrdenServicio.AutorizadoReparacion)
                    ordenDb.AutorizarReparacion();

                _presupuestoRepository.AprobarAdicionalConTransicion(presupuestoDb.Id, idUsuario,
                    presupuestoDb.MedioRespuesta, observaciones, estadoDestino);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("ADICIONAL_APROBADO",
                    "id_orden=" + ordenDb.Id + " | id_presupuesto=" + presupuestoDb.Id, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al aprobar presupuesto adicional", ex);
            }
        }

        public void RegistrarRechazoAdicional(int idPresupuesto, string motivo, string medioRespuesta, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Presupuesto presupuestoDb = _presupuestoRepository.ObtenerPorId(idPresupuesto);

                if (presupuestoDb == null)
                    throw new ReglaNegocioException("El presupuesto seleccionado no existe.");

                if (presupuestoDb.Tipo != TipoPresupuesto.Adicional)
                    throw new ReglaNegocioException("Solo se puede rechazar un presupuesto adicional por este medio.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(presupuestoDb.IdOrden);

                presupuestoDb.Rechazar(motivo, medioRespuesta, observaciones);
                ordenDb.MarcarPresupuestoRechazado(motivo);

                _presupuestoRepository.RechazarAdicionalConTransicion(presupuestoDb.Id, idUsuario,
                    presupuestoDb.MotivoRechazo, presupuestoDb.MedioRespuesta, observaciones,
                    ordenDb.Resultado, ordenDb.ObservacionResultado, "Presupuesto adicional rechazado");

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("ADICIONAL_RECHAZADO",
                    "id_orden=" + ordenDb.Id + " | id_presupuesto=" + presupuestoDb.Id, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al rechazar presupuesto adicional", ex);
            }
        }

        public Presupuesto CrearBorrador(int idOrden, string tipo, List<DetallePresupuesto> items,
            decimal descuento, int diasGarantia, string observaciones)
        {
            try
            {
                ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.PendientePresupuesto)
                    throw new ReglaNegocioException("Solo se puede crear el borrador de una orden pendiente de presupuesto.");

                if (tipo != TipoPresupuesto.Original && tipo != TipoPresupuesto.Adicional)
                    throw new ReglaNegocioException("El tipo de presupuesto no es valido.");

                if (tipo == TipoPresupuesto.Original)
                {
                    if (_presupuestoRepository.ExisteOriginalActivo(idOrden))
                        throw new ReglaNegocioException("La orden ya tiene un presupuesto original activo. Debe anularlo antes de crear otro.");
                }
                else
                {
                    Presupuesto original = _presupuestoRepository.ObtenerOriginal(idOrden);

                    if (original == null || original.Estado != EstadoPresupuesto.Aprobado)
                        throw new ReglaNegocioException("La orden debe tener el presupuesto original aprobado para crear un borrador adicional.");

                    if (_presupuestoRepository.ExisteAdicionalPendiente(idOrden))
                        throw new ReglaNegocioException("La orden ya tiene un presupuesto adicional pendiente de respuesta.");
                }

                decimal subtotal = ValidarItemsYCalcularSubtotal(items);

                Presupuesto borradorToSave = Presupuesto.CrearBorrador(
                    idOrden, subtotal, descuento, diasGarantia, observaciones, tipo);

                Presupuesto borradorDb = _presupuestoRepository.CrearBorrador(borradorToSave, items);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("PRESUPUESTO_BORRADOR_CREADO",
                    "id_orden=" + idOrden + " | id_presupuesto=" + borradorDb.Id + " | tipo=" + tipo, "ORDENES");

                return borradorDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear borrador de presupuesto", ex);
            }
        }

        public void PublicarBorrador(int idPresupuesto)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Presupuesto borradorDb = _presupuestoRepository.ObtenerPorId(idPresupuesto);

                if (borradorDb == null)
                    throw new ReglaNegocioException("El presupuesto seleccionado no existe.");

                if (borradorDb.Estado != EstadoPresupuesto.Borrador)
                    throw new ReglaNegocioException("Solo se puede publicar un presupuesto en borrador.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(borradorDb.IdOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.PendientePresupuesto)
                    throw new ReglaNegocioException("Solo se puede publicar el borrador de una orden pendiente de presupuesto.");

                if (borradorDb.Tipo == TipoPresupuesto.Original)
                {
                    Diagnostico diagnostico = _diagnosticoRepository.ObtenerPorOrden(ordenDb.Id);

                    if (diagnostico == null || !diagnostico.EsReparable)
                        throw new ReglaNegocioException("La orden debe tener un diagnostico reparable para publicar el presupuesto.");

                    List<Presupuesto> todos = _presupuestoRepository.ListarPorOrden(ordenDb.Id);

                    foreach (Presupuesto p in todos)
                    {
                        if (p.Id != borradorDb.Id && p.Tipo == TipoPresupuesto.Original
                            && p.Estado != EstadoPresupuesto.Anulado)
                            throw new ReglaNegocioException("La orden ya tiene un presupuesto original activo.");
                    }
                }
                else
                {
                    Presupuesto original = _presupuestoRepository.ObtenerOriginal(ordenDb.Id);

                    if (original == null || original.Estado != EstadoPresupuesto.Aprobado)
                        throw new ReglaNegocioException("La orden debe tener el presupuesto original aprobado para publicar un adicional.");

                    List<Presupuesto> adicionales = _presupuestoRepository.ListarAdicionales(ordenDb.Id);

                    foreach (Presupuesto a in adicionales)
                    {
                        if (a.Id != borradorDb.Id
                            && (a.Estado == EstadoPresupuesto.Borrador || a.Estado == EstadoPresupuesto.Pendiente))
                            throw new ReglaNegocioException("La orden ya tiene un presupuesto adicional pendiente de respuesta.");
                    }
                }

                _presupuestoRepository.PublicarBorrador(borradorDb.Id, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("PRESUPUESTO_BORRADOR_PUBLICADO",
                    "id_orden=" + ordenDb.Id + " | id_presupuesto=" + borradorDb.Id, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al publicar borrador de presupuesto", ex);
            }
        }

        public void EliminarBorrador(int idPresupuesto)
        {
            try
            {
                ObtenerIdUsuarioSesion();

                Presupuesto borradorDb = _presupuestoRepository.ObtenerPorId(idPresupuesto);

                if (borradorDb == null)
                    throw new ReglaNegocioException("El presupuesto seleccionado no existe.");

                if (borradorDb.Estado != EstadoPresupuesto.Borrador)
                    throw new ReglaNegocioException("Solo se puede eliminar un presupuesto en borrador.");

                _presupuestoRepository.EliminarBorrador(borradorDb.Id);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("PRESUPUESTO_ELIMINADO",
                    "id_orden=" + borradorDb.IdOrden + " | id_presupuesto=" + borradorDb.Id, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar borrador de presupuesto", ex);
            }
        }

        public void AnularPresupuesto(int idPresupuesto, string motivo)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                if (string.IsNullOrWhiteSpace(motivo))
                    throw new ReglaNegocioException("El motivo de la anulacion es obligatorio.");

                Presupuesto presupuestoDb = _presupuestoRepository.ObtenerPorId(idPresupuesto);

                if (presupuestoDb == null)
                    throw new ReglaNegocioException("El presupuesto seleccionado no existe.");

                string estadoAnterior = presupuestoDb.Estado;

                presupuestoDb.Anular(motivo.Trim());

                OrdenServicio ordenDb = ObtenerOrdenExistente(presupuestoDb.IdOrden);
                string estadoOrdenAnterior = ordenDb.Estado;
                string nuevoEstadoOrden = null;

                if (presupuestoDb.Tipo == TipoPresupuesto.Original)
                {
                    if (estadoAnterior == EstadoPresupuesto.Aprobado)
                    {
                        List<Reparacion> reparaciones = _reparacionRepository.ListarPorOrden(ordenDb.Id);

                        if (reparaciones.Count > 0)
                            throw new ReglaNegocioException("No se puede anular el presupuesto aprobado porque la orden ya tiene reparaciones iniciadas.");

                        if (ordenDb.Estado != EstadoOrdenServicio.AutorizadoReparacion)
                            throw new ReglaNegocioException("No se puede anular el presupuesto aprobado en el estado actual de la orden.");

                        ordenDb.RevertirAPendientePresupuesto();
                        nuevoEstadoOrden = ordenDb.Estado;
                    }
                    else if (estadoAnterior == EstadoPresupuesto.Pendiente)
                    {
                        if (ordenDb.Estado != EstadoOrdenServicio.EsperandoRespuesta)
                            throw new ReglaNegocioException("No se puede anular el presupuesto pendiente en el estado actual de la orden.");

                        ordenDb.RevertirAPendientePresupuesto();
                        nuevoEstadoOrden = ordenDb.Estado;
                    }
                }
                else
                {
                    if (estadoAnterior == EstadoPresupuesto.Pendiente)
                    {
                        if (ordenDb.Estado != EstadoOrdenServicio.EsperandoRespuesta)
                            throw new ReglaNegocioException("No se puede anular el presupuesto pendiente en el estado actual de la orden.");

                        ordenDb.RevertirAPendientePresupuesto();
                        nuevoEstadoOrden = ordenDb.Estado;
                    }
                    else if (estadoAnterior == EstadoPresupuesto.Aprobado)
                    {
                        if (_presupuestoRepository.HasActividadPosterior(presupuestoDb.Id))
                            throw new ReglaNegocioException("No se puede anular el presupuesto adicional aprobado porque ya produjo actividad operativa.");

                        ordenDb.RevertirAPendientePresupuesto();
                        nuevoEstadoOrden = ordenDb.Estado;
                    }
                }

                _presupuestoRepository.AnularConTransicion(presupuestoDb.Id, presupuestoDb.MotivoAnulacion,
                    estadoOrdenAnterior, nuevoEstadoOrden, null, null,
                    "Presupuesto anulado. Motivo: " + presupuestoDb.MotivoAnulacion, false, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("PRESUPUESTO_ANULADO",
                    "id_orden=" + ordenDb.Id + " | id_presupuesto=" + presupuestoDb.Id
                    + " | tipo=" + presupuestoDb.Tipo + " | estado_anterior=" + estadoAnterior, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al anular presupuesto", ex);
            }
        }

        public void CancelarSolicitudAdicional(int idOrden)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.PendientePresupuesto)
                    throw new ReglaNegocioException("Solo se puede cancelar la solicitud adicional de una orden pendiente de presupuesto.");

                string estadoOrigen;

                if (!TryObtenerOrigenSolicitud(idOrden, out estadoOrigen))
                    throw new ReglaNegocioException("La orden no registra una solicitud de presupuesto adicional para cancelar.");

                List<Presupuesto> adicionales = _presupuestoRepository.ListarAdicionales(idOrden);

                foreach (Presupuesto a in adicionales)
                {
                    if (a.Estado == EstadoPresupuesto.Pendiente)
                        throw new ReglaNegocioException("No se puede cancelar la solicitud porque la orden tiene un presupuesto adicional pendiente de respuesta.");
                }

                string estadoAnterior = ordenDb.Estado;

                ordenDb.CancelarSolicitudAdicional(estadoOrigen);

                _presupuestoRepository.CancelarSolicitudAdicional(ordenDb.Id, estadoOrigen,
                    estadoAnterior, idUsuario, "Solicitud de presupuesto adicional cancelada.");

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("SOLICITUD_ADICIONAL_CANCELADA", "id_orden=" + idOrden, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar solicitud adicional", ex);
            }
        }

        public bool PuedeCancelarSolicitud(int idOrden)
        {
            try
            {
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.PendientePresupuesto)
                    return false;

                string estadoOrigen;

                if (!TryObtenerOrigenSolicitud(idOrden, out estadoOrigen))
                    return false;

                List<Presupuesto> adicionales = _presupuestoRepository.ListarAdicionales(idOrden);

                foreach (Presupuesto a in adicionales)
                {
                    if (a.Estado == EstadoPresupuesto.Pendiente)
                        return false;
                }

                return true;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al evaluar cancelacion de solicitud", ex);
            }
        }

        public void QuitarConsumo(int idReparacion, int idRepuesto, int cantidad)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Reparacion reparacionDb = _reparacionRepository.ObtenerPorId(idReparacion);

                if (reparacionDb == null)
                    throw new ReglaNegocioException("La reparacion seleccionada no existe.");

                if (reparacionDb.FechaFin.HasValue)
                    throw new ReglaNegocioException("La reparacion ya fue finalizada.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(reparacionDb.IdOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.EnReparacion)
                    throw new ReglaNegocioException("Solo se puede quitar consumo de una orden en reparacion.");

                if (cantidad <= 0)
                    throw new ReglaNegocioException("La cantidad a devolver debe ser mayor a cero.");

                List<ReparacionRepuesto> consumidos = _reparacionRepository.ListarConsumidos(idReparacion);
                ReparacionRepuesto fila = null;

                foreach (ReparacionRepuesto c in consumidos)
                {
                    if (c.IdRepuesto == idRepuesto)
                        fila = c;
                }

                if (fila == null)
                    throw new ReglaNegocioException("La reparacion no tiene consumo registrado del repuesto.");

                if (cantidad > fila.Cantidad)
                    throw new ReglaNegocioException("La cantidad a devolver supera la consumida.");

                _reparacionRepository.DevolverConsumo(idReparacion, idRepuesto, cantidad, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("CONSUMO_DEVUELTO",
                    "id_reparacion=" + idReparacion + " | id_repuesto=" + idRepuesto + " | cantidad=" + cantidad, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al quitar consumo", ex);
            }
        }

        public void CancelarOrden(int idOrden, string motivo)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);
                string estadoAnterior = ordenDb.Estado;

                ordenDb.Cancelar(motivo);

                Reparacion abierta = _reparacionRepository.ObtenerAbierta(idOrden);

                if (abierta != null)
                {
                    _reparacionRepository.CerrarAbiertaYCancelar(idOrden, motivo.Trim(), idUsuario);
                }
                else
                {
                    _ordenRepository.CambiarEstadoConHistorial(ordenDb.Id, estadoAnterior, ordenDb.Estado,
                        idUsuario, "Orden cancelada", ordenDb.Resultado, ordenDb.ObservacionResultado, null);
                }

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Orden cancelada", "id_orden=" + idOrden, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar orden", ex);
            }
        }

        public Entrega EntregarOrden(int idOrden, string entregadoA, string documentoReceptor, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);
                string estadoAnterior = ordenDb.Estado;

                ordenDb.Entregar();

                Entrega entregaToSave = Entrega.CrearNuevo(
                    idOrden, idUsuario, entregadoA, documentoReceptor, observaciones);

                Entrega entregaDb = _entregaRepository.CrearConTransicion(
                    entregaToSave, estadoAnterior, ordenDb.Estado, idUsuario, "Orden entregada");

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Orden entregada", "id_orden=" + idOrden, "ORDENES");

                return entregaDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al entregar orden", ex);
            }
        }

        public void CancelarEntrega(int idOrden, string motivo)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                if (string.IsNullOrWhiteSpace(motivo))
                    throw new ReglaNegocioException("El motivo de la cancelacion de la entrega es obligatorio.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);
                string estadoAnterior = ordenDb.Estado;

                Entrega entregaDb = _entregaRepository.ObtenerPorOrden(idOrden);

                if (entregaDb == null)
                    throw new ReglaNegocioException("La orden no tiene una entrega registrada para cancelar.");

                ordenDb.CancelarEntrega();

                bool limpiarResultado = ordenDb.Estado == EstadoOrdenServicio.EnPruebas;

                _entregaRepository.CancelarEntregaConTransicion(ordenDb.Id, estadoAnterior, ordenDb.Estado,
                    limpiarResultado, idUsuario, "Entrega cancelada. Motivo: " + motivo.Trim());

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("ENTREGA_CANCELADA",
                    "id_orden=" + idOrden + " | motivo=" + motivo.Trim(), "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar entrega", ex);
            }
        }

        public OrdenServicio ModificarRecepcion(int idOrden, string problema, string estadoFisico,
            string accesorios, string observacionesIngreso)
        {
            try
            {
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                ordenDb.ActualizarRecepcion(problema, estadoFisico, accesorios, observacionesIngreso);

                _ordenRepository.ActualizarRecepcion(ordenDb.Id, ordenDb.ProblemaInformado,
                    ordenDb.EstadoFisicoIngreso, ordenDb.AccesoriosIngreso, ordenDb.ObservacionesIngreso);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Modificacion de recepcion", "id_orden=" + idOrden, "ORDENES");

                return ordenDb;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar recepcion", ex);
            }
        }

        public List<OrdenServicio> Listar(bool incluirEntregadas = false)
        {
            try
            {
                return _ordenRepository.Listar(incluirEntregadas);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar ordenes", ex);
            }
        }

        public List<OrdenServicio> ListarPorCliente(int idCliente, bool incluirEntregadas = false)
        {
            try
            {
                return _ordenRepository.ListarPorCliente(idCliente, incluirEntregadas);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar ordenes del cliente", ex);
            }
        }

        public OrdenServicio ObtenerPorId(int id)
        {
            try
            {
                OrdenServicio orden = _ordenRepository.ObtenerPorId(id);

                if (orden == null)
                    throw new ReglaNegocioException("La orden seleccionada no existe.");

                return orden;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener orden", ex);
            }
        }

        public Diagnostico ObtenerDiagnostico(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _diagnosticoRepository.ObtenerPorOrden(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener diagnostico", ex);
            }
        }

        public Presupuesto ObtenerPresupuesto(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _presupuestoRepository.ObtenerOriginal(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener presupuesto", ex);
            }
        }

        public Presupuesto ObtenerPresupuestoOriginal(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _presupuestoRepository.ObtenerOriginal(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener presupuesto original", ex);
            }
        }

        public List<Presupuesto> ListarAdicionales(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _presupuestoRepository.ListarAdicionales(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar presupuestos adicionales", ex);
            }
        }

        public Presupuesto ObtenerPresupuestoPorId(int idPresupuesto)
        {
            try
            {
                Presupuesto presupuesto = _presupuestoRepository.ObtenerPorId(idPresupuesto);

                if (presupuesto == null)
                    throw new ReglaNegocioException("El presupuesto seleccionado no existe.");

                return presupuesto;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener presupuesto", ex);
            }
        }

        public List<Presupuesto> ListarTodosPresupuestos(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _presupuestoRepository.ListarPorOrden(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar presupuestos", ex);
            }
        }

        public bool ExisteAdicionalPendiente(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _presupuestoRepository.ExisteAdicionalPendiente(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar presupuestos adicionales pendientes", ex);
            }
        }

        public decimal CalcularMontoAutorizado(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _presupuestoRepository.CalcularMontoAutorizado(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular monto autorizado", ex);
            }
        }

        public List<DetallePresupuesto> ListarDetalle(int idPresupuesto)
        {
            try
            {
                return _presupuestoRepository.ListarDetalle(idPresupuesto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar detalle del presupuesto", ex);
            }
        }

        public List<HistorialEstadoOrden> ListarHistorial(int idOrden)
        {
            try
            {
                return _historialRepository.ListarPorOrden(idOrden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar historial de la orden", ex);
            }
        }

        public Entrega ObtenerEntrega(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _entregaRepository.ObtenerPorOrden(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener entrega", ex);
            }
        }

        public List<Usuario> ListarTecnicosElegibles()
        {
            try
            {
                List<Usuario> elegibles = new List<Usuario>();

                foreach (Usuario usuario in _usuarioRepository.Listar())
                {
                    if (!usuario.Activo)
                        continue;

                    if (TienePermisoEfectivo(usuario.Id, CodigosPermiso.OrdenesEditar))
                        elegibles.Add(usuario);
                }

                return elegibles;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar tecnicos elegibles", ex);
            }
        }

        public Reparacion IniciarReparacion(int idOrden)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                if (!ordenDb.IdTecnicoAsignado.HasValue)
                    throw new ReglaNegocioException("La orden debe tener un tecnico asignado para iniciar la reparacion.");

                Usuario tecnico = ValidarTecnicoElegible(ordenDb.IdTecnicoAsignado.Value);

                int idReparacion;

                if (ordenDb.Estado == EstadoOrdenServicio.AutorizadoReparacion)
                {
                    ordenDb.IniciarReparacion();

                    idReparacion = _reparacionRepository.IniciarReparacion(
                        idOrden, tecnico.Id, idUsuario);
                }
                else if (ordenDb.Estado == EstadoOrdenServicio.EnReparacion)
                {
                    if (_reparacionRepository.ObtenerAbierta(idOrden) != null)
                        throw new ReglaNegocioException("La orden ya tiene una reparacion abierta.");

                    idReparacion = _reparacionRepository.IniciarIntervencionAdicional(
                        idOrden, tecnico.Id, idUsuario);
                }
                else
                {
                    throw new ReglaNegocioException("Solo se puede iniciar la reparacion de una orden autorizada o en reparacion sin intervencion abierta.");
                }

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Reparacion iniciada",
                    "id_orden=" + idOrden + " | id_reparacion=" + idReparacion, "ORDENES");

                return _reparacionRepository.ObtenerPorId(idReparacion);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar reparacion", ex);
            }
        }

        public void ConsumirRepuesto(int idReparacion, int idRepuesto, int cantidad)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Reparacion reparacionDb = _reparacionRepository.ObtenerPorId(idReparacion);

                if (reparacionDb == null)
                    throw new ReglaNegocioException("La reparacion seleccionada no existe.");

                if (reparacionDb.FechaFin.HasValue)
                    throw new ReglaNegocioException("La reparacion ya fue finalizada.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(reparacionDb.IdOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.EnReparacion)
                    throw new ReglaNegocioException("Solo se puede consumir repuestos de una orden en reparacion.");

                Repuesto repuestoDb = _repuestoRepository.ObtenerPorId(idRepuesto);

                if (repuestoDb == null)
                    throw new ReglaNegocioException("El repuesto seleccionado no existe.");

                if (!repuestoDb.Activo)
                    throw new ReglaNegocioException("El repuesto seleccionado esta inactivo.");

                if (cantidad <= 0)
                    throw new ReglaNegocioException("La cantidad consumida debe ser mayor a cero.");

                _reparacionRepository.ConsumirRepuesto(idReparacion, idRepuesto, cantidad,
                    repuestoDb.CostoActual, idUsuario, "Consumo reparacion " + idReparacion);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Repuesto consumido",
                    "id_reparacion=" + idReparacion + " | id_repuesto=" + idRepuesto + " | cantidad=" + cantidad, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consumir repuesto", ex);
            }
        }

        public void FinalizarReparacion(int idReparacion, string trabajoRealizado, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Reparacion reparacionDb = _reparacionRepository.ObtenerPorId(idReparacion);

                if (reparacionDb == null)
                    throw new ReglaNegocioException("La reparacion seleccionada no existe.");

                if (reparacionDb.FechaFin.HasValue)
                    throw new ReglaNegocioException("La reparacion ya fue finalizada.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(reparacionDb.IdOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.EnReparacion)
                    throw new ReglaNegocioException("Solo se puede finalizar la reparacion de una orden en reparacion.");

                reparacionDb.Finalizar(trabajoRealizado, observaciones);
                ordenDb.FinalizarReparacion();

                _reparacionRepository.FinalizarReparacion(idReparacion,
                    reparacionDb.TrabajoRealizado, reparacionDb.Observaciones, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("Reparacion finalizada",
                    "id_reparacion=" + idReparacion + " | id_orden=" + reparacionDb.IdOrden, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al finalizar reparacion", ex);
            }
        }

        public Prueba RegistrarPrueba(int idReparacion, string descripcion, bool aprobada, string observaciones)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                Reparacion reparacionDb = _reparacionRepository.ObtenerPorId(idReparacion);

                if (reparacionDb == null)
                    throw new ReglaNegocioException("La reparacion seleccionada no existe.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(reparacionDb.IdOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.EnPruebas)
                    throw new ReglaNegocioException("Solo se puede registrar la prueba de una orden en pruebas.");

                if (_reparacionRepository.ObtenerAbierta(ordenDb.Id) != null)
                    throw new ReglaNegocioException("Existe una reparacion abierta en la orden.");

                Reparacion ultima = ObtenerUltimaIntervencion(ordenDb.Id);

                if (!ultima.FechaFin.HasValue)
                    throw new ReglaNegocioException("La ultima intervencion aun no fue finalizada.");

                if (ultima.Id != idReparacion)
                    throw new ReglaNegocioException("Solo se puede registrar la prueba de la ultima intervencion finalizada.");

                Prueba pruebaToSave = Prueba.CrearNuevo(idReparacion, ultima.IdUsuarioTecnico,
                    descripcion, aprobada ? ResultadoPrueba.Aprobada : ResultadoPrueba.RequiereRevision,
                    observaciones);

                // Solo persiste: la decision la toma FinalizarPruebas.
                int idPrueba = _reparacionRepository.RegistrarPrueba(idReparacion,
                    pruebaToSave.IdUsuarioTecnico, pruebaToSave.Descripcion,
                    pruebaToSave.Resultado, pruebaToSave.Observaciones);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("PRUEBA_REGISTRADA",
                    "id_orden=" + ordenDb.Id + " | id_reparacion=" + idReparacion
                    + " | id_prueba=" + idPrueba + " | resultado=" + pruebaToSave.Resultado, "ORDENES");

                return _reparacionRepository.ObtenerPruebaPorId(idPrueba);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar prueba", ex);
            }
        }

        public void FinalizarPruebas(int idOrden)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();
                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.EnPruebas)
                    throw new ReglaNegocioException("Solo se pueden finalizar las pruebas de una orden en pruebas.");

                if (_reparacionRepository.ObtenerAbierta(idOrden) != null)
                    throw new ReglaNegocioException("Existe una reparacion abierta en la orden.");

                Reparacion ultima = ObtenerUltimaIntervencion(idOrden);

                if (!ultima.FechaFin.HasValue)
                    throw new ReglaNegocioException("La ultima intervencion aun no fue finalizada.");

                List<Prueba> pruebas = _reparacionRepository.ListarPruebas(ultima.Id);
                List<Prueba> validas = new List<Prueba>();

                foreach (Prueba p in pruebas)
                {
                    if (p.Resultado != ResultadoPrueba.Anulada)
                        validas.Add(p);
                }

                if (validas.Count == 0)
                    throw new ReglaNegocioException("No hay pruebas validas para finalizar las pruebas de la orden.");

                bool todasAprobadas = true;

                foreach (Prueba p in validas)
                {
                    if (p.Resultado != ResultadoPrueba.Aprobada)
                    {
                        todasAprobadas = false;
                        break;
                    }
                }

                if (todasAprobadas)
                    ordenDb.FinalizarPruebasAprobadas();
                else
                    ordenDb.FinalizarPruebasRequiereRevision();

                _reparacionRepository.FinalizarPruebas(idOrden, todasAprobadas, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("PRUEBAS_FINALIZADAS",
                    "id_orden=" + idOrden + " | pruebas=" + validas.Count
                    + " | resultado=" + (todasAprobadas ? "TodasAprobadas" : "RequiereRevision"), "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al finalizar pruebas", ex);
            }
        }

        public void ReabrirPruebas(int idOrden, string motivo)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                if (string.IsNullOrWhiteSpace(motivo))
                    throw new ReglaNegocioException("El motivo de la reapertura de pruebas es obligatorio.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(idOrden);
                string estadoAnterior = ordenDb.Estado;

                ordenDb.ReabrirPruebas();

                _ordenRepository.ReabrirPruebasConTransicion(ordenDb.Id, estadoAnterior, ordenDb.Estado,
                    idUsuario, "Pruebas reabiertas. Motivo: " + motivo.Trim());

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("PRUEBAS_REABIERTAS",
                    "id_orden=" + idOrden + " | motivo=" + motivo.Trim(), "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al reabrir pruebas", ex);
            }
        }

        public void AnularPrueba(int idPrueba, string motivo)
        {
            try
            {
                int idUsuario = ObtenerIdUsuarioSesion();

                if (string.IsNullOrWhiteSpace(motivo))
                    throw new ReglaNegocioException("El motivo de la anulacion es obligatorio.");

                Prueba pruebaDb = _reparacionRepository.ObtenerPruebaPorId(idPrueba);

                if (pruebaDb == null)
                    throw new ReglaNegocioException("La prueba seleccionada no existe.");

                if (pruebaDb.Resultado == ResultadoPrueba.Anulada)
                    throw new ReglaNegocioException("La prueba ya se encuentra anulada.");

                Reparacion reparacionDb = _reparacionRepository.ObtenerPorId(pruebaDb.IdReparacion);

                if (reparacionDb == null)
                    throw new ReglaNegocioException("La reparacion seleccionada no existe.");

                OrdenServicio ordenDb = ObtenerOrdenExistente(reparacionDb.IdOrden);

                if (ordenDb.Estado != EstadoOrdenServicio.EnPruebas)
                    throw new ReglaNegocioException("Solo se puede anular la prueba de una orden en pruebas.");

                Reparacion ultima = ObtenerUltimaIntervencion(ordenDb.Id);

                if (pruebaDb.IdReparacion != ultima.Id)
                    throw new ReglaNegocioException("Solo se puede anular una prueba de la ultima intervencion.");

                pruebaDb.Anular(motivo.Trim(), idUsuario);

                // Sin cambio de estado ni reapertura.
                _reparacionRepository.AnularPrueba(idPrueba, pruebaDb.MotivoAnulacion, idUsuario);

                BitacoraService bitacoraService = new BitacoraService();
                bitacoraService.Registrar("PRUEBA_ANULADA",
                    "id_orden=" + ordenDb.Id + " | id_prueba=" + idPrueba, "ORDENES");
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al anular prueba", ex);
            }
        }

        public List<Reparacion> ListarReparaciones(int idOrden)
        {
            try
            {
                ObtenerOrdenExistente(idOrden);
                return _reparacionRepository.ListarPorOrden(idOrden);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar reparaciones", ex);
            }
        }

        public Reparacion ObtenerReparacion(int idReparacion)
        {
            try
            {
                Reparacion reparacion = _reparacionRepository.ObtenerPorId(idReparacion);

                if (reparacion == null)
                    throw new ReglaNegocioException("La reparacion seleccionada no existe.");

                return reparacion;
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener reparacion", ex);
            }
        }

        public List<ReparacionRepuesto> ListarConsumidos(int idReparacion)
        {
            try
            {
                if (_reparacionRepository.ObtenerPorId(idReparacion) == null)
                    throw new ReglaNegocioException("La reparacion seleccionada no existe.");

                return _reparacionRepository.ListarConsumidos(idReparacion);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar repuestos consumidos", ex);
            }
        }

        public List<Prueba> ListarPruebas(int idReparacion)
        {
            try
            {
                if (_reparacionRepository.ObtenerPorId(idReparacion) == null)
                    throw new ReglaNegocioException("La reparacion seleccionada no existe.");

                return _reparacionRepository.ListarPruebas(idReparacion);
            }
            catch (ReglaNegocioException ex)
            {
                throw new ReglaNegocioException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar pruebas", ex);
            }
        }

        private decimal ValidarItemsYCalcularSubtotal(List<DetallePresupuesto> items)
        {
            if (items == null || items.Count == 0)
                throw new ReglaNegocioException("El presupuesto debe tener al menos un item.");

            decimal subtotal = 0;

            foreach (DetallePresupuesto item in items)
            {
                if (item.TipoItem != TipoItemPresupuesto.ManoObra
                    && item.TipoItem != TipoItemPresupuesto.Servicio)
                    throw new ReglaNegocioException("El tipo de item solo puede ser mano de obra o servicio.");

                subtotal += item.Subtotal;
            }

            return subtotal;
        }

        private OrdenServicio ObtenerOrdenExistente(int idOrden)
        {
            OrdenServicio orden = _ordenRepository.ObtenerPorId(idOrden);

            if (orden == null)
                throw new ReglaNegocioException("La orden seleccionada no existe.");

            return orden;
        }

        private Reparacion ObtenerUltimaIntervencion(int idOrden)
        {
            List<Reparacion> reparaciones = _reparacionRepository.ListarPorOrden(idOrden);

            if (reparaciones.Count == 0)
                throw new ReglaNegocioException("La orden no tiene reparaciones registradas.");

            Reparacion ultima = reparaciones[0];

            foreach (Reparacion r in reparaciones)
            {
                if (r.NumeroIntervencion > ultima.NumeroIntervencion)
                    ultima = r;
            }

            return ultima;
        }

        private bool TryObtenerOrigenSolicitud(int idOrden, out string estadoOrigen)
        {
            // Origen = ULTIMA transicion a PendientePresupuesto desde Autorizado/EnReparacion/EnPruebas.
            // Se recorre hacia atras: la primera coincidencia es la solicitud vigente. Los
            // Aprobado/Rechazado de ciclos anteriores no bloquean (ver CancelarSolicitudAdicional).
            estadoOrigen = null;

            List<HistorialEstadoOrden> historial = _historialRepository.ListarPorOrden(idOrden);

            for (int i = historial.Count - 1; i >= 0; i--)
            {
                HistorialEstadoOrden h = historial[i];

                if (h.EstadoNuevo == EstadoOrdenServicio.PendientePresupuesto
                    && (h.EstadoAnterior == EstadoOrdenServicio.EnReparacion
                        || h.EstadoAnterior == EstadoOrdenServicio.EnPruebas
                        || h.EstadoAnterior == EstadoOrdenServicio.AutorizadoReparacion))
                {
                    estadoOrigen = h.EstadoAnterior;
                    return true;
                }
            }

            return false;
        }

        private Usuario ValidarTecnicoElegible(int idTecnico)
        {
            Usuario tecnico = _usuarioRepository.ObtenerPorId(idTecnico);

            if (tecnico == null)
                throw new ReglaNegocioException("El tecnico seleccionado no existe.");

            if (!tecnico.Activo)
                throw new ReglaNegocioException("El tecnico seleccionado esta inactivo.");

            if (!TienePermisoEfectivo(tecnico.Id, CodigosPermiso.OrdenesEditar))
                throw new ReglaNegocioException("El usuario seleccionado no tiene permiso para ser tecnico asignado.");

            return tecnico;
        }

        private bool TienePermisoEfectivo(int idUsuario, string codigo)
        {
            List<PermisoComponent> efectivos = _usuarioPermisoService.ListarPermisosEfectivos(idUsuario);

            foreach (PermisoComponent permiso in efectivos)
            {
                if (ContieneCodigo(permiso, codigo))
                    return true;
            }

            return false;
        }

        private bool ContieneCodigo(IPermisoComponent permiso, string codigo)
        {
            if (permiso == null)
                return false;

            if (!permiso.EsFamilia && !string.IsNullOrWhiteSpace(permiso.Codigo)
                && string.Equals(permiso.Codigo, codigo, StringComparison.OrdinalIgnoreCase))
                return true;

            foreach (IPermisoComponent hijo in permiso.Hijos)
            {
                if (ContieneCodigo(hijo, codigo))
                    return true;
            }

            return false;
        }

        private int ObtenerIdUsuarioSesion()
        {
            if (!SessionManager.HaySesionActiva() || SessionManager.ObtenerUsuarioActual() == null)
                throw new ReglaNegocioException("Debe iniciar sesion para gestionar ordenes.");

            return SessionManager.ObtenerUsuarioActual().Id;
        }
    }
}
