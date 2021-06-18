using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Database;
using Google.Android.Material.BottomNavigation;
using Java.Util;
using ShareLock.Adapter;
using ShareLock.EventListeners;
using ShareLock.Fragments;
using ShareLock.Helpers;
using ShareLock.Models;
using System;
using System.Collections.Generic;
using Fragment = Android.Support.V4.App.Fragment;

namespace ShareLock
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        TextView editHome;
        TextView homeName;

        EditText HomeName;
        EditText HomeAddress;
        EditText HomeBio;
        Button SaveHome;


        MemberAdapter memberAdapter;
        DoorLockAdapter doorLockAdapter;


        List<Members> memberList;
        List<DoorLock> doorLockList;

        RecyclerView memberRecyle;
        RecyclerView doorLockRecyle;

        ImageView addMemberBtn;
        ImageView addDoorLockBtn;
        
        MemberListener memberListener;
        DoorLockListener doorLockListener;

        AddDoorLockFragment addDoorLockFragment;
        RequestApprovalFragment requestApprovalFragment;
        AddMemberFragment addmemberFragment;


        LinearLayout HomePage;
        LinearLayout VisitsPage;
        LinearLayout PaymentPage;
        LinearLayout ProfilePage;
        LinearLayout NotificationPage;
        LinearLayout HomeEditPage;

        ImageView profileButton;
        ImageView notificationButton;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            memberRecyle = (RecyclerView)FindViewById(Resource.Id.familyMembersRecyclerView);
            doorLockRecyle = (RecyclerView)FindViewById(Resource.Id.doorlocksRecyclerView);
            textMessage = FindViewById<TextView>(Resource.Id.message);
            editHome = FindViewById<TextView>(Resource.Id.editHomeButton);

            HomeName = (EditText)FindViewById(Resource.Id.homeNameTxt);
            HomeAddress = (EditText)FindViewById(Resource.Id.homeaddressTxt);
            HomeBio = (EditText)FindViewById(Resource.Id.homeBioText);
            SaveHome = (Button)FindViewById(Resource.Id.saveHomeBtn);

            homeName = (TextView)FindViewById(Resource.Id.HomeName); ///Needed filter Retrieve first

            SaveHome.Click += SaveHome_Click;

            addMemberBtn = (ImageView)FindViewById(Resource.Id.addMember);
            addDoorLockBtn = (ImageView)FindViewById(Resource.Id.addDoorLock);
            

            ConnectLayoutViews();


            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            //Home
            
            RetrievedData();
            
            
            addMemberBtn.Click += AddMemberBtn_Click;
            addDoorLockBtn.Click += AddDoorLockBtn_Click;
            editHome.Click += EditHome_Click;
        }

        private void SaveHome_Click(object sender, EventArgs e)
        {
            string homeName = HomeName.Text;
            string homeAddress = HomeAddress.Text;
            string homeBio = HomeBio.Text;

            

            HashMap homeInfo = new HashMap();
            homeInfo.Put("HomeName", homeName); //This should be replaced by fullname
            homeInfo.Put("HomeAddres", homeAddress);
            homeInfo.Put("HomeBio", homeBio);
            
                DatabaseReference newNoteRef = AppDataHelper.GetDatabase().GetReference("homeInfo").Push();
                newNoteRef.SetValue(homeInfo);
                Toast.MakeText(SaveHome.Context, "Home Save!", ToastLength.Short).Show();

            HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
            HomePage.Visibility = Android.Views.ViewStates.Visible;
        }

        private void EditHome_Click(object sender, EventArgs e)
        {
            //Check if This is Home is not null first;
            HomePage.Visibility = Android.Views.ViewStates.Gone;
            VisitsPage.Visibility = Android.Views.ViewStates.Gone;
            PaymentPage.Visibility = Android.Views.ViewStates.Gone;
            ProfilePage.Visibility = Android.Views.ViewStates.Gone;
            NotificationPage.Visibility = Android.Views.ViewStates.Gone;
            HomeEditPage.Visibility = Android.Views.ViewStates.Visible;
            textMessage.Text = "Home Edit";
        }

        private void AddDoorLockBtn_Click(object sender, EventArgs e)
        {
            addDoorLockFragment = new AddDoorLockFragment();
            var trans = SupportFragmentManager.BeginTransaction();
            addDoorLockFragment.Show(trans, "new door lock");
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
            HomeEditPage = (LinearLayout)FindViewById(Resource.Id.EditHomeLayout);

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
            HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
            textMessage.Text = "Notifications";
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            HomePage.Visibility = Android.Views.ViewStates.Gone;
            VisitsPage.Visibility = Android.Views.ViewStates.Gone;
            PaymentPage.Visibility = Android.Views.ViewStates.Gone;
            ProfilePage.Visibility = Android.Views.ViewStates.Visible;
            NotificationPage.Visibility = Android.Views.ViewStates.Gone;
            HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
            textMessage.Text = "Profile";
        }

        private void RetrievedData()
        {
            memberListener = new MemberListener();
            memberListener.Create();
            memberListener.MemberRetrived += MemberListener_MemberRetrived;

            doorLockListener = new DoorLockListener();
            doorLockListener.Create();
            doorLockListener.DoorLockRetrived += DoorLockListener_DoorLockRetrived;
        }

        private void DoorLockListener_DoorLockRetrived(object sender, DoorLockListener.DoorLockDataEventArgs e)
        {
            doorLockList = e.DoorLock;
            SetUpDoorLockRecycler();
        }

        private void SetUpDoorLockRecycler()
        {
            doorLockRecyle.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(doorLockRecyle.Context, LinearLayoutManager.Horizontal, false));
            doorLockAdapter = new DoorLockAdapter(doorLockList);

            doorLockAdapter.ItemClick += DoorLockAdapter_ItemClick;
            doorLockRecyle.SetAdapter(doorLockAdapter);
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

        private void DoorLockAdapter_ItemClick(object sender, DoorLockAdapterClickEventArgs e)
        {
            throw new NotImplementedException();
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
                    HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    HomePage.Visibility = Android.Views.ViewStates.Gone;
                    VisitsPage.Visibility = Android.Views.ViewStates.Visible;
                    PaymentPage.Visibility = Android.Views.ViewStates.Gone;
                    ProfilePage.Visibility = Android.Views.ViewStates.Gone;
                    NotificationPage.Visibility = Android.Views.ViewStates.Gone;
                    HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    HomePage.Visibility = Android.Views.ViewStates.Gone;
                    VisitsPage.Visibility = Android.Views.ViewStates.Gone;
                    PaymentPage.Visibility = Android.Views.ViewStates.Visible;
                    ProfilePage.Visibility = Android.Views.ViewStates.Gone;
                    NotificationPage.Visibility = Android.Views.ViewStates.Gone;
                    HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }
    }
}

