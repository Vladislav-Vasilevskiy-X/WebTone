
using System;
using System.Reflection;

using Core.HtmlElements.PageFactories;
using Core.HtmlElements.PageFactories.Selenium;

using OpenQA.Selenium;

namespace Core.HtmlElements.Loaders.Decorators
{
	/// <summary>
	/// HtmlElementLocatorFactory class.
	/// </summary>
	public class HtmlElementLocatorFactory : CustomElementLocatorFactory
	{
		private readonly ISearchContext searchContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlElementLocatorFactory" /> class.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		public HtmlElementLocatorFactory(ISearchContext searchContext)
		{
			this.searchContext = searchContext;
		}

		/// <summary>
		/// Creates the locator.
		/// </summary>
		/// <param name="propertyInfo">The property info.</param>
		/// <returns>The value.</returns>
		public override IElementLocator CreateLocator(PropertyInfo propertyInfo)
		{
			return new AjaxElementLocator(this.searchContext, new HtmlElementPropertyAttributesHandler(propertyInfo));
		}

		/// <summary>
		/// Creates the locator.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The value.</returns>
		public override IElementLocator CreateLocator(Type type)
		{
			return new AjaxElementLocator(this.searchContext, new HtmlElementTypeAttributesHandler(type));
		}
	}
}