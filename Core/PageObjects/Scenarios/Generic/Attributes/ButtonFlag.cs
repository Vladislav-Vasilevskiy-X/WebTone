using System.Diagnostics.CodeAnalysis;

namespace Core.PageObjects.Scenarios.Generic.Attributes
{
	/// <summary>
	///     The button flags.
	/// </summary>
	public enum ButtonFlag
	{
		/// <summary>
		///     The SaveAndClose item.
		/// </summary>
		SaveAndClose,

		/// <summary>
		///     The Save item.
		/// </summary>
		Save,

		/// <summary>
		///     The SaveAndAddAnother item.
		/// </summary>
		SaveAndAddAnother,

		/// <summary>
		///     The SaveAndCloseNaturalCode item.
		/// </summary>
		SaveAndCloseNaturalCode,

		/// <summary>
		///     The Submit item.
		/// </summary>
		Submit,

		/// <summary>
		///     The Cancel item.
		/// </summary>
		Cancel,

		/// <summary>
		///     The Continue item.
		/// </summary>
		Continue,

		/// <summary>
		///     The Close item.
		/// </summary>
		Close,

		/// <summary>
		///     The  item.
		/// </summary>
		Change,

		/// <summary>
		///     The Apply item.
		/// </summary>
		Apply,

		/// <summary>
		///     The Create item.
		/// </summary>
		Create,

		/// <summary>
		///     The CreatePeriods item.
		/// </summary>
		CreatePeriods,

		/// <summary>
		///     The CreateTemplate item.
		/// </summary>
		CreateTemplate,

		/// <summary>
		///     The EditTemplate item.
		/// </summary>
		EditTemplate,

		/// <summary>
		///     The SaveAndAddAnotherBank item.
		/// </summary>
		SaveAndAddAnotherBank,

		/// <summary>
		///     The Run item.
		/// </summary>
		Run,

		/// <summary>
		///     The SaveAndAddAnotherNaturalCode item.
		/// </summary>
		SaveAndAddAnotherNaturalCode,

		/// <summary>
		///     The CloseIcon item.
		/// </summary>
		CloseIcon,

		/// <summary>
		///     The Delete item.
		/// </summary>
		Delete,

		/// <summary>
		///     The Generate item.
		/// </summary>
		Generate,

		/// <summary>
		///     The Previous item.
		/// </summary>
		Previous,

		/// <summary>
		///     The Next item.
		/// </summary>
		Next,

		/// <summary>
		///     The ViewAll item.
		/// </summary>
		ViewAll,

		/// <summary>
		///     The ClearAll item.
		/// </summary>
		ClearAll,

		/// <summary>
		///     The PayInvoice item.
		/// </summary>
		PayInvoice,

		/// <summary>
		///     The Print item.
		/// </summary>
		Print,

		/// <summary>
		///     The Download item.
		/// </summary>
		Download,

		/// <summary>
		///     The Finalize item.
		/// </summary>
		Finalize,

		/// <summary>
		///     The DeliverInvoice item.
		/// </summary>
		DeliverInvoice,

		/// <summary>
		///     The RemovePreBills item.
		/// </summary>
		RemovePreBills,

		/// <summary>
		///     The Clear item.
		/// </summary>
		Clear,

		/// <summary>
		///     The Export item.
		/// </summary>
		Export,

		/// <summary>
		///     The GoToTimeSheet item.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeSheet")] GoToTimeSheet,

		/// <summary>
		///     The Expand All item.
		/// </summary>
		ExpandAll,

		/// <summary>
		///     The Collapse All item.
		/// </summary>
		CollapseAll,

		/// <summary>
		///     The Close Matters item.
		/// </summary>
		CloseMatters,

		/// <summary>
		///     The Reopen Matters item.
		/// </summary>
		ReopenMatters,

		/// <summary>
		///     The Delete Matters item.
		/// </summary>
		DeleteMatters,

		/// <summary>
		///     The Gear item.
		/// </summary>
		Gear
	}
}