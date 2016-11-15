using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Core.GeneralUtils;
using Core.PageObjects.Scenarios.Generic.Attributes;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.Objects;
using Muyou.LinqToWindows.Extensions;

namespace Core.PageObjects.Scenarios.Generic
{
	/// <summary>
	///     LabelScenarios class.
	/// </summary>
	public class LabelScenarios : Scenario
	{
		/// <summary>
		///     Check if labels exist in the page and if exist, check its text value.
		/// </summary>
		/// <typeparam name="TView">Type of the view.</typeparam>
		public void CheckLabels<TView>() where TView : View
		{
			var pageObject = Resolve<TView>();
			CheckLabels(pageObject);
		}

		/// <summary>
		///     Check if labels exist in the page and if exist, check its text value.
		/// </summary>
		/// <param name="pageObject">The view or block object.</param>
		public void CheckLabels(object pageObject)
		{
			var labelProperties = pageObject.GetPublicPropertiesWithAttribute(typeof(LabelAttribute));
			foreach (var labelProperty in labelProperties)
			{
				var labelElement = labelProperty.GetValue(pageObject) as TextBlock;
				if (labelElement == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
						"The expected type of the label is {0} but actually it is {1}", typeof(TextBlock).Name,
						labelProperty.PropertyType.Name));
				}

				var labelAttribute = labelProperty.GetCustomAttribute<LabelAttribute>();
				var isLabelPresent = labelElement.DisplayedWithTimeout(TimeSpan.FromSeconds(3));
				Verify.IsTrue(isLabelPresent,
					string.Format(CultureInfo.InvariantCulture, "Element '{0}' not exist", labelElement.Name));
				if (isLabelPresent)
				{
					Verify.IsTrue(labelElement.Text.Replace("\u00A0", " ").Contains(labelAttribute.Text),
						string.Format(CultureInfo.InvariantCulture, "Actual label text '{0}' not contains expected text '{1}'.",
							labelElement.Text, labelAttribute.Text));
				}
			}

			// recursive check of inner blocks
			var blockProperties =
				pageObject.GetType().GetProperties().Where(prop => typeof(HtmlElement).IsAssignableFrom(prop.PropertyType)).ToList();
			if (!blockProperties.Any())
			{
				return;
			}

			blockProperties.Select(x => x.GetValue(pageObject, null)).ForEach(CheckLabels);
		}
	}
}