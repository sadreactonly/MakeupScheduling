using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MakeupScheduling.Adapters;
using MakeupScheduling.Models;

namespace MakeupScheduling
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
		ListView listView;
		CalendarView calendarView;

		DateTime selectedDate = DateTime.Now.Date;
		List<Appointment> appointments = new List<Appointment>();
		
		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

			listView = FindViewById<ListView>(Resource.Id.listView1); 
			listView.ItemClick += OnListItemClick; 
			calendarView = FindViewById<CalendarView>(Resource.Id.calendarView1);
			calendarView.DateChange += CalendarView_DateChange;
		}

		protected override void OnResume()
		{
			appointments = AppointmentDatabase.Instance.GetAppointmentsFromDay(selectedDate);
			listView.Adapter = new AppointmentsAdapter(this, appointments);
			base.OnResume();

		}
		private void CalendarView_DateChange(object sender, CalendarView.DateChangeEventArgs e)
		{
			selectedDate = new DateTime(e.Year,e.Month+1,e.DayOfMonth);
			appointments = AppointmentDatabase.Instance.GetAppointmentsFromDay(selectedDate);
			listView.Adapter = new AppointmentsAdapter(this, appointments);
		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var listView = sender as ListView;
			var t = appointments[e.Position];
			Toast.MakeText(this, t.Name, ToastLength.Short).Show();

			Intent intent = new Intent(this, typeof(NewAppointmentActivity));
			Bundle bundle = new Bundle();

			bundle.PutLong("id", t.Id);
			intent.PutExtras(bundle);
			StartActivity(intent);
		}

		public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
			if (selectedDate == null)
			{
				Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
				  .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
			}
			else
			{
				Intent intent = new Intent(this, typeof(NewAppointmentActivity));
				Bundle bundle = new Bundle();
				
				bundle.PutIntArray("date",new  int[3]{selectedDate.Year,selectedDate.Month,selectedDate.Day});
				intent.PutExtras(bundle);
				StartActivity(intent);
				//Finish();
			}
			
          
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_manage)
            {
				// Handle the camera action
				Intent intent = new Intent(this, typeof(ReportsActivity));
			    StartActivity(intent);
            }
         
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
      
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

