using DOMAIN.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Features.Usuarios
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        private Usuario() { }

        public static Usuario CrearNuevo(string userName, string password)
        {
            // Hago validación porque puede venir de una fuente no confiable (ej: un formulario o de un error de programación)

            if (string.IsNullOrEmpty(userName))
                throw new ReglaNegocioException("El Nombre de Usuario es obligatorio.");
            if (string.IsNullOrEmpty(password))
                throw new ReglaNegocioException("La Contraseña es obligatoria.");
            return new Usuario
            {
                Username = userName,
                Password = password
            };
        }

        public static Usuario CargarDesdeDB(int id, string userName, string password)
        {

            return new Usuario
            {
                Id = id,
                Username = userName,
                Password = password
            };
        }
    }
}
