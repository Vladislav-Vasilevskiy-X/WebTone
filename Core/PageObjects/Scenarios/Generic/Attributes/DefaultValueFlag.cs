using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     Flag for <see cref="TypifiedElement" />
	///     <see cref="Empty" /> used for default empty value of <see cref="TextInput" />
	///     <see cref="Selected" /> used for default selected value of <see cref="CheckBox" />
	///     <see cref="NotSelected" /> used for default not selected value of <see cref="CheckBox" />
	/// </summary>
	public enum DefaultValueFlag
	{
		/// <summary>
		///     The Empty item.
		/// </summary>
		Empty,

		/// <summary>
		///     The Selected item.
		/// </summary>
		Selected,

		/// <summary>
		///     The NotSelected item.
		/// </summary>
		NotSelected,

		/// <summary>
		///     The Active item.
		/// </summary>
		Active
	}
}