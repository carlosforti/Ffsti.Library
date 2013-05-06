using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ffsti.Library
{
    /// <summary>
    /// Methods to calculate hashes
    /// </summary>
    public class Hashes
    {
        /// <summary>
        /// Calculates the MD5 Hash for a given string
        /// </summary>
        /// <param name="value">String to calculate the hash</param>
        public static string MD5Hash(string value)
        {
            var md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(value);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calculates the SHA256 Hash for a given string
        /// </summary>
        /// <param name="value">String to calculate the hash</param>
        public static string SHA256Hash(string value)
        {
            var sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(value);
            byte[] hash = sha256.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calculate the MD5 Hash for a given file
        /// </summary>
        /// <param name="fileName">Name of the file to calculate the hash</param>
        public static string GetMD5HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
