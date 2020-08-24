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
using MakeupScheduling.Models;

namespace MakeupScheduling
{
	[Activity(Label = "ReportsActivity")]
	public class ReportsActivity : Activity
	{
		private AppointmentDatabase appointmentDatabase = AppointmentDatabase.Instance;
		TextView monthlyEarnings;
		TextView monthlyAppointments;
		TextView monthlyWorkingDays;

		TextView yearlyEarnings;
		TextView yearlyAppointments;
		TextView yearlyWorkingDays;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.reports_layout);

			monthlyEarnings = FindViewById<TextView>(Resource.Id.textView1);
			monthlyAppointments = FindViewById<TextView>(Resource.Id.textView2);
			monthlyWorkingDays = FindViewById<TextView>(Resource.Id.textView3);

			yearlyEarnings = FindViewById<TextView>(Resource.Id.textView5);
			yearlyAppointments = FindViewById<TextView>(Resource.Id.textView6);
			yearlyWorkingDays = FindViewById<TextView>(Resource.Id.textView7);

			GenerateReports();
		}

		private void GenerateReports()
		{
			
			GenerateReport(appointmentDatabase.GetAppointments().Where(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year), ReportType.Monthly);
			
			GenerateReport(appointmentDatabase.GetAppointments().Where(x => x.Date.Year == DateTime.Now.Year), ReportType.Yearly);
		}

		private void GenerateReport(IEnumerable<Appointment> list, ReportType type)
		{
			float earning = 0;
			int appointmentsCount = list.Count();
			int workingDaysCount = list.GroupBy(x => x.Date).Count();
			foreach (var app in list)
			{
				earning += app.Price;
			}

			switch (type)
			{
				case ReportType.Monthly:
					monthlyEarnings.Text = "Earnings: " + earning;
					monthlyAppointments.Text = "Appointments: " + appointmentsCount;
					monthlyWorkingDays.Text = "Working days: " + workingDaysCount;
					break;
				case ReportType.Yearly:
					yearlyEarnings.Text = "Earnings: " + earning;
					yearlyAppointments.Text = "Appointments: " + appointmentsCount;
					yearlyWorkingDays.Text = "Working days: " + workingDaysCount;
					break;
			}
		}
	
	}
}