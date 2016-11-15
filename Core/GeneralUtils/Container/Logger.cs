using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Core.GeneralUtils.MSTest;
using Core.GeneralUtils.Verification;
using Core.SeleniumUtils;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.GeneralUtils.Container
{
	/// <summary>
	///     This class holds the functionality for logging the EndPoint and Selenium actions.
	/// </summary>
	public static class Logger
	{
		private const string NoTestContextToLog = "NoTestContextToLog";

		private static readonly object locker = new object();

		private static readonly Dictionary<string, List<string>> testToLogDictionary = new Dictionary<string, List<string>>
		{
			{NoTestContextToLog, new List<string>()}
		};

		private static string logPath;

		private static bool? localRun;

		/// <summary>
		///     Gets the test context.
		/// </summary>
		/// <value>The test context.</value>
		private static TestContext testContext
		{
			get { return MSTestContext.Instance; }
		}

		/// <summary>
		///     Gets the full name of the test.
		/// </summary>
		/// <value>The full name of the test.</value>
		private static string FullTestName
		{
			get
			{
				// Test context can be null when action should be logged from method with "ClassInitialize" attribute
				if (testContext == null)
				{
					return null;
				}

				return testContext.FullyQualifiedTestClassName + "." + testContext.TestName;
			}
		}

		/// <summary>
		///     Gets a value indicating whether the current run is a local one or not.
		/// </summary>
		private static bool LocalRun
		{
			get
			{
				if (!localRun.HasValue)
				{
					localRun = !Process.GetProcessesByName("TEX.Agent").Any();
				}

				return localRun.Value;
			}
		}

		/// <summary>
		///     Create the html file to log into.
		/// </summary>
		private static void CreateLogFile()
		{
			if (!string.IsNullOrEmpty(logPath))
			{
				return;
			}

			var path = Path.GetDirectoryName(testContext.TestRunDirectory) + "\\TestLog\\";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			logPath = Path.Combine(path, "TestLog.html");

			using (var outfile = new StreamWriter(logPath, true))
			{
				outfile.WriteLine("<head>");
				outfile.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\" />");
				outfile.WriteLine("<title>Test Run Log</title>");
				outfile.WriteLine("</head>");
				outfile.WriteLine("<body>");
				outfile.WriteLine("<h2 aligne=\"center\">Test Run Log</h2>");
				outfile.WriteLine("<table border=\"1\" cellpadding=\"3\">");
				outfile.WriteLine("<tr bgcolor=\"#CCCCCC\" style=\"font-weight:bold\">");
				outfile.WriteLine("<td style=\"text-align:center\">Result</td>");
				outfile.WriteLine("<td style=\"text-align:left\">Log</td>");
				outfile.WriteLine("</tr>");
			}
		}

		/// <summary>
		///     Log the result into html file.
		/// </summary>
		/// <param name="testPassed">When test fails at Verification, this is false.</param>
		private static void PrintTestResults(bool testPassed = true)
		{
			if (LocalRun)
			{
				return;
			}

			lock (locker)
			{
				CreateLogFile();

				using (var outfile = new StreamWriter(logPath, true))
				{
					List<string> thereIsNoTestLogList;
					VerifyContext.Current.IsTrue(testToLogDictionary.TryGetValue(NoTestContextToLog, out thereIsNoTestLogList),
						"There are no missed test logs connected to tests");

					foreach (var text in thereIsNoTestLogList)
					{
						outfile.WriteLine(text);
						outfile.WriteLine("<br>");
					}

					testToLogDictionary[NoTestContextToLog].Clear();

					outfile.WriteLine("<tr>");

					// Test name
					outfile.WriteLine("<td valign=\"top\" colspan=\"3\">");
					outfile.WriteLine(testContext.TestName + "&nbsp;&nbsp;&nbsp;" + testContext.FullyQualifiedTestClassName);
					outfile.WriteLine("</td>");
					outfile.WriteLine("</tr><tr>");

					// Result
					var result = testContext.CurrentTestOutcome.ToString() == "Passed" && testPassed ? "Passed" : "Failed";
					outfile.WriteLine(result == "Passed" ? "<td valign=\"top\">" : "<td valign=\"top\" BGCOLOR=\"#FF8080\">");
					outfile.WriteLine(result);
					outfile.WriteLine("</td>");

					// Log
					outfile.WriteLine("<td valign=\"top\">");

					List<string> testLog;
					VerifyContext.Current.IsTrue(testToLogDictionary.TryGetValue(FullTestName, out testLog),
						"No Log for test " + FullTestName);

					foreach (var text in testLog)
					{
						outfile.WriteLine(text.Replace("\n", "<br>").Replace("<script>", "#script#").Replace("</script>", "#/script#"));
						outfile.WriteLine("<br>");
					}

					outfile.WriteLine("</td>");
					outfile.WriteLine("</tr>");
				}

				testToLogDictionary.Remove(FullTestName);
			}
		}

		/// <summary>
		///     Log message to VS console and Test output every time.
		///     In case it is a VM run, it logs to a separate log file as additional log.
		/// </summary>
		/// <param name="text">The text to log.</param>
		public static void WriteLine(string text)
		{
			if (testContext != null)
			{
				testContext.WriteLine(text.Replace('{', '(').Replace('}', ')'));
			}

			if (!LocalRun)
			{
				AddToTestToLogDictionary(text.Replace('{', '(').Replace('}', ')'));
			}
		}

		/// <summary>
		///     Log API request.
		/// </summary>
		/// <param name="requestLog">The request log.</param>
		public static void LogAPIRequest(List<string> requestLog)
		{
			if (LocalRun)
			{
				return;
			}

			AddRangeToTestToLogDictionary(requestLog);
		}

		/// <summary>
		///     Log API respond.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <param name="statusCode">The status code.</param>
		public static void LogAPIRespond(string content, HttpStatusCode statusCode)
		{
			if (LocalRun)
			{
				return;
			}

			var message = string.Format(CultureInfo.CurrentCulture,
				"\nResponse:{3}\n\tStatus code: {0}-{1}\r\n\tResponse content: {2}", (int) statusCode, statusCode,
				string.IsNullOrWhiteSpace(content) ? "#empty#" : MSTestReportUtils.ReplaceBrackets(content),
				DateTime.Now.ToString("G", CultureInfo.InvariantCulture));

			AddToTestToLogDictionary(message);
			AddToTestToLogDictionary("-------");

			if (statusCode != HttpStatusCode.OK)
			{
				testContext.WriteLine(message);
			}
		}

		/// <summary>
		///     Log the exceptions if there are any.
		/// </summary>
		/// <param name="assertFailures">The exceptions.</param>
		public static void Verify(List<UnitTestVerifyException> assertFailures)
		{
			if (!assertFailures.Any())
			{
				PrintTestResults();
				return;
			}

			var formattedTestFailures = new StringBuilder();
			formattedTestFailures.AppendLine("\nThe folowing verifiers failed:");
			assertFailures.Select(ex => ex.Message)
				.ForEach(m => formattedTestFailures.AppendLine(m.ReplaceFirst("Assert", "Verify")));
			WriteLine(formattedTestFailures.ToString());

			WriteLine("Stack Trace:");
			assertFailures.ForEach(x => WriteLine(string.Format(CultureInfo.InvariantCulture, "\n{0}", x.StackTrace)));
			PrintTestResults(false);
		}

		/// <summary>
		///     Adding log to specified test in testToLogDictionary.
		/// </summary>
		/// <param name="message">Message to add to test log.</param>
		private static void AddToTestToLogDictionary(string message)
		{
			List<string> testLog;
			lock (locker)
			{
				var testName = FullTestName;
				if (testName == null)
				{
					testToLogDictionary[NoTestContextToLog].Add(message);
					return;
				}

				if (testToLogDictionary.TryGetValue(testName, out testLog))
				{
					testLog.Add(message);
				}
				else
				{
					testToLogDictionary.Add(testName, new List<string> {message});
				}
			}
		}

		/// <summary>
		///     Adding log range to specified test in testToLogDictionary.
		/// </summary>
		/// <param name="messageList">Messages list to add to test log.</param>
		private static void AddRangeToTestToLogDictionary(List<string> messageList)
		{
			List<string> testLog;
			lock (locker)
			{
				var testName = FullTestName;
				if (testName == null)
				{
					testToLogDictionary[NoTestContextToLog].AddRange(messageList);
					return;
				}

				if (testToLogDictionary.TryGetValue(testName, out testLog))
				{
					testLog.AddRange(messageList);
				}
				else
				{
					testToLogDictionary.Add(testName, messageList);
				}
			}
		}

		/// <summary>
		///     The log test summary.
		/// </summary>
		/// <param name="type">
		///     The type.
		/// </param>
		/// <param name="testName">
		///     The test Name.
		/// </param>
		public static void LogTestSummary(Type type, string testName)
		{
			WriteLine(string.Empty);
			WriteLine("### Test Summary: ");
			try
			{
				var documentation = SummaryReader.XmlFromMember(type.GetMethod(testName));
				WriteLine(documentation["summary"].InnerText.Trim());
			}
			catch (Exception e)
			{
				WriteLine("An error occurred during summary reading of this test.");
				WriteLine(e.Message);
			}

			WriteLine(string.Empty);
		}
	}
}