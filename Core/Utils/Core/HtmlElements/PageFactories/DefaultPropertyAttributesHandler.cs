
using System;
using System.Linq;
using System.Reflection;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Core.HtmlElements.PageFactories
{
	/// <summary>
	/// DefaultPropertyAttributesHandler class.
	/// </summary>
	public class DefaultPropertyAttributesHandler : AttributesHandler
	{
		/// <summary>
		/// Gets the property.
		/// </summary>
		/// <value>The property.</value>
		public PropertyInfo Property { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultPropertyAttributesHandler" /> class.
		/// </summary>
		/// <param name="property">The property.</param>
		public DefaultPropertyAttributesHandler(PropertyInfo property)
		{
			this.Property = property;
		}

		/// <summary>
		/// Shoulds the cache.
		/// </summary>
		/// <returns>The value.</returns>
		public override bool ShouldCache()
		{
			return this.Property.GetCustomAttribute<CacheLookupAttribute>(false) != null;
		}

		/// <summary>
		/// Builds the by.
		/// </summary>
		/// <returns>The value.</returns>
		public override By BuildBy()
		{
			By ans = null;

			var findBys = (FindsByAttribute[])this.Property.GetCustomAttributes(typeof(FindsByAttribute), false);
			if (findBys.Length > 0)
			{
				ans = this.BuildByFromFindsByValues(findBys.ToArray());
			}

			if (ans == null)
			{
				ans = this.BuildByFromDefault();
			}

			if (ans == null)
			{
				throw new ArgumentException("Cannot determine how to locate element " + this.Property);
			}

			return ans;
		}

		/// <summary>
		/// Builds the by from default.
		/// </summary>
		/// <returns>The value.</returns>
		protected virtual By BuildByFromDefault()
		{
			return new ByChained(By.Id(this.Property.Name), By.Name(this.Property.Name));
		}
	}
}