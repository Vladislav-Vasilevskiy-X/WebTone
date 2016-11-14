
using System;
using System.Globalization;
using System.IO;

namespace FTBSeleniumUtilities.Utils
{
	/// <summary>
	/// PathUtils class.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Utils")]
	public static class PathUtils
	{
		#region Constants

		private const string TestDataFolderName = "TestData";

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Get full path by relative path to the file.
		/// </summary>
		/// <param name="relativePath">Relative path string.</param>
		/// <returns>The full path.</returns>
		public static string GetFullPath(string relativePath)
		{
			var fullPath = Path.Combine(Environment.CurrentDirectory, string.Format(CultureInfo.InvariantCulture, @"..\..\{0}", relativePath));
			if (!File.Exists(fullPath))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File '{0}' doesn't exist", relativePath));
			}

			return fullPath;
		}

		/// <summary>
		/// Gets the test data resource path.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>The result.</returns>
		public static string GetTestDataResourcePath(string fileName)
		{
			var fullPath = Path.Combine(Environment.CurrentDirectory, string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", TestDataFolderName, fileName));
			if (!File.Exists(fullPath))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File '{0}' doesn't exist", fileName));
			}

			return fullPath;
		}

		#endregion
	}
}