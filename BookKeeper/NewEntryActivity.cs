
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BookKeeper
{
	[Activity(Label = "Ny händelse")]
	/*
	 * An Activity for adding a new entry, or editing an existing one.
	 */
	public class NewEntryActivity : Activity
	{
		TextView dateDisplay;
		Button dateSelectButton;
		Spinner typeSpinner, moneyAccountSpinner, taxSpinner;
		ArrayAdapter incomeAdapter, expenseAdapter;
		BookkeeperManager bookkeeperManager;
		bool isIncomeAccount;
		DateTime selectedDate;
		public static readonly string editEntry = "EDIT_ENTRY";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_new_entry);

			bookkeeperManager = BookkeeperManager.Instance;

			SetSpinners();
			SetRadioButtons();
			SetDatePicker();
			SetTaxCalculator();

			if (Intent.GetBooleanExtra(editEntry, false))
			{
				SetSaveButtonEdit();
				SetDate();
				SetDescription();
				SetSelectedSpinners();
				SetAmount();
			}
			else
			{
				SetSaveButton();
			}
		}

		/*
		 * Sets up the date picker.
		 */
		private void SetDatePicker()
		{
			dateDisplay = FindViewById<TextView>(Resource.Id.date_display);
			dateSelectButton = FindViewById<Button>(Resource.Id.date_select_button);
			dateSelectButton.Click += DateSelectOnClick;
		}

		/*
		 * Displays the date picker fragment.
		 */
		void DateSelectOnClick(object sender, EventArgs eventArgs)
		{
			DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
				 {
					 dateDisplay.Text = time.ToLongDateString();
					 selectedDate = time;
				 });
			frag.Show(FragmentManager, DatePickerFragment.TAG);
		}

		/*
		 * Populates the Spinners.
		 */
		private void SetSpinners()
		{
			typeSpinner = FindViewById<Spinner>(Resource.Id.type_spinner);
			moneyAccountSpinner = FindViewById<Spinner>(Resource.Id.money_account_spinner);
			taxSpinner = FindViewById<Spinner>(Resource.Id.tax_spinner);

			incomeAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, bookkeeperManager.IncomeAccounts);
			typeSpinner.Adapter = incomeAdapter;

			taxSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, bookkeeperManager.TaxRates);

			expenseAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem,
			                                  bookkeeperManager.ExpenseAccounts);

			moneyAccountSpinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem,
												   bookkeeperManager.MoneyAccounts);
		}

		/*
		 * Sets up the radio buttons so that the proper accounts are shown when they are clicked.
		 */
		private void SetRadioButtons()
		{
			isIncomeAccount = true;
			RadioButton incomeRB = FindViewById<RadioButton>(Resource.Id.incomeRadioButton);
			RadioButton expenseRB = FindViewById<RadioButton>(Resource.Id.expenseRadioButton);

			incomeRB.Click += delegate (object sender, EventArgs e)
			{
				typeSpinner.Adapter = incomeAdapter;
				isIncomeAccount = true;
			};

			expenseRB.Click += delegate (object sender, EventArgs e)
			{
				typeSpinner.Adapter = expenseAdapter;
				isIncomeAccount = false;
			};
		}

		/*
		 * Sets up the EditText for the amount, and the tax rate Spinner,
		 * so that when they are changed, the net amount is shown.
		 */
		private void SetTaxCalculator()
		{
			EditText grossEdit = FindViewById<EditText>(Resource.Id.amount_edit);
			grossEdit.AfterTextChanged += delegate { calculateTax(); };
			taxSpinner.ItemSelected += delegate { calculateTax(); };
		}

		/*
		 * Displays the net amount (without taxes).
		 */
		private void calculateTax()
		{
			EditText grossEdit = FindViewById<EditText>(Resource.Id.amount_edit);
			TextView netAmount = FindViewById<TextView>(Resource.Id.net_amount_text);
			double gross, tax, net;
			try
			{
				gross = Double.Parse(grossEdit.Text);
				tax = bookkeeperManager.TaxRates[taxSpinner.SelectedItemPosition].Rate;
				net = gross / (1 + tax);
				netAmount.Text = string.Format("{0:0.00}", net);
			}
			catch (Exception)
			{
				netAmount.Text = "-";
			}
		}

		/*
		 * Sets up the Save button for creating a new entry, so that the new entry
		 * is added in the database.
		 */
		private void SetSaveButton()
		{
			Button saveButton = FindViewById<Button>(Resource.Id.save_button);
			saveButton.Click += delegate{
				double amount = Double.Parse(FindViewById<EditText>(Resource.Id.amount_edit).Text);
				string description = FindViewById<EditText>(Resource.Id.description_edit).Text;
				Account moneyAccount = bookkeeperManager.MoneyAccounts[moneyAccountSpinner.SelectedItemPosition];
				Account incomeOrExpenseAccount;
				if (isIncomeAccount)
					incomeOrExpenseAccount = bookkeeperManager.IncomeAccounts[typeSpinner.SelectedItemPosition];
				else
					incomeOrExpenseAccount = bookkeeperManager.ExpenseAccounts[typeSpinner.SelectedItemPosition];
				TaxRate rate = bookkeeperManager.TaxRates[taxSpinner.SelectedItemPosition];
				DateTime date = selectedDate;

				Entry entry = new Entry(amount, description, isIncomeAccount, moneyAccount.Nr, incomeOrExpenseAccount.Nr,
										rate.Rate, date);
				bookkeeperManager.AddEntry(entry);
				StartActivity(new Intent(this, typeof(MainActivity)));
			};
		}

		/*
		 * Sets up the Save button for when in editing mode, so that the current entry
		 * is updated in the database.
		 */
		private void SetSaveButtonEdit()
		{
			Button saveButton = FindViewById<Button>(Resource.Id.save_button);
			saveButton.Click += delegate
			{
				Entry entry = bookkeeperManager.SelectedEntry;
				entry.Amount = Double.Parse(FindViewById<EditText>(Resource.Id.amount_edit).Text);
				entry.Description = FindViewById<EditText>(Resource.Id.description_edit).Text;
				entry.MoneyAccount = bookkeeperManager.MoneyAccounts[moneyAccountSpinner.SelectedItemPosition].Nr;
				if (isIncomeAccount)
					entry.IncomeOrExpenseAccount = bookkeeperManager.IncomeAccounts[typeSpinner.SelectedItemPosition].Nr;
				else
					entry.IncomeOrExpenseAccount = bookkeeperManager.ExpenseAccounts[typeSpinner.SelectedItemPosition].Nr;
				entry.TaxRate = bookkeeperManager.TaxRates[taxSpinner.SelectedItemPosition].Rate;
				entry.Date = selectedDate;

				bookkeeperManager.UpdateEntry(entry);
				StartActivity(new Intent(this, typeof(MainActivity)));
			};
		}

		/*
		 * Sets the correct income/expense radio button as selected,
		 * depending on the current entry's account type, in editing mode.
		 */
		private void SetSelectedRadioButton()
		{
			isIncomeAccount = bookkeeperManager.SelectedEntry.IsIncomeAccount;
			RadioButton incomeRB = FindViewById<RadioButton>(Resource.Id.incomeRadioButton);
			incomeRB.Selected = isIncomeAccount;
			RadioButton expenseRB = FindViewById<RadioButton>(Resource.Id.expenseRadioButton);
			expenseRB.Selected = !isIncomeAccount;
		}

		/*
		 * Displays the date of the current entry, in editing mode.
		 */
		private void SetDate()
		{
			TextView dateText = FindViewById<TextView>(Resource.Id.date_display);
			dateText.Text = bookkeeperManager.SelectedEntry.DateString;
		}

		/*
		 * Displays the description of the current entry, in editing mode.
		 */
		private void SetDescription()
		{
			FindViewById<EditText>(Resource.Id.description_edit).Text = bookkeeperManager.SelectedEntry.Description;
		}

		/*
		 * Sets the spinners' selections to the current entry's accounts and tax rate,
		 * when the Activity is used for editing an existing Entry.
		 */
		private void SetSelectedSpinners()
		{
			typeSpinner.SetSelection(getTypeSpinnerPosition());
			moneyAccountSpinner.SetSelection(getMoneySpinnerPosition());
			taxSpinner.SetSelection(getTaxSpinnerPosition());
		}

		/*
		 * Helper method for SetSelectedSpinners().
		 */
		private int getTypeSpinnerPosition()
		{
			List<Account> accounts;
			if (isIncomeAccount)
				accounts = bookkeeperManager.IncomeAccounts;
			else
				accounts = bookkeeperManager.ExpenseAccounts;
			int accountNr = bookkeeperManager.SelectedEntry.IncomeOrExpenseAccount;
			Account account = (Account)accounts.Where(a => a.Nr == accountNr);
			return accounts.IndexOf(account);
		}

		/*
		 * Helper method for SetSelectedSpinners().
		 */
		private int getMoneySpinnerPosition()
		{
			List<Account> accounts = bookkeeperManager.MoneyAccounts;
			int accountNr = bookkeeperManager.SelectedEntry.MoneyAccount;
			Account account = (Account)accounts.Where(a => a.Nr == accountNr);
			return accounts.IndexOf(account);
		}

		/*
		 * Helper method for SetSelectedSpinners().
		 */
		private int getTaxSpinnerPosition()
		{
			List<TaxRate> taxRates = bookkeeperManager.TaxRates;
			double rate = bookkeeperManager.SelectedEntry.TaxRate;
			TaxRate taxRate = (TaxRate)taxRates.Where(t => t.Rate == rate);
			return taxRates.IndexOf(taxRate);
		}

		/*
		 * Sets the amount to the current entry's amount,
		 * when in editing mode.
		 */
		private void SetAmount() 
		{
			FindViewById<EditText>(Resource.Id.amount_edit).Text = string.Format("{0:0.00}",bookkeeperManager.SelectedEntry.Amount);
		}
	}
}
