
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
	public class EntryListActivity : Activity
	{
		BookkeeperManager bookkeeperManager;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_entry_list);

			bookkeeperManager = BookkeeperManager.Instance;
			List<Entry> entries = bookkeeperManager.Entries;
			ListView entryList = FindViewById<ListView>(Resource.Id.all_entries_list_view);
			entryList.Adapter = new EntryAdapter(this, entries);


		}
	}
}
