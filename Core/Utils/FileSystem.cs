using System.IO;

namespace FTBSeleniumUtilities.Utils
{
	/// <summary>
	/// FileSystem class.
	/// </summary>
	public class FileSystem
	{
		/// <summary>
		/// Forces the delete.
		/// </summary>
		/// <param name="path">The path.</param>
		public void ForceDelete(string path)
		{
			if (!Directory.Exists(path))
			{
				return;
			}

			var baseFolder = new DirectoryInfo(path);

			foreach (var item in baseFolder.EnumerateDirectories("*", SearchOption.AllDirectories))
			{
				item.Attributes = this.ResetAttributes(item.Attributes);
			}

			foreach (var item in baseFolder.EnumerateFiles("*", SearchOption.AllDirectories))
			{
				item.Attributes = this.ResetAttributes(item.Attributes);
			}

			baseFolder.Delete(true);
		}

		/// <summary>
		/// Resets the attributes.
		/// </summary>
		/// <param name="attributes">The attributes.</param>
		/// <returns>The result.</returns>
		private FileAttributes ResetAttributes(FileAttributes attributes)
		{
			return attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
		}
	}
}