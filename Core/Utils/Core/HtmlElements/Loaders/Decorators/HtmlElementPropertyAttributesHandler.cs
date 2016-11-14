
using System;
using System.Reflection;

using Core.HtmlElements.Attributes;
using Core.HtmlElements.PageFactories;
using Core.HtmlElements.Utils;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Core.HtmlElements.Loaders.Decorators
{
	/// <summary>
	/// HtmlElementPropertyAttributesHandler class.
	/// </summary>
	public class HtmlElementPropertyAttributesHandler : DefaultPropertyAttributesHandler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HtmlElementPropertyAttributesHandler" /> class.
		/// </summary>
		/// <param name="property">The property.</param>
		public HtmlElementPropertyAttributesHandler(PropertyInfo property) : base(property)
		{
		}

		/// <summary>
		/// Builds the by.
		/// </summary>
		/// <returns>The value.</returns>
		public override By BuildBy()
		{
			if (HtmlElementUtils.IsHtmlElement(this.Property) || HtmlElementUtils.IsHtmlElementList(this.Property))
			{
				var type = HtmlElementUtils.IsHtmlElementList(this.Property) ? HtmlElementUtils.GetGenericParameterType(this.Property) : this.Property.PropertyType;
				return this.BuildByFromHtmlElementAttributes(type);
			}
			else
			{
				return base.BuildBy();
			}
		}

		/// <summary>
		/// Builds the by from HTML element attributes.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The value.</returns>
		private By BuildByFromHtmlElementAttributes(Type type)
		{
			var findBys = (FindsByAttribute[])this.Property.GetCustomAttributes(typeof(FindsByAttribute));
			if (findBys.Length > 0)
			{
				return this.BuildByFromFindsByValues(findBys);
			}

			var blocks = (BlockAttribute[])type.GetCustomAttributes(typeof(BlockAttribute), true);
			if (blocks.Length > 0)
			{
				var block = blocks[0];
				var findsBy = block.Value;
				return this.BuildByFromFindsBy(findsBy);
			}

			return this.BuildByFromDefault();
		}
	}
}