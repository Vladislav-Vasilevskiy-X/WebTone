
using System.Collections.ObjectModel;

using Core.HtmlElements.PageFactories.Selenium;

using OpenQA.Selenium;

namespace Core.HtmlElements.PageFactories
{
	/// <summary>
	/// Base class to provide attributes handling for selenium locators.
	/// Added <see cref="DefaultElementLocator(ISearchContext, By)"/> to create locator using <see cref="By"/>
	/// </summary>
	public class DefaultElementLocator : IElementLocator
	{
		private readonly ISearchContext searchContext;

		private readonly bool shouldCache;

		private readonly By by;

		private IWebElement cachedElement;

		private ReadOnlyCollection<IWebElement> cachedElementList;

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultElementLocator"/> class.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		/// <param name="attributesHandler">The attributes handler.</param>
		public DefaultElementLocator(ISearchContext searchContext, AttributesHandler attributesHandler)
		{
			this.searchContext = searchContext;
			this.shouldCache = attributesHandler.ShouldCache();
			this.by = attributesHandler.BuildBy();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultElementLocator"/> class.
		/// </summary>
		/// <param name="searchContext">The search context.</param>
		/// <param name="by">The by.</param>
		public DefaultElementLocator(ISearchContext searchContext, By by)
		{
			this.searchContext = searchContext;
			this.shouldCache = false;
			this.by = by;
		}

		/// <summary>
		/// Finds the element.
		/// </summary>
		/// <returns><see cref="IWebElement"/> instance.</returns>
		public virtual IWebElement FindElement()
		{
			if (this.cachedElement != null && this.shouldCache)
			{
				return this.cachedElement;
			}

			var element = this.searchContext.FindElement(this.by);
			if (this.shouldCache)
			{
				this.cachedElement = element;
			}

			return element;
		}

		/// <summary>
		/// Finds the elements.
		/// </summary>
		/// <returns>Collecteion of <see cref="IWebElement"/>s.</returns>
		public virtual ReadOnlyCollection<IWebElement> FindElements()
		{
			if (this.cachedElementList != null && this.shouldCache)
			{
				return this.cachedElementList;
			}

			var elements = this.searchContext.FindElements(this.by);
			if (this.shouldCache)
			{
				this.cachedElementList = elements;
			}

			return elements;
		}
	}
}