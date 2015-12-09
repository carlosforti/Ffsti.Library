using System.Security.Cryptography;
using Ffsti.Library.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ffsti.Library.Tests
{
    [TestClass]
    public class CryptoTest
    {
        [TestMethod]
        public void TestAesCrypto()
        {
            var crypto = new AesCryptography();

            const string originalString = "AES cryptography";
            var cryptoString = crypto.Encrypt(originalString);
            var decryptString = crypto.Decrypt(cryptoString);

            Assert.AreEqual(originalString, decryptString);
        }

        [TestMethod]
        public void TestRijndaelCrypto()
        {
            var crypto = new RijndaelCryptography();

            const string originalString = "Rijndael cryptography";
            var cryptoString = crypto.Encrypt(originalString);
            var decryptString = crypto.Decrypt(cryptoString);

            Assert.AreEqual(originalString, decryptString);
        }

        [TestMethod]
        public void TestBaseCrypto()
        {
            var crypto = new BaseCryptography<AesManaged>();

            const string originalString = "Rijndael cryptography";
            var cryptoString = crypto.Encrypt(originalString);
            var decryptString = crypto.Decrypt(cryptoString);

            Assert.AreEqual(originalString, decryptString);
        }
    }
}
