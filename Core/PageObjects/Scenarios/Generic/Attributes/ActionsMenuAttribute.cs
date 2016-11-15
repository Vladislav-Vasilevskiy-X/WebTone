using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     Attribute for Actions menu list.
	/// </summary>
	[SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ActionsMenuAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="ActionsMenuAttribute" /> class.
		/// </summary>
		/// <param name="buttonFlag">The button flag.</param>
		public ActionsMenuAttribute(ActionsButtonFlag buttonFlag)
		{
			ActionsButtonFlag = buttonFlag;
		}

		/// <summary>
		///     Gets the actions button flag.
		/// </summary>
		/// <value>The actions button flag.</value>
		public ActionsButtonFlag ActionsButtonFlag { get; private set; }
	}
}