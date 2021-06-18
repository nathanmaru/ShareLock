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
using System.Linq;
using Fragment = Android.Support.V4.App.Fragment;

namespace ShareLock
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        //TextView editHome;
        //TextView homeName;

        EditText SearchHometxt;
        EditText HomeName;
        EditText HomeAddress;
        EditText HomeBio;
        Button SaveHome;


        MemberAdapter memberAdapter;
        DoorLockAdapter doorLockAdapter;
        SearchHomeAdapter searchHomeAdapter;
        //SavedHomeAdapter savedHomeAdapter;


        List<Members> memberList;
        List<DoorLock> doorLockList;
        //List<Home> homeList;

        //ActiveUser activeUsername;

        RecyclerView memberRecyle;
        RecyclerView doorLockRecyle;
        RecyclerView homeSearchRecycle;
        //RecyclerView homeSavedRecycle;

        ImageView addMemberBtn;
        ImageView addDoorLockBtn;
        
        MemberListener memberListener;
        DoorLockListener doorLockListener;
        //HomeListener homeListener;

        AddDoorLockFragment addDoorLockFragment;
        //RequestApprovalFragment requestApprovalFragment;
        AddMemberFragment addmemberFragment;


        LinearLayout HomePage;
        LinearLayout VisitsPage;
        LinearLayout PaymentPage;
        LinearLayout ProfilePage;
        LinearLayout NotificationPage;
        LinearLayout HomeEditPage;
        LinearLayout SearchHome;

        ImageView profileButton;
        ImageView notificationButton;
        ImageView searchHomeButton;
        string Username;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            memberRecyle = (RecyclerView)FindViewById(Resource.Id.familyMembersRecyclerView);
            doorLockRecyle = (RecyclerView)FindViewById(Resource.Id.doorlocksRecyclerView);
            homeSearchRecycle = (RecyclerView)FindViewById(Resource.Id.HomeSearchRecyclerView);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            //editHome = FindViewById<TextView>(Resource.Id.editHomeButton);

            HomeName = (EditText)FindViewById(Resource.Id.homeNameTxt);
            HomeAddress = (EditText)FindViewById(Resource.Id.homeaddressTxt);
            HomeBio = (EditText)FindViewById(Resource.Id.homeBioText);
            SaveHome = (Button)FindViewById(Resource.Id.saveHomeBtn);

            SearchHometxt = (EditText)FindViewById(Resource.Id.searchHomeTxt);

            SearchHometxt.TextChanged += SearchHometxt_TextChanged;

            //homeName = (TextView)FindViewById(Resource.Id.HomeName); ///Needed filter Retrieve first

            SaveHome.Click += SaveHome_Click;

            addMemberBtn = (ImageView)FindViewById(Resource.Id.addMember);
            addDoorLockBtn = (ImageView)FindViewById(Resource.Id.addDoorLock);
            searchHomeButton = (ImageView)FindViewById(Resource.Id.addSavedHome);


            ConnectLayoutViews();


            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            //Home
            
            RetrievedData();
            Username = Intent.GetStringExtra("userName");


            addMemberBtn.Click += AddMemberBtn_Click;
            addDoorLockBtn.Click += AddDoorLockBtn_Click;
            //editHome.Click += EditHome_Click;
            searchHomeButton.Click += SearchHomeButton_Click;
        }

        private void SearchHomeButton_Click(object sender, EventArgs e)
        {
            HomePage.Visibility = Android.Views.ViewStates.Gone;
            VisitsPage.Visibility = Android.Views.ViewStates.Gone;
            PaymentPage.Visibility = Android.Views.ViewStates.Gone;
            ProfilePage.Visibility = Android.Views.ViewStates.Gone;
            NotificationPage.Visibility = Android.Views.ViewStates.Gone;
            HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
            SearchHome.Visibility = Android.Views.ViewStates.Visible;
        }

        private void SearchHometxt_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            ///
            List<DoorLock> SearchResult =
                (from doorlock in doorLockList
                 where doorlock.Address.ToLower().Contains(SearchHometxt.Text.ToLower()) ||
                 doorlock.FamilyName.ToLower().Contains(SearchHometxt.Text.ToLower())
                 select doorlock).ToList();
            doorLockAdapter = new DoorLockAdapter(SearchResult);
            homeSearchRecycle.SetAdapter(doorLockAdapter);
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
            SearchHome.Visibility = Android.Views.ViewStates.Gone;
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
            SearchHome = (LinearLayout)FindViewById(Resource.Id.SearchHomeLayout);

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
            SearchHome.Visibility = Android.Views.ViewStates.Gone;
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
            SearchHome.Visibility = Android.Views.ViewStates.Gone;
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

            /*homeListener = new HomeListener();
            homeListener.Create();
            homeListener.HomeRetrived += HomeListener_HomeRetrived;*/
        }

      

        private void SetupSearchHomeRecycler()
        {
            homeSearchRecycle.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(homeSearchRecycle.Context));
            searchHomeAdapter = new SearchHomeAdapter(doorLockList);

            searchHomeAdapter.ItemClick += SearchHomeAdapter_ItemClick;
            homeSearchRecycle.SetAdapter(searchHomeAdapter);
        }

        private void SearchHomeAdapter_ItemClick(object sender, SearchHomeAdapterClickEventArgs e)
        {
            
        }

        private void DoorLockListener_DoorLockRetrived(object sender, DoorLockListener.DoorLockDataEventArgs e)
        {
            doorLockList = e.DoorLock;
            FilterDoorLocks();
            SetUpDoorLockRecycler();
            SetupSearchHomeRecycler();
        }

        private void FilterDoorLocks()
        {
            List<DoorLock> filterLocks = (from doorlock in doorLockList
                                          where doorlock.Username.Contains(ActiveUser.username)
                                          select doorlock).ToList();
            doorLockAdapter = new DoorLockAdapter(filterLocks);
            doorLockRecyle.SetAdapter(doorLockAdapter);
        }

        private void SetUpDoorLockRecycler()
        {
            doorLockRecyle.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(doorLockRecyle.Context));
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
            DoorLock thisDoorLock = doorLockList[e.Position];
            EditDoorLockFragment editDoorLockFragment = new EditDoorLockFragment(thisDoorLock);
            var trans = SupportFragmentManager.BeginTransaction();
            editDoorLockFragment.Show(trans, "edit");
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
                    SearchHome.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    HomePage.Visibility = Android.Views.ViewStates.Gone;
                    VisitsPage.Visibility = Android.Views.ViewStates.Visible;
                    PaymentPage.Visibility = Android.Views.ViewStates.Gone;
                    ProfilePage.Visibility = Android.Views.ViewStates.Gone;
                    NotificationPage.Visibility = Android.Views.ViewStates.Gone;
                    HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
                    SearchHome.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    HomePage.Visibility = Android.Views.ViewStates.Gone;
                    VisitsPage.Visibility = Android.Views.ViewStates.Gone;
                    PaymentPage.Visibility = Android.Views.ViewStates.Visible;
                    ProfilePage.Visibility = Android.Views.ViewStates.Gone;
                    NotificationPage.Visibility = Android.Views.ViewStates.Gone;
                    HomeEditPage.Visibility = Android.Views.ViewStates.Gone;
                    SearchHome.Visibility = Android.Views.ViewStates.Gone;
                    textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }
    }
}

