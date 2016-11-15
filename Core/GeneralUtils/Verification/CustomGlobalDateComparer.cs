using System;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;

namespace Core.GeneralUtils.Verification
{
	/// <summary>
	///     Global date custom comparer.
	/// </summary>
	public class CustomGlobalDateComparer : BaseTypeComparer
	{
		private readonly string globalDate = "GlobalDate";

		/// <summary>
		///     Initializes a new instance of the <see cref="CustomGlobalDateComparer" /> class.
		///     Global date comparer.
		/// </summary>
		public CustomGlobalDateComparer()
			: base(RootComparerFactory.GetRootComparer())
		{
		}

		/// <summary>
		///     Compare tool.
		/// </summary>
		/// <param name="parms">The result.</param>
		public override void CompareType(CompareParms parms)
		{
			if (parms.Object1.ToString() != parms.Object2.ToString())
			{
				AddDifference(parms);
			}
		}

		/// <summary>
		///     Is match.
		/// </summary>
		/// <param name="type1">The type 1.</param>
		/// <param name="type2">The type 2.</param>
		/// <returns>If they match or not.</returns>
		public override bool IsTypeMatch(Type type1, Type type2)
		{
			if (type1 != null && type2 != null)
			{
				return type1.Name == globalDate && type2.Name == globalDate;
			}

			return false;
		}
	}
}