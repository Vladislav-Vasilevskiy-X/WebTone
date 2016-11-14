
using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core
{
	/// <summary>
	/// Browser manager.
	/// </summary>
	public class BrowserManager
	{
		private readonly ConcurrentDictionary<string, Browser> availableBrowsers;

		private static readonly object fileDownloadTestLockObject = new object();

		private static readonly object locker = new object();

		/// <summary>
		/// Gets lock object for File Download Tests.
		/// </summary>
		/// <value>The file download test lock object.</value>
		public object FileDownloadTestLockObject
		{
			get
			{
				return fileDownloadTestLockObject;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether a test with [FileDownloadTest] is in progress.
		/// </summary>
		/// <value>The is file download test in progress.</value>
		public bool IsFileDownloadTestInProgress { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BrowserManager" /> class.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
		public BrowserManager()
		{
			this.availableBrowsers = new ConcurrentDictionary<string, Browser>();
			var usersPools = TestConfig.GetConfig.Users;
			foreach (var usersPool in usersPools)
			{
				this.availableBrowsers.TryAdd(usersPool.Pool, new Browser());
			}
		}

		/// <summary>
		/// Returns available Browser instance.
		/// </summary>
		/// <param name="pool">Pool of users.</param>
		/// <returns>Browser instance.</returns>
		public Browser GetAvailableBrowser(string pool)
		{
			lock (locker)
			{
				Browser result;

				if (!this.availableBrowsers.TryRemove(pool, out result))
				{
					Assert.Fail("Failed to unload browser.");
				}

				return result;
			}
		}

		/// <summary>
		/// Releases Browser instance.
		/// </summary>
		/// <param name="userHandler">The user handler.</param>
		/// <param name="browser">Browser instance.</param>
		public void ReleaseBrowser(string userHandler, Browser browser)
		{
			lock (locker)
			{
				this.availableBrowsers.TryAdd(userHandler, browser);
			}
		}
	}
}