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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareLock.Fragments
{
    public class RequestApprovalFragment : AndroidX.Fragment.App.DialogFragment
    {
        TextView visitorName;
        TextView doorName;
        TextView message;
        Button approveBtn;
        Button declineBtn;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.VisitNeedApproveLayout, container, false);
            visitorName = (TextView)view.FindViewById(Resource.Id.visitorName);
            doorName = (TextView)view.FindViewById(Resource.Id.doorName);
            message = (TextView)view.FindViewById(Resource.Id.messageTxt);
            approveBtn = (Button)view.FindViewById(Resource.Id.approveBtn);
            declineBtn = (Button)view.FindViewById(Resource.Id.declineBtn);

            approveBtn.Click += ApproveBtn_Click;
            declineBtn.Click += DeclineBtn_Click;

            return view;
        }

        private void DeclineBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void ApproveBtn_Click(object sender, EventArgs e)
        {
            string visitorname = visitorName.Text;
            string doorname = doorName.Text;
            string messageTxt = message.Text;
            //also pass home id

            //Do something to Retrieve FullName
            //RetrieveFullName();

            HashMap requestInfo = new HashMap();
            requestInfo.Put("Visitorname", visitorname);
            requestInfo.Put("DoorName", doorname);
            requestInfo.Put("Message", messageTxt);

            AndroidX.AppCompat.App.AlertDialog.Builder dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(Activity);
            dialog.SetTitle("Approving Visitor");
            dialog.SetMessage("Are you sure your want to give this person a one time password?");
            dialog.SetPositiveButton("Continue", (senderAlert, args) =>
            {
                DatabaseReference newRequestRef = AppDataHelper.GetDatabase().GetReference("requestInfo").Push();
                newRequestRef.SetValue(requestInfo);
                Toast.MakeText(approveBtn.Context, "Approve Successfully!", ToastLength.Short).Show();
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