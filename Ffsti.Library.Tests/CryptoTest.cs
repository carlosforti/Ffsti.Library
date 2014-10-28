using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ffsti.Library.Tests
{
	[TestClass]
	public class CryptoTest
	{
		[TestMethod]
		public void TestAesCrypto()
		{
			Cryptography.AesCryptography crypto = new Cryptography.AesCryptography();

			var originalString = "AES cryptography";
			var cryptoString = crypto.Encrypt(originalString);

			var decryptString = crypto.Decrypt(cryptoString);

			Assert.AreEqual(originalString, decryptString);
		}

		[TestMethod]
		public void TestRijndaelCrypto()
		{
			Cryptography.RijndaelCryptography crypto = new Cryptography.RijndaelCryptography();

			var originalString = "Rijndael cryptography";
			var cryptoString = crypto.Encrypt(originalString);

			var decryptString = crypto.Decrypt(cryptoString);

			Assert.AreEqual(originalString, decryptString);
		}
	}
}
