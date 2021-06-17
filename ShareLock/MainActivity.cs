using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using ShareLock.Adapter;
using ShareLock.EventListeners;
using ShareLock.Fragments;
using ShareLock.Models;
using System;
using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;

namespace ShareLock
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        RecyclerView memberRecyle;
        MemberAdapter memberAdapter;
        List<Members> memberList;
        TextView homeBtn;
        EditText homeEdit;
        ImageView addMemberBtn;
        AddMemberFragment addmemberFragment;
        MemberListener memberListener;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            memberRecyle = (RecyclerView)FindViewById(Resource.Id.familyMembersRecyclerView);
            textMessage = FindViewById<TextView>(Resource.Id.message);
            homeBtn = (TextView)FindViewById(Resource.Id.HomeNameBtn);
            homeEdit =(EditText)FindViewById(Resource.Id.homenameTxt);
            addMemberBtn = (ImageView)FindViewById(Resource.Id.addMember);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            //Home
            //CreateDummyData();
            RetrievedData();
            
            homeBtn.Click += HomeBtn_Click;
            addMemberBtn.Click += AddMemberBtn_Click;
        }

        private void RetrievedData()
        {
            memberListener = new MemberListener();
            memberListener.Create();
            memberListener.MemberRetrived += MemberListener_MemberRetrived;
        }

        private void MemberListener_MemberRetrived(object sender, MemberListener.MemberDataEventArgs e)
        {
            memberList = e.Member;
            SetUpMemberRecycler();
        }

        private void AddMemberBtn_Click(object sender, EventArgs e)
        {
            addmemberFragment = new AddMemberFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            addmemberFragment.Show(trans, "new member");
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            if (homeBtn.Visibility == Android.Views.ViewStates.Visible)
            {
                homeBtn.Visibility = Android.Views.ViewStates.Gone;
                homeEdit.Visibility = Android.Views.ViewStates.Visible;
            }
            else
            {
                homeEdit.ClearFocus();
                homeBtn.Visibility = Android.Views.ViewStates.Visible;
                homeEdit.Visibility = Android.Views.ViewStates.Gone;
            }
        }

        private void CreateDummyData()
        {
            memberList = new List<Members>();
            memberList.Add(new Members { Fullname = "Jonathan D. Aplacador", ID = "1", ProfilePictureID = "1", Role = "Son" });
            memberList.Add(new Members { Fullname = "Christine Joy D. Aplacador", ID = "1", ProfilePictureID = "1", Role = "Daughter" });
            memberList.Add(new Members { Fullname = "Jericho D. Aplacador", ID = "1", ProfilePictureID = "1", Role = "Son" });
            memberList.Add(new Members { Fullname = "Kassandra Jane D. Aplacador", ID = "1", ProfilePictureID = "1", Role = "Daughter" });
        }

        private void SetUpMemberRecycler()
        {
            memberRecyle.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(memberRecyle.Context, LinearLayoutManager.Horizontal, false));
            memberAdapter = new MemberAdapter(memberList);

            memberAdapter.ItemClick += MemberAdapter_ItemClick;
            memberRecyle.SetAdapter(memberAdapter);
        }

        private void MemberAdapter_ItemClick(object sender, MemberAdapterClickEventArgs e)
        {
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }
    }
}

