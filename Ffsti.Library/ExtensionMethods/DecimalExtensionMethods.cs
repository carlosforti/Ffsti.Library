namespace Ffsti
{
	/// <summary>
	/// Extension Methods for use with decimals
	/// </summary>
	public static class DecimalExtensionMethods
	{
		/// <summary>
		/// <para>Convert a number to a percentage value</para>
		/// <para>i.e. 10 to 0.1</para>
		/// </summary>
		/// <param name="number">Number to be converted</param>
		/// <returns>Percentage form of number</returns>
		public static decimal ToPercentage(this double number)
		{
			return (decimal)(number / 100.0);
		}
	} 
}