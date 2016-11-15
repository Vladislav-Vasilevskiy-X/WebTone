using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     The attribute should be used to define the <see cref="CheckBox" /> which will be selected.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SelectableCheckBoxAttribute : Attribute
	{
	}
}