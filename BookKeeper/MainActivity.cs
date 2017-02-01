using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;

namespace BookKeeper
{
	[Activity(Label = "BookKeeper", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_main);
			Button newEntryButton = FindViewById<Button>(Resource.Id.new_entry_button);
			newEntryButton.Click += delegate {
				StartActivity(new Intent(this, typeof(NewEntryActivity)));
			};


		}
	}


}