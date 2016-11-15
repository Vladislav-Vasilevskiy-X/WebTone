using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.GeneralUtils;
using Core.PageObjects.Views;
using Core.SeleniumUtils;
using Core.SeleniumUtils.Core;
using Core.SeleniumUtils.Core.HtmlElements.Elements;
using Core.SeleniumUtils.Core.Objects;
using MailKit.Search;

namespace Core.PageObjects.Scenarios
{
	/// <summary>
	///     PagingItem enum.
	/// </summary>
	public enum PagingItem
	{
		/// <summary>
		///     The Previous item.
		/// </summary>
		[StringValue("Previous")] Previous,

		/// <summary>
		///     The Next item.
		/// </summary>
		[StringValue("Next")] Next
	}

	/// <summary>
	///     GridScenarios class.
	/// </summary>
	public class GridScenarios : Scenario
	{
		/// <summary>
		///     Gets a value indicating whether is paging present.
		/// </summary>
		/// <value>The is paging present.</value>
		public bool IsPagingPresent
		{
			get { return new BaseGridView().PagingStatus.ChangeTimeout(TimeSpan.FromSeconds(5)).Displayed; }
		}

		/// <summary>
		///     Navigates by the page number.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <exception cref="System.ArgumentException">The page number  + pageNumber+ is not displayed.</exception>
		public void NavigateByPageNumber(int pageNumber)
		{
			var pagingView = new BaseGridView();
			if (pageNumber <= pagingView.PageNumbersButtons.Count)
			{
				for (var i = 0; i < pagingView.PageNumbersButtons.Count; i++)
				{
					var pagingButton = pagingView.PageNumbersButtons[i];
					if (pageNumber == i + 1)
					{
						pagingButton.Click();
						break;
					}
				}
			}
			else
			{
				throw new ArgumentException("The paginate button " + pageNumber + " is not displayed");
			}
		}

		/// <summary>
		///     Navigates to first page.
		/// </summary>
		public void NavigateToFirstPage()
		{
			var pagingView = new BaseGridView();

			if (pagingView.PageNumbersButtons.Any())
			{
				NavigateByPageNumber(1);
			}
		}

		/// <summary>
		///     Navigates to last page.
		/// </summary>
		public void NavigateToLastPage()
		{
			var pagingView = new BaseGridView();

			if (pagingView.PageNumbersButtons.Any())
			{
				NavigateByPageNumber(pagingView.PageNumbersButtons.Count);
			}
		}

		/// <summary>
		///     Navigates to <see cref="PagingItem" />.
		/// </summary>
		/// <param name="pagingItem">The paging item.</param>
		/// <exception cref="System.ArgumentException">The paginate button  + pagingItem.ToStringValue() +  is not displayed.</exception>
		public void NavigateToItem(PagingItem pagingItem)
		{
			GetPaginateTextButton(pagingItem).Click();
		}

		/// <summary>
		///     Gets the active page number.
		/// </summary>
		/// <returns>Active Page Number.</returns>
		public int GetActivePageNumber()
		{
			var numberAsText =
				new BaseGridView().PageNumbersButtons.Where(button => button.GetAttribute("class").Contains("current"))
					.Select(b => b.Text)
					.FirstOrDefault();
			return Convert.ToInt32(numberAsText, CultureInfo.InvariantCulture);
		}

		/// <summary>
		///     Determines whether paginate button disabled.
		/// </summary>
		/// <param name="pagingItem">The paging item.</param>
		/// <returns>The Flag.</returns>
		public bool IsPaginateTextButtonDisabled(PagingItem pagingItem)
		{
			return GetPaginateTextButton(pagingItem).GetAttribute("class").Contains("disabled");
		}

		/// <summary>
		///     Gets the paginate text button.
		/// </summary>
		/// <param name="pagingItem">The paging item.</param>
		/// <returns>The button.</returns>
		private Button GetPaginateTextButton(PagingItem pagingItem)
		{
			var pagingView = new BaseGridView();
			var foundButtons =
				pagingView.PageTextButtons.Where(button => button.Text.Equals(pagingItem.ToStringValue())).ToList();
			if (!foundButtons.Any())
			{
				throw new ArgumentException("The paginate button " + pagingItem.ToStringValue() + " is not displayed");
			}

			return foundButtons.First();
		}

