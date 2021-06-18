using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ShareLock.EventListeners;
using ShareLock.Models;
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
        List<Account> AccountList;
        AccountListener accountListener;

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
            RetriveData();

            loginBtn.Click += LoginBtn_Click;
            signupRedirector.Click += SignupRedirector_Click;
        }

        private void RetriveData()
        {
            accountListener = new AccountListener();
            accountListener.Create();
            accountListener.AccountRetrived += AccountListener_AccountRetrived;
        }

        private void AccountListener_AccountRetrived(object sender, AccountListener.AccountDataEventArgs e)
        {
            AccountList = e.Account;
        }

        private void SignupRedirector_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(SignUpActivity));
            StartActivity(intent);

        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            //Check Text Fields

            //Check from Existing Account
            if (CheckExistingAccounts() == 1)
            {
                LogIn();
                Toast.MakeText(loginBtn.Context, "Login Success!", ToastLength.Short).Show();
                var intent1 = new Intent(this, typeof(MainActivity));

                ActiveUser.username = username.Text;
                intent1.PutExtra("userName", username.Text);
                //pass username through extras
                StartActivity(intent1);
                
            }
            else
            {
                Toast.MakeText(loginBtn.Context, "Username or Password don't exist!", ToastLength.Short).Show();
            }
            
        }

        private int CheckExistingAccounts()
        {
            List<Account> SearchResult =
                (from account in AccountList
                 where account.Username.Contains(username.Text) &&
                 account.Password.Contains(password.Text)
                 select account).ToList();
            return SearchResult.Count;
        }

        private void LogIn()
        {
            
        }
    }
}