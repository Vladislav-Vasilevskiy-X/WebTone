using System;
using System.Globalization;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.InsertWrappers
{
	/// <summary>
	///     RadioFillableElementWrapper class.
	/// </summary>
	public class RadioFillableElementWrapper : BaseFillableElementWrapper<Radio>
	{
		/// <summary>
		///     Enter the value from <see cref="object" /> using the methods of <see cref="TElement" />
		/// </summary>
		/// <value>The Action.</value>
		protected override Action<Radio, object> EnterDataAction
		{
			get
			{
				return (radioButton, modelValue) =>
				{
					var stringModelValue = Convert.ToString(modelValue, CultureInfo.InvariantCulture);
					radioButton.SelectByValue(stringModelValue);
				};
			}
		}
	}
}