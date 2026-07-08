using System;
using System.Collections.Generic;
using System.Linq;
using DOMAIN.Features.Usuarios;
using REPOSITORY.Features.Integridad;
using REPOSITORY.Features.Usuarios;

namespace APPLICATION.Features.Integridad
{
    public class IntegridadService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IntegridadRepository _integridadRepository;

        public IntegridadService()
        {
            _usuarioRepository = new UsuarioRepository();
            _integridadRepository = new IntegridadRepository();
        }

        public void Inicializar()
        {
            _integridadRepository.Inicializar();
        }

        private void CalcularDVHsPendientes()
        {
            try
            {
                List<Usuario> usuarios = _usuarioRepository.Listar();
                bool huboCambios = false;

                foreach (Usuario usuario in usuarios)
                {
                    if (string.IsNullOrEmpty(usuario.DVH))
                    {
                        string dvh = DigitoVerificadorHelper.CalcularDVH(usuario);
                        _usuarioRepository.ActualizarDVH(usuario.Id, dvh);
                        huboCambios = true;
                    }
                }

                if (huboCambios)
                    RecalcularDVVUsuarios();
            }
            catch
            {
            }
        }

        public bool VerificarIntegridadUsuarios()
        {
            try
            {
                List<UserDVH> filas = _usuarioRepository.ObtenerTodosDVH();

                if (filas.Count == 0)
                    return true;

                foreach (UserDVH fila in filas)
                {
                    Usuario usuario = _usuarioRepository.ObtenerPorId(fila.Id);
                    string dvhCalculado = DigitoVerificadorHelper.CalcularDVH(usuario);

                    if (string.IsNullOrEmpty(usuario.DVH))
                        return false;

                    if (dvhCalculado != usuario.DVH)
                        return false;
                }

                string dvvCalculado = DigitoVerificadorHelper.CalcularDVV(filas);
                string dvvAlmacenado = _integridadRepository.ObtenerDVV("Usuarios");

                if (dvvAlmacenado == null)
                    return false;

                return dvvCalculado == dvvAlmacenado;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RecalcularDVVUsuarios()
        {
            var filas = _usuarioRepository.ObtenerTodosDVH();
            string dvv = DigitoVerificadorHelper.CalcularDVV(filas);
            _integridadRepository.GuardarDVV("Usuarios", dvv);
        }

        public void RecalcularTodosDV()
        {
            List<Usuario> usuarios = _usuarioRepository.Listar();

            foreach (Usuario usuario in usuarios)
            {
                string dvh = DigitoVerificadorHelper.CalcularDVH(usuario);
                _usuarioRepository.ActualizarDVH(usuario.Id, dvh);
            }

            RecalcularDVVUsuarios();
        }
    }
}
