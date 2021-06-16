using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareLock.Helpers
{
    public static class AppDataHelper
    {
        public static FirebaseDatabase GetDatabase()
        {
            var app = FirebaseApp.InitializeApp(Application.Context);
            FirebaseDatabase database;
            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                    .SetApplicationId("sharelock-de728")
                    .SetApiKey("AIzaSyCAl7gXvYnU7zbNe4ITzYOMNykHuogAJCQ")
                    .SetDatabaseUrl("https://sharelock-de728-default-rtdb.firebaseio.com/")
                    .SetStorageBucket("sharelock-de728.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(Application.Context, option);
                database = FirebaseDatabase.GetInstance(app);

            }
            else
            {
                database = FirebaseDatabase.GetInstance(app);
            }
            return database;
        }
    }
}