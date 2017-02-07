using System;
using SQLite;

namespace BookKeeper
{
	public class Entry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }
		public double Amount { get; set; }
		public string Description { get; set; }
		public bool IsIncomeAccount { get; set; }
		public int MoneyAccount { get; set; }
		public int IncomeOrExpenseAccount { get; set; }
		public double TaxRate { get; set; }
		public DateTime Date { get; set; }
		public String DateString
		{
			get
			{
				return Date.Year + "-" + Date.Month + "-" + Date.Day;
			}
		}

		public Entry()
		{
		}

		public Entry(double amount, string description, bool isIncomeAccount, int moneyAccount, int incomeOrExpenseAccount, double taxRate,
		             DateTime date)
		{
			Amount = amount;
			Description = description;
			IsIncomeAccount = isIncomeAccount;
			MoneyAccount = moneyAccount;
			IncomeOrExpenseAccount = incomeOrExpenseAccount;
			TaxRate = taxRate;
			Date = date;
		}
	}
}