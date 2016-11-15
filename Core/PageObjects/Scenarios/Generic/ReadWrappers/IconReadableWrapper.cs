using System;
using System.Linq;
using System.Reflection;
using Core.PageObjects.Scenarios.Generic.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     The icon readable wrapper.
	/// </summary>
	public class IconReadableWrapper : BaseReadableElementWrapper<Icon>
	{
		/// <summary>
		///     Gets class of icon element.
		/// </summary>
		/// <value>The get value from element.</value>
		protected override Func<Icon, object> GetValueFromElement
		{
			get { return iconElement => iconElement.GetAttribute("class"); }
		}

		/// <summary>
		///     Preprocesses (if necessary) and sets the model value.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="modelProperty">The model property.</param>
		/// <param name="value">The value.</param>
		protected override void SetModelValue(object model, PropertyInfo modelProperty, object value)
		{
			var propertyType = modelProperty.PropertyType;

			if (value == null || !propertyType.IsEnum)
			{
				modelProperty.SetValue(model, null);
				return;
			}

			var stringValue = value.ToString();

			// Go over all enum values and check their IconValue attribute
			var enumValues = propertyType.GetEnumValues();
			foreach (var enumValue in enumValues)
			{
				var enumName = propertyType.GetEnumName(enumValue);
				var enumValueMember = propertyType.GetMember(enumName)[0];

				// Enum value has IconValue attribute with appropriate value, namely is contained by stringValue
				// E.g., value = "icon icon_warning" and enumeration has the following enum value:
				//	IconValue("icon_warning") Warning,
				if (
					enumValueMember.GetCustomAttributes(typeof(IconValueAttribute))
						.OfType<IconValueAttribute>()
						.Any(iconValueAttribute => stringValue.Contains(iconValueAttribute.Value)))
				{
					modelProperty.SetValue(model, enumValue);
					return;
				}
			}

			// Nothing found - set default value
			modelProperty.SetValue(model, null);
		}
	}
}