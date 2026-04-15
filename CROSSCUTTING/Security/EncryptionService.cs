using APPLICATION.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CROSSCUTTING.Security
{
    public class EncryptionService : IEncryptionService
    {
        private const string Passphrase = "SesionUsuarioSecretKey";
        private static readonly byte[] Salt = new byte[] { 0x21, 0x43, 0x65, 0x87, 0xA9, 0xCB, 0xED, 0x0F, 0x10, 0x32, 0x54, 0x76, 0x98, 0xBA, 0xDC, 0xFE };

        public string Encrypt(string plainText)
        {
            if (plainText == null)
                return null;

            using (var aes = new AesManaged())
            {
                var key = new Rfc2898DeriveBytes(Passphrase, Salt, 10000);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs, Encoding.UTF8))
                    {
                        sw.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            if (cipherText == null)
                return null;

            using (var aes = new AesManaged())
            {
                var key = new Rfc2898DeriveBytes(Passphrase, Salt, 10000);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                var buffer = Convert.FromBase64String(cipherText);
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(buffer))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
