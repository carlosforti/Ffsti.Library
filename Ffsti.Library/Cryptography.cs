using System;
using System.Security.Cryptography;
using System.Text;

namespace Ffsti
{
    /// <summary>
    /// Cryptographic methods
    /// </summary>
    public class Cryptography
    {
        private byte[] key = ASCIIEncoding.ASCII.GetBytes(@"Put key here");
        private byte[] iv = ASCIIEncoding.ASCII.GetBytes(@"Put IV here");

        private RijndaelManaged rij = new RijndaelManaged();

        /// <summary>
        /// Creates the Cryptography class using the given secret key and initialization vector
        /// </summary>
        /// <param name="secretKey">Secret key as string</param>
        /// <param name="initializationVector">Initialization Vector as string</param>
        public Cryptography(string secretKey, string initializationVector)
        {
            key = Encoding.ASCII.GetBytes(secretKey);
            iv = Encoding.ASCII.GetBytes(initializationVector);
        }

        /// <summary>
        /// Creates the Cryptography class using the given secret key and initialization vector
        /// </summary>
        /// <param name="secretKey">Secret key as a byte array</param>
        /// <param name="initializationVector">Initialization Vector as byte array</param>
        public Cryptography(byte[] secretKey, byte[] initializationVector)
        {
            key = secretKey;
            iv = initializationVector;
        }

        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="value">String to be encrypted</param>
        public string Encrypt(string value)
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
        /// Decrypt a string
        /// </summary>
        /// <param name="value">String to be decrypted</param>
        public string Decrypt(string value)
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
