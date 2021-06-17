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
        ImageView addMemberBtn;
        AddMemberFragment addmemberFragment;
        MemberListener memberListener;

        LinearLayout HomePage;
        LinearLayout VisitsPage;
        LinearLayout PaymentPage;
        LinearLayout ProfilePage;
        LinearLayout NotificationPage;

        ImageView profileButton;
        ImageView notificationButton;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            memberRecyle = (RecyclerView)FindViewById(Resource.Id.familyMembersRecyclerView);
            textMessage = FindViewById<TextView>(Resource.Id.message);
            
            
            addMemberBtn = (ImageView)FindViewById(Resource.Id.addMember);

            ConnectLayoutViews();


            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            //Home
            
            RetrievedData();
            
            
            addMemberBtn.Click += AddMemberBtn_Click;
        }

        private void ConnectLayoutViews()
        {
            HomePage = (LinearLayout)FindViewById(Resource.Id.HomeLayout);
            VisitsPage = (LinearLayout)FindViewById(Resource.Id.VisitLayout);
            PaymentPage = (LinearLayout)FindViewById(Resource.Id.PaymentLayout);
            ProfilePage = (LinearLayout)FindViewById(Resource.Id.ProfileLayout);
            NotificationPage = (LinearLayout)FindViewById(Resource.Id.NotificationLayout);
            profileButton = (ImageView)FindViewById(Resource.Id.profile);
            notificationButton = (ImageView)FindViewById(Resource.Id.notification);

            profileButton.Click += ProfileButton_Click;
            notificationButton.Click += NotificationButton_Click;
        }

        private void NotificationButton_Click(object sender, EventArgs e)
        {
            HomePage.Visibility = Android.Views.ViewStates.Gone;
            VisitsPage.Visibility = Android.Views.ViewStates.Gone;
            PaymentPage.Visibility = Android.Views.ViewStates.Gone;
            ProfilePage.Visibility = Android.Views.ViewStates.Gone;
            NotificationPage.Visibility = Android.Views.ViewStates.Visible;
            textMessage.Text = "Notifications";
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            HomePage.Visibility = Android.Views.ViewStates.Gone;
            VisitsPage.Visibility = Android.Views.ViewStates.Gone;
            PaymentPage.Visibility = Android.Views.ViewStates.Gone;
            ProfilePage.Visibility = Android.Views.ViewStates.Visible;
            NotificationPage.Visibility = Android.Views.ViewStates.Gone;
            textMessage.Text = "Profile";
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
                    HomePage.Visibility = Android.Views.ViewStates.Visible;
                    VisitsPage.Visibility = Android.Views.ViewStates.Gone;
                    PaymentPage.Visibility = Android.Views.ViewStates.Gone;
                    ProfilePage.Visibility = Android.Views.ViewStates.Gone;
                    NotificationPage.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    HomePage.Visibility = Android.Views.ViewStates.Gone;
                    VisitsPage.Visibility = Android.Views.ViewStates.Visible;
                    PaymentPage.Visibility = Android.Views.ViewStates.Gone;
                    ProfilePage.Visibility = Android.Views.ViewStates.Gone;
                    NotificationPage.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    HomePage.Visibility = Android.Views.ViewStates.Gone;
                    VisitsPage.Visibility = Android.Views.ViewStates.Gone;
                    PaymentPage.Visibility = Android.Views.ViewStates.Visible;
                    ProfilePage.Visibility = Android.Views.ViewStates.Gone;
                    NotificationPage.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }
    }
}

