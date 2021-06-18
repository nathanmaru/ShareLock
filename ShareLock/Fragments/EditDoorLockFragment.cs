using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Java.Util;
using ShareLock.Helpers;
using ShareLock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareLock.Fragments
{
    public class EditDoorLockFragment : AndroidX.Fragment.App.DialogFragment
    {
        EditText DoorId;
        EditText Doorname;
        EditText Password;
        EditText Address;
        EditText OwnerName;
        Button Addbtn;
        ActiveUser activeusername;
        DoorLock thisDoorLock;
        public EditDoorLockFragment(DoorLock doorLock)
        {
            thisDoorLock = doorLock;
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.EditDoorLock, container, false);
            DoorId = (EditText)view.FindViewById(Resource.Id.doorLockId);
            Doorname = (EditText)view.FindViewById(Resource.Id.doorlockName);
            Password = (EditText)view.FindViewById(Resource.Id.doorLockPassword);
            Address = (EditText)view.FindViewById(Resource.Id.doorLockAddress);
            OwnerName = (EditText)view.FindViewById(Resource.Id.familyOwner);
            Addbtn = (Button)view.FindViewById(Resource.Id.EditDoorLockButton);

            Doorname.Text = thisDoorLock.DoorLockName;
            DoorId.Text = thisDoorLock.DoorLockId;
            Password.Text = thisDoorLock.Password;
            Address.Text = thisDoorLock.Address;
            OwnerName.Text = thisDoorLock.FamilyName;


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



            AndroidX.AppCompat.App.AlertDialog.Builder dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(Activity);
            dialog.SetTitle("Adding DoorLock");
            dialog.SetMessage("Are you sure?");
            dialog.SetPositiveButton("Continue", (senderAlert, args) =>
            {
                AppDataHelper.GetDatabase().GetReference("doorLockInfo/" + thisDoorLock.ID + "/DoorName").SetValue(doorName);
                AppDataHelper.GetDatabase().GetReference("doorLockInfo/" + thisDoorLock.ID + "/Key").SetValue(doorId);
                AppDataHelper.GetDatabase().GetReference("doorLockInfo/" + thisDoorLock.ID + "/Password").SetValue(password);
                AppDataHelper.GetDatabase().GetReference("doorLockInfo/" + thisDoorLock.ID + "/Address").SetValue(password);
                AppDataHelper.GetDatabase().GetReference("doorLockInfo/" + thisDoorLock.ID + "/OwnerName").SetValue(password);
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