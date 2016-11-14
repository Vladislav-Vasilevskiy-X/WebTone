
namespace Core.ErrorHandling
{
	/// <summary>
	/// UI Error Handler interface.
	/// </summary>
	internal interface IUIErrorHandler
	{
		/// <summary>
		/// Gets a value indicating whether error exists.
		/// </summary>
		/// <value>
		///   <c>true</c> if error exists; otherwise, <c>false</c>.
		/// </value>
		bool ErrorExists { get; }

		/// <summary>
		/// Handles the error.
		/// </summary>
		void HandleError();
	}
}