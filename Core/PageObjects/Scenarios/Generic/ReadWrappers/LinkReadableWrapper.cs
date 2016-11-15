using System;
using System.Net.Mime;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     The link readable wrapper.
	/// </summary>
	public class LinkReadableWrapper : BaseReadableElementWrapper<Link>
	{
		/// <summary>
		///     Gets <see cref="MediaTypeNames.Text" />
		/// </summary>
		/// <value>The get value from element.</value>
		protected override Func<Link, object> GetValueFromElement
		{
			get { return linkElement => linkElement.Text; }
		}
	}
}