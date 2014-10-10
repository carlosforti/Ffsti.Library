using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Extension Methods for Objects
/// </summary>
public static class ObjectExtensionMethods
{
	/// <summary>
	/// Workaround similar ao With do VB.net
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="item"></param>
	/// <param name="work"></param>
	public static void Use<T>(this T item, Action<T> work)
	{
		work(item);
	}
}
