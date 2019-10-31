using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MakeupScheduling.Models;

namespace MakeupScheduling
{
	[Activity(Label = "NewAppointmentActivity")]
	public class NewAppointmentActivity : Activity
	{
		Button saveButton;
		EditText name;
		Button startTimeButton;
		Button endTimeButton;
		Button deleteButton;

		CheckBox extras;
		Appointment appointment = new Appointment();
		TextView startTimeText;
		TextView endTimeText;
		TimeSpan startTime;
		TimeSpan endTime;
		string mode;
		long id;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.AppointmentView);

			saveButton = FindViewById<Button>(Resource.Id.button1);
			saveButton.Click += SaveButton_Click;
			name = FindViewById<EditText>(Resource.Id.editText2);
			startTimeButton = FindViewById<Button>(Resource.Id.button2);
			endTimeButton = FindViewById<Button>(Resource.Id.button3);
			extras = FindViewById<CheckBox>(Resource.Id.checkBox1);

			startTimeText = FindViewById<TextView>(Resource.Id.textView4);
			endTimeText = FindViewById<TextView>(Resource.Id.textView5);
			deleteButton = FindViewById<Button>(Resource.Id.button4);
			startTimeButton.Click += StartTimeDialog_Click;
			endTimeButton.Click += EndTimeDialog_Click;
			deleteButton.Click += DeleteButton_Click;

		    id = this.Intent.GetLongExtra("id",-1L);
			if (id != -1)
			{
				appointment = AppointmentDatabase.Instance.GetAppointment(id);
				mode = "edit";
				SetAppointmentView(mode);
			}
			else
			{
				mode = "create";
				deleteButton.Visibility = ViewStates.Invisible;
			}

		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			AppointmentDatabase.Instance.DeleteAppointment(id);
			Finish();
		}

		private void EndTimeDialog_Click(object sender, EventArgs e)
		{
			DateTime currently = DateTime.Now;
			TimePickerDialog dialog = new TimePickerDialog(this, SetEndTime, currently.Hour, currently.Minute, true);
			dialog.Show();
		}

		private void StartTimeDialog_Click(object sender, EventArgs e)
		{
			DateTime currently = DateTime.Now;
			TimePickerDialog dialog = new TimePickerDialog(this, SetStartTime, currently.Hour, currently.Minute, true);
			dialog.Show();
		}
		private void SetStartTime(object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			appointment.StartTime = new TimeSpan(e.HourOfDay,e.Minute,0);
			startTimeText.Text = e.HourOfDay + ":" + e.Minute;
		}
		private void SetEndTime(object sender, TimePickerDialog.TimeSetEventArgs e)
		{
			appointment.EndTime = new TimeSpan(e.HourOfDay, e.Minute, 0);
			endTimeText.Text = e.HourOfDay+":"+e.Minute;
		}
		private void SetAppointmentView(string mode)
		{
			name.Text = appointment.Name;
			startTimeText.Text = appointment.StartTime.ToString();
			endTimeText.Text = appointment.EndTime.ToString();
			extras.Checked = appointment.Extras;
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			appointment.Name = name.Text;
			appointment.Extras = extras.Checked;

			if (mode != "edit")
			{
				int[] array = this.Intent.GetIntArrayExtra("date");
				appointment.Date = new DateTime(array[0], array[1], array[2]);
				AppointmentDatabase.Instance.AddAppointment(appointment);

			}
			else
			{
				AppointmentDatabase.Instance.UpdateAppointment(appointment);
			}

			Finish();
		}
	}
}