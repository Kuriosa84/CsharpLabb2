using System;
using SQLite;

namespace BookKeeper
{
	/*
	 * This class represents an account - income, expense or money account.
	 */
	public class Account
	{
		[PrimaryKey]
		public int Nr { get; set; }
		public string Name { get; set; }
		public AccountType Type { get; set; }

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