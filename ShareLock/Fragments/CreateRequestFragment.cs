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
    public class CreateRequestFragment : AndroidX.Fragment.App.DialogFragment
    {
        TextView familyName;
        TextView doorName;
        EditText message;
        Button sendRequest;
        DoorLock thisDoorLock;
        public CreateRequestFragment(DoorLock doorLock)
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
           View view = inflater.Inflate(Resource.Layout.SendRequest, container, false);
            //familyName = (TextView)view.FindViewById(Resource.Id.FamilyNameTxt);
            message = (EditText)view.FindViewById(Resource.Id.yourmessage);
            //doorName = (TextView)view.FindViewById(Resource.Id.doorLockName);
            sendRequest = (Button)view.FindViewById(Resource.Id.sendRequestBtn);
            
            sendRequest.Click += SendRequest_Click;
            return view;
        }

        private void SendRequest_Click(object sender, EventArgs e)
        {
            string Message = message.Text;
            //string DoorName = doorName.Text;

            HashMap requestInfo = new HashMap();
            requestInfo.Put("Visitorname", ActiveUser.username);
            requestInfo.Put("DoorName", thisDoorLock.DoorLockName);
            requestInfo.Put("OwnerUsername", thisDoorLock.Username);
            requestInfo.Put("Message", Message);
            AndroidX.AppCompat.App.AlertDialog.Builder dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(Activity);
            dialog.SetTitle("Requesting Permission");
            dialog.SetMessage("Are you sure?");
            dialog.SetPositiveButton("Continue", (senderAlert, args) =>
            {
                DatabaseReference newRequestRef = AppDataHelper.GetDatabase().GetReference("requestInfo").Push();
                newRequestRef.SetValue(requestInfo);
                Toast.MakeText(sendRequest.Context, "Sent Successfully!", ToastLength.Short).Show();
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