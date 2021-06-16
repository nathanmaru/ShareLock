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
    public class MemberListener : Java.Lang.Object, IValueEventListener
    {
        List<Members> memberList = new List<Members>();
        public event EventHandler<MemberDataEventArgs> MemberRetrived;

        public class MemberDataEventArgs : EventArgs
        {
            public List<Members> Member { get; set; }
        }
        public void OnCancelled(DatabaseError error)
        {
            
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                memberList.Clear();
                foreach (DataSnapshot memberData in child)
                {
                    Members member = new Members();
                    member.ID = memberData.Key;
                    member.Email = memberData.Child("Email").Value.ToString();
                    member.Role = memberData.Child("Role").Value.ToString();
                    memberList.Add(member);
                }
                MemberRetrived.Invoke(this, new MemberDataEventArgs { Member = memberList });
            }
        }
        public void Create()
        {
            DatabaseReference memberRef = AppDataHelper.GetDatabase().GetReference("memberInfo");
            memberRef.AddValueEventListener(this);
        }
    }
}