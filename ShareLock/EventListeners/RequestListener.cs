using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using System;
using ShareLock.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShareLock.Helpers;

namespace ShareLock.EventListeners
{
    public class RequestListener : Java.Lang.Object, IValueEventListener
    {
        List<Request> requestList = new List<Request>();
        public event EventHandler<RequestDataEventArgs> RequestRetrived;

        public class RequestDataEventArgs : EventArgs
        {
            public List<Request> Request { get; set; }
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
                requestList.Clear();
                foreach (DataSnapshot memberData in child)
                {
                    Request request = new Request();
                    request.ID = memberData.Key;
                    request.DoorLockId = memberData.Child("DoorLockID").Value.ToString();
                    request.Fullname = memberData.Child("Fullname").Value.ToString();
                    request.Message = memberData.Child("Message").Value.ToString();
                    request.OwnerUsername = memberData.Child("OwnerUsername").Value.ToString();
                    request.isApprove = memberData.Child("isApprove").Value.ToString();
                    request.UserID = memberData.Child("UserID").Value.ToString();
                    request.OneTimePassword = memberData.Child("OTP").Value.ToString();

                    requestList.Add(request);
                }
                RequestRetrived.Invoke(this, new RequestDataEventArgs { Request = requestList });
            }
        }
        public void Create()
        {
            DatabaseReference requestRef = AppDataHelper.GetDatabase().GetReference("requestInfo");
            requestRef.AddValueEventListener(this);
        }
    }
}