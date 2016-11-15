using System;
using Core.GeneralUtils.Constants;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     DefaultSortingColumnAttribute class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DefaultSortingColumnAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultSortingColumnAttribute" /> class.
		/// </summary>
		/// <param name="sortOrder">The sort order.</param>
		public DefaultSortingColumnAttribute(SortOrder sortOrder)
		{
			SortOrder = sortOrder;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultSortingColumnAttribute" /> class.
		/// </summary>
		public DefaultSortingColumnAttribute()
		{
			SortOrder = SortOrder.Ascending;
		}

		/// <summary>
		///     Gets the sort order.
		/// </summary>
		/// <value>The sort order.</value>
		public SortOrder SortOrder { get; private set; }
	}
}