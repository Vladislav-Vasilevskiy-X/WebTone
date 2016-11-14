using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using Container;
using Extensions;
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Verification
{
	/// <summary>
	/// Class for verification.
	/// </summary>
	[Serializable]
	public class Verify
	{
		private readonly List<UnitTestVerifyException> exceptions = new List<UnitTestVerifyException>();

		/// <summary>
		/// Gets or sets the verify failed.
		/// </summary>
		/// <value>The verify failed.</value>
		public Action<UnitTestVerifyException> VerifyFailed { get; set; }

		/// <summary>
		/// Gets a value indicating whether exceptions exist.
		/// </summary>
		/// <value>The has fails.</value>
		public bool HasFails
		{
			get
			{
				return this.exceptions.Count != 0;
			}
		}

		/// <summary>
		/// Arethe different objects equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="membersToIgnore">The members to ignore.</param>
		/// <param name="ignoreOrder">The ignore order.</param>
		/// <param name="errorMessage">The error message.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Verification.Verify.IsTrue(System.Boolean,System.String)")]
		public void AreDifferentObjectsEqual(object expected, object actual, IList<string> membersToIgnore = null, bool ignoreOrder = true, string errorMessage = "")
		{
			var config = new ComparisonConfig { MaxDifferences = 100, IgnoreObjectTypes = true, IgnoreCollectionOrder = ignoreOrder, CustomComparers = new List<BaseTypeComparer> { new CustomGlobalDateComparer() } };

			if (membersToIgnore != null)
			{
				config.MembersToIgnore = membersToIgnore.ToList();
			}

			var compareLogic = new CompareLogic { Config = config };
			var result = compareLogic.Compare(expected, actual);
			this.IsTrue(result.AreEqual, errorMessage + ": " + result.DifferencesString);
		}

		/// <summary>
		/// Are the collections equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreOrder">The ignore order.</param>
		/// <param name="errorMessage">The error message.</param>
		/// <param name="membersToIgnore">The members to ignore.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Verification.Verify.IsTrue(System.Boolean,System.String)")]
		public void AreCollectionsEqual<T>(IList<T> expected, IList<T> actual, bool ignoreOrder = true, string errorMessage = "", IList<string> membersToIgnore = null)
		{
			var config = new ComparisonConfig { MaxDifferences = 100, IgnoreCollectionOrder = ignoreOrder, CustomComparers = new List<BaseTypeComparer> { new CustomGlobalDateComparer() } };

			if (membersToIgnore != null)
			{
				config.MembersToIgnore = membersToIgnore.ToList();
			}

			var compareLogic = new CompareLogic { Config = config };
			var result = compareLogic.Compare(expected, actual);
			this.IsTrue(result.AreEqual, errorMessage + ": " + result.DifferencesString);
		}

		/// <summary>
		/// Verify is the collection contains values from another collection.
		/// </summary>
		/// <typeparam name="T">The Type of collections.</typeparam>
		/// <param name="expected">The Collection with expected values.</param>
		/// <param name="actual">The Collection that must contains expected values.</param>
		/// <param name="errorMessage">The error message.</param>
		public void CollectionContains<T>(IList<T> expected, IList<T> actual, string errorMessage = "")
		{
			var errors = new StringBuilder();

			foreach (var value in expected.Where(value => !actual.Contains(value)))
			{
				errors.AppendFormat(CultureInfo.InvariantCulture, "The following value is missing: {0}", value);
				errors.AppendLine();
			}

			this.AreEqual(0, errors.Length, errors.AppendFormat(CultureInfo.InvariantCulture, errorMessage).AppendLine().AppendFormat(CultureInfo.InvariantCulture, "Actual values are:").AppendLine().AppendFormat(CultureInfo.InvariantCulture, string.Join("\n", actual.ToArray())).ToString());
		}

		/// <summary>
		/// Verify <see cref="actualValue"/> contains <see cref="expectedStringSegment"/>.
		/// </summary>
		/// <param name="expectedStringSegment">The expected string segment.</param>
		/// <param name="actualValue">The actual string.</param>
		/// <param name="errorMessage">The error message.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Verification.Verify.IsTrue(System.Boolean,System.String)")]
		public void StringContains(object expectedStringSegment, string actualValue, string errorMessage = "")
		{
			this.IsTrue(actualValue.Contains(expectedStringSegment.ToString()), errorMessage + " The actual string '" + actualValue + "' not contains expected string segment '" + expectedStringSegment + "'");
		}

		/// <summary>
		/// Determines whether [is collection empty] [the specified actual].
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="actual">The actual.</param>
		/// <param name="errorMessage">The error message.</param>
		public void IsCollectionEmpty<T>(ICollection<T> actual, string errorMessage = "")
		{
			this.IsNotNull(actual, errorMessage);
			this.AreEqual(0, actual.Count, errorMessage);
		}

		/// <summary>
		/// Determines whether [is collection not empty] [the specified actual].
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="actual">The actual.</param>
		/// <param name="errorMessage">The error message.</param>
		public void IsCollectionNotEmpty<T>(ICollection<T> actual, string errorMessage = "")
		{
			this.IsNotNull(actual, errorMessage);
			this.AreNotEqual(0, actual.Count, errorMessage);
		}

		/// <summary>
		/// Determines whether the specified condition is true.
		/// </summary>
		/// <param name="condition">The condition.</param>
		public void IsTrue(bool condition)
		{
			this.IsTrue(condition, string.Empty, null);
		}

		/// <summary>
		/// Determines whether the specified condition is true.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="message">The message.</param>
		public void IsTrue(bool condition, string message)
		{
			this.IsTrue(condition, message, null);
		}

		/// <summary>
		/// Determines whether the specified condition is true.
		/// </summary>
		/// <param name="condition">The condition.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void IsTrue(bool condition, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.IsTrue(condition, message, parameters));
		}

		/// <summary>
		/// Are the equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		public void AreEqual<T>(T expected, T actual)
		{
			this.AreEqual(expected, actual, string.Empty, null);
		}

		/// <summary>
		/// Are the equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		public void AreEqual<T>(T expected, T actual, string message)
		{
			this.AreEqual(expected, actual, message, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual<T>(T expected, T actual, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreEqual(expected, actual, message, parameters));
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		public void AreEqual(object expected, object actual)
		{
			this.AreEqual(expected, actual, string.Empty, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(object expected, object actual, string message)
		{
			this.AreEqual(expected, actual, message, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(object expected, object actual, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreEqual(expected, actual, message, parameters));
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		public void AreEqual(double expected, double actual, double delta)
		{
			this.AreEqual(expected, actual, delta, string.Empty, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(double expected, double actual, double delta, string message)
		{
			this.AreEqual(expected, actual, delta, message, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(double expected, double actual, double delta, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreEqual(expected, actual, delta, message, parameters));
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		public void AreEqual(float expected, float actual, float delta)
		{
			this.AreEqual(expected, actual, delta, string.Empty, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(float expected, float actual, float delta, string message)
		{
			this.AreEqual(expected, actual, delta, message, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(float expected, float actual, float delta, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreEqual(expected, actual, delta, message, parameters));
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase)
		{
			this.AreEqual(expected, actual, ignoreCase, string.Empty, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, string message)
		{
			this.AreEqual(expected, actual, ignoreCase, message, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreEqual(expected, actual, ignoreCase, message, parameters));
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture)
		{
			this.AreEqual(expected, actual, ignoreCase, culture, string.Empty, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="message">The message.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message)
		{
			this.AreEqual(expected, actual, ignoreCase, culture, message, null);
		}

		/// <summary>
		/// Arethe equal.
		/// </summary>
		/// <param name="expected">The expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreEqual(expected, actual, ignoreCase, culture, message, parameters));
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		public void AreNotEqual<T>(T notExpected, T actual)
		{
			this.AreNotEqual(notExpected, actual, string.Empty, null);
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual<T>(T notExpected, T actual, string message)
		{
			this.AreNotEqual(notExpected, actual, message, null);
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <typeparam name="T">The type of the T.</typeparam>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual<T>(T notExpected, T actual, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, message, parameters));
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		public void AreNotEqual(object notExpected, object actual)
		{
			this.AreNotEqual(notExpected, actual, string.Empty, null);
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(object notExpected, object actual, string message)
		{
			this.AreNotEqual(notExpected, actual, message, null);
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(object notExpected, object actual, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, message, parameters));
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		public void AreNotEqual(double notExpected, double actual, double delta)
		{
			this.AreNotEqual(notExpected, actual, delta, string.Empty, null);
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(double notExpected, double actual, double delta, string message)
		{
			this.AreNotEqual(notExpected, actual, delta, message, null);
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(double notExpected, double actual, double delta, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, delta, message, parameters));
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		public void AreNotEqual(float notExpected, float actual, float delta)
		{
			this.AreNotEqual(notExpected, actual, delta, string.Empty, null);
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(float notExpected, float actual, float delta, string message)
		{
			this.AreNotEqual(notExpected, actual, delta, message, null);
		}

		/// <summary>
		/// Arethe not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="delta">The delta.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(float notExpected, float actual, float delta, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, delta, message, parameters));
		}

		/// <summary>
		/// Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase)
		{
			this.AreNotEqual(notExpected, actual, ignoreCase, string.Empty, null);
		}

		/// <summary>
		/// Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message)
		{
			this.AreNotEqual(notExpected, actual, ignoreCase, message, null);
		}

		/// <summary>
		/// Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, ignoreCase, message, parameters));
		}

		/// <summary>
		/// Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture)
		{
			this.AreNotEqual(notExpected, actual, ignoreCase, culture, string.Empty, null);
		}

		/// <summary>
		/// Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="message">The message.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message)
		{
			this.AreNotEqual(notExpected, actual, ignoreCase, culture, message, null);
		}

		/// <summary>
		/// Are the not equal.
		/// </summary>
		/// <param name="notExpected">The not expected.</param>
		/// <param name="actual">The actual.</param>
		/// <param name="ignoreCase">The ignore case.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="message">The message.</param>
		/// <param name="parameters">The parameters.</param>
		public void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreNotEqual(notExpected, actual, ignoreCase, culture, message, parameters));
		}

		/// <summary>
		/// Checks objects are not the same.
		/// </summary>
		/// <param name="notExpected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		public void AreNotSame(object notExpected, object actual)
		{
			this.AreNotEqual(notExpected, actual, string.Empty, null);
		}

		/// <summary>
		/// Checks objects are not the same.
		/// </summary>
		/// <param name="notExpected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		/// <param name="message">The Message.</param>
		public void AreNotSame(object notExpected, object actual, string message)
		{
			this.AreNotEqual(notExpected, actual, message, null);
		}

		/// <summary>
		/// Checks objects are not the same.
		/// </summary>
		/// <param name="notExpected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void AreNotSame(object notExpected, object actual, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreNotSame(notExpected, actual, message, parameters));
		}

		/// <summary>
		/// Checks objects are the same.
		/// </summary>
		/// <param name="expected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		public void AreSame(object expected, object actual)
		{
			this.ExecuteSafely(() => Assert.AreSame(expected, actual));
		}

		/// <summary>
		/// Checks objects are the same.
		/// </summary>
		/// <param name="expected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		/// <param name="message">The Message.</param>
		public void AreSame(object expected, object actual, string message)
		{
			this.ExecuteSafely(() => Assert.AreSame(expected, actual, message));
		}

		/// <summary>
		/// Checks objects are the same.
		/// </summary>
		/// <param name="expected">The Expected object.</param>
		/// <param name="actual">The Actual object.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void AreSame(object expected, object actual, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.AreSame(expected, actual, message, parameters));
		}

		/// <summary>
		/// Calls assert's Fail method.
		/// </summary>
		public void Fail()
		{
			this.Fail(string.Empty, null);
		}

		/// <summary>
		/// Calls assert's Fail method.
		/// </summary>
		/// <param name="message">The Message.</param>
		public void Fail(string message)
		{
			this.Fail(message, null);
		}

		/// <summary>
		/// Calls assert's Fail method.
		/// </summary>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void Fail(string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.Fail(message, parameters));
		}

		/// <summary>
		/// Calls assert's Inconclusive method.
		/// </summary>
		public void Inconclusive()
		{
			this.Inconclusive(string.Empty, null);
		}

		/// <summary>
		/// Calls assert's Inconclusive method.
		/// </summary>
		/// <param name="message">The Message.</param>
		public void Inconclusive(string message)
		{
			this.Inconclusive(message, null);
		}

		/// <summary>
		/// Calls assert's Inconclusive method.
		/// </summary>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void Inconclusive(string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.Inconclusive(message, parameters));
		}

		/// <summary>
		/// Checks whether condition is false.
		/// </summary>
		/// <param name="condition">The Condition.</param>
		public void IsFalse(bool condition)
		{
			this.IsFalse(condition, string.Empty, null);
		}

		/// <summary>
		/// Checks whether condition is false.
		/// </summary>
		/// <param name="condition">The Condition.</param>
		/// <param name="message">The Message.</param>
		public void IsFalse(bool condition, string message)
		{
			this.IsFalse(condition, message, null);
		}

		/// <summary>
		/// Checks whether condition is false.
		/// </summary>
		/// <param name="condition">The Condition.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsFalse(bool condition, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.IsFalse(condition, message, parameters));
		}

		/// <summary>
		/// Checks whether value is an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="expectedType">The Expected Type.</param>
		public void IsInstanceOfType(object value, Type expectedType)
		{
			this.IsInstanceOfType(value, expectedType, string.Empty, null);
		}

		/// <summary>
		/// Checks whether value is an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="expectedType">The Expected Type.</param>
		/// <param name="message">The Message.</param>
		public void IsInstanceOfType(object value, Type expectedType, string message)
		{
			this.IsInstanceOfType(value, expectedType, message, null);
		}

		/// <summary>
		/// Checks whether value is an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="expectedType">The Expected Type.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsInstanceOfType(object value, Type expectedType, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.IsInstanceOfType(value, expectedType, message, parameters));
		}

		/// <summary>
		/// Checks whether value is not an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="wrongType">The Wrong Type.</param>
		public void IsNotInstanceOfType(object value, Type wrongType)
		{
			this.IsNotInstanceOfType(value, wrongType, string.Empty, null);
		}

		/// <summary>
		/// Checks whether value is not an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="wrongType">The Wrong Type.</param>
		/// <param name="message">The Message.</param>
		public void IsNotInstanceOfType(object value, Type wrongType, string message)
		{
			this.IsNotInstanceOfType(value, wrongType, message, null);
		}

		/// <summary>
		/// Checks whether value is not an instance of apropriate type.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="wrongType">The Wrong Type.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsNotInstanceOfType(object value, Type wrongType, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.IsNotInstanceOfType(value, wrongType, message, parameters));
		}

		/// <summary>
		/// Checks whether value is not null.
		/// </summary>
		/// <param name="value">The Value.</param>
		public void IsNotNull(object value)
		{
			this.IsNotNull(value, string.Empty, null);
		}

		/// <summary>
		/// Checks whether value is not null.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="message">The Message.</param>
		public void IsNotNull(object value, string message)
		{
			this.IsNotNull(value, message, null);
		}

		/// <summary>
		/// Checks whether value is not null.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsNotNull(object value, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.IsNotNull(value, message, parameters));
		}

		/// <summary>
		/// Checks whether value is null.
		/// </summary>
		/// <param name="value">The Value.</param>
		public void IsNull(object value)
		{
			this.IsNull(value, string.Empty, null);
		}

		/// <summary>
		/// Checks whether value is null.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="message">The Message.</param>
		public void IsNull(object value, string message)
		{
			this.IsNull(value, message, null);
		}

		/// <summary>
		/// Checks whether value is null.
		/// </summary>
		/// <param name="value">The Value.</param>
		/// <param name="message">The Message.</param>
		/// <param name="parameters">The Parameters.</param>
		public void IsNull(object value, string message, params object[] parameters)
		{
			this.ExecuteSafely(() => Assert.IsNull(value, message, parameters));
		}

		/// <summary>
		/// Replaces Null Chars.
		/// </summary>
		/// <param name="input">The Text.</param>
		/// <returns>Reformated text.</returns>
		public string ReplaceNullChars(string input)
		{
			return Assert.ReplaceNullChars(input);
		}

		/// <summary>
		/// Checks exceptions.
		/// </summary>
		public void Check()
		{
			Logger.Verify(this.exceptions);

			if (!this.HasFails)
			{
				return;
			}

			var builder = new StringBuilder();
			builder.AppendLine("\nThe folowing verifiers failed:");
			this.exceptions.Select(ex => ex.Message).ForEach(m => builder.AppendLine(m.ReplaceFirst("Assert", "Verify")));

			var first = this.exceptions.First();
			throw new UnitTestVerifyException(new AggregateException(builder.ToString(), this.exceptions), first.StackTrace);
		}

		/// <summary>
		/// Execute action safely.
		/// </summary>
		/// <param name="action">The Action.</param>
		private void ExecuteSafely(Action action)
		{
			try
			{
				action();
			}
			catch (UnitTestAssertException e)
			{
				var exception = new UnitTestVerifyException(e, this.ClenupStackTrace());
				this.exceptions.Add(exception);
				if (this.VerifyFailed != null)
				{
					this.VerifyFailed(exception);
				}
			}
		}

		/// <summary>
		/// Cleans up Stack Trace.
		/// </summary>
		/// <returns>Call stack.</returns>
		private string ClenupStackTrace()
		{
			var stackTrace = new StackTrace(3, true);
			var frames = stackTrace.GetFrames().TakeWhile(x => x.GetILOffset() != -1);
			return this.GetCallStack(frames);
		}

		/// <summary>
		/// Gets Call Stack.
		/// </summary>
		/// <param name="frames">The Frames.</param>
		/// <returns>Call stack.</returns>
		private string GetCallStack(IEnumerable<StackFrame> frames)
		{
			var text = "at";
			var format = "in {0}:line {1}";
			var flag = true;
			var stringBuilder = new StringBuilder(255);
			foreach (var frame in frames)
			{
				var method = frame.GetMethod();
				if (method != null)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(Environment.NewLine);
					}

					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "   {0} ", new object[] { text });
					var declaringType = method.DeclaringType;
					if (declaringType != null)
					{
						stringBuilder.Append(declaringType.FullName.Replace('+', '.'));
						stringBuilder.Append(".");
					}

					stringBuilder.Append(method.Name);
					if (method is MethodInfo && method.IsGenericMethod)
					{
						var genericArguments = method.GetGenericArguments();
						stringBuilder.Append("[");
						var j = 0;
						var flag2 = true;
						while (j < genericArguments.Length)
						{
							if (!flag2)
							{
								stringBuilder.Append(",");
							}
							else
							{
								flag2 = false;
							}

							stringBuilder.Append(genericArguments[j].Name);
							j++;
						}

						stringBuilder.Append("]");
					}

					stringBuilder.Append("(");
					var parameters = method.GetParameters();
					var flag3 = true;
					for (var k = 0; k < parameters.Length; k++)
					{
						if (!flag3)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							flag3 = false;
						}

						var str = "<UnknownType>";
						if (parameters[k].ParameterType != null)
						{
							str = parameters[k].ParameterType.Name;
						}

						stringBuilder.Append(str + " " + parameters[k].Name);
					}

					stringBuilder.Append(")");
					if (frame.GetILOffset() != -1)
					{
						string text2 = null;
						try
						{
							text2 = frame.GetFileName();
						}
						catch (SecurityException)
						{
						}

						if (text2 != null)
						{
							stringBuilder.Append(' ');
							stringBuilder.AppendFormat(CultureInfo.InvariantCulture, format, new object[] { text2, frame.GetFileLineNumber() });
						}
					}
				}
			}

			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}
	}
}