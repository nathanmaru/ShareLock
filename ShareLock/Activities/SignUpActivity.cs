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
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        EditText fullname;
        EditText username;
        EditText email;
        EditText password;
        Button signUpBtn;
        TextView loginRedirector;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.SignUpLayout);

            fullname = (EditText)FindViewById(Resource.Id.fullnameTxt);
            username = (EditText)FindViewById(Resource.Id.usernameTxt);
            email = (EditText)FindViewById(Resource.Id.emailText);
            password = (EditText)FindViewById(Resource.Id.passwordTxt);
            signUpBtn = (Button)FindViewById(Resource.Id.signupBtn);
            loginRedirector = (TextView)FindViewById(Resource.Id.loginRedirect);

            signUpBtn.Click += SignUpBtn_Click;
            loginRedirector.Click += LoginRedirector_Click;

        }

        private void LoginRedirector_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }

        private void SignUpBtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}