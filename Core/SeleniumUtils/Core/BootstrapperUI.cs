using Core.GeneralUtils.Container;
using Microsoft.Practices.Unity;

namespace Core.SeleniumUtils.Core
{
	/// <summary>
	///     Class to hold model mappings settings.
	/// </summary>
	public static class BootstrapperUI
	{
		/// <summary>
		///     Create maps for each elements. Required by automapper.
		/// </summary>
		public static void Bootstrap()
		{
			//UnityBootstrapper.Container.RegisterType<ThreadBrowserManager>(new PerThreadLifetimeManager());
			//UnityBootstrapper.Container.RegisterType<BrowserManager>(new ContainerControlledLifetimeManager());
			UnityBootstrapper.Container.RegisterType<Browser>(new ContainerControlledLifetimeManager());
		}
	}
}