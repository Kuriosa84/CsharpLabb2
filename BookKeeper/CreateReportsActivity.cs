
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BookKeeper
{
	[Activity(Label = "CreateReportsActivity")]
	/*
	 * This activity displays reports on the different accounts.
	 */
	public class CreateReportsActivity : Activity
	{
		Button accountReportsButton, taxReportButton;
		TextView reportTV;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_create_reports);

			BookkeeperManager bookkeeperManager = BookkeeperManager.Instance;

			accountReportsButton = FindViewById<Button>(Resource.Id.account_report_button);
			taxReportButton = FindViewById<Button>(Resource.Id.tax_report_button);
			reportTV = FindViewById<TextView>(Resource.Id.report_text);

			accountReportsButton.Click += delegate {
				reportTV.Text = bookkeeperManager.GetAccountReport();
			};

			taxReportButton.Click += delegate {
				reportTV.Text = bookkeeperManager.GetTaxReport();
			};
		}
	}
}
