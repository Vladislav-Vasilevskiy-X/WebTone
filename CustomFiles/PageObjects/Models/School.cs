namespace CustomFiles.PageObjects.Models
{
	/// <summary>
	/// School model.
	/// </summary>
	public class School
	{
		/// <summary>
		///		School's name.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		///		City/country where it's located.
		/// </summary>
		public string Address { get; set; }
		
		/// <summary>
		///		Contact phone of the school.
		/// </summary>
		public string ContactPhone { get; set; }
		
		/// <summary>
		///		Schedule. Time from-to school works.
		/// </summary>
		public string Schedule { get; set; }

		/// <summary>
		///		Site.
		/// </summary>
		public string Site { get; set; }

		/// <summary>
		///		Shool's mail.
		/// </summary>
		public string Mail { get; set; }
	}
}
