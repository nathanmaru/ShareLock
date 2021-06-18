using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Database;
using ShareLock.Helpers;
using ShareLock.Models;

namespace ShareLock.Fragments
{
    public class AddDoorLockFragment : AndroidX.Fragment.App.DialogFragment
    {
        EditText DoorId;
        EditText Doorname;
        EditText Password;
        EditText Address;
        EditText OwnerName;
        Button Addbtn;
        ActiveUser activeusername;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.AddDoorLocksLayout, container, false);
            DoorId = (EditText)view.FindViewById(Resource.Id.doorLockId);
            Doorname = (EditText)view.FindViewById(Resource.Id.doorLockName);
            Password = (EditText)view.FindViewById(Resource.Id.doorLockPassword);
            Address = (EditText)view.FindViewById(Resource.Id.doorLockAddress);
            OwnerName = (EditText)view.FindViewById(Resource.Id.familyOwner);
            Addbtn = (Button)view.FindViewById(Resource.Id.addDoorLockBtn);

            Addbtn.Click += Addbtn_Click;

            return view;
        }

        private void Addbtn_Click(object sender, EventArgs e)
        {
            string doorId = DoorId.Text;
            string doorName = Doorname.Text;
            string password = Password.Text;
            string address = Address.Text;
            string ownername = OwnerName.Text;
            //also pass home id

            //Do something to Retrieve FullName
            //RetrieveFullName();

            HashMap doorlockInfo = new HashMap();
            doorlockInfo.Put("Key", doorId); 
            doorlockInfo.Put("DoorName", doorName);
            doorlockInfo.Put("Password", password);
            doorlockInfo.Put("Username", ActiveUser.username);
            doorlockInfo.Put("Address", address);
            doorlockInfo.Put("FamilyName", ownername);
            doorlockInfo.Put("OTP", "not set");

            AndroidX.AppCompat.App.AlertDialog.Builder dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(Activity);
            dialog.SetTitle("Adding DoorLock");
            dialog.SetMessage("Are you sure?");
            dialog.SetPositiveButton("Continue", (senderAlert, args) =>
            {
                DatabaseReference newNoteRef = AppDataHelper.GetDatabase().GetReference("doorLockInfo").Push();
                newNoteRef.SetValue(doorlockInfo);
                Toast.MakeText(Addbtn.Context, "DoorLock Added!", ToastLength.Short).Show();
                this.Dismiss();
            });
            dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                dialog.Dispose();
            });
            dialog.Show();
        }
    }
}