using System;

namespace Core.PageObjects.Scenarios
{
	/// <summary>
	///     Link Text Attribute for Side navigation items.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class LinkTextAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="LinkTextAttribute" /> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public LinkTextAttribute(string value)
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