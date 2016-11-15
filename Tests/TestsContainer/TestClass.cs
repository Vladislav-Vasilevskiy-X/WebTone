using System;
using Core.SeleniumUtils.Core;
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
	}
}