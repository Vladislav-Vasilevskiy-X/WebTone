
namespace TestDataHandling.Placeholders
{
	/// <summary>
	/// Operation Period Model.
	/// </summary>
	public class OperationPeriodModel
	{
		/// <summary>
		/// Gets or sets the operation.
		/// </summary>
		/// <value>The operation.</value>
		public string Operation { get; set; }

		/// <summary>
		/// Gets or sets the periods amount.
		/// </summary>
		/// <value>The periods amount.</value>
		public string PeriodsAmount { get; set; }

		/// <summary>
		/// Gets or sets the period.
		/// </summary>
		/// <value>The period.</value>
		public PeriodType? Period { get; set; }
	}
}