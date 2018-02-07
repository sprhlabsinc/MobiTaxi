
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	[Activity(Label = "MapActivity")]
	public class MapActivity : FragmentActivity, IOnMapReadyCallback, ILocationListener
	{
		Button menu_but, pick_location_but, search_but, destination_but;
		EditText search_txt;
		TextView result_txt;
		RelativeLayout contactus_layout;

		public Location _currentLocation;
		LocationManager _locationManager;

		string _locationProvider;
		public string _strAddress;
		GoogleMap map;
		Marker marker;

		public static MapActivity mapActivity;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_map);
			Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);   //keyboard disabled
			mapActivity = this;

			search_txt = FindViewById<EditText>(Resource.Id.search_txt);
			result_txt = FindViewById<TextView>(Resource.Id.result_txt);

			var menu = FindViewById<FlyOutContainer>(Resource.Id.FlyOutContainer);
			menu_but = FindViewById<Button>(Resource.Id.menu_but);
			menu_but.Click += (sender, e) =>
			{
				menu.AnimatedOpened = !menu.AnimatedOpened;
			};
			pick_location_but = FindViewById<Button>(Resource.Id.pick_location_but);
			pick_location_but.Click += delegate
			{
				StartActivity(new Intent(Application.Context, typeof(PickActivity)));
			};
			search_but = FindViewById<Button>(Resource.Id.search_but);
			search_but.Click += delegate
			{
				if (search_txt.Text.Length != 0)
				{
					result_txt.Text = search_txt.Text;
				}
			};
			destination_but = FindViewById<Button>(Resource.Id.destination_but);
			contactus_layout = FindViewById<RelativeLayout>(Resource.Id.contactus_layout);
			contactus_layout.Click += delegate
			{
				Finish();
				StartActivity(new Intent(Application.Context, typeof(MyRideActivity)));
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
