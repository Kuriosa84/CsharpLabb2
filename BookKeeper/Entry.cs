using System;
using SQLite;

namespace BookKeeper
{
	public class Entry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }
		public double Amount { get; set; }
		public bool IsIncomeAccount { get; set; }
		public int MoneyAccount { get; set; }
		public int IncomeOrExpenseAccount { get; set; }
		public double TaxRate { get; set; }
		public DateTime Date { get; set; }

		public Entry()
		{
		}

		public Entry(double amount, bool isIncomeAccount, int moneyAccount, int incomeOrExpenseAccount, double taxRate,
		             DateTime date)
		{
			Amount = amount;
			IsIncomeAccount = isIncomeAccount;
			MoneyAccount = moneyAccount;
			IncomeOrExpenseAccount = incomeOrExpenseAccount;
			TaxRate = taxRate;
			Date = date;
		}
	}
}