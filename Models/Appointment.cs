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
using SQLite;

namespace MakeupScheduling.Models
{
	public class Appointment
	{
		[PrimaryKey, AutoIncrement]
		public long Id { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public string Name { get; set; }
		public float Price { get; set; } = 1000;
		public bool Extras { get; set; } = false;
		public DateTime Date { get; set; }
	}
}