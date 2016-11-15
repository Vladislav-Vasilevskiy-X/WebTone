using System;
using System.Collections.Generic;
using System.IO;
using Core.GeneralUtils.Container;
using Core.GeneralUtils.MSTest;
using Core.GeneralUtils.Verification;
using Core.SeleniumUtils;
using Core.SeleniumUtils.Core;
using Core.SeleniumUtils.Core.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.TestsContainer
{
	[TestClass]
	public abstract class BaseTest : UIInfrastructureObject
	{
		/// <summary>
		///     Collection of disposable items e.g. browser
		/// </summary>
		private static readonly IList<IDisposable> AssemblyRecycleBin = new List<IDisposable>();

		/// <summary>
		///     Gets or sets a value indicating whether quit browser after test.
		///     Default value is false.
		/// </summary>
		protected bool QuitBrowserAfterTest { get; set; }

		/// <summary>
		///     Disposable wrapper for the browser. All opened browsers are disposed(quit) after the tests are finished
		/// </summary>
		private class BrowserWrapper : IDisposable
		{
			#region Fields

			private readonly Browser browser;

			#endregion

			#region Constructors and Destructors

			public BrowserWrapper(Browser browser)
			{
				this.browser = browser;
			}

			#endregion

			#region Public Methods and Operators

			public void Dispose()
			{
				browser.Quit();
			}

			#endregion
		}

		#region Public Methods and Operators

		/// <summary>
		///     Actions executed after all tests
		/// </summary>
		[AssemblyCleanup]
		public static void AssemblyCleanup()
		{
			foreach (var disposable in AssemblyRecycleBin)
			{
				disposable.Dispose();
			}
		}

		/// <summary>
		///     Initialize anything before all tests here
		/// </summary>
		/// <param name="testContext">TestContext represeting test info.</param>
		[AssemblyInitialize]
		public static void AssemblyInitialize(TestContext testContext)
		{
			AssemblyUtilities.SetEntryAssembly();
			BootstrapperUI.Bootstrap();
		}

		[TestInitialize]
		public virtual void TestInitialize()
		{
			try
			{
				MSTestReportUtils.AddIssueToReport(TestContext);
				VerifyContext.Current = new Verify();
				VerifyContext.Current.VerifyFailed += OnVerifyFailed;

				Login();
				SetupCustomInitialize();
			}
			catch
			{
				MakeAndSaveScreenshot();
				throw;
			}
		}

		/// <summary>
		///     Actions which are executed after each test
		/// </summary>
		[TestCleanup]
		public void TestCleanup()
		{
			try
			{
				if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
				{
					MakeAndSaveScreenshot();
				}

				// execute SetupCustomTestCleanUp that can be overriden in derived test class
				SetupCustomTestCleanUp();
				VerifyContext.Current.VerifyFailed -= OnVerifyFailed;
				if (QuitBrowserAfterTest)
				{
					Browser.Quit();
				}
				else
				{
					//add browser to collection of disposable items which will be disposed after all tests will finished
					AssemblyRecycleBin.Add(new BrowserWrapper(Browser));
				}
			}
			finally
			{
				Verify.Check();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		///     Override in derived test class to use additional initialization actions
		/// </summary>
		protected virtual void SetupCustomInitialize()
		{
		}

		/// <summary>
		///     Override in derived test class to use additional cleanup actions
		/// </summary>
		protected virtual void SetupCustomTestCleanUp()
		{
		}

		private void OnVerifyFailed(UnitTestVerifyException obj)
		{
			MakeAndSaveScreenshot();
			TestContext.WriteLine(obj.Message);
			TestContext.WriteLine(obj.StackTrace);
		}

		/// <summary>
		///     The login.
		/// </summary>
		private void Login()
		{
		}

		/// <summary>
		///     Creates and saves screenshot.
		/// </summary>
		protected void MakeAndSaveScreenshot()
		{
			var path = Path.GetDirectoryName(TestContext.TestRunDirectory) + "\\ScreenShots\\";
			Directory.CreateDirectory(path);
			var sspath = path + TestContext.TestName + "_" + Guid.NewGuid() + ".jpeg";

			Logger.WriteLine("Test failed. Screenshot name: " + Path.GetFileName(sspath));
			Browser.SaveScreenshot(sspath);
		}

		#endregion
	}
}