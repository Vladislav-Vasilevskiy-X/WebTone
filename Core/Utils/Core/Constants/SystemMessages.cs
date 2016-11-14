
namespace Core.Constants
{
	/// <summary>
	/// The system messages.
	/// </summary>
	public static class SystemMessages
	{
		public const string PreBillDetailsDiscountValueErrorMessage = "Your current changes have not been saved as they would result in a Discount value that is greater than the sum of Fees.";

		public const string FieldEnabled = "Field is enabled.";

		public const string FieldVerify = "Returned input value is as expected.";

		public const string EqualsMessage = "Objects should be the same";

		public const string NotEqualsMessage = "Objects should not be the same";

		public const string ErrorHeaderMessage = "Form Contains Errors";

		public const string NoDataAvailableInTable = "No data available in table.";

		public const string ThereAreNoExpensesForThisTimePeriod = "There are no expenses for this time period.";

		public const string DeleteSinglePreBillConfirmationMessage = "Are you sure you want to delete this Pre-Bill? All edits to time and expense entries you have made have been saved, " + "but all Pre-Bill edits and discounts you have made will be lost. This action cannot be reversed.";

		public const string DeleteConfirmationMessage = "Are you sure you want to delete these Pre-Bills? All edits to time and expense entries you have made have been saved, " + "but all Pre-Bill edits and discounts you have made will be lost. This action cannot be reversed.";

		public const string EmailRequestMessage = "You have selected to email the entire general ledger account transactions list. This may take some time to generate.";

		public const string PrintRequestMessage = "You have selected to print the entire general ledger account transactions list. This may take some time to generate.";

		public const string DownloadRequestMessage = "You have selected to download the entire general ledger account transactions list. This may take some time to generate.";

		public const string TrustAccountClosedReopenProfileMatterPageError = "A Trust Account for this matter is closed; you can reopen the Trust Account from the matter profile page.";

		public const string TrustAccountClosedAgainstThisMatterError = "A Trust Account for this matter is closed; you can reopen the Trust Account from the matter page.";

		public const string TrustAccountOpenedAgainstThisMatterError = "A Trust Account already exists against this Matter.";

		public const string TrustAccountClosedMatterError = "A Trust Account for closed matter cannot be created.";

		public const string OffsetsEmailRequestMessage = "You have selected to email the entire general ledger account transaction offsets list. This may take some time to generate.";

		public const string OffsetsPrintRequestMessage = "You have selected to print the entire general ledger account transaction offsets list. This may take some time to generate.";

		public const string OffsetsDownloadRequestMessage = "You have selected to download the entire general ledger account transaction offsets list. This may take some time to generate.";

		public const string RemovedPreBillsConfirmationMessage = "Selected Pre-Bills have been removed successfully";

		// Payment & Invoices
		public const string InvoiceCannotMakeOverpaymentFromAdjustment = "You cannot make an overpayment from an adjustment.";

		public const string InvoiceCannotMakeExcessPaymentFromAdjustment = "You cannot make an excess payment from an adjustment.";

		public const string InvoiceNumberNotFound = "Invoice Number not found.";

		public const string WriteOffInvoiceConfirmationMessage = "Write off this invoice? All edits to time and expense entries will be saved, but all edits and adjustments to the Pre-Bill will be lost.";

		public const string InvoiceAmountNotValidError = "Amount must be a number between 0.01 and 999,999,999.99.";

		public const string CannotOverpayFromTrustAccountError = "Cannot overpay invoice from trust account.";

		public const string PaymentAmountIsGreaterThanBalanceError = "Amount cannot be greater than trust account balance.";

		public const string InvoiceNumberMissingError = "Invoice Number is required.";

		public const string InvoicePaymentTypeMissingError = "Payment Type is required.";

		public const string InvoiceTypeMissingError = "Type is required.";

		public const string TransactionSuccessfullyReversed = "Transaction successfully reversed.";

		public const string ConfirmDeletePaymentMessage = "Are you sure you want to delete this payment?";

		public const string ConfirmDeleteTransactionMessage = "Are you sure you want to delete this new transaction?";

		public const string NoTransactionsToDisplay = "No transactions to display";

		public const string CannotReverseDueToInsufficientTrustAccountBalance = "Cannot reverse due to insufficient Trust Account balance.";

