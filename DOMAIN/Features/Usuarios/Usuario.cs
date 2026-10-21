using DOMAIN.Exceptions;
using ABSTRACTIONS.Features.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Features.Usuarios
{
    public class Usuario : IUsuario
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool Activo { get; private set; }
        public string DVH { get; private set; }

        private Usuario() { }

        public static Usuario CrearNuevo(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ReglaNegocioException("El Nombre de Usuario es obligatorio.");
            if (string.IsNullOrEmpty(password))
                throw new ReglaNegocioException("La Contraseña es obligatoria.");
            return new Usuario
            {
                Username = userName,
                Password = password,
                Activo = true
            };
        }

        public static Usuario CargarDesdeDB(int id, string userName, string password, string dvh = "", bool activo = true)
        {
            return new Usuario
            {
                Id = id,
                Username = userName,
                Password = password,
                DVH = dvh,
                Activo = activo
            };
        }

        public void Desactivar()
        {
            Activo = false;
        }

        public void Reactivar()
        {
            Activo = true;
        }

        public void SetDVH(string dvh)
        {
            DVH = dvh;
        }
    }
}
