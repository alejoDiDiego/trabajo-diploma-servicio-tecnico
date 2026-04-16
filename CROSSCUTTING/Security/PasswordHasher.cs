using APPLICATION.Interfaces;
using System;
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

            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, KeySize);

            // Creamos el payload con la cantidad de bytes necesarios
            byte[] payload = new byte[SaltSize + KeySize];
            // Combinamos Salt + Hash en un solo arreglo para poder recuperarlos luego
            Buffer.BlockCopy(salt, 0, payload, 0, SaltSize);
            Buffer.BlockCopy(hash, 0, payload, SaltSize, KeySize);

            return Convert.ToBase64String(payload);
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] payload = Convert.FromBase64String(hashedPassword);

            // Toma los primeros (SaltSize) 
            byte[] salt = payload.Take(SaltSize).ToArray();

            // Saltea (SaltSize) y lo que queda es la llave
            byte[] storedKey = payload.Skip(SaltSize).ToArray();

            byte[] generatedKey = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, KeySize);

            return CryptographicOperations.FixedTimeEquals(storedKey, generatedKey);
        }

    }
}
