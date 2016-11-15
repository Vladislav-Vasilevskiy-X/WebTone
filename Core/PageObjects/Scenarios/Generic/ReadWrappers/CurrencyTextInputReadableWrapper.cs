using System;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     CurrencyTextInputReadableWrapper class.
	/// </summary>
	public class CurrencyTextInputReadableWrapper : BaseReadableElementWrapper<CurrencyTextInput>
	{
		/// <summary>
		///     Gets Action to retrieve the value using the members of <see cref="TElement" />
		/// </summary>
		/// <value>The get value from element.</value>
		protected override Func<CurrencyTextInput, object> GetValueFromElement
		{
			get { return currencyTextInputElement => currencyTextInputElement.Text; }
		}
	}
}