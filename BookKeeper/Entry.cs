using System;
using SQLite;

namespace BookKeeper
{
	/*
	 * This class represents a bookkeeping entry in the database.
	 */
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
				string month = string.Format("{0,2:D2}", Date.Month);
				string day = string.Format("{0,2:D2}", Date.Day);
				return Date.Year + "-" + month + "-" + day;
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