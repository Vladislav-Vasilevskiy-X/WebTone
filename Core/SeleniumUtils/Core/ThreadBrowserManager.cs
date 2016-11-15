using Container;
using Microsoft.Practices.Unity;

namespace Core
{
	/// <summary>
	/// Thread Browser manager.
	/// </summary>
	public class ThreadBrowserManager
	{
		private Browser browserForThread;

		/// <summary>
		/// Gets Browser instance per thread.
		/// </summary>
		/// <param name="pool">Pool of users.</param>
		/// <returns>The Browser.</returns>
		public Browser GetThreadBrowser(string pool)
		{
			if (this.browserForThread == null)
			{
				this.browserForThread = UnityBootstrapper.Container.Resolve<BrowserManager>().GetAvailableBrowser(pool);
			}

			return this.browserForThread;
		}

		/// <summary>
		/// Releases Browser instance per thread.
		/// </summary>
		/// <param name="pool">Pool of users.</param>
		public void Release(string pool)
		{
			UnityBootstrapper.Container.Resolve<BrowserManager>().ReleaseBrowser(pool, this.browserForThread);
			this.browserForThread = null;
		}
	}
}
