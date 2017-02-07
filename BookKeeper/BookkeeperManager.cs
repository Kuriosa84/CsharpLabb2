using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using SQLite;

namespace BookKeeper
{
	public class BookkeeperManager
	{
		private static BookkeeperManager instance;

		private readonly string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
									 + @"\database.db";
		public List<Entry> Entries
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(dbPath);
				db.CreateTable<Entry>();
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
				db.CreateTable<Account>();
				List<Account> result = db.Table<Account>().Where(a => a.Type == Account.AccountType.Income).ToList();
				db.Close();
				return result;
			}
		}
		public List<Account> ExpenseAccounts
		{
			get
			{
				SQLiteConnection db = new SQLiteConnection(dbPath);
				db.CreateTable<Account>();
				List<Account> result = db.Table<Account>().Where(a => a.Type == Account.AccountType.Expense).ToList();
				db.Close();
				return result;
			}
		}
		public List<Account> MoneyAccounts
		{
			get
			{ 
				SQLiteConnection db = new SQLiteConnection(dbPath);
				db.CreateTable<Account>();
				List<Account> result = db.Table<Account>().Where(a => a.Type == Account.AccountType.Money).ToList();
				db.Close();
				return result;
			}
		}
		public List<TaxRate> TaxRates
		{ 
			get
			{
				SQLiteConnection db = new SQLiteConnection(dbPath);
				db.CreateTable<TaxRate>();
				List<TaxRate> result = db.Table<TaxRate>().ToList();
				db.Close();
				return result;
			}
		}

		private BookkeeperManager()
		{
			FillDatabase();
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

		public void AddEntry(Entry entry)
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);
			db.CreateTable<Entry>();
			db.Insert(entry);
			db.Close();
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
				db.Insert(new Account(1000, "Fådda mutor", Account.AccountType.Income));
				db.Insert(new Account(2000, "Försäljning", Account.AccountType.Income));
				db.Insert(new Account(3000, "Fådda gåvor", Account.AccountType.Income));

				db.Insert(new Account(1020, "Gedda mutor", Account.AccountType.Expense));
				db.Insert(new Account(2030, "Investeringar", Account.AccountType.Expense));
				db.Insert(new Account(3040, "Mat", Account.AccountType.Expense));
				db.Insert(new Account(4050, "Resor", Account.AccountType.Expense));

				db.Insert(new Account(1100, "Kontokort", Account.AccountType.Money));
				db.Insert(new Account(2200, "Kreditkort", Account.AccountType.Money));
				db.Insert(new Account(3300, "Bankkonto", Account.AccountType.Money));

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
	}
}
