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
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        EditText username;
        EditText password;
        Button loginBtn;
        TextView signupRedirector;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.LoginLayout);

            username = (EditText)FindViewById(Resource.Id.usernameTxt);
            password = (EditText)FindViewById(Resource.Id.passwordTxt);
            loginBtn = (Button)FindViewById(Resource.Id.loginBtn);
            signupRedirector = (TextView)FindViewById(Resource.Id.signupRedirect);

            loginBtn.Click += LoginBtn_Click;
            signupRedirector.Click += SignupRedirector_Click;
        }

        private void SignupRedirector_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SignUpActivity));
            StartActivity(intent);

        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}