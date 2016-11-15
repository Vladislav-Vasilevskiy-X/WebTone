using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Core.GeneralUtils;
using Core.PageObjects.Scenarios.Generic.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.Objects;

namespace Core.PageObjects.Scenarios.Generic
{
	/// <summary>
	///     ButtonScenarios class.
	/// </summary>
	public class ButtonScenarios : Scenario
	{
		/// <summary>
		///     Verifies the button disabled.
		/// </summary>
		/// <typeparam name="TPageObject">The type of the T page object.</typeparam>
		/// <param name="pageObject">The page object.</param>
		/// <param name="buttonFlag">The button flag.</param>
		public void VerifyButtonDisabled<TPageObject>(TPageObject pageObject, ButtonFlag buttonFlag) where TPageObject : class
		{
			VerifyButtonEnabled(pageObject, buttonFlag, false);
		}

		/// <summary>
		///     Verifies the button enabled.
		/// </summary>
		/// <typeparam name="TPageObject">The type of the T page object.</typeparam>
		/// <param name="pageObject">The page object.</param>
		/// <param name="buttonFlag">The button flag.</param>
		public void VerifyButtonEnabled<TPageObject>(TPageObject pageObject, ButtonFlag buttonFlag) where TPageObject : class
		{
			VerifyButtonEnabled(pageObject, buttonFlag, true);
		}

		/// <summary>
		///     Verifies the button enabled/disabled.
		/// </summary>
		/// <typeparam name="TPageObject">The type of the T page object.</typeparam>
		/// <param name="pageObject">The page object.</param>
		/// <param name="buttonFlag">The button flag.</param>
		/// <param name="enabled">if set to <c>true</c> verify button enabled, <c>false</c> - disabled.</param>
		public void VerifyButtonEnabled<TPageObject>(TPageObject pageObject, ButtonFlag buttonFlag, bool enabled)
			where TPageObject : class
		{
			Verify.AreEqual(GetButton(pageObject, buttonFlag).Enabled, enabled);
		}

		/// <summary>
		///     Clicks the button.
		/// </summary>
		/// <typeparam name="TPageObject">The type of the T page object.</typeparam>
		/// <param name="buttonFlag">The button flag.</param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public void ClickButton<TPageObject>(ButtonFlag buttonFlag) where TPageObject : class, new()
		{
			GetButton(new TPageObject(), buttonFlag).ClickAndWaitAjax();
		}

		/// <summary>
		///     Clicks the button.
		/// </summary>
		/// <typeparam name="TPageObject">The type of the T page object.</typeparam>
		/// <param name="pageObject">The page object.</param>
		/// <param name="buttonFlag">The button flag.</param>
		public void ClickButton<TPageObject>(TPageObject pageObject, ButtonFlag buttonFlag) where TPageObject : class
		{
			GetButton(pageObject, buttonFlag).ClickAndWaitAjax();
		}

		/// <summary>
		///     Clicks the button.
		/// </summary>
		/// <typeparam name="TPageObject">The type of the T page object.</typeparam>
		/// <param name="buttonFlag">The button flag.</param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
		public void ClickButtonWithoutAjax<TPageObject>(ButtonFlag buttonFlag) where TPageObject : class, new()
		{
			GetButton(new TPageObject(), buttonFlag).Click();
		}

		/// <summary>
		///     Clicks the button without WaitAjax.
		/// </summary>
		/// <typeparam name="TPageObject">The type of the T page object.</typeparam>
		/// <param name="pageObject">The page object.</param>
		/// <param name="buttonFlag">The button flag.</param>
		public void ClickButtonWithoutAjax<TPageObject>(TPageObject pageObject, ButtonFlag buttonFlag)
			where TPageObject : class
		{
			GetButton(pageObject, buttonFlag).Click();
		}

		/// <summary>
		///     Gets the property of type <see cref="Button" /> with attribute <see cref="ButtonActionAttribute" />
		/// </summary>
		/// <param name="value">The page object.</param>
		/// <param name="buttonFlag">The ButtonFlag.</param>
		/// <returns>The found button element.</returns>
		public Button GetButton(object value, ButtonFlag buttonFlag)
		{
			return
				value.GetPropertyValuesWithFilteredAttribute<Button, ButtonActionAttribute>(x => x.ButtonFlag.Equals(buttonFlag))
					.First();
		}

		/// <summary>
		///     Clicks the button on Invoice list for filtering invoices by age.
		/// </summary>
		/// <typeparam name="TPageObject">The type of the T page object.</typeparam>
		/// <param name="pageObject">The page object.</param>
		/// <param name="filteringButtonFlag">The button flag.</param>
		public void ClickFilteringButton<TPageObject>(TPageObject pageObject, FilteringButtonFlag filteringButtonFlag)
			where TPageObject : class
		{
			GetFilteringButton(pageObject, filteringButtonFlag).ClickAndWaitAjax();
		}

		/// <summary>
		///     Gets the property of type <see cref="Button" /> with attribute <see cref="FilteringButtonActionAttribute" />
		/// </summary>
		/// <param name="value">The page object.</param>
		/// <param name="filteringButtonFlag">The FilteringButtonFlag.</param>
		/// <returns>The found button element.</returns>
		public Button GetFilteringButton(object value, FilteringButtonFlag filteringButtonFlag)
		{
			return
				value.GetPropertyValuesWithFilteredAttribute<Button, FilteringButtonActionAttribute>(
					x => x.FilteringButtonFlag.Equals(filteringButtonFlag)).First();
		}
	}
}