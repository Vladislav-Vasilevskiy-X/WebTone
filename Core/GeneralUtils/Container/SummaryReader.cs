using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Core.GeneralUtils.Container
{
	/// <summary>
	///     Helper class to read summary of tests.
	/// </summary>
	public static class SummaryReader
	{
		/// <summary>
		///     A cache used to remember Xml documentation for assemblies.
		/// </summary>
		private static readonly Dictionary<Assembly, XmlDocument> cache = new Dictionary<Assembly, XmlDocument>();

		/// <summary>
		///     A cache used to store failure exceptions for assembly lookups.
		/// </summary>
		private static readonly Dictionary<Assembly, Exception> failCache = new Dictionary<Assembly, Exception>();

		/// <summary>
		///     Reads the summary of a method.
		/// </summary>
		/// <param name="methodInfo">
		///     The MethodInfo.
		/// </param>
		/// <returns>
		///     The XML for the method.
		/// </returns>
		public static XmlElement XmlFromMember(MethodInfo methodInfo)
		{
			var parametersString = string.Empty;
			foreach (var parameterInfo in methodInfo.GetParameters())
			{
				if (parametersString.Length > 0)
				{
					parametersString += ",";
				}

				parametersString += parameterInfo.ParameterType.FullName;
			}

			// Remove “()” if parametersString is empty.
			if (parametersString.Length > 0)
			{
				return XmlFromName(methodInfo.DeclaringType, 'M', methodInfo.Name + "(" + parametersString + ")");
			}

			return XmlFromName(methodInfo.DeclaringType, 'M', methodInfo.Name);
		}

		/// <summary>
		///     Obtains the XML Element that describes a reflection element by searching the
		///     members for a member that has a name that describes the element.
		/// </summary>
		/// <param name="type">
		///     The type or parent type, used to fetch the assembly.
		/// </param>
		/// <param name="prefix">
		///     The prefix as seen in the name attribute in the documentation XML.
		/// </param>
		/// <param name="name">
		///     Full name qualifier for the element.
		/// </param>
		/// <returns>
		///     The Xml member that has a name that describes the specified reflection element.
		/// </returns>
		private static XmlElement XmlFromName(Type type, char prefix, string name)
		{
			string fullName;

			if (string.IsNullOrEmpty(name))
			{
				fullName = prefix + ":" + type.FullName;
			}
			else
			{
				fullName = prefix + ":" + type.FullName + "." + name;
			}

			var xmlDocument = XmlFromAssembly(type.Assembly);

			XmlElement matchedElement = null;

			foreach (XmlElement xmlElement in xmlDocument["doc"]["members"])
			{
				if (xmlElement.Attributes["name"].Value.Equals(fullName))
				{
					if (matchedElement != null)
					{
						throw new Exception("Multiple matches", null);
					}

					matchedElement = xmlElement;
				}
			}

			if (matchedElement == null)
			{
				throw new Exception("Could not find documentation for  element", null);
			}

			return matchedElement;
		}

		/// <summary>
		///     Obtains the documentation file for the specified assembly.
		/// </summary>
		/// <param name="assembly">
		///     The assembly to find the XML document for.
		/// </param>
		/// <returns>
		///     The XML document.
		/// </returns>
		/// <remarks>
		///     This version uses a cache to preserve the assemblies, so that
		///     the XML file is not loaded and parsed on every single lookup.
		/// </remarks>
		public static XmlDocument XmlFromAssembly(Assembly assembly)
		{
			if (failCache.ContainsKey(assembly))
			{
				throw failCache[assembly];
			}

			try
			{
				if (!cache.ContainsKey(assembly))
				{
					// load the docuemnt into the cache
					cache[assembly] = XmlFromAssemblyNonCached(assembly);
				}

				return cache[assembly];
			}
			catch (Exception exception)
			{
				failCache[assembly] = exception;
				throw exception;
			}
		}

		/// <summary>
		///     Loads and parses the documentation file for the specified assembly.
		/// </summary>
		/// <param name="assembly">The assembly to find the XML document for.</param>
		/// <returns>The XML document.</returns>
		private static XmlDocument XmlFromAssemblyNonCached(Assembly assembly)
		{
			var assemblyFilename = assembly.CodeBase;

			const string Prefix = "file:///";

			if (assemblyFilename.StartsWith(Prefix))
			{
				StreamReader streamReader;

				try
				{
					var pathXml = Path.ChangeExtension(assemblyFilename.Substring(Prefix.Length), ".xml");
					streamReader = new StreamReader(pathXml);
				}
				catch (FileNotFoundException exception)
				{
					throw new Exception(
						"XML documentation doesn't exist. Ensure it is produced when solution builds. This can be set at the properties of it.",
						exception);
				}

				var xmlDocument = new XmlDocument();
				xmlDocument.Load(streamReader);
				return xmlDocument;
			}

			throw new Exception("Could not ascertain assembly filename", null);
		}
	}
}