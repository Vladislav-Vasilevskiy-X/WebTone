using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.SeleniumUtils
{
	/// <summary>
	///     Util for working with attributes.
	/// </summary>
	public static class AttributeUtils
	{
		#region - Methods -

		/// <summary>
		///     From a lambda expresion of a property, return the property info.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="propertyLambda">The lambda expression of the property.</param>
		/// <exception cref="System.ArgumentException">Argument Exception.</exception>
		/// <returns>Property info of the property.</returns>
		public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyLambda)
		{
			var member = propertyLambda.Body as MemberExpression;
			if (member == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
					"Expression '{0}' doesn't refers to a property.", propertyLambda));
			}

			var propInfo = member.Member as PropertyInfo;
			if (propInfo == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
					"Expression '{0}' refers to a field, not a property.", propertyLambda));
			}

			return propInfo;
		}

		/// <summary>
		///     Gets the custom attributes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyLambda">The property lambda.</param>
		/// <param name="attributeType">Type of the attribute.</param>
		/// <returns>Object array.</returns>
		public static object[] GetCustomAttributes<T>(Expression<Func<T>> propertyLambda, Type attributeType)
		{
			var info = GetPropertyInfo(propertyLambda);
			return attributeType == null ? info.GetCustomAttributes(true) : info.GetCustomAttributes(attributeType, true);
		}

		/// <summary>
		///     Gets the custom attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyLambda">The property lambda.</param>
		/// <param name="attributeType">Type of the attribute.</param>
		/// <returns>The object.</returns>
		public static object GetCustomAttribute<T>(Expression<Func<T>> propertyLambda, Type attributeType)
		{
			var info = GetPropertyInfo(propertyLambda);
			return info.GetCustomAttribute(attributeType, true);
		}

		/// <summary>
		///     From the lambda expression of a property, return the value of a custom attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyLambda">The property lambda.</param>
		/// <param name="attributeType">Type of the custom attribute.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>The object.</returns>
		public static object Get<T>(Expression<Func<T>> propertyLambda, Type attributeType, string propertyName)
		{
			var attribute = GetCustomAttribute(propertyLambda, attributeType);
			var info = attributeType.GetProperty(propertyName);
			return info.GetValue(attribute);
		}

		#endregion
	}
}