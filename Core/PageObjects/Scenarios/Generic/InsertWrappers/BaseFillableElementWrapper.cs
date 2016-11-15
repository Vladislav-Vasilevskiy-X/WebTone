using System;
using System.Reflection;

namespace Core.PageObjects.Scenarios.Generic.InsertWrappers
{
	/// <summary>
	///     BaseFillableElementWrapper class.
	/// </summary>
	/// <typeparam name="TElement">The TElement.</typeparam>
	public abstract class BaseFillableElementWrapper<TElement> : IFillableElement
		where TElement : class
	{
		/// <summary>
		///     Gets Enter the value from <see cref="object" /> using the methods of <see cref="TElement" />
		/// </summary>
		/// <value>The enter data action.</value>
		protected abstract Action<TElement, object> EnterDataAction { get; }

		/// <summary>
		///     Gets The type of the web element.
		/// </summary>
		/// <value>The type.</value>
		public Type ElementType
		{
			get { return typeof(TElement); }
		}

		/// <summary>
		///     Enters the model property value to page object.
		/// </summary>
		/// <param name="pageObject">The page object.</param>
		/// <param name="matchedPageObjectProperty">The matched page object property.</param>
		/// <param name="modelValue">The model value.</param>
		public void EnterModelPropertyValueToPageObject(object pageObject, PropertyInfo matchedPageObjectProperty,
			object modelValue)
		{
			var element = matchedPageObjectProperty.GetValue(pageObject) as TElement;
			EnterDataAction(element, modelValue);
		}
	}
}