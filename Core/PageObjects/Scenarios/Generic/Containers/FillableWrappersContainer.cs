using System;
using System.Collections.Generic;
using Core.PageObjects.Scenarios.Generic.InsertWrappers;

namespace Core.PageObjects.Scenarios.Generic.Containers
{
	/// <summary>
	///     Container class to register fillableElement wrappers which are used by generic insert scenarios.
	/// </summary>
	public class FillableWrappersContainer
	{
		private readonly Dictionary<Type, IFillableElement> registeredWrappers = new Dictionary<Type, IFillableElement>();

		/// <summary>
		///     Initializes a new instance of the <see cref="FillableWrappersContainer" /> class. .
		///     Register default wrappers.
		/// </summary>
		public FillableWrappersContainer()
		{
			RegisterWrapper(new SelectFillableElementWrapper());
			RegisterWrapper(new TextInputFillableElementWrapper());
			RegisterWrapper(new CheckBoxFillableElementWrapper());
			RegisterWrapper(new TristateCheckBoxFillableElementWrapper());
			RegisterWrapper(new CurrencyTextInputFillableElementWrapper());
		}

		/// <summary>
		///     Addd the wrapper to the dictionary of registered wrappers.
		/// </summary>
		/// <param name="fillableElement">The fillableElement.</param>
		public void RegisterWrapper(IFillableElement fillableElement)
		{
			registeredWrappers.Add(fillableElement.ElementType, fillableElement);
		}

		/// <summary>
		///     Gets the fillableElement wrapper by the type of the element.
		/// </summary>
		/// <param name="elementType">Type of the wrapped element.</param>
		/// <returns>The fillableElement wrapper.</returns>
		public IFillableElement GetWrapperByElementType(Type elementType)
		{
			IFillableElement fillableElement;
			registeredWrappers.TryGetValue(elementType, out fillableElement);
			return fillableElement;
		}
	}
}