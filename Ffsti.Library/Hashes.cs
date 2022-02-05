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
    }
}
