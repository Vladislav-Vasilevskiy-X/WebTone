using System;
using System.Globalization;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.InsertWrappers
{
	/// <summary>
	///     TristateCheckBoxFillableElementWrapper class.
	/// </summary>
	public class TristateCheckBoxFillableElementWrapper : BaseFillableElementWrapper<TristateCheckBox>
	{
		/// <summary>
		///     Select/deselect or leave as partially the tristate checkbox.
		/// </summary>
		/// <value>The action.</value>
		protected override Action<TristateCheckBox, object> EnterDataAction
		{
			get
			{
				return (tristateCheckBox, modelValue) =>
				{
					var stringModelValue = modelValue.ToString();
					if (tristateCheckBox != null)
					{
						if (stringModelValue.Equals("Unlocked") || string.IsNullOrEmpty(stringModelValue))
						{
							tristateCheckBox.Deselect();
						}
						else if (stringModelValue.Equals("Locked"))
						{
							tristateCheckBox.Select();
						}
						else if (stringModelValue.Equals("Partially") || stringModelValue.Equals("None"))
						{
							// do nothing as such statuses are readonly
						}
						else
						{
							tristateCheckBox.Set(Convert.ToBoolean(modelValue, CultureInfo.InvariantCulture));
						}
					}
				};
			}
		}
	}
}