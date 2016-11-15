using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.SeleniumUtils.Core.HtmlElements.Loaders.Decorators.ProxyHandlers;
using Core.SeleniumUtils.Core.HtmlElements.PageFactories;
using ImpromptuInterface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Core.SeleniumUtils.Core
{
	/// <summary>
	///     Ajax Timeout Extensions.
	/// </summary>
	public static class AjaxTimeoutExtensions
	{
		/// <summary>
		///     Changes the timeout for <see cref="IWebElement" /> proxy.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="time">The time.</param>
		/// <returns>The IWebElement.</returns>
		public static IWebElement ChangeTimeout(this IWebElement element, TimeSpan time)
		{
			ChangeTimeoutForProxy(element, time);
			return element;
		}

		/// <summary>
		///     Restores default timeout for List of <see cref="IWebElement" />.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The IWebElement.</returns>
		public static IWebElement RestoreDefaultTimeout(this IWebElement element)
		{
			RestoreDefaultTimeoutForProxy(element);
			return element;
		}

		/// <summary>
		///     Changes the timeout for <see cref="IWebElement" /> proxy.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <param name="time">The time.</param>
		/// <returns>The list of IWebElements.</returns>
		public static IList<IWebElement> ChangeTimeout(this ReadOnlyCollection<IWebElement> elements, TimeSpan time)
		{
			ChangeTimeoutForProxy(elements, time);
			return elements;
		}

		/// <summary>
		///     Restores default timeout for List of <see cref="IWebElement" />.
		/// </summary>
		/// <param name="elements">The elements.</param>
		/// <returns>The list of IWebElements.</returns>
		public static IList<IWebElement> RestoreDefaultTimeout(this ReadOnlyCollection<IWebElement> elements)
		{
			RestoreDefaultTimeoutForProxy(elements);
			return elements;
		}

		/// <summary>
		///     Changes the timeout <see cref="HtmlElement" /> or <see cref="TypifiedElement" />.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="element">The element.</param>
		/// <param name="time">The time.</param>
		/// <returns>The object of T type.</returns>
		public static T ChangeTimeout<T>(this T element, TimeSpan time) where T : IWrapsElement
		{
			ChangeTimeoutForProxy(element.WrappedElement, time);
			return element;
		}

		/// <summary>
		///     Restores default timeout for <see cref="HtmlElement" /> or <see cref="TypifiedElement" />.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="element">The element.</param>
		/// <param name="time">The time.</param>
		/// <returns>The object of T type.</returns>
		public static T RestoreDefaultTimeout<T>(this T element, TimeSpan time) where T : IWrapsElement
		{
			ChangeTimeoutForProxy(element.WrappedElement, time);
			return element;
		}

		/// <summary>
		///     Changes the timeout for List of children of <see cref="HtmlElement" /> and <see cref="TypifiedElement" />.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="elements">The element.</param>
		/// <param name="time">The time.</param>
		/// <returns>The list of objects of T type.</returns>
		public static IList<T> ChangeTimeout<T>(this IList<T> elements, TimeSpan time) where T : IWrapsElement
		{
			ChangeTimeoutForProxy(elements, time);
			return elements;
		}

		/// <summary>
		///     Restores default timeout for List of children of <see cref="HtmlElement" /> and <see cref="TypifiedElement" />.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="elements">The elements.</param>
		/// <returns>The list of objects of T type.</returns>
		public static IList<T> RestoreDefaultTimeout<T>(this IList<T> elements) where T : IWrapsElement
		{
			RestoreDefaultTimeoutForProxy(elements);
			return elements;
		}

		/// <summary>
		///     Changes the timeout for proxied object.
		/// </summary>
		/// <param name="proxy">The proxy.</param>
		/// <param name="time">The time.</param>
		private static void ChangeTimeoutForProxy(object proxy, TimeSpan time)
		{
			var proxyHandler = proxy.UndoActLike() as INamedElementLocatorHandler;
			Assert.IsNotNull(proxyHandler, "proxyHandler is null");
			var ajaxElementLocator = (AjaxElementLocator) proxyHandler.Locator;
			ajaxElementLocator.TimeoutInSeconds = time.Seconds;
		}

		/// <summary>
		///     Restores default timeout for proxied object.
		/// </summary>
		/// <param name="proxy">The proxy.</param>
		private static void RestoreDefaultTimeoutForProxy(object proxy)
		{
			var proxyHandler = proxy.UndoActLike() as INamedElementLocatorHandler;
			Assert.IsNotNull(proxyHandler, "proxyHandler is null");
			var ajaxElementLocator = (AjaxElementLocator) proxyHandler.Locator;
			ajaxElementLocator.RestoreDefaults();
		}
	}
}