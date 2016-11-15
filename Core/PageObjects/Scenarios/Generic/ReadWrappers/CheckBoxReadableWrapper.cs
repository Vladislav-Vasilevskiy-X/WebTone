using System;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     CheckBoxReadableWrapper class.
	/// </summary>
	public class CheckBoxReadableWrapper : BaseReadableElementWrapper<CheckBox>
	{
		/// <summary>
		///     Gets <see cref="CheckBox.Selected" />
		/// </summary>
		/// <value>The get value from element.</value>
		protected override Func<CheckBox, object> GetValueFromElement
		{
			get { return checkBoxElement => checkBoxElement.Selected; }
		}
	}
}