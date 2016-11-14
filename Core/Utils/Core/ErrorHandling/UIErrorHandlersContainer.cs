
using System.Collections.Generic;

namespace Core.ErrorHandling
{
	/// <summary>
	/// Container that holdы implemented error handlers.
	/// </summary>
	internal static class UIErrorHandlersContainer
	{
		/// <summary>
		/// This list should hold all implemented error handlers.
		/// </summary>
		private static readonly IList<IUIErrorHandler> errorHandlers = new List<IUIErrorHandler> { new FatalErrorHandler() };

		/// <summary>
		/// Gets the error handlers.
		/// </summary>
		/// <value>
		/// The error handlers.
		/// </value>
		internal static IList<IUIErrorHandler> ErrorHandlers
		{
			get
			{
				return errorHandlers;
			}
		}
	}
}