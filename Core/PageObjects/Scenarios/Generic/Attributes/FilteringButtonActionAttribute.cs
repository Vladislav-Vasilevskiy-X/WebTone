using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     FilteringButtonActionAttribute class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class FilteringButtonActionAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="FilteringButtonActionAttribute" /> class.
		/// </summary>
		/// <param name="filteringButtonFlag">The filtering button flag.</param>
		public FilteringButtonActionAttribute(FilteringButtonFlag filteringButtonFlag)
		{
			FilteringButtonFlag = filteringButtonFlag;
		}

		/// <summary>
		///     Gets the filtering button flag.
		/// </summary>
		/// <value>The filtering button flag.</value>
		public FilteringButtonFlag FilteringButtonFlag { get; private set; }
	}
}