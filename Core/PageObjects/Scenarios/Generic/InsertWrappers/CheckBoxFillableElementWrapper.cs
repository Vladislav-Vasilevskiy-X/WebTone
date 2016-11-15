using System;
using System.Globalization;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.InsertWrappers
{
	/// <summary>
	///     CheckBoxFillableElementWrapper class.
	/// </summary>
	public class CheckBoxFillableElementWrapper : BaseFillableElementWrapper<CheckBox>
	{
		/// <summary>
		///     Selects or deselects checkbox depands on model value.
		/// </summary>
		/// <value>The Action.</value>
		protected override Action<CheckBox, object> EnterDataAction
		{
			get
			{
				return (checkboxElement, modelValue) =>
				{
					var boolModelValue = Convert.ToBoolean(modelValue, CultureInfo.InvariantCulture);
					if (checkboxElement != null)
					{
						checkboxElement.Set(boolModelValue);
					}
				};
			}
		}
	}
}