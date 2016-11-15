using System;
using System.Reflection;

namespace Core.PageObjects.Scenarios.Generic.ReadWrappers
{
	/// <summary>
	///     IReadableElement interface.
	/// </summary>
	public interface IReadableElement
	{
		/// <summary>
		///     Gets The type of the web element.
		/// </summary>
		/// <value>The type of the element.</value>
		Type ElementType { get; }

		/// <summary>
		///     Reads the page object property to model value.
		/// </summary>
		/// <param name="pageObject">The page object.</param>
		/// <param name="matchedPageObjectProperty">The matched page object property.</param>
		/// <param name="model">The model.</param>
		/// <param name="modelProperty">The model property.</param>
		void ReadPageObjectPropertyToModelProperty(object pageObject, PropertyInfo matchedPageObjectProperty, object model,
			PropertyInfo modelProperty);
	}
}