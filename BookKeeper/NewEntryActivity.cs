
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
	public class NewEntryActivity : Activity
	{
		TextView dateDisplay;
		Button dateSelectButton;
		Spinner typeSpinner, moneyAccountSpinner, taxSpinner;
		ArrayAdapter incomeAdapter, expenseAdapter, taxAdapter, moneyAccountAdapter;
		BookkeeperManager bookkeeperManager;
		bool isIncomeAccount;
		DateTime selectedDate;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_new_entry);

			bookkeeperManager = BookkeeperManager.Instance;

			SetSpinners();
			SetRadioButtons();
			SetDatePicker();
			SetSaveButton();
		}

		private void SetDatePicker()
		{
			dateDisplay = FindViewById<TextView>(Resource.Id.date_display);
			dateSelectButton = FindViewById<Button>(Resource.Id.date_select_button);
			dateSelectButton.Click += DateSelectOnClick;
		}

		void DateSelectOnClick(object sender, EventArgs eventArgs)
		{
			DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
				 {
					 dateDisplay.Text = time.ToLongDateString();
					 selectedDate = time;
				 });
			frag.Show(FragmentManager, DatePickerFragment.TAG);
		}



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

			moneyAccountAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem,
			                                       bookkeeperManager.MoneyAccounts);
			moneyAccountSpinner.Adapter = moneyAccountAdapter;

		}

		private void SetRadioButtons()
		{
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

		private void SetSaveButton()
		{
			Button saveButton = FindViewById<Button>(Resource.Id.save_button);
			saveButton.Click += delegate{
				double amount = Double.Parse(FindViewById<EditText>(Resource.Id.amount_edit).Text);
				Account moneyAccount = bookkeeperManager.MoneyAccounts[moneyAccountSpinner.SelectedItemPosition];
				Account incomeOrExpenseAccount;
				if (isIncomeAccount)
					incomeOrExpenseAccount = bookkeeperManager.IncomeAccounts[typeSpinner.SelectedItemPosition];
				else
					incomeOrExpenseAccount = bookkeeperManager.ExpenseAccounts[typeSpinner.SelectedItemPosition];
				TaxRate rate = bookkeeperManager.TaxRates[taxSpinner.SelectedItemPosition];
				DateTime date = selectedDate;

				Entry entry = new Entry(amount, isIncomeAccount, moneyAccount.Nr, incomeOrExpenseAccount.Nr,
										rate.Rate, date);
				bookkeeperManager.AddEntry(entry);
				
			};
		}
	}
}
