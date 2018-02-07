
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MOB1TAXI
{
	[Activity(Label = "LoginActivity")]
	public class LoginActivity : Activity
	{
		Button login_but, register_but, facebook_login_but, forgotpassword_but;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_login);

			Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);   //keyboard disabled

			register_but = FindViewById<Button>(Resource.Id.register_but);
			register_but.Click += delegate
			{
				Finish();
				StartActivity(new Intent(Application.Context, typeof(RegisterActivity)));
			};
			login_but = FindViewById<Button>(Resource.Id.app_login_but);
			login_but.Click += delegate
			{
				StartActivity(new Intent(Application.Context, typeof(MapActivity)));
			};
			facebook_login_but = FindViewById<Button>(Resource.Id.facebook_login_but);
			facebook_login_but.Click += delegate
			{
				//StartActivity(new Intent(Application.Context, typeof(MyRideActivity)));
			};
			forgotpassword_but = FindViewById<Button>(Resource.Id.forgotpassword_but);
			forgotpassword_but.Click += delegate
			{
				Toast.MakeText(this, "forgot password button clicked!", ToastLength.Long).Show();
			};
		}
	}
}
