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
    public class DoorLockListener : Java.Lang.Object, IValueEventListener
    {
        List<DoorLock> doorLockList = new List<DoorLock>();
        public event EventHandler<DoorLockDataEventArgs> DoorLockRetrived;
        public class DoorLockDataEventArgs : EventArgs
        {
            public List<DoorLock> DoorLock { get; set; }
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
                doorLockList.Clear();
                foreach (DataSnapshot memberData in child)
                {
                    DoorLock doorLock = new DoorLock();
                    doorLock.ID = memberData.Key;
                    doorLock.DoorLockId = memberData.Child("Key").Value.ToString();
                    doorLock.DoorLockName = memberData.Child("DoorName").Value.ToString();
                    doorLock.Password = memberData.Child("Password").Value.ToString();
                    doorLock.Username = memberData.Child("Username").Value.ToString();
                    doorLockList.Add(doorLock);
                }
                DoorLockRetrived.Invoke(this, new DoorLockDataEventArgs { DoorLock = doorLockList });
            }
        }
        public void Create()
        {
            DatabaseReference doorlockRef = AppDataHelper.GetDatabase().GetReference("doorLockInfo");
            doorlockRef.AddValueEventListener(this);
        }
    }
}