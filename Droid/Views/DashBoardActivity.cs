using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace BluetoothTestApp.Droid.Views
{
    [Activity(Label = "DashBoardActivity", Theme = "@style/ActionBarTheme")]
    public class DashBoardActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DashBoard);
            // Create your application here
        }
    }
}