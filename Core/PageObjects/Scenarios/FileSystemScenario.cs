using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Core.SeleniumUtils.Core;
using Core.SeleniumUtils.Core.Objects;

namespace Core.PageObjects.Scenarios
{
	/// <summary>
	///     Methods for processing downloaded files.
	/// </summary>
	public class FileSystemScenario : Scenario
	{
		/// <summary>
		///     Method waits until file downloading completes.
		/// </summary>
		/// <param name="timeout">Timeout of file download in seconds.</param>
		public void WaitUntilAnyFileDownloaded(int timeout = 30)
		{
			var fileDownloaded = false;

			Thread.Sleep(1000);

			Waiter.SpinWait(
				() =>
				{
					var downloadDirectory = Browser.DownloadedFilesDirectory;
					if (downloadDirectory.Exists && !downloadDirectory.GetFiles().Any(f => f.Extension == ".part") &&
					    !downloadDirectory.GetFiles().Any(f => f.Length == 0))
					{
						fileDownloaded = true;
						return true;
					}

					return false;
				},
				TimeSpan.FromSeconds(timeout));

			if (!fileDownloaded)
			{
				throw new Exception(string.Format(CultureInfo.InvariantCulture, "File has not been downloaded for {0} seconds.",
					timeout));
			}
		}

		/// <summary>
		///     Determines whether file is used by another process.
		/// </summary>
		/// <param name="file">File Info.</param>
		/// <returns>
		///     State of file.
		/// </returns>
		private bool IsFileUnlocked(FileInfo file)
		{
			try
			{
				var fpath = file.FullName;
				var fs = File.OpenWrite(fpath);
				fs.Close();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		///     Method waits until exact file downloading completes.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <param name="timeout">Timeout of file download in seconds.</param>
		public void WaitUntilExactFileDownloaded(string fileName, int timeout = 30)
		{
			var fileDownloaded = false;

			Thread.Sleep(1000);

			Waiter.SpinWait(
				() =>
				{
					var downloadDirectory = Browser.DownloadedFilesDirectory;
					var lastFile = downloadDirectory.GetFiles().OrderBy(f => f.LastWriteTime).LastOrDefault();
					if (lastFile.Exists && IsFileUnlocked(lastFile) && lastFile.Name == fileName && lastFile.Extension != ".part")
					{
						fileDownloaded = true;
						return true;
					}

					return false;
				},
				TimeSpan.FromSeconds(timeout));

			if (!fileDownloaded)
			{
				throw new Exception(string.Format(CultureInfo.InvariantCulture, "File has not been downloaded for {0} seconds.",
					timeout));
			}
		}

		/// <summary>
		///     Method returns first file from directory with files downloaded from UI.
		/// </summary>
		/// <returns>Info of first file in downloaded files directory.</returns>
		public FileInfo GetFirstFileFromDownloadFilesDirectory()
		{
			return Browser.DownloadedFilesDirectory.GetFiles().First();
		}

		/// <summary>
		///     Method gets last downloaded file info.
		/// </summary>
		/// <returns>The FileInfo.</returns>
		public FileInfo GetLastDownloadedFile()
		{
			return Browser.DownloadedFilesDirectory.GetFiles().OrderBy(f => f.LastWriteTime).Last();
		}

		/// <summary>
		///     Method deletes DownloadFilesDirectory. .
		///     It's that complex because simple recursive deletion of directory sometimes throws System.IOException.
		/// </summary>
		public void DeleteDownloadDirectory()
		{
			var downloadFilesDirectory = Browser.DownloadedFilesDirectory;

			if (downloadFilesDirectory.Exists)
			{
				try
				{
					foreach (var file in downloadFilesDirectory.GetFiles().Select(f => f.FullName))
					{
						File.SetAttributes(file, FileAttributes.Normal);
						File.Delete(file);
					}

					foreach (var dir in downloadFilesDirectory.GetDirectories())
					{
						dir.Delete(true);
					}

					downloadFilesDirectory.Delete(true);
				}
				catch (Exception)
				{
					// ignored
				}
			}
		}
	}
}