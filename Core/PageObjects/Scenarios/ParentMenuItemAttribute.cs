using System;

namespace Core.PageObjects.Scenarios
{
	/// <summary>
	///     Parent menu item Attribute for Side navigation items.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ParentMenuItemAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="ParentMenuItemAttribute" /> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public ParentMenuItemAttribute(string value)
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