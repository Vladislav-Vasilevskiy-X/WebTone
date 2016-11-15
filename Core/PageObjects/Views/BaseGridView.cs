using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.Objects;
using OpenQA.Selenium.Support.PageObjects;

namespace Core.PageObjects.Views
{
	/// <summary>
	///     A base grid view.
	/// </summary>
	/// <seealso cref="View" />
	public class BaseGridView : View
	{
		/// <summary>
		///     Gets or sets the show entries select.
		/// </summary>
		/// <value>
		///     The show entries select.
		/// </value>
		[FindsBy(How = How.CssSelector, Using = ".dataTables_length select")]
		public Select ShowEntriesSelect { get; set; }

		/// <summary>
		///     Gets or sets the show entries message.
		/// </summary>
		/// <value>
		///     The show entries message.
		/// </value>
		[FindsBy(How = How.CssSelector, Using = ".dataTables_info")]
		public TextBlock ShowEntriesMessage { get; set; }

		/// <summary>
		///     Gets or sets the select all CheckBox.
		/// </summary>
		/// <value>
		///     The select all CheckBox.
		/// </value>
		[FindsBy(How = How.XPath, Using = "//*[contains(@id, 'selectAll')]")]
		public CheckBox SelectAllCheckBox { get; set; }

		/// <summary>
		///     Gets or sets the grid headers.
		/// </summary>
		/// <value>
		///     The grid headers.
		/// </value>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Used by HtmlElement decorator")]
		[FindsBy(How = How.XPath, Using = "//*[contains(@class, 'dataTable')]/thead//th[string-length()>0]")]
		public IList<Button> GridHeaders { get; set; }

		/// <summary>
		///     Gets or sets the page text buttons.
		/// </summary>
		/// <value>
		///     The page text buttons.
		/// </value>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Used by HtmlElement decorator")]
		[FindsBy(How = How.CssSelector, Using = "[id*=paginate] a[id]")]
		public IList<Button> PageTextButtons { get; set; }

		/// <summary>
		///     Gets or sets the page numbers buttons.
		/// </summary>
		/// <value>
		///     The page numbers buttons.
		/// </value>
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "Used by HtmlElement decorator")]
		[FindsBy(How = How.CssSelector, Using = "[id*=paginate] a:not([id])")]
		public IList<Button> PageNumbersButtons { get; set; }

		/// <summary>
		///     Gets or sets the paging status.
		/// </summary>
		/// <value>
		///     The paging status.
		/// </value>
		[FindsBy(How = How.CssSelector, Using = "[id*=paginate]")]
		public Button PagingStatus { get; set; }
	}
}