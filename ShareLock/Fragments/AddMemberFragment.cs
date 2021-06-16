using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Database;
using Java.Util;
using ShareLock.Helpers;
using System;


namespace ShareLock.Fragments
{
    public class AddMemberFragment : AndroidX.Fragment.App.DialogFragment
    {
        EditText emailTxt;
        EditText roleTxt;
        Button addBtn;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.AddMember, container, false);
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            emailTxt = (EditText)view.FindViewById(Resource.Id.emailInput);
            roleTxt = (EditText)view.FindViewById(Resource.Id.roleInput);
            addBtn = (Button)view.FindViewById(Resource.Id.addMemberBtn);

            addBtn.Click += AddBtn_Click;
            return view;

            
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            string email = emailTxt.Text;
            string role = roleTxt.Text;

            //Do something to Retrieve FullName
            RetrieveFullName();

            HashMap memberInfo = new HashMap();
            memberInfo.Put("Email", email); //This should be replaced by fullname
            memberInfo.Put("Role", role);
            AndroidX.AppCompat.App.AlertDialog.Builder dialog = new AlertDialog.Builder(Activity);
            dialog.SetTitle("Adding Member");
            dialog.SetMessage("Are you sure your want to add this user as a member?");
            dialog.SetPositiveButton("Continue", (senderAlert, args) =>
            {
                DatabaseReference newNoteRef = AppDataHelper.GetDatabase().GetReference("memberInfo").Push();
                newNoteRef.SetValue(memberInfo);
                Toast.MakeText(addBtn.Context, "Member Added!", ToastLength.Short).Show();
                this.Dismiss();
            });
            dialog.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                dialog.Dispose();
            });
            dialog.Show();
        }

        private void RetrieveFullName()
        {
            
        }
    }
}