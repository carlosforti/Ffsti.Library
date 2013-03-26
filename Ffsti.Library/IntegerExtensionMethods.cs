using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti
{
	public static class IntegerExtensionMethods
	{
		public static long Fatorial(this int value)
		{
			if (value == 1)
				return value;
			else
				return value * Fatorial(value - 1);
		}
	}
}
