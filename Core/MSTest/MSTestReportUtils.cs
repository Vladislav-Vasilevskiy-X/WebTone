using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTest
{
	/// <summary>
	/// MSTestReportUtils class.
	/// </summary>
	public static class MSTestReportUtils
	{
		/// <summary>
		/// Check if current running test methods contains the attribute <see cref="IssueAttribute" />.
		/// Parse this attribute and put data into .trx report using <see cref="Assert.Inconclusive()" />.
		/// </summary>
		/// <param name="testContext">The MSTestContext.</param>
		public static void AddIssueToReport(TestContext testContext)
		{
			var theClassName = testContext.FullyQualifiedTestClassName;
			var testName = testContext.TestName;

			var currentlyRunningClassType = Assembly.GetCallingAssembly().GetTypes().FirstOrDefault(f => f.FullName == theClassName);
			if (currentlyRunningClassType != null)
			{
				var currentlyRunningMethod = currentlyRunningClassType.GetMethod(testName);
				var issueAttributes = currentlyRunningMethod.GetCustomAttributes(typeof(IssueAttribute), true) as IEnumerable<IssueAttribute>;
				if (issueAttributes != null)
				{
					var firstOne = issueAttributes.FirstOrDefault();
					if (firstOne != null)
					{
						Assert.Inconclusive("Test failed by issue: Id: {0}, Description: {1}, Link: {2}", firstOne.Id, firstOne.Description, firstOne.Link);
					}
				}
			}
		}

		/// <summary>
		/// As Mstest has issue with formatting of test message and throws <see cref="FormatterException" /> when test message contain placeholders like '{0' or '2}'
		/// the braces should be replaced with angle brackets.
		/// </summary>
		/// <param name="testMessage">The test message.</param>
		/// <returns>Result value.</returns>
		public static string ReplaceBrackets(string testMessage)
		{
			return testMessage.Replace('{', '[').Replace('}', ']');
		}
	}
}