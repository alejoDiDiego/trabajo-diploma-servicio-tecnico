using APPLICATION.Interfaces;
using System;
using System.Security.Cryptography;

namespace CROSSCUTTING.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        public string HashPassword(string password)
        {
            if (password == null)
                return null;

            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var key = deriveBytes.GetBytes(KeySize);
                var payload = new byte[SaltSize + KeySize];
                Buffer.BlockCopy(salt, 0, payload, 0, SaltSize);
                Buffer.BlockCopy(key, 0, payload, SaltSize, KeySize);
                return Convert.ToBase64String(payload);
            }
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null || password == null)
                return false;

            var payload = Convert.FromBase64String(hashedPassword);
            if (payload.Length != SaltSize + KeySize)
                return false;

            var salt = new byte[SaltSize];
            var storedKey = new byte[KeySize];
            Buffer.BlockCopy(payload, 0, salt, 0, SaltSize);
            Buffer.BlockCopy(payload, SaltSize, storedKey, 0, KeySize);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                var key = deriveBytes.GetBytes(KeySize);
                return FixedTimeEquals(storedKey, key);
            }
        }

        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length)
                return false;

            var result = 0;
            for (int i = 0; i < left.Length; i++)
                result |= left[i] ^ right[i];

            return result == 0;
        }
    }
}
