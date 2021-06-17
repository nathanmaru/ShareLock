using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareLock.Activities
{
    [Activity(Label = "GetStartedActivity", Theme = "@style/AppTheme", MainLauncher = true)]
    public class GetStartedActivity : Activity
    {
        Button signUpRedirect;
        TextView loginRedirect;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.GetStartedLayout);

            signUpRedirect = (Button)FindViewById(Resource.Id.getstartedBtn);
            loginRedirect = (TextView)FindViewById(Resource.Id.loginBtn);

            signUpRedirect.Click += SignUpRedirect_Click;
            loginRedirect.Click += LoginRedirect_Click;
        }

        private void LoginRedirect_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }

        private void SignUpRedirect_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }
    }
}