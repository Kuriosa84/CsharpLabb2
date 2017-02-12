using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace BookKeeper
{
	public class EntryAdapter : BaseAdapter
	{
		private Activity activity;
		private List<Entry> entries;

		public EntryAdapter(Activity activity, List<Entry> entries)
		{
			this.activity = activity;
			this.entries = entries;
		}

		public override int Count
		{
			get
			{
				return entries.Count;
			}
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return new JavaObjectWrapper() { Obj = entries[position] };
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.entry_list_item, 
			                                                           parent, false);

			view.FindViewById<TextView>(Resource.Id.entry_left_column).Text = 
				entries[position].DateString;
			view.FindViewById<TextView>(Resource.Id.entry_middle_column).Text =
				    entries[position].Description;
			view.FindViewById<TextView>(Resource.Id.entry_right_column).Text =
				    entries[position].Amount + " kr";
			return view;
		}
	}
}
