using System;
using System.Collections.Generic;
using Core.PageObjects.Scenarios.Generic.ReadWrappers;
using Core.SeleniumUtils.Core;

namespace Core.PageObjects.Scenarios.Generic.Containers
{
	/// <summary>
	///     The readable wrappers container.
	/// </summary>
	public class ReadableWrappersContainer
	{
		private readonly Dictionary<Type, IReadableElement> registeredWrappers = new Dictionary<Type, IReadableElement>();

		/// <summary>
		///     Initializes a new instance of the <see cref="ReadableWrappersContainer" /> class. .
		///     Register default wrappers.
		/// </summary>
		public ReadableWrappersContainer()
		{
			RegisterWrapper(new TextBlockReadableWrapper());
			RegisterWrapper(new TextInputReadableWrapper());
			RegisterWrapper(new CheckBoxReadableWrapper());
			RegisterWrapper(new SelectReadableWrapper());
			RegisterWrapper(new LinkReadableWrapper());
			RegisterWrapper(new CurrencyTextInputReadableWrapper());
			RegisterWrapper(new IconReadableWrapper());
		}

		/// <summary>
		///     Add the wrapper to the dictionary of registered wrappers.
		/// </summary>
		/// <param name="readableElement">The readableElement wrapper.</param>
		public void RegisterWrapper(IReadableElement readableElement)
		{
			registeredWrappers.Add(readableElement.ElementType, readableElement);
		}

		/// <summary>
		///     Gets the readableElement wrapper by the type of the element.
		/// </summary>
		/// <param name="elementType">Type of the wrapped element.</param>
		/// <returns>The readableElement wrapper.</returns>
		public IReadableElement GetWrapperByElementType(Type elementType)
		{
			IReadableElement readableElement;
			registeredWrappers.TryGetValue(elementType, out readableElement);

			if (readableElement == null)
			{
				throw new MissingImplementationException("Wrapper not set for element type " + elementType);
			}

			return readableElement;
		}
	}
}