using System;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     TextInputReadableWrapper class.
	/// </summary>
	public class TextInputReadableWrapper : BaseReadableElementWrapper<TextInput>
	{
		/// <summary>
		///     Gets Action to retrieve the value using the members of <see cref="TElement" />
		/// </summary>
		/// <value>The get value from element.</value>
		protected override Func<TextInput, object> GetValueFromElement
		{
			get { return textInputElement => textInputElement.Text; }
		}
	}
}