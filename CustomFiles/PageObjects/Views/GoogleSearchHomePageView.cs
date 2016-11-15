using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.Objects;
using OpenQA.Selenium.Support.PageObjects;

namespace CustomFiles.PageObjects.Views
{
	public class GoogleSearchHomePageView : View
	{
		[FindsBy(How = How.CssSelector, Using = "input[name=btnI]")]
		public TextInput FeelingLucky { get; set; }

		[FindsBy(How = How.CssSelector, Using = "input[name=btnK]")]
		public TextInput GoogleSearch { get; set; }
	}
}