
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
		Spinner typeSpinner;
		ArrayAdapter incomeAdapter;
		ArrayAdapter expenseAdapter;
		string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + @"\database.db";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_new_entry);

			BookkeeperManager bookkeeperManager = BookkeeperManager.Instance;

			SQLiteConnection db = new SQLiteConnection(dbPath);

			FillDatabase();
			SetSpinners();
			SetRadioButtons();
		}

		private void FillDatabase()
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);

			db.CreateTable<Entry>();
			db.CreateTable<Account>();
			db.CreateTable<TaxRate>();

			var accounts = db.Table<Account>().ToList();

			if (accounts.Count == 0)
			{
				string[] incomeAccounts = Resources.GetStringArray(Resource.Array.income_type_array);
				string[] incomeNumbers = Resources.GetStringArray(Resource.Array.income_number_array);
				for (int i = 0; i < incomeAccounts.Length; i++)
				{
					Account newAccount = new Account { Nr = Int32.Parse(incomeNumbers[i]), Name = incomeAccounts[i], Type = Account.AccountType.Income };
					db.Insert(newAccount);
				}

				string[] expenseAccounts = Resources.GetStringArray(Resource.Array.expense_type_array);
				string[] expenseNumbers = Resources.GetStringArray(Resource.Array.expense_number_array);
				for (int i = 0; i < expenseAccounts.Length; i++)
				{
					Account newAccount = new Account { Nr = Int32.Parse(expenseNumbers[i]), Name = expenseAccounts[i], Type = Account.AccountType.Expense };
					db.Insert(newAccount);
				}

				string[] moneyAccounts = Resources.GetStringArray(Resource.Array.money_accounts_array);
				string[] moneyNumbers = Resources.GetStringArray(Resource.Array.money_numbers_array);
				for (int i = 0; i < moneyAccounts.Length; i++)
				{
					Account newAccount = new Account { Nr = Int32.Parse(moneyNumbers[i]), Name = moneyAccounts[i], Type = Account.AccountType.Money };
					db.Insert(newAccount);
				}
			}

			TableQuery<TaxRate> taxrates = db.Table<TaxRate>();

			if (taxrates.Count() == 0)
			{
				string[] taxes = Resources.GetStringArray(Resource.Array.tax_array);
				for (int i = 0; i < taxes.Length; i++)
				{
					TaxRate taxRate = new TaxRate { Rate = Double.Parse(taxes[i]) };
					db.Insert(taxRate);
				}
			}

			db.Close();
		}

		private void SetSpinners()
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);

			typeSpinner = FindViewById<Spinner>(Resource.Id.type_spinner);
			Spinner moneyAccountSpinner = FindViewById<Spinner>(Resource.Id.money_account_spinner);
			Spinner taxSpinner = FindViewById<Spinner>(Resource.Id.tax_spinner);

			var accounts = db.Table<Account>().ToList();

			incomeAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, accounts);
			typeSpinner.Adapter = incomeAdapter;

			var taxes = db.Table<TaxRate>().ToList();

			ArrayAdapter taxAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, taxes);
			taxSpinner.Adapter = taxAdapter;

			var expenses = db.Table<Account>().Where(a => a.Type == Account.AccountType.Expense).ToList();

			expenseAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem,
				                                  expenses);

			var moneyAccounts = db.Table<Account>().Where(a => a.Type == Account.AccountType.Money).ToList();

			ArrayAdapter moneyAccountAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem,
			                                                    moneyAccounts);
			moneyAccountSpinner.Adapter = moneyAccountAdapter;

			db.Close();
		}

		private static List<Account> makeListFromIEnumerable(IEnumerable<Account> query)
		{
			Account account = query.First();
			List<Account> accountList = new List<Account>();
			accountList.Add(account);
			return accountList;
		}

		private static List<Object> makeList(IEnumerable<Object> query)
		{
			Object thing = query.First();
			List<Object> result = new List<Object>();
			result.Add(thing);
			return result;
		}



		private void SetRadioButtons()
		{
			RadioButton incomeRB = FindViewById<RadioButton>(Resource.Id.incomeRadioButton);
			RadioButton expenseRB = FindViewById<RadioButton>(Resource.Id.expenseRadioButton);

			incomeRB.Click += delegate (object sender, EventArgs e)
			{
				typeSpinner.Adapter = incomeAdapter;
			};

			expenseRB.Click += delegate (object sender, EventArgs e)
			{
				typeSpinner.Adapter = expenseAdapter;
			};
		}
	}
}
