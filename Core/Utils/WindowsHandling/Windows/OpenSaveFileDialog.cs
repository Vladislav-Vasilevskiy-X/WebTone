
using System.Linq;

using Muyou.LinqToWindows;
using Muyou.LinqToWindows.Windows.Types.Dialogs;

namespace WindowsHandling.Windows
{
	/// <summary>
	/// OpenSaveFileDialog class.
	/// </summary>
	public class OpenSaveFileDialog : ClosingWindow
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OpenSaveFileDialog" /> class.
		/// </summary>
		public OpenSaveFileDialog() : base("Opening", "Save As", "File Upload", "Open")
		{
		}

		/// <summary>
		/// Closes this instance.
		/// </summary>
		public override void Close()
		{
			WinAPIUtils.CloseWindowStartsWith(this.GetPossibleName());
		}

		/// <summary>
		/// Selects the file.
		/// </summary>
		/// <param name="fullPath">The full path.</param>
		public void SelectFile(string fullPath)
		{
			var shell = new Shell();
			var windowPtr = WinAPIUtils.WaitForWindow(this.GetPossibleName());
			var openDialog = shell.Windows.Where(x => x.WindowHandle.Equals(windowPtr)).Cast<Dialog>().Single().Cast<OpenFileDialog>();
			openDialog.SelectFile(fullPath);
		}

		/// <summary>
		/// Selects the file from test data.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		public void SelectFileFromTestData(string fileName)
		{
			var fullPath = PathUtils.GetTestDataResourcePath(fileName);
			this.SelectFile(fullPath);
		}
	}
}