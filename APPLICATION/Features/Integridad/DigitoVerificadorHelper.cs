using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DOMAIN.Features.Usuarios;

namespace APPLICATION.Features.Integridad
{
    public static class DigitoVerificadorHelper
    {
        public static string CalcularDVH(Usuario usuario)
        {
            string raw = $"{usuario.Username}|1|{usuario.Password}|2|{usuario.Id}|3";

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return Convert.ToBase64String(hash);
            }
        }

        public static string CalcularDVV(List<UserDVH> filas)
        {
            var ordenadas = filas.OrderBy(f => f.Id).ToList();
            string raw = string.Join("|", ordenadas.Select(f => $"{f.DVH}|{f.Id}"));

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
