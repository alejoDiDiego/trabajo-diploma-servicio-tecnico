using System;
using System.Collections.Generic;
using DOMAIN.Features.Bitacora;
using REPOSITORY.Features.Bitacora;
using SERVICES.Auth;

namespace APPLICATION.Features.Bitacora
{
    public class BitacoraService
    {
        private readonly BitacoraRepository _repository;

        public BitacoraService()
        {
            _repository = new BitacoraRepository();
        }

        public void Inicializar()
        {
            _repository.Inicializar();
        }

        public void Registrar(string actividad, string detalle, string tipoActividad)
        {
            string usuario = SessionManager.HaySesionActiva()
                ? SessionManager.ObtenerUsuarioActual().Username
                : "Sistema";

            EntradaBitacora entrada = EntradaBitacora.Crear(0, DateTime.Now, usuario, actividad, detalle, tipoActividad);
            _repository.Insertar(entrada);
        }

        public void Registrar(string usuario, string actividad, string detalle, string tipoActividad)
        {
            EntradaBitacora entrada = EntradaBitacora.Crear(0, DateTime.Now, usuario, actividad, detalle, tipoActividad);
            _repository.Insertar(entrada);
        }

        public List<EntradaBitacora> Listar()
        {
            return _repository.Listar();
        }

        public List<EntradaBitacora> Buscar(string usuario, DateTime? desde, DateTime? hasta, string tipoActividad)
        {
            return _repository.Buscar(usuario, desde, hasta, tipoActividad);
        }

        public string[] ObtenerTiposActividad()
        {
            return new string[]
            {
                "SESION",
                "USUARIOS",
                "PERMISOS",
                "IDIOMAS",
                "CONTROL_CAMBIOS",
                "INTEGRIDAD",
                "CLIENTES",
                "EQUIPOS",
                "TIPOS_EQUIPO",
                "MARCAS"
            };
        }
    }
}
