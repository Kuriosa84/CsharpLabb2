using System;
using System.Collections.Generic;
using SQLite;

namespace BookKeeper
{
	public class BookkeeperManager
	{
		private static BookkeeperManager instance;

		public List<Entry> Entries { get; set; }
		public List<Account> IncomeAccounts { get; set; }
		public List<Account> ExpenseAccounts { get; set; }
		public List<Account> MoneyAccounts { get; set; }
		public List<TaxRate> TaxRates { get; set; }

		private BookkeeperManager()
		{
			
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
			string dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			Entries.Add(entry);
			SQLiteConnection db = new SQLiteConnection(dbPath + @"\database.db");
			db.CreateTable<Entry>();
			db.Insert(entry);
			db.Close();
		}
	}
}
