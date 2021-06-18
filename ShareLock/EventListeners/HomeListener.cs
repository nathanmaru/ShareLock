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
    public class HomeListener : Java.Lang.Object, IValueEventListener
    {
        List<Home> homeList = new List<Home>();
        public event EventHandler<HomeDataEventArgs> HomeRetrived;

        public class HomeDataEventArgs : EventArgs
        {
            public List<Home> Home { get; set; }
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
                homeList.Clear();
                foreach (DataSnapshot memberData in child)
                {
                    Home home = new Home();
                    home.ID = memberData.Key;
                    home.HomeName = memberData.Child("HomeName").Value.ToString();
                    home.HomeAddress = memberData.Child("HomeAddres").Value.ToString();
                    home.HomeBio = memberData.Child("HomeBio").Value.ToString();
                    
                    homeList.Add(home);
                }
                HomeRetrived.Invoke(this, new HomeDataEventArgs { Home = homeList });
            }
        }
        public void Create()
        {
            DatabaseReference homeRef = AppDataHelper.GetDatabase().GetReference("homeInfo");
            homeRef.AddValueEventListener(this);
        }
    }
}