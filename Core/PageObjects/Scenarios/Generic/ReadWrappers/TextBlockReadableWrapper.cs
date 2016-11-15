using System;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     TextBlockReadableWrapper class.
	/// </summary>
	public class TextBlockReadableWrapper : BaseReadableElementWrapper<TextBlock>
	{
		/// <summary>
		///     Gets <see cref="TextBlock.Text" />
		/// </summary>
		/// <value>The get value from element.</value>
		protected override Func<TextBlock, object> GetValueFromElement
		{
			get { return textBlockElement => textBlockElement.Text; }
		}
	}
}