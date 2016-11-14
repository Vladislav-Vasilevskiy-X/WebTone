
using System;
using System.Linq;

using OpenQA.Selenium;

namespace Core.ErrorHandling
{
	/// <summary>
	/// Hadler for red fatal error.
	/// </summary>
	internal class FatalErrorHandler : BaseUIErrorHandler
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FatalErrorHandler"/> class.
		/// </summary>
		internal FatalErrorHandler()
			: base(TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(15), "Fatal error still displayed after attempts to refresh")
		{
		}

		/// <summary>
		/// Gets a value indicating whether error exists.
		/// </summary>
		/// <value>
		///   <c>true</c> if error exists; otherwise, <c>false</c>.
		/// </value>
		public override bool ErrorExists
		{
			get
			{
				return this.Browser.WebDriver.OriginalDriver.FindElements(By.CssSelector(".span16>h1")).FirstOrDefault(x => x.Text.Equals("Error!")) != null;
			}
		}

		/// <summary>
		/// Perform browser refresh.
		/// </summary>
		protected override void TryHandleError()
		{
			var currentPage = this.Browser.CurrentUrl;
			Console.WriteLine("Fatal Error appeared. Current link is: {0}", currentPage);
			this.Browser.Refresh();
			this.Browser.WaitAjax();
		}
	}
}