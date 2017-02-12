
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
	[Activity(Label = "EntryListActivity")]
	/*
	 * A list of all entries in the database. When an entry is clicked, the user
	 * reaches an editing page for that particular entry.
	 */
	public class EntryListActivity : Activity
	{
		BookkeeperManager bookkeeperManager;
		List<Entry> entries;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_entry_list);

			bookkeeperManager = BookkeeperManager.Instance;
			entries = bookkeeperManager.Entries;
			ListView entryList = FindViewById<ListView>(Resource.Id.android_R_id_list);
			EntryAdapter entryAdapter = new EntryAdapter(this, entries);
			entryList.Adapter = entryAdapter;

			entryList.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				bookkeeperManager.SelectedEntry = entries[e.Position];
				Intent intent = new Intent(this, typeof(NewEntryActivity));
				intent.PutExtra(NewEntryActivity.editEntry, true);
				StartActivity(intent);
			};

		}
	}
}
