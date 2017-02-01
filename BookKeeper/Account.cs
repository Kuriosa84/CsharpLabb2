using System;
using SQLite;

namespace BookKeeper
{
	public class Account
	{
		[PrimaryKey]
		public int Nr { get; set; }
		public string Name { get; set; }
		//public int Type { get; set; }
		public enum AccountType
		{
			Income,
			Expense,
			Money
		}
		public AccountType Type;


		public Account()
		{
		}

		public Account(int nr, string name, AccountType type)
		{
			Nr = nr;
			Name = name;
			Type = type;
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", Name, Nr);
		}
	}
}
