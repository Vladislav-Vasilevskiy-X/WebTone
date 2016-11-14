
using System.Globalization;
using System.Linq;

using Container;

using OpenQA.Selenium;

namespace Core.HtmlElements.Elements
{
	/// <summary>
	/// TextInput class.
	/// </summary>
	public class TextInput : TypifiedElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextInput" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public TextInput(IWebElement element) : base(element)
		{
		}

		/// <summary>
		/// The clear using key and send keys.
		/// </summary>
		/// <param name="keys">
		/// The keys.
		/// </param>
		/// <param name="clearKey">
		/// The clear key.
		/// </param>
		private void ClearUsingKeyAndSendKeys(string keys, string clearKey)
		{
			this.TestContext.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text with {0} and type '{1}' to input '{2}'", clearKey, keys, this.Name));
			Enumerable.Range(0, this.Text.Length).ToList().ForEach(arg => this.WrappedElement.SendKeys(clearKey));

			this.WrappedElement.Clear();

			this.WrappedElement.SendKeys(keys);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear input '{0}'", this.Name));
			this.WrappedElement.Clear();
		}

		/// <summary>
		/// Clears the using backspace.
		/// </summary>
		public void ClearUsingBackspace()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear input '{0}' with Backspace", this.Name));
			Enumerable.Range(0, this.Text.Length).ToList().ForEach(arg => this.WrappedElement.SendKeys(Keys.Backspace));
		}

		/// <summary>
		/// Sends the keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public virtual void SendKeys(string keys)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Type '{0}' to input '{1}'", keys, this.Name));
			this.WrappedElement.SendKeys(keys);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
		}

		/// <summary>
		/// Clears the and send keys.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public virtual void ClearAndSendKeys(string keys)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text and type '{0}' to input '{1}'", keys, this.Name));
			this.WrappedElement.Clear();

			if (this.Text.Length > 0)
			{
				this.WrappedElement.Clear();
			}

			this.WrappedElement.SendKeys(keys);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
		}

		/// <summary>
		/// The clear using backspace and send keys.
		/// </summary>
		/// <param name="keys">
		/// The keys.
		/// </param>
		public virtual void ClearUsingBackspaceAndSendKeys(string keys)
		{
			this.ClearUsingKeyAndSendKeys(keys, Keys.Backspace);
		}

		/// <summary>
		/// The clear using delete and send keys.
		/// </summary>
		/// <param name="keys">
		/// The keys.
		/// </param>
		public virtual void ClearUsingDeleteAndSendKeys(string keys)
		{
			this.ClearUsingKeyAndSendKeys(keys, Keys.Delete);
		}

		/// <summary>
		/// Clears the and send one key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void ClearAndSendOneKey(string key)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text and type '{0}' to input '{1}'", key, this.Name));

			if (this.Text.Length > 0)
			{
				this.WrappedElement.Clear();
			}

			this.WrappedElement.SendKeys(key);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
		}

		/// <summary>
		/// Clears the and send keys without focus lost.
		/// </summary>
		/// <param name="keys">The keys.</param>
		public void ClearAndSendKeysWithoutFocusLost(string keys)
		{
			this.WrappedElement.SendKeys(Keys.Control + "a");
			this.WrappedElement.SendKeys(Keys.Backspace);
			this.WrappedElement.SendKeys(keys);
		}

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get
			{
				if ("textarea" == (this.WrappedElement.TagName))
				{
					return this.WrappedElement.Text;
				}

				var enteredText = this.WrappedElement.GetAttribute("value");
				if (enteredText == null)
				{
					return string.Empty;
				}

				return enteredText;
			}
		}
	}
}