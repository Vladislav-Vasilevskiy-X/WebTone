using System;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     SelectReadableWrapper class.
	/// </summary>
	public class SelectReadableWrapper : BaseReadableElementWrapper<Select>
	{
		/// <summary>
		///     Gets <see cref="Select.SelectedOption" />
		/// </summary>
		/// <value>The get value from element.</value>
		protected override Func<Select, object> GetValueFromElement
		{
			get { return selectElement => selectElement.SelectedOption().Text; }
		}
	}
}