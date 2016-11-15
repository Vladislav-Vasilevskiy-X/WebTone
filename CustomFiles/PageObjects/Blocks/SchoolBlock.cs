using Core.PageObjects.Scenarios.Generic.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using OpenQA.Selenium.Support.PageObjects;

namespace CustomFiles.PageObjects.Blocks
{
	[Block(How = How.CssSelector, Using = ".lists .org .orgcontent")]
	public class SchoolBlock : HtmlElement
	{
		[ModelPropertyName("Name")]
		[FindsBy(How = How.CssSelector, Using = "div:nth-child(1)")]
		public TextBlock SchoolName { get; set; }

		[ModelPropertyName("Address")]
		[FindsBy(How = How.CssSelector, Using = "div:nth-child(2)")]
		public TextBlock SchoolAddress { get; set; }

		[ModelPropertyName("ContactPhone")]
		[FindsBy(How = How.CssSelector, Using = "div:nth-child(3)")]
		public TextBlock SchoolContactPhone { get; set; }

		[ModelPropertyName("Schedule")]
		[FindsBy(How = How.CssSelector, Using = "div:nth-child(4)")]
		public TextBlock SchoolSchedule { get; set; }

		[ModelPropertyName("Site")]
		[FindsBy(How = How.CssSelector, Using = "div.org_web_main noindex:nth-child(1) a")]
		public Link SchoolSite { get; set; }

		[ModelPropertyName("Mail")]
		[FindsBy(How = How.CssSelector, Using = "div.org_web_main noindex:nth-child(2) a")]
		public Link SchoolMail { get; set; }
	}
}
