using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     Represents slider-like web element, for instance ledes element on admin pages.
	/// </summary>
	/// <seealso cref="Core.HtmlElements.Elements.TypifiedElement" />
	public class Switcher : TypifiedElement
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Switcher" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public Switcher(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Gets the label.
		/// </summary>
		/// <value>The label.</value>
		public IWebElement Label
		{
			get
			{
				try
				{
					return WrappedElement.FindElement(By.XPath("following-sibling::label"));
				}
				catch
				{
					return null;
				}
			}
		}

		/// <summary>
		///     Gets the label text.
		/// </summary>
		/// <value>The label text.</value>
		public string LabelText
		{
			get
			{
				var label = Label;
				return label == null ? null : label.Text;
			}
		}

		/// <summary>
		///     Clicks this instance.
		/// </summary>
		public void Click()
		{
			Browser.ExecuteJavaScript("document.getElementById('" + GetAttribute("id") + "').click();");
		}

		/// <summary>
		///     Toggles the specified on.
		/// </summary>
		/// <param name="on">if set to <c>true</c> [on].</param>
		public void Toggle(bool on)
		{
			var clickIsNeeded = Selected ^ on;

			if (clickIsNeeded)
			{
				Click();
			}
		}
	}
}