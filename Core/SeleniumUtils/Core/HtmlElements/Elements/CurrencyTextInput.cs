using System;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     CurrencyTextInput class.
	/// </summary>
	public class CurrencyTextInput : TextInput
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CurrencyTextInput" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public CurrencyTextInput(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Sends the keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public override void SendKeys(string keys)
		{
			TestContext.WriteLine(string.Format(CultureInfo.InvariantCulture, "Type '{0}' to input '{1}'", keys, Name));
			WrappedElement.SendKeys(keys.IndexOf(",", StringComparison.Ordinal) != -1 ? keys.Replace(",", string.Empty) : keys);
			Waiter.Try(() => Browser.ExecuteJavaScript("$(arguments[0]).keyup();", OriginalWebElement));
		}

		/// <summary>
		///     Clears the and send keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public override void ClearAndSendKeys(string keys)
		{
			TestContext.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text and type '{0}' to input '{1}'", keys,
				Name));
			WrappedElement.Clear();

			if (Text.Length > 0)
			{
				WrappedElement.Clear();
			}

			WrappedElement.SendKeys(keys.IndexOf(",", StringComparison.Ordinal) != -1 ? keys.Replace(",", string.Empty) : keys);
			Waiter.Try(() => Browser.ExecuteJavaScript("$(arguments[0]).keyup();", OriginalWebElement));
		}

		/// <summary>
		///     The clear using backspace and send keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public override void ClearUsingBackspaceAndSendKeys(string keys)
		{
			TestContext.WriteLine(string.Format(CultureInfo.InvariantCulture,
				"Clear text with Backspace and type '{0}' to input '{1}'", keys, Name));
			Enumerable.Range(0, Text.Length).ToList().ForEach(arg => WrappedElement.SendKeys(Keys.Backspace));

			if (Text.Length > 0)
			{
				WrappedElement.Clear();
			}

			WrappedElement.SendKeys(keys.IndexOf(",", StringComparison.Ordinal) != -1 ? keys.Replace(",", string.Empty) : keys);
			Waiter.Try(() => Browser.ExecuteJavaScript("$(arguments[0]).keyup();", OriginalWebElement));
		}
	}
}