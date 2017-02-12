using System;
using SQLite;

namespace BookKeeper
{
	/*
	 * This class represents a tax rate in the bookkeeping database.
	 */
	public class TaxRate
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }
		public double Rate { get; set; }

		public TaxRate()
		{
		}

		public TaxRate(double rate)
		{
			Rate = rate;
		}

		public override string ToString()
		{
			return string.Format("{0:0}%", Rate*100);
		}
	}
}
