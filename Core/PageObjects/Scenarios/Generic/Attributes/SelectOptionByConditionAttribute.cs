using System;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     Search Option Condition.
	/// </summary>
	public enum SearchOptionCondition
	{
		/// <summary>
		///     The Contains item.
		/// </summary>
		Contains,

		/// <summary>
		///     The Equals item.
		/// </summary>
		Equals
	}

	/// <summary>
	///     Select Option By Condition Attribute used by GenericScenarios.
	///     If the attribute specified over the <see cref="Select" /> element with value
	///     <see cref="PageObjects.Scenarios.Generic.Attributes.SearchOptionCondition.Equals" />
	///     then <see cref="GenericScenarios.EnterModelData{TPageObject,TModel}(TModel,TPageObject)" /> for such property would
	///     use <see cref="Select.SelectByText" /> action.
	///     Otherwise <see cref="Select.SelectByPartialText" /> will be used as default action as before.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SelectOptionByConditionAttribute : Attribute
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="SelectOptionByConditionAttribute" /> class.
		/// </summary>
		/// <param name="searchOptionCondition">The search option condition.</param>
		public SelectOptionByConditionAttribute(SearchOptionCondition searchOptionCondition)
		{
			SearchOptionCondition = searchOptionCondition;
		}

		/// <summary>
		///     Gets the search option condition.
		/// </summary>
		/// <value>The search option condition.</value>
		public SearchOptionCondition SearchOptionCondition { get; private set; }
	}
}