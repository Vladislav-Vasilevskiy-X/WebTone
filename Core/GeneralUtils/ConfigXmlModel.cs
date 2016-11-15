using System.Xml.Serialization;

namespace Core.GeneralUtils
{
	[XmlRoot("Config")]
	public class ConfigXmlModel
	{
		/// <summary>
		///     Gets or sets the browser.
		/// </summary>
		/// <value>The browser.</value>
		[XmlElement("Browser")]
		public string Browser { get; set; }
	}
}