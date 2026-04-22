using APPLICATION.Interfaces;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace CROSSCUTTING.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32;  // 256 bit
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return null;

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, _hashAlgorithm))
            {
                hash = pbkdf2.GetBytes(KeySize);
            }

            // Creamos el payload con la cantidad de bytes necesarios
            byte[] payload = new byte[SaltSize + KeySize];
            // Combinamos Salt + Hash en un solo arreglo para poder recuperarlos luego
            Buffer.BlockCopy(salt, 0, payload, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, payload, SaltSize, KeySize);

            return Convert.ToBase64String(payload);
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(password)) return false;

            byte[] payload;
            try
            {
                payload = Convert.FromBase64String(hashedPassword);
            }
            catch (FormatException)
            {
                return false;
            }

            if (payload.Length != SaltSize + KeySize) return false;

            // Toma los primeros (SaltSize) 
            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(payload, 0, salt, 0, SaltSize);

            // Saltea (SaltSize) y lo que queda es la llave
            byte[] storedKey = new byte[KeySize];
            Buffer.BlockCopy(payload, SaltSize, storedKey, 0, KeySize);

            byte[] generatedKey;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, _hashAlgorithm))
            {
                generatedKey = pbkdf2.GetBytes(KeySize);
            }

            return storedKey.SequenceEqual(generatedKey);
        }

    }
}
