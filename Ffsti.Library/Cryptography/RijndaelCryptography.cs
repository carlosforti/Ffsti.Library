using System.Security.Cryptography;

namespace Ffsti.Library.Cryptography
{
	/// <summary>
	/// 
	/// </summary>
	public class RijndaelCryptography : BaseCryptography<RijndaelManaged>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="secretKey"></param>
		public RijndaelCryptography(string secretKey)
			: base(secretKey) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="secretKey"></param>
		public RijndaelCryptography(byte[] secretKey)
			: base(secretKey) { }

		/// <summary>
		/// 
		/// </summary>
		public RijndaelCryptography()
			: base() { }
	}
}
