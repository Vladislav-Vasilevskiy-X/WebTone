using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     The attribute should be used over the page object properties which values shold be trimmed when perform Get
	///     operations.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class TrimAttribute : Attribute
	{
	}
}