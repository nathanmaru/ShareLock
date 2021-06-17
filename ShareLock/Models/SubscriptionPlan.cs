using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareLock.Models
{
    public class SubscriptionPlan
    {
        public string ID { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Duration { get; set; }
        public string MemberLimit { get; set; }
        public string DoorLockLimit { get; set; }
    }
}