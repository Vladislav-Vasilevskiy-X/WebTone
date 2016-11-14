
using System;
using System.Globalization;
using System.Linq;

using OpenQA.Selenium;

namespace Core.HtmlElements.Elements
{
	/// <summary>
	/// CurrencyTextInput class.
	/// </summary>
	public class CurrencyTextInput : TextInput
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CurrencyTextInput" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public CurrencyTextInput(IWebElement element) : base(element)
		{
		}

		/// <summary>
		/// Sends the keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public override void SendKeys(string keys)
		{
			this.TestContext.WriteLine(string.Format(CultureInfo.InvariantCulture, "Type '{0}' to input '{1}'", keys, this.Name));
			this.WrappedElement.SendKeys((keys.IndexOf(",", StringComparison.Ordinal) != -1) ? keys.Replace(",", string.Empty) : keys);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
		}

		/// <summary>
		/// Clears the and send keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public override void ClearAndSendKeys(string keys)
		{
			this.TestContext.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text and type '{0}' to input '{1}'", keys, this.Name));
			this.WrappedElement.Clear();

			if (this.Text.Length > 0)
			{
				this.WrappedElement.Clear();
			}

			this.WrappedElement.SendKeys((keys.IndexOf(",", StringComparison.Ordinal) != -1) ? keys.Replace(",", string.Empty) : keys);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
		}

		/// <summary>
		/// The clear using backspace and send keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public override void ClearUsingBackspaceAndSendKeys(string keys)
		{
			this.TestContext.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text with Backspace and type '{0}' to input '{1}'", keys, this.Name));
			Enumerable.Range(0, this.Text.Length).ToList().ForEach(arg => this.WrappedElement.SendKeys(Keys.Backspace));

			if (this.Text.Length > 0)
			{
				this.WrappedElement.Clear();
			}

			this.WrappedElement.SendKeys((keys.IndexOf(",", StringComparison.Ordinal) != -1) ? keys.Replace(",", string.Empty) : keys);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
		}
	}
}