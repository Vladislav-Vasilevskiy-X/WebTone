using System;
using System.Globalization;
using System.Reflection;
using Core.PageObjects.Scenarios.Generic.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.InsertWrappers
{
	/// <summary>
	///     Defines the behavior of <see cref="Select" /> element for Generic enter data scenarios.
	/// </summary>
	public class SelectFillableElementWrapper : IFillableElement
	{
		/// <summary>
		///     Gets The type of the web element.
		/// </summary>
		/// <value>The type of the element.</value>
		public Type ElementType
		{
			get { return typeof(Select); }
		}

		/// <summary>
		///     Select by exact or partial text depends on <see cref="SelectOptionByConditionAttribute.SearchOptionCondition" />
		/// </summary>
		/// <param name="pageObject">The page object.</param>
		/// <param name="matchedPageObjectProperty">The matched page object property.</param>
		/// <param name="modelValue">The model value.</param>
		public void EnterModelPropertyValueToPageObject(object pageObject, PropertyInfo matchedPageObjectProperty,
			object modelValue)
		{
			var stringModelValue = Convert.ToString(modelValue, CultureInfo.InvariantCulture);
			var element = matchedPageObjectProperty.GetValue(pageObject) as Select;
			if (element != null && !string.IsNullOrEmpty(stringModelValue))
			{
				if (!matchedPageObjectProperty.IsDefined(typeof(SelectOptionByConditionAttribute)) ||
				    matchedPageObjectProperty.GetCustomAttribute<SelectOptionByConditionAttribute>()
					    .SearchOptionCondition.Equals(SearchOptionCondition.Contains))
				{
					element.SelectByPartialText(stringModelValue);
				}
				else
				{
					element.SelectByText(stringModelValue);
				}
			}
		}
	}
}