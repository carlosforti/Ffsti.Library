using System;
using System.Security.Cryptography;
using System.Text;

namespace Ffsti
{
    public static class Cryptography
    {
        private static byte[] key = Encoding.ASCII.GetBytes(@"Put Key Here");
        private static byte[] iv = Encoding.ASCII.GetBytes("@Put IV Here");
        private static RijndaelManaged rij = new RijndaelManaged();

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
    }
}
