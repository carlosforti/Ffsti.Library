using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
