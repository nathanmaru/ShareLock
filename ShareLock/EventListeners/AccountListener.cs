using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using ShareLock.Helpers;
using ShareLock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareLock.EventListeners
{
    public class AccountListener : Java.Lang.Object, IValueEventListener
    {
        List<Account> accountList = new List<Account>();
        public event EventHandler<AccountDataEventArgs> AccountRetrived;

        public class AccountDataEventArgs : EventArgs
        {
            public List<Account> Account { get; set; }
        }
        public void OnCancelled(DatabaseError error)
        {
            throw new NotImplementedException();
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                accountList.Clear();
                foreach (DataSnapshot memberData in child)
                {
                    Account account = new Account();
                    account.ID = memberData.Key;
                    account.Fullname = memberData.Child("Fullname").Value.ToString();
                    account.Username = memberData.Child("Username").Value.ToString();
                    account.Email = memberData.Child("Email").Value.ToString();
                    account.Password = memberData.Child("Password").Value.ToString();
                    accountList.Add(account);
                }
                AccountRetrived.Invoke(this, new AccountDataEventArgs { Account = accountList });
            }
        }
        public void Create()
        {
            DatabaseReference accountRef = AppDataHelper.GetDatabase().GetReference("accountInfo");
            accountRef.AddValueEventListener(this);
        }
    }
}