using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;

namespace BookKeeper
{
	[Activity(Label = "BookKeeper", MainLauncher = true)]
	/*
	 * The main menu of this app.
	 */
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_main);
			BookkeeperManager bookkeeperManager = BookkeeperManager.Instance;
			Button newEntryButton = FindViewById<Button>(Resource.Id.new_entry_button);
			newEntryButton.Click += delegate {
				Intent intent = new Intent(this, typeof(NewEntryActivity));
				intent.PutExtra(NewEntryActivity.editEntry, false);
				StartActivity(new Intent(this, typeof(NewEntryActivity)));
			};

			Button showAllEntriesButton = FindViewById<Button>(Resource.Id.show_all_entries_button);
			showAllEntriesButton.Click += delegate {
				StartActivity(new Intent(this, typeof(EntryListActivity)));
			};

			Button createReportsButton = FindViewById<Button>(Resource.Id.create_reports_button);
			createReportsButton.Click += delegate {
				StartActivity(new Intent(this, typeof(CreateReportsActivity)));
			};

		}
	}


}