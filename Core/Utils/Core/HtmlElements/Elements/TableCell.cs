
using System.Collections.ObjectModel;
using System.Globalization;

using Container;

using OpenQA.Selenium;

namespace Core.HtmlElements.Elements
{
	/// <summary>
	/// The Table standard cell UI element.
	/// </summary>
	public class TableCell : TypifiedElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TableCell" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public TableCell(IWebElement element) : base(element)
		{
		}

		/// <summary>
		/// Performs a Click on the icon.
		/// </summary>
		public void Click()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Click the link '{0}'", this.Name));
			this.WrappedElement.Click();
		}

		/// <summary>
		/// Finds the elements.
		/// </summary>
		/// <param name="by">The by.</param>
		/// <returns>The collection.</returns>
		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			return this.WrappedElement.FindElements(by);
		}
	}
}