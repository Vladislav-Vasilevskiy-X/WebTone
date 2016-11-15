using System.Linq;
using Core.GeneralUtils;
using Core.PageObjects.Scenarios.Generic.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic
{
	/// <summary>
	///     The aggregating class which contains the reference to the webdriver html block,.
	///     related to the block model.
	///     and the index of the model in the grid.
	/// </summary>
	/// <typeparam name="TModel">The type of the model.</typeparam>
	public class HtmlElementToBlockMappingModel<TModel>
	{
		/// <summary>
		///     Gets or sets the index.
		/// </summary>
		/// <value>The index.</value>
		public int Index { get; set; }

		/// <summary>
		///     Gets or sets the HTML element block.
		/// </summary>
		/// <value>The HTML element block.</value>
		public object HtmlElementBlock { get; set; }

		/// <summary>
		///     Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public TModel Model { get; set; }

		/// <summary>
		///     Gets the get selectable item.
		/// </summary>
		/// <value>The get selectable item.</value>
		public CheckBox GetSelectableItem
		{
			get
			{
				return
					HtmlElementBlock.GetPropertyValuesWithFilteredAttribute<CheckBox, SelectableCheckBoxAttribute>(x => true).First();
			}
		}
	}
}