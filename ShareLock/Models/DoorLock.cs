﻿using Android.App;
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
    public class DoorLock
    {
        public string ID { get; set; }
        public string DoorLockName { get; set; }
        public string Password { get; set; }
    }
}