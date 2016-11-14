
using System;
using System.Linq;

using FTB.Utilities.CustomAttributes;

namespace FTB.Utilities.Extensions
{
	/// <summary>
	/// StringValueExtension class.
	/// </summary>
	public static class StringValueExtension
	{
		/// <summary>
		/// From a Enum value gives the String Value attribute.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result Value.</returns>
		public static string ToStringValue(this Enum value)
		{
			var attributes = (StringValueAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false);
			return ((attributes != null) && (attributes.Length > 0)) ? attributes[0].Value : value.ToString();
		}

		/// <summary>
		/// From a Enum value gives the String Values array.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The String Values array.</returns>
		public static string[] ToStringValues(this Enum value)
		{
			var attributes = (StringValueAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false);
			return ((attributes != null) && (attributes.Length > 0)) ? attributes.Select(x => x.Value).ToArray() : default(string[]);
		}

		/// <summary>
		/// Given a string search the enum value associated.
		/// </summary>
		/// <typeparam name="T">The Type.</typeparam>
		/// <param name="value">The string.</param>
		/// <returns>The object of T type.</returns>
		public static T ToEnum<T>(this string value)
		{
			foreach (T item in Enum.GetValues(typeof(T)))
			{
				var attributes = (StringValueAttribute[])item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false);
				if ((attributes != null) && (attributes.Length > 0) && (attributes[0].Value.Equals(value)))
				{
					return item;
				}
			}

			return (T)Enum.Parse(typeof(T), value, true);
		}
	}
}