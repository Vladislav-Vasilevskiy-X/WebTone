
using System;

namespace Core.HtmlElements.Attributes
{
	/// <summary>
	/// NameAttribute class.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
	public sealed class NameAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NameAttribute" /> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public NameAttribute(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }
	}
}