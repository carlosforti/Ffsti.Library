using System;
using System.Security.Cryptography;
using System.Text;

namespace Ffsti
{
    /// <summary>
    /// Cryptographic methods
    /// </summary>
    public static class Cryptography
    {
        private static byte[] key = ASCIIEncoding.ASCII.GetBytes(@"Put key here");
        private static byte[] iv = ASCIIEncoding.ASCII.GetBytes(@"Put IV here");

        private static RijndaelManaged rij = new RijndaelManaged();

        /// <summary>
        /// Initialize the key and initialization vector for the crypto using strings
        /// </summary>
        /// <param name="secretKey">The cryptographic secret key</param>
        /// <param name="initializationVector">The initialization vector</param>
        public static void Initialize(string secretKey, string initializationVector)
        {
            key = Encoding.ASCII.GetBytes(secretKey);
            iv = Encoding.ASCII.GetBytes(initializationVector);
        }

        /// <summary>
        /// Initialize the key and initialization vector for the crypto using arrays of byte
        /// </summary>
        /// <param name="secretKey">The cryptographic secret key</param>
        /// <param name="initializationVector">The initialization vector</param>
        public static void Initialize(byte[] secretKey, byte[] initializationVector)
        {
            key = secretKey;
            iv = initializationVector;
        }
        /// <summary>
        /// Encrypt a string using the initialized  key and iv
        /// </summary>
        /// <param name="value">String to encrypt</param>
        public static string Encrypt(string value)
        {
            rij.Key = key;
            rij.IV = iv;

            byte[] o = new byte[value.Length * 4];
            byte[] i = Encoding.UTF8.GetBytes(value);

            value = Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
            ICryptoTransform encryptor = rij.CreateEncryptor(key, iv);
            o = encryptor.TransformFinalBlock(i, 0, i.Length);
            string resultado = Encoding.ASCII.GetString(o);

            rij.Clear();

            return Convert.ToBase64String(o);
        }

        /// <summary>
        /// Decrypt a string using the initialized key and iv
        /// </summary>
        /// <param name="value">String to decrypt</param>
        public static string Decrypt(string value)
        {
            rij.Key = key;
            rij.IV = iv;

            byte[] o = new byte[value.Length * 4];
            byte[] i = Convert.FromBase64String(value);

            ICryptoTransform decryptor = rij.CreateDecryptor(key, iv);
            o = decryptor.TransformFinalBlock(i, 0, i.Length);
            string resultado = Encoding.ASCII.GetString(o);

            rij.Clear();

            return resultado;
        }
    }
}
