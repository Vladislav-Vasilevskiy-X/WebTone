
using System;
using System.Globalization;

using Container;

using OpenQA.Selenium;

namespace Core.HtmlElements.Elements
{
	/// <summary>
	/// The class that represents read-only text, e.g. label.
	/// </summary>
	public class TextBlock : TypifiedElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextBlock" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public TextBlock(IWebElement element) : base(element)
		{
		}

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get
			{
				var textContentValue = this.WrappedElement.GetAttribute("textContent");
				if (!string.IsNullOrEmpty(textContentValue))
				{
					return textContentValue;
				}

				return this.WrappedElement.GetAttribute("value") ?? this.WrappedElement.Text;
			}
		}

		/// <summary>
		/// Sends the keys not allowed.
		/// </summary>
		/// <param name="keys">The keys sequesnce.</param>
		/// <exception cref="System.ArgumentException">The text of element + this.Name +  was changed from ' + initialTextValue +  to ' +.
		/// keys + '.</exception>
		public void SendKeysNotAllowed(string keys)
		{
			var initialTextValue = this.Text;
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Type '{0}' to input '{1}'", keys, this.Name));
			this.WrappedElement.SendKeys(keys);
			Waiter.Try(() => this.Browser.ExecuteJavaScript("$(arguments[0]).keyup();", this.OriginalWebElement));
			if (initialTextValue.Equals(keys))
			{
				throw new ArgumentException("The text of element" + this.Name + " was changed from '" + initialTextValue + " to '" + keys + "'");
			}
		}
	}
}