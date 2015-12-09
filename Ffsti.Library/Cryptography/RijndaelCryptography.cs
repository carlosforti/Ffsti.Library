using System.Security.Cryptography;

namespace Ffsti.Library.Cryptography
{
	public class RijndaelCryptography : BaseCryptography<RijndaelManaged>
	{
		public RijndaelCryptography(string secretKey)
			: base(secretKey) { }

		public RijndaelCryptography(byte[] secretKey)
			: base(secretKey) { }

		public RijndaelCryptography()
			: base() { }
	}
}
