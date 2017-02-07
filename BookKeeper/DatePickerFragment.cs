using System;
using Android.App;
using Android.OS;
using Android.Widget;

namespace BookKeeper
{
	public class DatePickerFragment : DialogFragment, DatePickerDialog.IOnDateSetListener
	{
		public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

		Action<DateTime> dateSelectedHandler = delegate { };

		public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
		{
			DatePickerFragment frag = new DatePickerFragment();
			frag.dateSelectedHandler = onDateSelected;
			return frag;
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			DateTime currently = DateTime.Now;
			DatePickerDialog dialog = new DatePickerDialog(Activity,
														   this,
														   currently.Year,
														   currently.Month,
														   currently.Day);
			return dialog;
		}

		public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
		{
			// monthOfYear is a value between 0 and 11, not 1 and 12
			DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
			dateSelectedHandler(selectedDate);
		}
	}
}
