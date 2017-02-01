using System;
using SQLite;

namespace BookKeeper
{
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
