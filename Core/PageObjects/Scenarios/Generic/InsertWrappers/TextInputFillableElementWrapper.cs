using System;
using System.Globalization;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.InsertWrappers
{
	/// <summary>
	///     The TextInputFillableElementWrapper.
	/// </summary>
	public class TextInputFillableElementWrapper : BaseFillableElementWrapper<TextInput>
	{
		/// <summary>
		///     Send model value to the input.
		/// </summary>
		/// <value>The action.</value>
		protected override Action<TextInput, object> EnterDataAction
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