		// Time & Billing. Pre-bills
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "TimeLine")]
		public const string TimeLineSaveMessage = "Time Entries updated successfully";

		public const string OnePreBillSelected = "Pre-Bill Selected (1)";

		public const string TwoPreBillsSelected = "Pre-Bills Selected (2)";

		public const string PreBillsGenerationMessage = "Pre-Bills queued for creation. Close";

		public const string ExpenseEntryMaximumTotal = "Expense Entry Total should be maximum of 9,999,999.00.";

		public const string TimeEntriesUpdated = "Time Entries updated successfully.";

		public const string PreBillUpdatedSuccessfully = "Pre-Bill updated successfully.";

		public const string PreBillUpdatedSuccessfullyWithClose = PreBillUpdatedSuccessfully + " Close";

		public const string PreBillSetToFinalizedStatusSuccessfullyClose = "Pre-Bill set to Finalized status successfully. Close";

		public const string PreBillResetTopreBillStatusSuccessfullyClose = "Pre-bill reset to Pre-Bill status successfully. Close";

		public const string ExpenseEntriesUpdatedSuccessfullyClose = "Expense entries updated successfully. Close";

		public const string NotFinalizedPreBillMessage = "This Pre-Bill has not been finalized. It will be finalized when it is generated.";

		public const string PreBillGroupGenerationMessage = "Selected Pre-Bills have been queued for creation.";

		public const string InvoiceGenerationPreBillNotFinalized = "One or more selected Pre-Bills are not finalized. They will be finalized when they are generated.";

		public const string DeletedPreBillDetail = "This Pre-Bill is no longer available for editing. This could be because it has been converted to an invoice, written off or removed.";

		public const string TrustAccountListSelected = "You have requested to download {0} trust account(s). This may take some time to generate.";

		// Print Request dialog
		public const string PrintRequestSingleItem = " item is ready to be printed";

		public const string PrintRequestSeveralItems = " items are  ready to be printed";

		// Admin -> Time & Billing
		public const string InitialInvoiceNumberDefault = "Initial Invoice Number should be a number between 1 and 9,999,999,999.";

		public const string InitialInvoiceNumberCertain = "Initial Invoice Number should be greater than the current maximum invoice number of {0}, and no more than 9,999,999,999.";

		public const string TaxRateErrorMessage = "Set Tax Rate must be in the range 0.00000 - 10000.00000";

		public const string TrustReplenishmentMessageDefaultText = "Please note: Funds have fallen below the agreed upon amount. For more information please refer to your retainer agreement ot contact your attorney.";

		public const string TrustReplenishmentMessageFieldErrorMessage = "Trust replenishment message is required.";

		public const string TaxTypeAndNumberIsRequiredError = "Both Tax Type and Number are required for each Business Registration Number.";

		public const string OnlyOneTaxTypeIsAllowedError = "Only one registration number is allowed per tax type.";

		public const string TaxRateUpdatingMessage = "Tax Rate has been successfully updated.";

		// Admin -> Time & Billing -> Invoices
		public const string InvalidDueDateErrorMessage = "Due days should be a number between 0 and 365.";

		public const string DueDateFirstMessage = "Payment terms of 0 days will display on invoice as \"Due Upon Receipt\".";

		public const string DueDateSecondMessage = "Payment terms of a specific number of days will calculate the due date based on the invoice date.";

		// General
		public const string ThereWasAProblemSavingChangesClose = "There was a problem saving changes. Close";

		public const string NoMatchingRecordsFound = "No matching records found";

		public const string NoRecordsAvailable = "No data available in table";

		public const string ChangesSavedSuccessfully = "Changes saved successfully.";

		public const string ChangesHaveBeenSaved = "Changes have been saved.";

		public const string AccessDenied = "Access Denied";

		public const string ChangesWillNotBeSaved = "Changes will not be saved.";

		public const string UnsavedChanges = "You have made changes to this page.  Do you want to save those changes before proceeding?";

		// Matter Time Entry Widget, Time Entries list
		public const string TimeIsRequired = "Time is required.";

		public const string MatterIsRequired = "Matter is required.";

		public const string ClientIsRequired = "Client is required.";

		public const string TimeDurationIsRequired = "Time must be entered for at least one day.";

		public const string ExternalNarrativeIsRequired = "External Narrative is required.";

		public const string ExternalNarrativeOrExpenseCodeIsRequired = "External Narrative or Expense Code is required.";

		public const string TimeEntrySubmittedSuccessfully = "Time Entry submitted successfully";

		public const string ExpenseEntrySubmittedSuccessfully = "Expense Entry submitted successfully.";

		/// <summary>
		/// The asterisk text.
		/// </summary>
		public const string AsteriskText = "*";

		// Time & Billing -> Manage Pre-Bill Groups
		public const string UniqueGroupNameError = "A unique Group Name is required.";

		public const string NoItemsSelectedError = "At least one selection is required.";

		public const string GroupTypeRequiredError = "Group Type is required.";

		public const string ClientValueFromRequiredRequiredError = "Client From Value Required.";

		public const string ClientValueToRequiredRequiredError = "Client To Value Required.";

		public const string ClientPreBillGroupSelectMultipleClients = "Multiple";

		public const string NoClientsFoundMessage = "No Clients found.";

		public const string SelectionIsRequired = "At least one selection is required.";

		// Time & Billing Reporting
		public const string ReportDeliveryDialogFileSuccessfullyGenerated = "1 item is ready to be downloaded";

		public const string ReportDeliveryDialogFileNameIsRequired = "FileName is required";

		public const string ReportDeliveryDialogMatterIsRequired = "Matter is Required";

		public const string ReportDeliveryDialogClientIsRequired = "Client is Required";

		public const string ReportDeliveryDialogMatterOrClientIsRequired = "Matter or Client Required";

		public const string ReportDeliveryDialogDateRangeIsExceededOneYear = "The date range cannot exceed 365 days.";

		public const string ReportDeliveryDialogInvalidDate = "Invalid Date";

		public const string ReportDeliveryDialogEndDateIsRequired = "EndDate is required";

		public const string ReportDeliveryDialogStartDateIsRequired = "StartDate is required";	
		
		public const string ReportDeliveryDialogUserIsRequired = "User is required";

		// Timer
		public const string TimerSavedSuccessfully = "Timer Saved Successfully";

		public const string TimerSubmittedSuccessfully = "Timer Submitted Successfully";

		public const string ExternalNarrativeOrActivityOrTaskIsRequired = "External Narrative or Activity or Task is required.";

		public const string ExternalNarrativeOrActivityIsRequired = "External Narrative or Activity is required.";

		public const string ExternalNarrativeOrTaskIsRequired = "External Narrative or Task is required.";

		// Time & Billing -> Pre-bills -> Delivery invoice dialog
		// Information messages to user
		public const string IndividualMessageForPartOfPreBillsAlert = "One or more Pre-Bills already have an invoice message. The message entered below will be displayed on the remaining invoices.";

		public const string IndividualMessageForAllPreBillsAlert = "The message entered below will be displayed on all invoices.";

		public const string IndividualMessageForAllPreBillsWithMessagesAlert = "All selected Pre-Bills already have an invoice message. Click Generate to continue.";

		// FCCanada
		public const string AbsentChanges = "There are no changes to save.";

		public const string TaxRateRequired = "At least one tax rate is required.";

		// eBilling
		public const string EBillingFirmIdSuccessMessage = "eBilling changes saved successfully.";

		public const string LedesFirmIdRequiredMessage = "LEDES Firm ID is required.";

		public const string LedesFirmIdMaximumLengthMessage = "LEDES Firm ID must be less than or equal to 20 characters.";

		public const string LedesClientMatterIdMaximumLengthMessage = "LEDES Client Matter ID must be less than or equal to 20 characters.";

		public const string LedesFileSavedToDocumentsFolderMessage = "Copy of the LEDES file was successfully saved to the matter Time & Billing folder";

		public const string LedesFileDuplicateSavedToFolderMessage = "A copy of this file is already saved to the Time & Billing Folder";
	
		// Matters
		public const string TheMatterCannotBeClosedBecauseFinancialDataIsStillOpen = "The matter cannot be closed because financial data is still open.";

		public const string MattersSelected = "{0} matter(s) selected.";

		public const string MattersAlreadyClosed = "{0} matter(s) already closed.";

		public const string MattersAlreadyOpen = "{0} matter(s) already open.";

		public const string MattersThatAreAlreadyClosedWillNotBeAffected = "Matters that are already closed will not be affected.";

		public const string MattersThatAreAlreadyOpenWillNotBeAffected = "Matters that are already open will not be affected.";

		public const string MattersClosedSuccessfully = "{0} matter(s) closed successfully. Close";

		public const string MattersDeletedSuccessfully = "{0} matter(s) deleted successfully.";

		public const string MattersReopenedSuccessfully = "{0} matter(s) reopened successfully. Close";

		public const string MoreInformation = "More information";

		public const string HideInformation = "Hide information";

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CanNot")]
		public const string MattersCanNotBeClosed = "{0} of {1} matter(s) cannot be closed.";

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CanNot")]
		public const string MattersCanNotBeReopened = "{0} of {1} matter(s) cannot be reopened.";

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CanNot")]
		public const string MattersHaveAssociationsAndCanNotBeClosed = "{0} of {1} matter(s) have associations and cannot be closed.";
		public const string CloseMatters = "Close {0} matter(s)?";
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CanNot")]
		public const string MattersHaveAssociationsAndCanNotBeDeleted = "{0} of {1} matter(s) have associations and cannot be deleted.";

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CanNot")]
		public const string MattersHaveAssociationsAndCanNotBeReopened = "{0} of {1} matter(s) have associations and cannot be reopened.";

		public const string DeleteMatters = "Delete {0} matter(s)?";

		public const string ReopenMatters = "Reopen {0} matter(s)?";

		// Firm Central UK
		public const string SaveVatNumberMessage = "Changes have been saved.";
	}
}