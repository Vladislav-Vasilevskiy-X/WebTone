
using System;
using System.Collections.ObjectModel;

using Core.HtmlElements.Loaders.Decorators.ProxyHandlers;
using Core.HtmlElements.PageFactories;

using OpenQA.Selenium;

namespace Core
{
	/// <summary>
	/// Decorator for <see cref="IWebDriver"/> that contains overriden FindElement, FindElements methods that use <see cref="AjaxElementLocator"/>
	/// </summary>
	public class AjaxWebDriver : IWebDriver, IHasInputDevices, IJavaScriptExecutor, ITakesScreenshot
	{
		/// <summary>
		/// Gets the original driver that holds original FindElement, FindElements methods without <see cref="AjaxElementLocator"/> procesing.
		/// </summary>
		/// <value>
		/// The original driver.
		/// </value>
		public IWebDriver OriginalDriver { get; private set; }

		/// <summary>
		/// Gets an <see cref="T:OpenQA.Selenium.IKeyboard" /> object for sending keystrokes to the browser.
		/// </summary>
		/// <value>The IKeyboard.</value>
		public IKeyboard Keyboard
		{
			get
			{
				return ((IHasInputDevices)this.OriginalDriver).Keyboard;
			}
		}

		/// <summary>
		/// Gets an <see cref="T:OpenQA.Selenium.IMouse" /> object for sending mouse commands to the browser.
		/// </summary>
		/// <value>The IMouse.</value>
		public IMouse Mouse
		{
			get
			{
				return ((IHasInputDevices)this.OriginalDriver).Mouse;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AjaxWebDriver"/> class.
		/// </summary>
		/// <param name="originalDriver">The original driver.</param>
		public AjaxWebDriver(IWebDriver originalDriver)
		{
			this.OriginalDriver = originalDriver;
		}

		/// <summary>
		/// Finds the first <see cref="T:OpenQA.Selenium.IWebElement" /> using the given method.
		/// </summary>
		/// <param name="by">The locating mechanism to use.</param>
		/// <returns>
		/// The first matching <see cref="T:OpenQA.Selenium.IWebElement" /> on the current context.
		/// </returns>
		public IWebElement FindElement(By @by)
		{
			return WebElementNamedProxyHandler.NewInstance(new AjaxElementLocator(this.OriginalDriver, @by), string.Empty);
		}

		/// <summary>
		/// Finds all <see cref="T:OpenQA.Selenium.IWebElement">IWebElements</see> within the current context.
		/// using the given mechanism.
		/// </summary>
		/// <param name="by">The locating mechanism to use.</param>
		/// <returns>
		/// A <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of all <see cref="T:OpenQA.Selenium.IWebElement">WebElements</see>
		/// matching the current criteria, or an empty list if nothing matches.
		/// </returns>
		public ReadOnlyCollection<IWebElement> FindElements(By @by)
		{
			return new ReadOnlyCollection<IWebElement>(WebElementListNamedProxyHandler.NewInstance(new AjaxElementLocator(this.OriginalDriver, @by), string.Empty));
		}

		/// <summary>
		/// Close the current window, quitting the browser if it is the last window currently open.
		/// </summary>
		public void Close()
		{
			this.OriginalDriver.Close();
		}

		/// <summary>
		/// Quits this driver, closing every associated window.
		/// </summary>
		public void Quit()
		{
			this.OriginalDriver.Quit();
		}

		/// <summary>
		/// Instructs the driver to change its settings.
		/// </summary>
		/// <returns>
		/// An <see cref="T:OpenQA.Selenium.IOptions" /> object allowing the user to change.
		/// the settings of the driver.
		/// </returns>
		public IOptions Manage()
		{
			return this.OriginalDriver.Manage();
		}

		/// <summary>
		/// Instructs the driver to navigate the browser to another location.
		/// </summary>
		/// <returns>
		/// An <see cref="T:OpenQA.Selenium.INavigation" /> object allowing the user to access.
		/// the browser's history and to navigate to a given URL.
		/// </returns>
		public INavigation Navigate()
		{
			return this.OriginalDriver.Navigate();
		}

		/// <summary>
		/// Instructs the driver to send future commands to a different frame or window.
		/// </summary>
		/// <returns>
		/// An <see cref="T:OpenQA.Selenium.ITargetLocator" /> object which can be used to select.
		/// a frame or window.
		/// </returns>
		public ITargetLocator SwitchTo()
		{
			return this.OriginalDriver.SwitchTo();
		}

		/// <summary>
		/// Gets or sets the URL the browser is currently displaying.
		/// </summary>
		/// <remarks>
		/// Setting the <see cref="P:OpenQA.Selenium.IWebDriver.Url" /> property will load a new web page in the current browser window.
		/// This is done using an HTTP GET operation, and the method will block until the.
		/// load is complete. This will follow redirects issued either by the server or.
		/// as a meta-redirect from within the returned HTML. Should a meta-redirect "rest".
		/// for any duration of time, it is best to wait until this timeout is over, since.
		/// should the underlying page change while your test is executing the results of.
		/// future calls against this interface will be against the freshly loaded page.
		/// </remarks>
		/// <seealso cref="M:OpenQA.Selenium.INavigation.GoToUrl(System.String)" />
		/// <seealso cref="M:OpenQA.Selenium.INavigation.GoToUrl(System.Uri)" />
		/// <value>The url.</value>
		public string Url
		{
			get
			{
				return this.OriginalDriver.Url;
			}

			set
			{
				this.OriginalDriver.Url = value;
			}
		}

		/// <summary>
		/// Gets the title of the current browser window.
		/// </summary>
		/// <value>The value.</value>
		public string Title
		{
			get
			{
				return this.OriginalDriver.Title;
			}
		}

		/// <summary>
		/// Gets the source of the page last loaded by the browser.
		/// </summary>
		/// <remarks>
		/// If the page has been modified after loading (for example, by JavaScript).
		/// there is no guarantee that the returned text is that of the modified page.
		/// Please consult the documentation of the particular driver being used to.
		/// determine whether the returned text reflects the current state of the page.
		/// or the text last sent by the web server. The page source returned is a.
		/// representation of the underlying DOM: do not expect it to be formatted.
		/// or escaped in the same way as the response sent from the web server.
		/// </remarks>
		/// <value>The value.</value>
		public string PageSource
		{
			get
			{
				return this.OriginalDriver.PageSource;
			}
		}

		/// <summary>
		/// Gets the current window handle, which is an opaque handle to this.
		/// window that uniquely identifies it within this driver instance.
		/// </summary>
		/// <value>The value.</value>
		public string CurrentWindowHandle
		{
			get
			{
				return this.OriginalDriver.CurrentWindowHandle;
			}
		}

		/// <summary>
		/// Gets the window handles of open browser windows.
		/// </summary>
		/// <value>The value.</value>
		public ReadOnlyCollection<string> WindowHandles
		{
			get
			{
				return this.OriginalDriver.WindowHandles;
			}
		}

		/// <summary>
		/// Executes JavaScript in the context of the currently selected frame or window.
		/// </summary>
		/// <param name="script">The JavaScript code to execute.</param>
		/// <param name="args">The arguments to the script.</param>
		/// <returns>
		/// The value returned by the script.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The <see cref="M:OpenQA.Selenium.IJavaScriptExecutor.ExecuteScript(System.String,System.Object[])" />method executes JavaScript in the context of.
		/// the currently selected frame or window. This means that "document" will refer.
		/// to the current document. If the script has a return value, then the following.
		/// steps will be taken:.
		/// </para>
		/// <para>
		///   <list type="bullet">
		///     <item>
		///       <description>For an HTML element, this method returns a <see cref="T:OpenQA.Selenium.IWebElement" /></description>
		///     </item>
		///     <item>
		///       <description>For a number, a <see cref="T:System.Int64" /> is returned</description>
		///     </item>
		///     <item>
		///       <description>For a boolean, a <see cref="T:System.Boolean" /> is returned</description>
		///     </item>
		///     <item>
		///       <description>For all other cases a <see cref="T:System.String" /> is returned.</description>
		///     </item>
		///     <item>
		///       <description>For an array,we check the first element, and attempt to return a.
		/// <see cref="T:System.Collections.Generic.List`1" /> of that type, following the rules above. Nested lists are not.
		/// supported.</description>
		///     </item>
		///     <item>
		///       <description>If the value is null or there is no return value,.
		/// <see langword="null" /> is returned.</description>
		///     </item>
		///   </list>
		/// </para>
		/// <para>
		/// Arguments must be a number (which will be converted to a <see cref="T:System.Int64" />),.
		/// a <see cref="T:System.Boolean" />, a <see cref="T:System.String" /> or a <see cref="T:OpenQA.Selenium.IWebElement" />.
		/// An exception will be thrown if the arguments do not meet these criteria.
		/// The arguments will be made available to the JavaScript via the "arguments" magic.
		/// variable, as if the function were called via "Function.apply".
		/// </para>
		/// </remarks>
		public object ExecuteScript(string script, params object[] args)
		{
			return ((IJavaScriptExecutor)this.OriginalDriver).ExecuteScript(script, args);
		}

		/// <summary>
		/// Executes JavaScript asynchronously in the context of the currently selected frame or window.
		/// </summary>
		/// <param name="script">The JavaScript code to execute.</param>
		/// <param name="args">The arguments to the script.</param>
		/// <returns>
		/// The value returned by the script.
		/// </returns>
		public object ExecuteAsyncScript(string script, params object[] args)
		{
			return ((IJavaScriptExecutor)this.OriginalDriver).ExecuteAsyncScript(script, args);
		}

		/// <summary>
		/// Gets a <see cref="T:OpenQA.Selenium.Screenshot" /> object representing the image of the page on the screen.
		/// </summary>
		/// <returns>
		/// A <see cref="T:OpenQA.Selenium.Screenshot" /> object containing the image.
		/// </returns>
		public Screenshot GetScreenshot()
		{
			return ((ITakesScreenshot)this.OriginalDriver).GetScreenshot();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize((object)this);
		}

		/// <summary>
		/// Disposes the specified disposing.
		/// </summary>
		/// <param name="disposing">The disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			this.OriginalDriver.Dispose();
		}
	}
}