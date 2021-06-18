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
    public class Request
    {
        public string ID { get; set; }
        public string Fullname { get; set; }

        public string Message { get; set; }

        public string DoorLockId { get; set; } ///FK
        public string DoorLockName { get; set; }
        public string UserID { get; set; } ///FK
        public string isApprove { get; set; } ///FK
        public string OneTimePassword { get; set; } ///FK

    }
}