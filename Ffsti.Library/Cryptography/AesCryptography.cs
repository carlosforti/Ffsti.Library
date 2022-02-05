using System.Security.Cryptography;

namespace Ffsti.Library.Cryptography
{
	/// <summary>
	/// 
	/// </summary>
    public class AesCryptography : BaseCryptography<AesManaged>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="secretKey"></param>
		public AesCryptography(string secretKey)
			: base(secretKey) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="secretKey"></param>
		public AesCryptography(byte[] secretKey)
			: base(secretKey) { }

		/// <summary>
		/// 
		/// </summary>
		public AesCryptography()
			: base() { }
	}
}
