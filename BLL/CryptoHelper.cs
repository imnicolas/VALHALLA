using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BLL
{
    public class CryptoHelper
    {
        // Llave maestra estática de 32 bytes (256 bits) para el alcance de este Trabajo Práctico.
        // En un entorno productivo real, esto se leería de un KMS (Key Management Service) o variables de entorno seguras.
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("ValhallaSecretoMasterKey2026!@#$"); 

        public byte[] Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.GenerateIV(); // Genera un Vector de Inicialización aleatorio para mayor seguridad

                using (var msEncrypt = new MemoryStream())
                {
                    // Prepend the IV to the final encrypted blob so we can decrypt it later
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    using (var csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        public string Decrypt(byte[] cipherData)
        {
            if (cipherData == null || cipherData.Length == 0)
                throw new ArgumentNullException(nameof(cipherData));

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;

                using (var msDecrypt = new MemoryStream(cipherData))
                {
                    // Read the IV from the first 16 bytes of the stream
                    byte[] iv = new byte[aesAlg.IV.Length];
                    msDecrypt.Read(iv, 0, iv.Length);
                    aesAlg.IV = iv;

                    using (var csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
