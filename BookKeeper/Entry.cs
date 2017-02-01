using System;
using SQLite;

namespace BookKeeper
{
	public class Entry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }
		public bool IsIncomeAccount { get; set; }
		public int MoneyAccount { get; set; }
		public int IncomeOrExpenseAccount { get; set; }
		public double Rate { get; set; }
		public DateTime Date { get; set; }

		//public Account MoneyAccount { get; set; }
		//public Account IncomeOrExpenseAccount { get; set; }
		//public TaxRate Rate { get; set; }

		public Entry(int id, bool isIncomeAccount, Account moneyAccount, Account incomeOrExpenseAccount,
		            TaxRate rate)
		{
			Id = id;
			IsIncomeAccount = isIncomeAccount;
			MoneyAccount = moneyAccount.Nr;
			IncomeOrExpenseAccount = incomeOrExpenseAccount.Nr;
			Rate = rate.Rate;
		}
	}
}