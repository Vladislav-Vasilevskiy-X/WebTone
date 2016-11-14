
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Container;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Core.HtmlElements.Elements
{
	/// <summary>
	/// Select class.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Select", Justification = "It isn't recommended to use keyword as the name of a type.")]
	public class Select : TypifiedElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Select" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public Select(IWebElement element) : base(element)
		{
		}

		/// <summary>
		/// Gets the select.
		/// </summary>
		/// <returns>The value.</returns>
		private SelectElement GetSelect()
		{
			return new SelectElement(this.WrappedElement);
		}

		/// <summary>
		/// Gets a value indicating whether is multiple.
		/// </summary>
		/// <value>The is multiple.</value>
		public bool IsMultiple
		{
			get
			{
				return this.GetSelect().IsMultiple;
			}
		}

		/// <summary>
		/// Gets the options.
		/// </summary>
		/// <value>The options.</value>
		public IList<IWebElement> Options
		{
			get
			{
				return this.GetSelect().Options;
			}
		}

		/// <summary>
		/// Gets all selected options.
		/// </summary>
		/// <value>All selected options.</value>
		public IList<IWebElement> AllSelectedOptions
		{
			get
			{
				return this.GetSelect().AllSelectedOptions;
			}
		}

		/// <summary>
		/// Selecteds the option.
		/// </summary>
		/// <returns>The value.</returns>
		public IWebElement SelectedOption()
		{
			return this.GetSelect().SelectedOption;
		}

		/// <summary>
		/// Determines whether [has selected option].
		/// </summary>
		/// <returns>The value.</returns>
		public bool HasSelectedOption()
		{
			foreach (var option in this.Options)
			{
				if (option.Selected)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Selects the by text.
		/// </summary>
		/// <param name="text">The text.</param>
		public void SelectByText(string text)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Select the option from element '{0}' by text '{1}'", this.Name, text));
			this.GetSelect().SelectByText(text);
		}

		/// <summary>
		/// Selects the by partial text.
		/// </summary>
		/// <param name="text">The text.</param>
		public void SelectByPartialText(string text)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Select the option from element '{0}' by partial text '{1}'", this.Name, text));
			var selectElement = this.GetSelect();
			var fullOptionText = selectElement.Options.Select(opt => opt.Text).First(x => x.Contains(text));
			this.GetSelect().SelectByText(fullOptionText);
		}

		/// <summary>
		/// Selects the index of the by.
		/// </summary>
		/// <param name="index">The index.</param>
		public void SelectByIndex(int index)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Select the option from element '{0}' by index '{1}'", this.Name, index));
			this.GetSelect().SelectByIndex(index);
		}

		/// <summary>
		/// Selects the by value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void SelectByValue(string value)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Select the option from element '{0}' by value '{1}'", this.Name, value));
			this.GetSelect().SelectByValue(value);
		}

		/// <summary>
		/// Deselects all.
		/// </summary>
		public void DeselectAll()
		{
			this.GetSelect().DeselectAll();
		}

		/// <summary>
		/// Deselects the by text.
		/// </summary>
		/// <param name="text">The text.</param>
		public void DeselectByText(string text)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Deselect the option from element '{0}' by text '{1}'", this.Name, text));
			this.GetSelect().DeselectByText(text);
		}

		/// <summary>
		/// Deselects the index of the by.
		/// </summary>
		/// <param name="index">The index.</param>
		public void DeselectByIndex(int index)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Deselect the option from element '{0}' by index '{1}'", this.Name, index));
			this.GetSelect().DeselectByIndex(index);
		}

		/// <summary>
		/// Deselects the by value.
		/// </summary>
		/// <param name="value">The value.</param>
		public void DeselectByValue(string value)
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Deselect the option from element '{0}' by value '{1}'", this.Name, value));
			this.GetSelect().DeselectByValue(value);
		}
	}
}