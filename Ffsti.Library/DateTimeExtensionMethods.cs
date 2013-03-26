using System;
using System.Globalization;

namespace Ffsti
{
	public static class DateTimeExtensionMethods
	{
		public static string GetMonthName(this DateTime dateTime)
		{
			DateTimeFormatInfo info = new DateTimeFormatInfo();

			return info.MonthNames[dateTime.Month - 1];
		}

		public static DateTime GetFirstDateOfMonth(this DateTime dateTime)
		{
			return dateTime.AddDays((dateTime.Day * -1) + 1);
		}

		public static DateTime GetLastDateOfMonth(this DateTime dateTime)
		{
			return dateTime.AddMonths(1).GetFirstDateOfMonth().AddDays(-1);
		}

		public static DateTime GetFirstBusinessDateOfWeek(this DateTime dateTime)
		{
			return dateTime.AddDays(((int)dateTime.DayOfWeek * -1) + 1);
		}

		public static DateTime GetLastBusinessDateOfWeek(this DateTime dateTime)
		{
			return dateTime.AddDays(DayOfWeek.Saturday - dateTime.DayOfWeek - 1);
		}
	}
}
