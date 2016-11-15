using System;
using System.Linq.Expressions;
using Core.SeleniumUtils;

namespace Core.GeneralUtils
{
	/// <summary>
	///     Attribute "StringValue".
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public sealed class StringValueAttribute : Attribute
	{
		#region - Constructors -

		/// <summary>
		///     Initializes a new instance of the <see cref="StringValueAttribute" /> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public StringValueAttribute(string value)
		{
			Value = value;
		}

		#endregion

		#region - Properties -

		/// <summary>
		///     Gets the value.
		/// </summary>
		/// <value>
		///     The value.
		/// </value>
		public string Value { get; private set; }

		#endregion

		#region - Methods -

		/// <summary>
		///     Gets the attribute value for a given property.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="propertyLambda">The property lambda.</param>
		/// <returns>The attribute value.</returns>
		public static string Get<T>(Expression<Func<T>> propertyLambda)
		{
			return (string) AttributeUtils.Get(propertyLambda, typeof(StringValueAttribute), "Value");
		}

		#endregion
	}
}