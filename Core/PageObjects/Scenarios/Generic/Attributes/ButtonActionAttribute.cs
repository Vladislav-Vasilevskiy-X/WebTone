using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     ButtonActionAttribute class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ButtonActionAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="ButtonActionAttribute" /> class.
		/// </summary>
		/// <param name="buttonFlag">The button flag.</param>
		public ButtonActionAttribute(ButtonFlag buttonFlag)
		{
			ButtonFlag = buttonFlag;
		}

		/// <summary>
		///     Gets the button flag.
		/// </summary>
		/// <value>The button flag.</value>
		public ButtonFlag ButtonFlag { get; private set; }
	}
}