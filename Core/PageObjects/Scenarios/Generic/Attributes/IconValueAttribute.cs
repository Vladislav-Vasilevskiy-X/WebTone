using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     The attributes should be applied to enum fields to allow them to be mapped from Icon element.
	///     The value should correspond class attribute of Icon element.
	///     For example, .
	///     <span class="icon_warning" />.
	///     can be mapped to.
	///     enum TrustAccountMinBalanceIcon.
	///     {
	///     None,.
	///     [IconValue("icon_warning")]
	///     Warning
	///     }.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class IconValueAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="IconValueAttribute" /> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public IconValueAttribute(string value)
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