using System;
using System.Reflection;
using Core.GeneralUtils.TestDataHandling;

namespace Core.SeleniumUtils
{
	/// <summary>
	///     Assembly Utilities.
	/// </summary>
	public static class AssemblyUtilities
	{
		/// <summary>
		///     Use as first line in tests.
		/// </summary>
		public static void SetEntryAssembly()
		{
			SetEntryAssembly(Assembly.GetCallingAssembly());
		}

		/// <summary>
		///     Allows setting the Entry Assembly when needed, for examle <see cref="Assembly.GetEntryAssembly" /> is used in
		///     <see cref="JsonTestDataReader" />.
		///     Use AssemblyUtilities.SetEntryAssembly() as first line in tests.
		/// </summary>
		/// <param name="assembly">Assembly to set as entry assembly.</param>
		public static void SetEntryAssembly(Assembly assembly)
		{
			var manager = new AppDomainManager();
			var entryAssemblyfield = manager.GetType()
				.GetField("m_entryAssembly", BindingFlags.Instance | BindingFlags.NonPublic);
			entryAssemblyfield.SetValue(manager, assembly);

			var domain = AppDomain.CurrentDomain;
			var domainManagerField = domain.GetType().GetField("_domainManager", BindingFlags.Instance | BindingFlags.NonPublic);
			domainManagerField.SetValue(domain, manager);
		}
	}
}