
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MOB1TAXI
{
	[Activity(Label = "MyRideActivity")]
	public class MyRideActivity : FragmentActivity, IOnMapReadyCallback, ILocationListener
	{
		Button menu_but, cancel_ride_but, call_but, message_but, destination_but;
		TextView name_txt, address_txt, time_txt;
		RelativeLayout booking_layout;

		Location _currentLocation;
		LocationManager _locationManager;

		string _locationProvider;
		string _strAddress;
		GoogleMap map;
		Marker marker;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_myride);

			name_txt = FindViewById<TextView>(Resource.Id.name_txt);
			address_txt = FindViewById<TextView>(Resource.Id.address_txt);
			time_txt = FindViewById<TextView>(Resource.Id.time_txt);

			var menu = FindViewById<FlyOutContainer>(Resource.Id.FlyOutContainer);
			menu_but = FindViewById<Button>(Resource.Id.menu_but);
			menu_but.Click += (sender, e) =>
			{
				menu.AnimatedOpened = !menu.AnimatedOpened;
			};
			cancel_ride_but = FindViewById<Button>(Resource.Id.cancel_ride_but);
			cancel_ride_but.Click += delegate
			{
				Finish();
			};
			call_but = FindViewById<Button>(Resource.Id.call_but);
			call_but.Click += delegate
			{
				Toast.MakeText(this, "call button clicked!", ToastLength.Long).Show();
			};
			message_but = FindViewById<Button>(Resource.Id.message_but);
			message_but.Click += delegate
			{
				Toast.MakeText(this, "message button clicked!", ToastLength.Long).Show();
			};
			destination_but = FindViewById<Button>(Resource.Id.destination_but);
			destination_but.Click += delegate
			{
				Toast.MakeText(this, "destination button clicked!", ToastLength.Long).Show();
			};
			booking_layout = FindViewById<RelativeLayout>(Resource.Id.booking_layout);
			booking_layout.Click += delegate
			{
				Finish();
				StartActivity(new Intent(Application.Context, typeof(MapActivity)));
			};
			InitializeLocationManager();

			var mapFragment =
				(SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
			mapFragment.GetMapAsync(this);
		}

		void InitializeLocationManager()
		{
			_locationManager = (LocationManager)GetSystemService(LocationService);
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};
			IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				_locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				_locationProvider = string.Empty;
			}

			string bestProvider = _locationManager.GetBestProvider(criteriaForLocationService, true);
			Location location = _locationManager.GetLastKnownLocation(bestProvider);
			if (location != null)
			{
				OnLocationChanged(location);
			}
			_locationManager.RequestLocationUpdates(bestProvider, 300000, 0, this);
		}

		protected override void OnResume()
		{
			base.OnResume();
			_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
		}

		protected override void OnPause()
		{
			base.OnPause();
			_locationManager.RemoveUpdates(this);
		}

		public void OnLocationChanged(Location location)
		{
			try
			{
				_currentLocation = location;

				if (_currentLocation == null)
					Log.Debug("current location = ", "Unable to determine your location. Try again in a short while.");
				else
				{
					Log.Debug("current location = ", string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude));

					Geocoder geocoder = new Geocoder(this);

					//The Geocoder class retrieves a list of address from Google over the internet
					IList<Address> addressList = geocoder.GetFromLocation(_currentLocation.Latitude, _currentLocation.Longitude, 10);

					Address address = addressList.FirstOrDefault();

					if (address != null)
					{
						StringBuilder deviceAddress = new StringBuilder();

						for (int i = 0; i < address.MaxAddressLineIndex; i++)
							deviceAddress.Append(address.GetAddressLine(i))
								.AppendLine(",");

						_strAddress = deviceAddress.ToString();

						if (map != null)
						{
							display();
						}
					}
					else
						_strAddress = "Unable to determine the address.";
				}
			}
			catch
			{
				_strAddress = "Unable to determine the address.";
			}
		}

		public void OnProviderDisabled(string provider)
		{

		}

		public void OnProviderEnabled(string provider)
		{

		}

		public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{

		}

		public void OnMapReady(GoogleMap googleMap)
		{
			map = googleMap;
			map.UiSettings.ZoomControlsEnabled = false;
			//googleMap.MyLocationEnabled = true;

			if (_currentLocation != null)
			{

				display();
			}
		}

		public void display()
		{
			LatLng curLatLng = new LatLng(_currentLocation.Latitude, _currentLocation.Longitude);

			if (marker != null)
			{
				marker.Remove();

			}
			marker = map.AddMarker(new MarkerOptions()
							   .SetPosition(curLatLng)
							   .SetTitle(_strAddress));
			map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(curLatLng, 12));
		}
	}
}
