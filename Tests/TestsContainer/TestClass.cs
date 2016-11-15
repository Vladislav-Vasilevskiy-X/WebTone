using System;
using System.Linq;
using Core.SeleniumUtils;
using Core.SeleniumUtils.Core;
using CustomFiles.PageObjects.Models;
using CustomFiles.PageObjects.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.TestsContainer
{
	[TestClass]
	public class TestClass : BaseTest
	{
		[TestMethod]
		public void DefaultGoogleSearchPageInputsTextTest()
		{
			// Arrange
			Browser.Open(new Uri("http://www.google.com/ncr"));

			// Act
			Waiter.SpinWait(() => new GoogleSearchHomePageView().FeelingLucky.Displayed, TimeSpan.FromSeconds(20));
			var view = new GoogleSearchHomePageView();

			// Assert
			Verify.AreEqual("I'm Feeling Lucky", view.FeelingLucky.Text);
			Verify.AreEqual("Google Search", view.GoogleSearch.Text);
		}

		[TestMethod]
		public void GetSchoolsToJsonFile()
		{
			// Arrange
			Browser.Open(new Uri("http://mogu.by/obrazovanie/minsk/srednie-shkoli/srednie-shkoli1.htm"));
			this.Browser.WaitAjax();
			Waiter.SpinWait(() => new SchoolsPageView().MinskTab.Displayed, TimeSpan.FromSeconds(20));
			
			// Act
			var models = this.GenericScenarios.GetGridModels<SchoolsPageView, School>();
			Verify.IsNotNull(models);
		}
	}
}