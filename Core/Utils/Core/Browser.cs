
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using Container;
using Core.Objects;
using WindowsHandling;
using Core.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Core
{
	/// <summary>
	/// Browser class.
	/// </summary>
	public class Browser : UIInfrastructureObject, IDisposable
	{
		private static readonly object BrowserInitializeLockObject = new object();
		private IntPtr browserProcessHandle = default(IntPtr);

		#region Fields

		private DriverService driverService;

		private AjaxWebDriver webDriver;

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the current browser Url.
		/// </summary>
		/// <value>The current URL.</value>
		public string CurrentUrl
		{
			get
			{
				return this.WebDriver.Url;
			}
		}

		/// <summary>
		/// Gets Directory where Browser will download the files from UI.
		/// </summary>
		/// <value>The downloaded files directory.</value>
		public DirectoryInfo DownloadedFilesDirectory
		{
			get
			{
				return new DirectoryInfo(Environment.CurrentDirectory + @"\DownloadedFiles");
			}
		}

		/// <summary>
		/// Gets Reference to the WebDriver.
		/// </summary>
		/// <value>The web driver.</value>
		public AjaxWebDriver WebDriver
		{
			get
			{
				if (this.webDriver == null)
				{
					this.webDriver = new AjaxWebDriver(this.Start());
				}

				return this.webDriver;
			}
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Accepts the confirmation safely.
		/// </summary>
		public void AcceptConfirmation()
		{
			try
			{
				this.WebDriver.SwitchTo().Alert().Accept();
			}
			catch (InvalidOperationException e)
			{
				Logger.WriteLine("Error occurred while closing of modal dialog: " + e.Message);
			}
		}

		/// <summary>
		/// This method for verification if an alert message is present.
		/// </summary>
		/// <returns>This will return true if an alert message is on the screen. </returns>
		public bool IsAlertPresent()
		{
			try
			{
				this.WebDriver.SwitchTo().Alert();
				return true;
			}
			catch (NoAlertPresentException)
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the alert message.
		/// </summary>
		/// <returns>The alert message.</returns>
		public string GetAlertMessage()
		{
			Waiter.SpinWaitEnsureSatisfied(this.IsAlertPresent, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1), "Alert not appeared");
			return this.WebDriver.SwitchTo().Alert().Text;
		}

		/// <summary>
		/// Close browser.
		/// </summary>
		public void Close()
		{
			if (this.webDriver == null)
			{
				return;
			}

			this.webDriver.Close();
			this.webDriver = null;
		}

		/// <summary>
		/// Dismisses the confirmation.
		/// </summary>
		public void DismissConfirmation()
		{
			this.WebDriver.SwitchTo().Alert().Dismiss();
		}

		/// <summary>
		/// Wrapper for <see cref="IJavaScriptExecutor.ExecuteScript" />.
		/// </summary>
		/// <param name="javaScript">JS string.</param>
		/// <param name="args">The IWebelements.</param>
		/// <returns>The result of the script executing.</returns>
		public object ExecuteJavaScript(string javaScript, params object[] args)
		{
			return this.WebDriver.ExecuteScript(javaScript, args);
		}

		/// <summary>
		/// Initializes an instance of an <see cref="IWebDriver" /> for usage with Chrome.
		/// </summary>
		/// <returns>The IWebDriver.</returns>
		public IWebDriver InitializeChromeDriver()
		{
			this.driverService = ChromeDriverService.CreateDefaultService(".");
			this.driverService.Start();
			return new RemoteWebDriver(this.driverService.ServiceUrl, DesiredCapabilities.Chrome());
		}

		/// <summary>
		/// Initializes an instance of an <see cref="IWebDriver" /> for usage with Internet Explorer.
		/// </summary>
		/// <returns>The IWebDriver.</returns>
		public IWebDriver InitializeInternetExplorerDriver()
		{
			// var path = "D:\\Cobalt QED Testing\\ExampleTestSuites\\SeleniumNetTests\\SeleniumNetTests\\Driver\\"; 
			this.driverService = InternetExplorerDriverService.CreateDefaultService("..\\..\\Driver");
			this.driverService.Start();
			return new RemoteWebDriver(this.driverService.ServiceUrl, DesiredCapabilities.InternetExplorer());
		}

		/// <summary>
		/// Naviget to url.
		/// </summary>
		/// <param name="url">The URL.</param>
		public void Open(Uri url)
		{
			this.WebDriver.Navigate().GoToUrl(url);
		}

		/// <summary>
		/// Quit browser.
		/// </summary>
		public void Quit()
		{
			if (this.webDriver == null)
			{
				return;
			}

			this.webDriver.Quit();
			this.webDriver = null;

			if (this.driverService != null)
			{
				this.driverService.Dispose();
			}
		}

		/// <summary>
		/// Refreshes this instance.
		/// </summary>
		public void Refresh()
		{
			this.WebDriver.Navigate().Refresh();
		}

		/// <summary>
		/// Save the screenshot from browser.
		/// </summary>
		/// <param name="path">The path.</param>
		public void SaveScreenshot(string path)
		{
			this.WaitReadyState();

			this.WebDriver.GetScreenshot().SaveAsFile(path, ImageFormat.Jpeg);
		}

		/// <summary>
		/// Brings to foreground.
		/// </summary>
		public void BringToForeground()
		{
			this.WaitReadyState();

			// withdrawing current browser window to foreground
			// launching notepad because browser window does not go to foreground if other browser window is already in foreground
			Logger.WriteLine("Making full display screenshot...");
			var notepad = Process.Start("notepad.exe");
			Logger.WriteLine("Notepad to foreground...");
			WinAPIUtils.ForceForegroundWindow(notepad.MainWindowHandle);
			Logger.WriteLine("Browser to foreground...");
			WinAPIUtils.ForceForegroundWindow(this.browserProcessHandle);
			Logger.WriteLine("Kill notepad...");
			notepad.Kill();
		}

		/// <summary>
		/// Wait for AJAX requests to be finished.
		/// </summary>
		/// <param name="assert">The Assert.</param>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "WaitAjax")]
		public void WaitAjax(bool assert = true)
		{
			var ready = new Func<bool>(() => (bool)this.ExecuteJavaScript("return (typeof($) === 'undefined') ? true : !$.active;"));

			var ajaxExecutedSuccessfully = Waiter.SpinWait(ready, TimeSpan.FromSeconds(90), TimeSpan.FromMilliseconds(100));
			const string ErrorMessage = "Browser.WaitAjax() failed!";
			
			if (!ajaxExecutedSuccessfully)
			{
				this.TestContext.WriteLine(ErrorMessage);
			}

			if (assert)
			{
				Assert.IsTrue(ajaxExecutedSuccessfully, ErrorMessage);
			}
		}

		/// <summary>
		/// Wait for javascript readyState = complete.
		/// </summary>
		public void WaitReadyState()
		{
			var ready = new Func<bool>(() => (bool)this.ExecuteJavaScript("return document.readyState == 'complete'"));

			Assert.IsTrue(Waiter.SpinWait(ready, TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(100)), "Browser.WaitReadyState() failed.");
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initializes an instance of an <see cref="IWebDriver" /> for usage with Firefox.
		/// </summary>
		/// <returns>The FirefoxDriver.</returns>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		private FirefoxDriver InitializeFirefoxDriver()
		{
			var profile = new FirefoxProfile { AcceptUntrustedCertificates = true, EnableNativeEvents = true };

			profile.SetPreference("browser.helperApps.alwaysAsk.force", false);
			profile.SetPreference("browser.download.folderList", 2);
			profile.SetPreference("browser.download.dir", this.DownloadedFilesDirectory.FullName);
			profile.SetPreference("services.sync.prefs.sync.browser.download.manager.showWhenStarting", false);
			profile.SetPreference("browser.download.useDownloadDir", true);
			profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/octet-stream,text/plain,text/csv");

			FirefoxDriver browser;
			lock (BrowserInitializeLockObject)
			{
				var firefoxInstancesBefore = Process.GetProcessesByName("firefox");
				browser = new FirefoxDriver(new FirefoxBinary(), profile, TimeSpan.FromSeconds(Configurations.WebDriverCommandTimeout));
				var firefoxInstancesAfter = Process.GetProcessesByName("firefox");
				var newBrowserInstance = firefoxInstancesAfter.Except(firefoxInstancesBefore).First();
				this.browserProcessHandle = newBrowserInstance.MainWindowHandle;
			}

			return browser;
		}

		/// <summary>
		/// Initializes an instance of an <see cref="IWebDriver" /> for usage with Firefox.
		/// </summary>
		/// <returns>The FirefoxDriver.</returns>
		private FirefoxDriver InitializeMobileFirefoxDriver()
		{
			var profile = new FirefoxProfile { AcceptUntrustedCertificates = true, EnableNativeEvents = true };
			profile.SetPreference("general.useragent.override", "Mozilla/5.0 (iPhone; U; CPU iPhone OS 3_0 like Mac OS X; en-us) AppleWebKit/528.18 (KHTML, like Gecko) Version/4.0 Mobile/7A341 Safari/528.16");
			profile.SetPreference("browser.helperApps.alwaysAsk.force", false);
			profile.SetPreference("browser.download.folderList", 2);
			profile.SetPreference("browser.download.dir", this.DownloadedFilesDirectory.FullName);
			profile.SetPreference("services.sync.prefs.sync.browser.download.manager.showWhenStarting", false);
			profile.SetPreference("browser.download.useDownloadDir", true);
			return new FirefoxDriver(new FirefoxBinary(), profile, TimeSpan.FromSeconds(Configurations.WebDriverCommandTimeout));
		}

		/// <summary>
		/// Initialize webdriver instance.
		/// </summary>
		/// <returns>webdriver instance.</returns>
		private IWebDriver Start()
		{
			IWebDriver driver;
			switch (Config.GetConfig.Browser)
			{
				case EnvironmentBrowser.InternetExplorer:
					driver = this.InitializeInternetExplorerDriver();
					break;
				case EnvironmentBrowser.FirefoxAurora:
				case EnvironmentBrowser.FirefoxBeta:
				case EnvironmentBrowser.Firefox:
					driver = this.InitializeFirefoxDriver();
					break;
				case EnvironmentBrowser.ChromeCanary:
				case EnvironmentBrowser.Chrome:
					driver = this.InitializeChromeDriver();
					break;
				case EnvironmentBrowser.Safari:
					driver = this.InitializeChromeDriver();
					break;
				case EnvironmentBrowser.FirefoxMobile:
					driver = this.InitializeMobileFirefoxDriver();
					break;
				default:
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unexpected browser received: {0}.", Config.GetConfig.Browser));
			}

			driver.Manage().Window.Maximize();
			driver.Manage().Cookies.DeleteAllCookies();
			return driver;
		}

		#endregion

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize((object)this);
		}

		/// <summary>
		/// Disposes the specified disposing.
		/// </summary>
		/// <param name="disposing">The disposing.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			this.webDriver.Dispose();
		}

		/// <summary>
		/// Performs browser back navigation.
		/// </summary>
		public void BackNavigation()
		{
			this.WebDriver.Navigate().Back();
		}
	}
}