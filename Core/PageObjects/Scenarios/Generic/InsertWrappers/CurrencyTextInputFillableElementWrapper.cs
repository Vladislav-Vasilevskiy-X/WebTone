using System;
using System.Globalization;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.InsertWrappers
{
	/// <summary>
	///     CurrencyTextInputFillableElementWrapper class.
	/// </summary>
	public class CurrencyTextInputFillableElementWrapper : BaseFillableElementWrapper<CurrencyTextInput>
	{
		/// <summary>
		///     Send model value to the input.
		/// </summary>
		/// <value>The Action.</value>
		protected override Action<CurrencyTextInput, object> EnterDataAction
		{
			get
			{
				return (textInputElement, modelValue) =>
				{
					var stringModelValue = Convert.ToString(modelValue, CultureInfo.InvariantCulture);
					if (textInputElement != null)
					{
						textInputElement.ClearAndSendKeys(stringModelValue);
					}
				};
			}
		}
	}
}