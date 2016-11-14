
using System;
using System.Globalization;

using Container;

using Core.HtmlElements.Loaders.Decorators.ProxyHandlers;
using Core.HtmlElements.PageFactories;
using Core.Objects;

using ImpromptuInterface;

using Microsoft.CSharp.RuntimeBinder;

using OpenQA.Selenium;
using OpenQA.Selenium.Internal;

namespace Core.HtmlElements.Elements
{
	/// <summary>
	/// Base class for elements.
	/// </summary>
	public abstract class TypifiedElement : UIInfrastructureObject, IWrapsElement, INamed
	{
		private readonly IWebElement wrappedElement;

		/// <summary>
		/// Gets original <see cref="IWebElement" /> from the proxy object.
		/// </summary>
		/// <value>The original web element.</value>
		public IWebElement OriginalWebElement
		{
			get
			{
				var webElementNamedProxyHandler = this.wrappedElement.UndoActLike() as INamedElementLocatorHandler;
				return webElementNamedProxyHandler != null ? webElementNamedProxyHandler.Locator.FindElement() : null;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypifiedElement" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		protected TypifiedElement(IWebElement element)
		{
			this.wrappedElement = element;
		}

		/// <summary>
		/// Gets the <see cref="T:OpenQA.Selenium.IWebElement" /> wrapped by this object.
		/// </summary>
		/// <value>The value.</value>
		public IWebElement WrappedElement
		{
			get
			{
				return this.wrappedElement;
			}
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="TypifiedElement"/> is displayed.
		/// </summary>
		/// <value>
		///   <c>true</c> if displayed; otherwise, <c>false</c>.
		/// </value>
		public bool Displayed
		{
			get
			{
				try
				{
					return this.wrappedElement.Displayed;
				}
				catch (WebDriverTimeoutException)
				{
					return false;
				}
				catch (RuntimeBinderException)
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Check if this <see cref="TypifiedElement"/> Displayed the with timeout.
		/// </summary>
		/// <param name="time">The time.</param>
		/// <returns>The Flag.</returns>
		public bool DisplayedWithTimeout(TimeSpan time)
		{
			var webElementNamedProxyHandler = this.wrappedElement.UndoActLike() as INamedElementLocatorHandler;
			var ajaxElementLocator = (AjaxElementLocator)webElementNamedProxyHandler.Locator;
			ajaxElementLocator.TimeoutInSeconds = time.Seconds;
			ajaxElementLocator.ErrorHandlingEnabled = false;
			var isDisplayed = this.Displayed;
			ajaxElementLocator.RestoreDefaults();
			return isDisplayed;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="TypifiedElement"/> is enabled.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled
		{
			get
			{
				Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Check if element {0} enabled", this.Name));
				Waiter.SpinWait(() => this.wrappedElement.Enabled, TimeSpan.FromSeconds(10));
				return this.wrappedElement.Enabled;
			}
		}

		/// <summary>
		/// Gets a value indicating whether is link disabled.
		/// Checks if link contains class &gt; disabled.
		/// </summary>
		/// <value>The enabled by class.</value>
		public bool EnabledByClass
		{
			get
			{
				return !this.WrappedElement.GetAttribute("class").Contains("disabled");
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="TypifiedElement"/> is selected.
		/// </summary>
		/// <value>
		///   <c>true</c> if selected; otherwise, <c>false</c>.
		/// </value>
		public virtual bool Selected
		{
			get
			{
				return this.wrappedElement.Selected;
			}
		}

		/// <summary>
		/// Gets the attribute.
		/// </summary>
		/// <param name="attributeName">Name of the attribute.</param>
		/// <returns>Attribute value.</returns>
		public string GetAttribute(string attributeName)
		{
			return this.wrappedElement.GetAttribute(attributeName);
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value
		{
			get
			{
				return this.GetAttribute("value");
			}
		}

		/// <summary>
		/// Gets the value of a CSS property of this element.
		/// </summary>
		/// <param name="propertyName">The name of the CSS property to get the value of.</param>
		/// <returns>
		/// The value of the specified CSS property.
		/// </returns>
		/// <remarks>
		/// The value returned by the <see cref="M:OpenQA.Selenium.IWebElement.GetCssValue(System.String)" />
		/// method is likely to be unpredictable in a cross-browser environment.
		/// Color values should be returned as hex strings. For example, a.
		/// "background-color" property set as "green" in the HTML source, will.
		/// return "#008000" for its value.
		/// </remarks>
		public string GetCssValue(string propertyName)
		{
			return this.wrappedElement.GetCssValue(propertyName);
		}
	}
}