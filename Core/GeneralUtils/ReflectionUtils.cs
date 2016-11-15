using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Core.GeneralUtils
{
	/// <summary>
	///     ReflectionUtils class.
	/// </summary>
	public static class ReflectionUtils
	{
		/// <summary>
		///     Gets the public properties with attribute.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="attributeType">Type of the attribute.</param>
		/// <returns>The Collection.</returns>
		public static IList<PropertyInfo> GetPublicPropertiesWithAttribute(this object value, Type attributeType)
		{
			return value.GetType().GetPublicPropertiesWithAttribute(attributeType);
		}

		/// <summary>
		///     Gets the public properties with attribute.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="attributeType">Type of the attribute.</param>
		/// <returns>The Collection.</returns>
		public static IList<PropertyInfo> GetPublicPropertiesWithAttribute(this Type objectType, Type attributeType)
		{
			return
				objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => Attribute.IsDefined(x, attributeType))
					.ToList();
		}

		/// <summary>
		///     Gets the property values with filtered attribute.
		/// </summary>
		/// <typeparam name="TItem">The type of the T item.</typeparam>
		/// <typeparam name="TAttribute">The type of the T attribute.</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="filter">The filter.</param>
		/// <returns>The Collection.</returns>
		public static IEnumerable<TItem> GetPropertyValuesWithFilteredAttribute<TItem, TAttribute>(this object value,
			Predicate<TAttribute> filter) where TItem : class where TAttribute : Attribute
		{
			var propertiesWithAttribute =
				value.GetType()
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(
						x =>
							x.PropertyType == typeof(TItem) && Attribute.IsDefined(x, typeof(TAttribute)) &&
							filter(x.GetCustomAttribute<TAttribute>()))
					.ToArray();
			return propertiesWithAttribute.Length < 1
				? Enumerable.Empty<TItem>()
				: propertiesWithAttribute.Select(x => x.GetValue(value) as TItem);
		}

		/// <summary>
		///     Gets the public property values of type <see cref="TItem" />.
		/// </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>The list of objects.</returns>
		public static IList<TItem> GetPublicPropertyValuesOfType<TItem>(this object value) where TItem : class
		{
			var propertiesWithAttribute =
				value.GetType()
					.GetProperties(BindingFlags.Public | BindingFlags.Instance)
					.Where(x => x.PropertyType == typeof(TItem))
					.ToList();
			return propertiesWithAttribute.Count < 1
				? Enumerable.Empty<TItem>().ToList()
				: propertiesWithAttribute.Select(x => x.GetValue(value) as TItem).ToList();
		}

		/// <summary>
		///     Prints the public properties to console.
		/// </summary>
		/// <param name="value">The value.</param>
		public static void PrintPublicPropertiesToConsole(this object value)
		{
			var props = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var propertyInfo in props)
			{
				var propertyName = propertyInfo.Name;
				var propertyValue = propertyInfo.GetValue(value, null);
				if (propertyValue == null ||
				    (propertyValue.GetType().IsGenericType && propertyValue.GetType().GetGenericArguments().Length == 1))
				{
					continue;
				}

				Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={1}", propertyName, propertyValue));
			}
		}
	}
}