using Container;
using Microsoft.Practices.Unity;

using ContainerControlledLifetimeManager = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;

namespace Core
{
	/// <summary>
	/// Class to hold model mappings settings.
	/// </summary>
	public static class BootstrapperUI
	{
		/// <summary>
		/// Create maps for each elements. Required by automapper.
		/// </summary>
		public static void Bootstrap()
		{
			UnityBootstrapper.Container.RegisterType<ThreadBrowserManager>(new PerThreadLifetimeManager());
			UnityBootstrapper.Container.RegisterType<BrowserManager>(new ContainerControlledLifetimeManager());
		}
	}
}