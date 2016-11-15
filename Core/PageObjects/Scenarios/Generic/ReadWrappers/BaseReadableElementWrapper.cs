using System;
using System.Globalization;
using System.Reflection;
using Core.SeleniumUtils.Core.HtmlElements.Elements;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     Base class for mapping property values to element actions.
	/// </summary>
	/// <typeparam name="TElement">The type of the element.</typeparam>
	public abstract class BaseReadableElementWrapper<TElement> : IReadableElement
		where TElement : class
	{
		/// <summary>
		///     Gets Action to retrieve the value using the members of <see cref="TElement" />
		/// </summary>
		/// <value>The get value from element.</value>
		protected abstract Func<TElement, object> GetValueFromElement { get; }

		/// <summary>
		///     Gets the type of the web element.
		/// </summary>
		/// <value>The type.</value>
		public Type ElementType
		{
			get { return typeof(TElement); }
		}

		/// <summary>
		///     Reads the page object property to model value.
		///     If the element not displayed then do not read its value.
		///     If the type of a model property is not the same as the value read from element, then do not read it.
		/// </summary>
		/// <param name="pageObject">The page object.</param>
		/// <param name="matchedPageObjectProperty">The matched page object property.</param>
		/// <param name="model">The model.</param>
		/// <param name="modelProperty">The model property.</param>
		public void ReadPageObjectPropertyToModelProperty(object pageObject, PropertyInfo matchedPageObjectProperty,
			object model, PropertyInfo modelProperty)
		{
			var element = matchedPageObjectProperty.GetValue(pageObject) as TElement;
			if (element != null)
			{
				var typifiedElement = element as TypifiedElement;
				var htmlElement = element as HtmlElement;
				if ((typifiedElement == null || !typifiedElement.DisplayedWithTimeout(TimeSpan.FromMilliseconds(250))) &&
				    (htmlElement == null || !htmlElement.DisplayedWithTimeout(TimeSpan.FromMilliseconds(250))))
				{
					return;
				}

				object value = null;
				try
				{
					value = GetValueFromElement(element);
					SetModelValue(model, modelProperty, value);
				}
				catch (FormatException)
				{
					Console.WriteLine(@"The value {0} was not set to the property {1}. ", value, modelProperty.Name);
				}
			}
		}

		/// <summary>
		///     Preprocesses (if necessary) and sets the model value.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="modelProperty">The model property.</param>
		/// <param name="value">The value.</param>
		protected virtual void SetModelValue(object model, PropertyInfo modelProperty, object value)
		{
			modelProperty.SetValue(model, Convert.ChangeType(value, modelProperty.PropertyType, CultureInfo.InvariantCulture),
				null);
		}
	}
}