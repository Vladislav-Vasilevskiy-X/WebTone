
using System.Globalization;
using Container;
using OpenQA.Selenium;

namespace Core.HtmlElements.Elements
{
	/// <summary>
	/// Custom type. Takes text input or text block.
	/// For Admin -> Firm User Management -> User Defaults only. 
	/// Rate column can contain Text Input (if the user doesn't have any rate) or Text Block (if the user have already assigned current rate) for rate.
	/// </summary>
	public class CustomTextBlockInput : TypifiedElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomTextBlockInput" /> class.
		/// </summary>
		/// <param name="element">The Web element.</param>
		public CustomTextBlockInput(IWebElement element)
			: base(element)
		{
		}

		/// <summary>
		/// Gets text of the current element.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get
			{
				if (this.IsTextInput)
				{
					var enteredText = this.WrappedElement.GetAttribute("value");
					return enteredText == null ? string.Empty : enteredText;
				}

				var textContentValue = this.WrappedElement.GetAttribute("textContent");
				return !string.IsNullOrEmpty(textContentValue) ? textContentValue : this.WrappedElement.GetAttribute("value") ?? this.WrappedElement.Text;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current element is text input.
		/// </summary>
		/// <value>The is text input.</value>
		public bool IsTextInput
		{
			get
			{
				var attributeValue = this.WrappedElement.GetAttribute("readonly");
				if (string.IsNullOrEmpty(attributeValue))
				{
					return true;
				}

				return attributeValue.Contains("false");
			}
		}

		/// <summary>
		/// Clears and adds new text in text input.
		/// </summary>
		/// <param name="keys">Text to enter.</param>
		public void ClearAndSendKeys(string keys)
		{
			if (this.IsTextInput)
			{
				Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Clear text with Backspace and type '{0}' to input '{1}'", keys, this.Name));
				this.WrappedElement.Clear();
				if (this.Text.Length > 0)
				{
					this.WrappedElement.Clear();
				}

				this.WrappedElement.SendKeys(keys);
				Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
			}
		}

		/// <summary>
		/// Sends the one key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void SendOneKey(string key)
		{
			this.WrappedElement.SendKeys(key);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
		}
	}
}