using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using SQLite;

namespace BookKeeper
{
	/*
	 * Keeps track of accounts, entries and tax rates. Is a Singleton class.
	 */
	public class BookkeeperManager
	{
		private static BookkeeperManager instance;

		private readonly string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
									 + @"\database.db";

		/*
		 * If there are no accounts or tax rates in the database,
		 * they will be created in this constructor.
		 */
		private BookkeeperManager()
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);

			db.CreateTable<Entry>();
			db.CreateTable<Account>();
			db.CreateTable<TaxRate>();

			var accounts = db.Table<Account>().ToList();

			if (accounts.Count == 0)
			{
				db.Insert(new Account(1000, "Fådda mutor", AccountType.Income));
				db.Insert(new Account(2000, "Försäljning", AccountType.Income));
				db.Insert(new Account(3000, "Fådda gåvor", AccountType.Income));

				db.Insert(new Account(1020, "Gedda mutor", AccountType.Expense));
				db.Insert(new Account(2030, "Investeringar", AccountType.Expense));
				db.Insert(new Account(3040, "Mat", AccountType.Expense));
				db.Insert(new Account(4050, "Resor", AccountType.Expense));

				db.Insert(new Account(1100, "Kontokort", AccountType.Money));
				db.Insert(new Account(2200, "Kreditkort", AccountType.Money));
				db.Insert(new Account(3300, "Bankkonto", AccountType.Money));
			}

			TableQuery<TaxRate> taxrates = db.Table<TaxRate>();

			if (taxrates.Count() == 0)
			{
				db.Insert(new TaxRate(0.06));
				db.Insert(new TaxRate(0.12));
				db.Insert(new TaxRate(0.25));
			}
			db.Close();
		}

		public static BookkeeperManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BookkeeperManager();
				}

				return instance;
			}
		}

		public Entry SelectedEntry { get; set; }

		public List<Entry> Entries
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(dbPath);
				List<Entry> result = db.Table<Entry>().ToList();
				db.Close();
				return result;
			}
		}
		public List<Account> IncomeAccounts
		{ 
			get 
			{
				SQLiteConnection db = new SQLiteConnection(dbPath);
				List<Account> result = db.Table<Account>().Where(a => a.Type == AccountType.Income).ToList();
				db.Close();
				return result;
			}
		}
		public List<Account> ExpenseAccounts
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(dbPath);
				List<Account> result = db.Table<Account>().Where(a => a.Type == AccountType.Expense).ToList();
				db.Close();
				return result;
			}
		}
		public List<Account> MoneyAccounts
		{
			get
			{ 
				SQLiteConnection db = new SQLiteConnection(dbPath);
				List<Account> result = db.Table<Account>().Where(a => a.Type == AccountType.Money).ToList();
				db.Close();
				return result;
			}
		}
		public List<TaxRate> TaxRates
		{ 
			get
			{
				SQLiteConnection db = new SQLiteConnection(dbPath);
				List<TaxRate> result = db.Table<TaxRate>().ToList();
				db.Close();
				return result;
			}
		}

		public void AddEntry(Entry entry)
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);
			db.Insert(entry);
			db.Close();
		}

		public void UpdateEntry(Entry entry)
		{ 
			SQLiteConnection db = new SQLiteConnection(dbPath);
			db.Update(entry);
			db.Close();
		}

		/*
		 * Returns a report on how much tax each entry contributes with, as a string.
		 */
		public string GetTaxReport()
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);
			List<Entry> entries = db.Table<Entry>().ToList();
			string result = "";
			for (int i = 0; i < entries.Count; i++)
			{
				double tax = entries[i].Amount * (entries[i].TaxRate / (1 + entries[i].TaxRate));
				if (!entries[i].IsIncomeAccount)
					tax *= -1;

				result += entries[i].Description + "\n";
				result += string.Format("{0:0.00}", tax) + "\n\n";
			}
			db.Close();
			return result;
		}

		/*
		 * Returns a report on all accounts - income, expense and money accounts - 
		 * as a string.
		 */
		public string GetAccountReport()
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);
			List<Account> incomeAccounts = db.Table<Account>()
			                                 .Where(a => a.Type == AccountType.Income).ToList();
			List<Account> expenseAccounts = db.Table<Account>()
			                                  .Where(a => a.Type == AccountType.Expense).ToList();
			List<Account> moneyAccounts = db.Table<Account>()
			                                .Where(a => a.Type == AccountType.Money).ToList();

			string result = accountStringBuilder(incomeAccounts)
				+ accountStringBuilder(expenseAccounts)
				+ accountStringBuilder(moneyAccounts);

			db.Close();
			return result;
		}

		/*
		 * Helper method for GetAccountReport().
		 */
		private string accountStringBuilder(List<Account> accounts)
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);
			string result = "";
			for (int i = 0; i < accounts.Count; i++)
			{
				result += accounts[i] + "\n\n";

				int accountNr = accounts[i].Nr;
				List<Entry> entries;
				if (accounts[0].Type == AccountType.Money)
				{
					entries = db.Table<Entry>()
								.Where(e => e.MoneyAccount == accountNr)
								.ToList();
				}
				else
				{
					entries = db.Table<Entry>()
								.Where(e => e.IncomeOrExpenseAccount == accountNr)
								.ToList();
				}
				double accountTotal = 0;
				foreach (Entry entry in entries)
				{
					if (entry.IsIncomeAccount)
						accountTotal += entry.Amount;
					else
						accountTotal -= entry.Amount;
				}

				result += "Total: " + accountTotal + "\n";

				for (int j = 0; j < entries.Count; j++)
				{
					result += entries[j].DateString + " - " + entries[j].Description + ", ";
					result += entries[j].IsIncomeAccount ? "" : "-";
					result += entries[j].Amount + "\n";
				}
				result += "\n";
			}
			result += "************\n";
			db.Close();
			return result;
		}
	}
}