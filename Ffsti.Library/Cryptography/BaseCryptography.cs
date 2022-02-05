using System;
using System.Security.Cryptography;
using System.Text;

namespace Ffsti.Library.Cryptography
{
    /// <summary>
    /// Base class for cryptography.
    /// </summary>
    /// <typeparam name="T">A SymmetricAlgorithm derivated class (ie.: AesManaged)</typeparam>
    public class BaseCryptography<T>
        where T : SymmetricAlgorithm, new()
    {
        #region Fields
        private readonly SymmetricAlgorithm _algorithm = new T();

        /// <summary>
        /// Gets or sets the secret _key to the _algorithm
        /// </summary>
        private readonly string _key;

        #endregion

        #region Private Methods
        private static byte[] MergeIvAndData(byte[] iv, byte[] data)
        {
            var result = new byte[iv.Length + data.Length];

            for (var i = 0; i < iv.Length; i++)
            {
                result[i] = iv[i];
            }

            for (var i = 0; i < data.Length; i++)
            {
                result[i + iv.Length] = data[i];
            }

            return result;
        }

        private static byte[] GetIv(byte[] array, int ivLength)
        {
            var result = new byte[ivLength];

            for (var i = 0; i < ivLength; i++)
            {
                result[i] = array[i];
            }

            return result;
        }

        private static byte[] GetData(byte[] array, int ivLength)
        {
            var dataLength = array.Length - ivLength;

            var result = new byte[dataLength];

            for (var i = 0; i < dataLength; i++)
            {
                result[i] = array[i + ivLength];
            }

            return result;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates the Cryptography class using the given secret _key
        /// </summary>
        /// <param name="secretKey">Base64 secret _key</param>
        public BaseCryptography(string secretKey)
        {
            _key = secretKey;
        }

        /// <summary>
        /// Creates the Cryptography class using the given secret _key
        /// </summary>
        /// <param name="secretKey">Secret _key as a byte array</param>
        public BaseCryptography(byte[] secretKey)
            : this(Convert.ToBase64String(secretKey))
        { }

        /// <summary>
        /// Creates the Cryptography class using a random generated secret _key
        /// </summary>
        public BaseCryptography()
        {
            _algorithm.GenerateKey();
            _key = Convert.ToBase64String(_algorithm.Key);
        }

        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="value">String to be encrypted</param>
        public string Encrypt(string value)
        {
            _algorithm.Key = Convert.FromBase64String(_key);
            _algorithm.GenerateIV();

            var iv = _algorithm.IV;

            var input = Encoding.ASCII.GetBytes(value);

            var encryptor = _algorithm.CreateEncryptor(_algorithm.Key, iv);
            var output = encryptor.TransformFinalBlock(input, 0, input.Length);

            var ivData = MergeIvAndData(iv, output);

            _algorithm.Clear();

            return Convert.ToBase64String(ivData);
        }

        /// <summary>
        /// Decrypt a string
        /// </summary>
        /// <param name="value">String to be decrypted</param>
        public string Decrypt(string value)
        {
            _algorithm.Key = Convert.FromBase64String(_key);

            var ivData = Convert.FromBase64String(value);
            var iv = GetIv(ivData, _algorithm.IV.Length);
            var data = GetData(ivData, _algorithm.IV.Length);

            var decryptor = _algorithm.CreateDecryptor(_algorithm.Key, iv);
            var output = decryptor.TransformFinalBlock(data, 0, data.Length);
            var resultado = Encoding.ASCII.GetString(output);

            _algorithm.Clear();

            return resultado;
        }
        #endregion
    }
}
