﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Ffsti
{
	/// <summary>
	/// Extensions for Enums
	/// </summary>
	public static class EnumEx
	{
		/// <summary>
		/// Get the Description metadata from a enum value
		/// </summary>
		/// <param name="value">The enumerator value</param>
		/// <returns>Text in Description Metadata</returns>
		public static string GetDescriptionFromEnumValue(Enum value)
		{
			DescriptionAttribute attribute = value.GetType()
				.GetField(value.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false)
				.SingleOrDefault() as DescriptionAttribute;
			return attribute == null ? value.ToString() : attribute.Description;
		}

		/// <summary>
		/// Get the value in an enumerator using the Description Metadata
		/// </summary>
		/// <typeparam name="T">The enum type</typeparam>
		/// <param name="description">Description</param>
		/// <returns>The enumerator value</returns>
		public static T GetEnumValueFromDescription<T>(string description)
		{
			var type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException($"Description is not of type {type.Name}");

			FieldInfo[] fields = type.GetFields();
			var field = fields
							.SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute),false), 
								(f, a) => new { Field = f, Att = a })
							.SingleOrDefault(a => ((DescriptionAttribute)a.Att)
								.Description == description);
			return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
		}
	}
}