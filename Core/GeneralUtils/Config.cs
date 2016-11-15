using System.IO;
using System.Xml.Serialization;

namespace Core.GeneralUtils
{
	public static class Config
	{
		private const string fConfig = "Config.xml";

		private static readonly object LockObject = new object();

		private static ConfigXmlModel config;

		/// <summary>
		///     Gets the config.
		///     Deserializes TestConfig.xml into TestConfigXml object.
		/// </summary>
		/// <value>The get config.</value>
		public static ConfigXmlModel GetConfig
		{
			get
			{
				lock (LockObject)
				{
					if (config != null)
					{
						return config;
					}

					using (var reader = new StreamReader(fConfig))
					{
						var xmlSerializer = new XmlSerializer(typeof(ConfigXmlModel));
						config = (ConfigXmlModel) xmlSerializer.Deserialize(reader);
						return config;
					}
				}
			}
		}
	}
}