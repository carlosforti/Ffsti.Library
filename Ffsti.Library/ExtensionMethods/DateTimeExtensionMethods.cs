using System;
using System.Globalization;

namespace Ffsti
{
	/// <summary>
	/// Extension Methods for the DateTime type
	/// </summary>
	public static class DateTimeExtensionMethods
	{
		/// <summary>
		/// Returns the month name for a DateTime
		/// </summary>
		public static string GetMonthName(this DateTime dateTime)
		{
			return (new DateTimeFormatInfo()).MonthNames[dateTime.Month - 1];
		}

		/// <summary>
		/// Returns the first day of the month
		/// </summary>
		public static DateTime GetFirstDateOfMonth(this DateTime dateTime)
		{
			return dateTime.AddDays((dateTime.Day * -1) + 1);
		}

		/// <summary>
		/// Returns the last day of the month
		/// </summary>
		public static DateTime GetLastDateOfMonth(this DateTime dateTime)
		{
			return dateTime.AddMonths(1).GetFirstDateOfMonth().AddDays(-1);
		}

		/// <summary>
		/// Returns the first business date of the week from DateTime
		/// </summary>
		public static DateTime GetFirstBusinessDateOfWeek(this DateTime dateTime)
		{
			return dateTime.AddDays(((int)dateTime.DayOfWeek * -1) + 1);
		}

		/// <summary>
		/// Returns the last business date of the week from DateTime
		/// <param name="lastBusinessDayOfWeek">Indicate what is the last business day of the week</param>
		/// </summary>
		public static DateTime GetLastBusinessDateOfWeek(this DateTime dateTime, DayOfWeek lastBusinessDayOfWeek = DayOfWeek.Saturday)
		{
			return dateTime.AddDays(lastBusinessDayOfWeek - dateTime.DayOfWeek - 1);
		}
	}
}