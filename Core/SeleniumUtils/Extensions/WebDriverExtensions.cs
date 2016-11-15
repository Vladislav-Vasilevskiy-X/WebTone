using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Core.SeleniumUtils.Extensions
{
	/// <summary>
	///     The WebDriverExtensions class.
	/// </summary>
	public static class WebDriverExtensions
	{
		#region Public Methods and Operators

		/// <summary>
		///     Hover will make a new Action to perform the hover of the element passed in.
		/// </summary>
		/// <param name="driver">IWebDriver object to be used.</param>
		/// <param name="by">By parameter used to search for the element.</param>
		public static void Hover(this IWebDriver driver, By by)
		{
			var elementToHover = driver.FindElement(by);
			new Actions(driver).MoveToElement(elementToHover).Build().Perform();
		}

		/// <summary>
		///     Wait for an element on the page, default is 10 seconds.
		/// </summary>
		/// <param name="driver">The IWebDriver object.</param>
		/// <param name="by">what to wait for.</param>
		/// <param name="maxWaitInSeconds">timeout in seconds, default is 10 seconds.</param>
		/// <returns>the IWebElement.</returns>
		public static IWebElement WaitForElement(this IWebDriver driver, By by, int maxWaitInSeconds = 10)
		{
			return new WebDriverWait(driver, new TimeSpan(0, 0, maxWaitInSeconds)).Until(d => d.FindElement(by));
		}

		/// <summary>
		///     Waits until the page contains the specified element with the specified search string.
		/// </summary>
		/// <param name="driver">IWebDriver object to be used.</param>
		/// <param name="by">By parameter used to search for the element.</param>
		/// <param name="text">String to search for.</param>
		/// <param name="maxWaitInSeconds">timeout in seconds, default is 10 seconds.</param>
		public static void WaitForTextInElement(this IWebDriver driver, By by, string text, int maxWaitInSeconds = 10)
		{
			new WebDriverWait(driver, new TimeSpan(0, 0, maxWaitInSeconds)).Until(d => d.FindElement(by).Text.Contains(text));
		}

		#endregion
	}
}