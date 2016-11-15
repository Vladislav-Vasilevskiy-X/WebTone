using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     ModelPropertyNameAttribute class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ModelPropertyNameAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="ModelPropertyNameAttribute" /> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public ModelPropertyNameAttribute(string value)
		{
			Value = value;
		}

		/// <summary>
		///     Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; private set; }
	}
}