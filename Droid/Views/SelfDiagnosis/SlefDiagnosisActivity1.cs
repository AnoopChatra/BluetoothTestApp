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

namespace BluetoothTestApp.Droid.Views.SelfDiagnosis
{
    [Activity(Label = "SlefDiagnosisActivity1", Theme = "@style/ActionBarThemeWithBlue")]
    public class SlefDiagnosisActivity1 : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelfDiagnosisPage1);
        }
    }
}