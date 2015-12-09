using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ffsti.Library
{
    /// <summary>
    /// Methods for hash calculations
    /// </summary>
    public static class Hashes
    {
        /// <summary>
        /// Calculates the MD5 Hash for a string
        /// </summary>
        /// <param name="value">String to calculate the hash</param>
        public static string Md5Hash(this string value)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calculates the SHA256 Hash for a string
        /// </summary>
        /// <param name="value">String to calculate the hash</param>
        public static string Sha256Hash(this string value)
        {
            var sha256 = SHA256.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha256.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calculates the SHA-1 Hash for a string
        /// </summary>
        /// <param name="value">String to calculate the hash</param>
        public static string Sha1Hash(this string value)
        {
            var sha1 = SHA1.Create();// SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calculate the MD5 Hash for a given file
        /// </summary>
        /// <param name="fileName">Name of the file to calculate the hash</param>
        public static string GetMd5HashFromFile(string fileName)
        {
            var file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            var retVal = md5.ComputeHash(file);
            file.Close();

            var sb = new StringBuilder();
            foreach (var t in retVal)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
