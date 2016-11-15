using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     The attribute should be placed on the <see cref="TypifiedElement" /> to reflect its default value.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DefaultValueAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="DefaultValueAttribute" /> class.
		/// </summary>
		/// <param name="flag">The flag.</param>
		public DefaultValueAttribute(DefaultValueFlag flag)
		{
			Flag = flag;
		}

		/// <summary>
		///     Gets the flag.
		/// </summary>
		/// <value>The flag.</value>
		public DefaultValueFlag Flag { get; private set; }
	}
}