
using System;
using System.Runtime.Serialization;

namespace Core
{
	/// <summary>
	/// Missing Implementation Exception.
	/// </summary>
	[Serializable]
	public class MissingImplementationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MissingImplementationException"/> class.
		/// </summary>
		public MissingImplementationException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MissingImplementationException"/> class.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		public MissingImplementationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MissingImplementationException"/> class.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		/// <param name="innerException">
		/// The inner exception.
		/// </param>
		public MissingImplementationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MissingImplementationException"/> class.
		/// </summary>
		/// <param name="info">
		/// The info.
		/// </param>
		/// <param name="context">
		/// The context.
		/// </param>
		protected MissingImplementationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}