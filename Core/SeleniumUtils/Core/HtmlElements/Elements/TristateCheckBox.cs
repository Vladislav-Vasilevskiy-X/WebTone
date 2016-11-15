using System.Globalization;
using Core.GeneralUtils.Container;
using OpenQA.Selenium;

namespace Core.SeleniumUtils.Core.HtmlElements.Elements
{
	/// <summary>
	///     TristateCheckBox class.
	/// </summary>
	public class TristateCheckBox : CheckBox
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TristateCheckBox" /> class.
		/// </summary>
		/// <param name="element">The element.</param>
		public TristateCheckBox(IWebElement element) : base(element)
		{
		}

		/// <summary>
		///     Gets a value indicating whether the partially selected.
		/// </summary>
		/// <value>The partially selected.</value>
		public bool PartiallySelected
		{
			get
			{
				Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Check if element {0} partially selected", Name));
				return WrappedElement.GetAttribute("value").Equals("Partially");
			}
		}

		/// <summary>
		///     The deselect.
		/// </summary>
		public override void Deselect()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Uncheck the checkbox '{0}'", Name));
			if (PartiallySelected)
			{
				WrappedElement.Click();
				WrappedElement.Click();
			}
			else
			{
				base.Deselect();
			}
		}

		/// <summary>
		///     The select.
		/// </summary>
		public override void Select()
		{
			Logger.WriteLine(string.Format(CultureInfo.InvariantCulture, "Check the checkbox '{0}'", Name));
			if (PartiallySelected)
			{
				WrappedElement.Click();
			}
			else
			{
				base.Select();
			}
		}
	}
}