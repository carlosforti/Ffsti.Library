using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti
{
	public static class DecimalExtensionMethods
	{
		public static decimal ToPercentage(this double number)
		{
			return (decimal)(number / 100.0);
		}
	}
}