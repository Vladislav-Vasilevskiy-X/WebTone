using System;
using System.Collections.Generic;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     SkipFlag enum.
	/// </summary>
	public enum SkipFlag
	{
		/// <summary>
		///     The SkipFirst item.
		/// </summary>
		SkipFirst,

		/// <summary>
		///     The DoNotSkip item.
		/// </summary>
		DoNotSkip
	}

	/// <summary>
	///     The attribute should be placed on the <see cref="IList{T}" /> property which represents the list of grid blocks.
	///     If you want to skip first item (e.g. if it is a header) use <see cref="SkipFlag" /> SkipFirst as parameter.
	///     By default, no blocks are skipped.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class PropertyStorageListAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="PropertyStorageListAttribute" /> class.
		/// </summary>
		/// <param name="skipFlag">The skip flag.</param>
		public PropertyStorageListAttribute(SkipFlag skipFlag)
		{
			SkipFlag = skipFlag;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="PropertyStorageListAttribute" /> class.
		/// </summary>
		public PropertyStorageListAttribute()
		{
			SkipFlag = SkipFlag.DoNotSkip;
		}

		/// <summary>
		///     Gets the skip flag.
		/// </summary>
		/// <value>The skip flag.</value>
		public SkipFlag SkipFlag { get; private set; }
	}
}