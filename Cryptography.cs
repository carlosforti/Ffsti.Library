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
    }
}
