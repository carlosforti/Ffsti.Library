using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace Ffsti
//{
	/// <summary>
	/// Classe de métodos de extensão para inteiros
	/// </summary>
	public static class IntegerExtensionMethods
	{
		/// <summary>
		/// Calcula o fatorial de um dado numero
		/// </summary>
		/// <returns></returns>
		public static long Fatorial(this int value)
		{
			if (value == 1)
				return value;
			else
				return value * Fatorial(value - 1);
		}
	}
//}