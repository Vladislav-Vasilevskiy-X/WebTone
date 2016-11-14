
using System;
using System.Globalization;

using Core.HtmlElements.Attributes;
using Core.HtmlElements.Exceptions;
using Core.HtmlElements.PageFactories;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Core.HtmlElements.Loaders.Decorators
{
	/// <summary>
	/// HtmlElementTypeAttributesHandler class.
	/// </summary>
	public class HtmlElementTypeAttributesHandler : AttributesHandler
	{
		private readonly Type htmlElementType;

		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlElementTypeAttributesHandler" /> class.
		/// </summary>
		/// <param name="type">The type.</param>
		public HtmlElementTypeAttributesHandler(Type type)
		{
			this.htmlElementType = type;
		}

		/// <summary>
		/// Builds the by.
		/// </summary>
		/// <returns>The value.</returns>
		public override By BuildBy()
		{
			var blocks = (BlockAttribute[])this.htmlElementType.GetCustomAttributes(typeof(BlockAttribute), true);
			if (blocks.Length > 0)
			{
				var block = blocks[0];
				var findsBy = block.Value;
				return this.BuildByFromFindsBy(findsBy);
			}

			throw new HtmlElementsException(string.Format(CultureInfo.InvariantCulture, "Cannot determine how to locate instance of {0}", this.htmlElementType));
		}

		/// <summary>
		/// Shoulds the cache.
		/// </summary>
		/// <returns>The value.</returns>
		public override bool ShouldCache()
		{
			return false;
		}
	}
}