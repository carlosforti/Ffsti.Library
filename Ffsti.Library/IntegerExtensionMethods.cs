using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ffsti
{
	/// <summary>
	/// Extension methods for integers
	/// </summary>
	public static class IntegerExtensionMethods
	{
		/// <summary>
		/// Returns the fatorial of a integer
		/// </summary>
        /// <param name="value">The value to use as fatorial base</param>
		/// <returns>Number fatorial</returns>
		public static long Fatorial(this int value)
		{
			if (value == 1)
				return value;
			else
				return value * Fatorial(value - 1);
		}
    }
}