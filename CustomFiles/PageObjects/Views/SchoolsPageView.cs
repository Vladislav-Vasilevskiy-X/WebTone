using System.Collections.Generic;
using Core.PageObjects.Scenarios.Generic.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.Objects;
using CustomFiles.PageObjects.Blocks;
using OpenQA.Selenium.Support.PageObjects;

namespace CustomFiles.PageObjects.Views
{
	public class SchoolsPageView : View
	{
		[FindsBy(How = How.Id, Using = "b2")]
		public Button MinskTab { get; set; }

		[PropertyStorageList]
		public IList<SchoolBlock> Schools { get; set; }
	}
}
