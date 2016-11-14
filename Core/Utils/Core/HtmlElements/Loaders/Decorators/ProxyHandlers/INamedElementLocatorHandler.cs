
using Core.HtmlElements.PageFactories.Selenium;

namespace Core.HtmlElements.Loaders.Decorators.ProxyHandlers
{
	/// <summary>
	/// Interface to handle <see cref="IElementLocator"/> object and it's name.
	/// </summary>
	public interface INamedElementLocatorHandler
	{
		/// <summary>
		/// Gets the locator.
		/// </summary>
		/// <value>The locator.</value>
		IElementLocator Locator { get; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }
	}
}