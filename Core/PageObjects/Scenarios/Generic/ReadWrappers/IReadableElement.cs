using System;
using System.Diagnostics.CodeAnalysis;
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
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object",
			Justification = "pageObject is WebDriver term")]
		void ReadPageObjectPropertyToModelProperty(object pageObject, PropertyInfo matchedPageObjectProperty, object model,
			PropertyInfo modelProperty);
	}
}