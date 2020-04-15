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
using AndroidX.CardView.Widget;
using BluetoothTestApp.Droid.Views.SelfDiagnosis;

namespace BluetoothTestApp.Droid.Views
{
    [Activity(Label = "DashBoardActivity", Theme = "@style/ActionBarThemeWithBlue")]
    public class DashBoardActivity : AppCompatActivity
    {
        private CardView _cardViewSefDiagnosis;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DashBoard);
            _cardViewSefDiagnosis= FindViewById<CardView>(Resource.Id.cardViewSelfDiagnosis);
            _cardViewSefDiagnosis.Click += NaviageToSelfDiagnosis;
        }

        private void NaviageToSelfDiagnosis(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SlefDiagnosisActivity1));
            StartActivity(intent);
        }
    }
}