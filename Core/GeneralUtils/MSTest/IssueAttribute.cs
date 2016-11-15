using System;

namespace Core.GeneralUtils.MSTest
{
	/// <summary>
	///     IssueAttribute class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class IssueAttribute : Attribute
	{
		/// <summary>
		///     Gets or sets the id.
		/// </summary>
		/// <value>The id.</value>
		public int Id { get; set; }

		/// <summary>
		///     Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		///     Gets or sets the link.
		/// </summary>
		/// <value>The link.</value>
		public string Link { get; set; }
	}
}