using System;
using Core.SeleniumUtils.Core;
using Core.SeleniumUtils.Core.Objects;

namespace Core.PageObjects.Scenarios
{
	/// <summary>
	///     The scroll scenarios.
	/// </summary>
	public class ScrollScenarios : Scenario
	{
		/// <summary>
		///     Scroll all page down.
		/// </summary>
		/// <param name="maxScrolling">Value of max scrolling which can do.</param>
		public void ScrollAllPageDown(int maxScrolling = 50)
		{
			var safeStopper = 0;

			while (IsScrollingAvailable() && safeStopper < maxScrolling)
			{
				Browser.ExecuteJavaScript("window.scrollTo(0, window.scrollMaxY);");
				safeStopper++;
			}
		}

		/// <summary>
		///     Scroll page up.
		/// </summary>
		public void ScrollPageUp()
		{
			Browser.ExecuteJavaScript("window.scrollTo(0, 0);");
		}

		/// <summary>
		///     Checks whether page can be scrolled down .
		/// </summary>
		/// <returns>
		///     The <see cref="bool" />.
		/// </returns>
		public bool IsScrollingAvailable()
		{
			var currentScrollPos = Browser.ExecuteJavaScript("return window.scrollY;") as long?;
			var maxScrollPos = Browser.ExecuteJavaScript("return window.scrollMaxY;") as long?;

			return currentScrollPos < maxScrollPos;
		}

		/// <summary>
		///     Scrolls the inner element down by elements class name.
		/// </summary>
		/// <param name="elementClassName">The class name grid identifier.</param>
		/// <param name="maxScrolling">Number of max allowed attempts to scroll.</param>
		public void ScrollInnerElementDownByClassName(string elementClassName, int maxScrolling)
		{
			Waiter.SpinWait(
				() =>
					(bool) Browser.ExecuteJavaScript(@"return document.getElementsByClassName('" + elementClassName + "')[0] != null"),
				TimeSpan.FromSeconds(Configurations.Timeout));
			var safeStopper = 0;
			long initialScrollHeight;

			do
			{
				initialScrollHeight =
					(long)
						Browser.ExecuteJavaScript(@"return document.getElementsByClassName('" + elementClassName + "')[0].scrollHeight");
				Browser.ExecuteJavaScript(@"var obj = document.getElementsByClassName('" + elementClassName + "')[0];" +
				                          "obj.scrollTop = obj.scrollHeight;");
				Waiter.SpinWait(
					() => (bool) Browser.ExecuteJavaScript(@"return document.getElementsByClassName('loadSpinnerIcon')[0] == null"),
					TimeSpan.FromSeconds(2));
				safeStopper++;
			} while (initialScrollHeight !=
			         (long)
				         Browser.ExecuteJavaScript(@"return document.getElementsByClassName('" + elementClassName +
				                                   "')[0].scrollHeight") && safeStopper < maxScrolling);
		}
	}
}