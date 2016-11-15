using System.Reflection;
using Core.GeneralUtils.TestDataHandling;

namespace Core.SeleniumUtils
{
	/// <summary>
	///     TestDataReaderExtensions class.
	/// </summary>
	public static class TestDataReaderExtensions
	{
		/// <summary>
		///     Reads from end point tests.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="testDataReader">The test data reader.</param>
		/// <param name="file">The file.</param>
		/// <param name="testDataIds">The test data ids.</param>
		/// <returns>The result.</returns>
		public static T ReadFromEndPointTests<T>(this ITestDataReader testDataReader, string file, params string[] testDataIds)
		{
			var targetAssembly = Assembly.Load("EndPointsTests");
			return testDataReader.Read<T>(targetAssembly, file, testDataIds);
		}
	}
}