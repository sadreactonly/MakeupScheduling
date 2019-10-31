using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using MakeupScheduling.Models;
using PCLStorage;

namespace MakeupScheduling
{
	public class AppointmentDatabase
	{
		SQLiteConnection database;
		private static AppointmentDatabase instance = null;
		private static readonly object padlock = new object();
		public AppointmentDatabase()
		{
			database = GetConnection();
			database.CreateTable<Appointment>();
		}
		public SQLiteConnection GetConnection()
		{
			SQLiteConnection sqlitConnection;
			var sqliteFilename = "Appointments.db3";
			IFolder folder = FileSystem.Current.LocalStorage;
			string path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
			sqlitConnection = new SQLiteConnection(path);
			return sqlitConnection;
		}


		public static AppointmentDatabase Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new AppointmentDatabase();
					}
					return instance;
				}
			}
		}

		public Appointment GetAppointment(long id)
		{
			return database.Get<Appointment>(id);
		}
		public List<Appointment> GetAppointments()
		{
			var list = database.Table<Appointment>().ToList();
			list.Sort((x, y) => y.StartTime.CompareTo(x.StartTime));
			return list;
		}
		public List<Appointment> GetAppointmentsFromDay(DateTime date)
		{
			var list = database.Table<Appointment>().ToList().Where(x => x.Date==date).ToList();
			list.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
			return list;
		}
		public long AddAppointment(Appointment item)
		{
			database.Insert(item);
			return item.Id;
		}
		public long UpdateAppointment(Appointment item)
		{
			database.InsertOrReplace(item);
			return item.Id;
		}
		public int DeleteAppointment(long id)
		{
			
			return database.Delete<Appointment>(id);
			
		}

		public int DeleteAppointments()
		{
			return database.DeleteAll<Appointment>();		
		}

	}
}