		/// <summary>
		///     Sorts the grid by header.
		/// </summary>
		/// <param name="headerName">Name of the header.</param>
		/// <param name="sortingOrder">The sorting order.</param>
		/// <exception cref="System.ArgumentException">The ArgumentException.</exception>
		public void SortByHeader(string headerName, SortOrder sortingOrder)
		{
			var pagingView = new BaseGridView();
			IList<Button> headerButtons = new List<Button>();
			var headersAppeared = Waiter.SpinWait(
				() =>
				{
					headerButtons = pagingView.GridHeaders.Where(x => x.Text.Equals(headerName)).ToList();
					return headerButtons.Count > 0;
				},
				TimeSpan.FromSeconds(5));
			if (!headersAppeared)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Header with name {0} not exists",
					headerName));
			}

			var foundHeader = headerButtons.First();
			if (sortingOrder.Equals(SortOrder.Ascending))
			{
				while (foundHeader.GetAttribute("aria-sort") == null || !foundHeader.GetAttribute("aria-sort").Equals("ascending"))
				{
					foundHeader.Click();
				}
			}
			else
			{
				while (foundHeader.GetAttribute("aria-sort") == null || !foundHeader.GetAttribute("aria-sort").Equals("descending"))
				{
					foundHeader.Click();
				}
			}
		}

		/// <summary>
		///     Selects the entries amount to show.
		/// </summary>
		/// <param name="entriesAmount">The entries amount.</param>
		public void SelectEntriesAmountToShow(int entriesAmount)
		{
			var validAmountValues = new[] {10, 25, 50, 100};
			if (!validAmountValues.Contains(entriesAmount))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
					"You have entered invalid amount to show. The valid amounts are {0}.", string.Join(",", validAmountValues)));
			}

			var gridView = new BaseGridView();
			gridView.ShowEntriesSelect.SelectByValue(entriesAmount.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		///     Gets the total items count from bottom left corner.
		/// </summary>
		/// <returns>Total Items Count.</returns>
		public int GetTotalItemsCountFromBottomLeftCorner()
		{
			const int PositionInMessageOfTotalItemsCount = 5;
			return GetCountValuesFromBottomLeftCorner(PositionInMessageOfTotalItemsCount);
		}

		/// <summary>
		///     Gets the show entries message. For example 'Showing 1 to 25 of 36 entries' or .
		///     'Showing 1 to 25 of 33 entries (filtered from 35 total entries)'.
		/// </summary>
		/// <returns>The value.</returns>
		public string GetShowEntriesMessage()
		{
			return new BaseGridView().ShowEntriesMessage.Text.Trim();
		}

		/// <summary>
		///     Verifies the show entries message. For example 'Showing 1 to 25 of 36 entries' or .
		///     'Showing 1 to 25 of 33 entries (filtered from 35 total entries)'.
		/// </summary>
		/// <param name="firstPosition">The first position.</param>
		/// <param name="lastPosition">The last position.</param>
		/// <param name="totalItemsNumber">The total items number.</param>
		/// <param name="totalItemsWithoutFiltering">The total items without filtering.</param>
		public void VerifyShowEntriesMessage(int firstPosition, int lastPosition, int totalItemsNumber,
			int totalItemsWithoutFiltering = 0)
		{
			var expectedMessage = totalItemsWithoutFiltering == 0
				? string.Format(CultureInfo.InvariantCulture, "Showing {0} to {1} of {2} entries", firstPosition, lastPosition,
					totalItemsNumber)
				: string.Format(CultureInfo.InvariantCulture, "Showing {0} to {1} of {2} entries (filtered from {3} total entries)",
					firstPosition, lastPosition, totalItemsNumber, totalItemsWithoutFiltering);
			Verify.AreEqual(expectedMessage, GetShowEntriesMessage(), "Verify show entries message");
		}

		/// <summary>
		///     Gets the count values from bottom left corner.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>The value.</returns>
		private int GetCountValuesFromBottomLeftCorner(int position)
		{
			var gridView = new BaseGridView();
			var number = gridView.ShowEntriesMessage.Text.Split(' ').ElementAt(position);
			return Convert.ToInt32(number, CultureInfo.InvariantCulture);
		}

		/// <summary>
		///     Gets the item and stop when condition met.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="returnFunction">The return function.</param>
		/// <param name="conditionToMeet">The condition to meet.</param>
		/// <returns>The value.</returns>
		public T GetItemAndStopWhenConditionMet<T>(Func<T> returnFunction, Func<bool> conditionToMeet)
		{
			NavigateToFirstPage();

			do
			{
				if (conditionToMeet())
				{
					return returnFunction();
				}
			} while (IsPaginateTextButtonDisabled(PagingItem.Next));

			throw new ArgumentException("Search Condition in the grid was not met");
		}
	}
}