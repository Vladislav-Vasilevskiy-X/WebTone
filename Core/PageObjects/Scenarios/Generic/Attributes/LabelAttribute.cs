using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     The attribute should be placed over <see cref="TextBlock" /> properties which represent labels.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class LabelAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="LabelAttribute" /> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public LabelAttribute(string text)
		{
			Text = text;
		}

		/// <summary>
		///     Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; private set; }
	}
}