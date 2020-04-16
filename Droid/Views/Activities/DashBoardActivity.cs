using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.CardView.Widget;
using BluetoothTestApp.Droid.Services.Uitilities;
using BluetoothTestApp.Droid.Views.SelfDiagnosis;

namespace BluetoothTestApp.Droid
{
    [Activity(Label = "DashBoardActivity", Theme = "@style/ActionBarThemeWithBlue")]
    public class DashBoardActivity : AppCompatActivity
    {
        private CardView _cardViewSefDiagnosis;
        private ImageView _userImageView;
        private TextView _textViewUserName;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DashBoard);
            _cardViewSefDiagnosis= FindViewById<CardView>(Resource.Id.cardViewSelfDiagnosis);
            _userImageView = FindViewById<ImageView>(Resource.Id.userImage);
            _textViewUserName= FindViewById<TextView>(Resource.Id.textViewUserName);
            _userImageView.Click += OnClickUserImage;
            _cardViewSefDiagnosis.Click += NaviageToSelfDiagnosis;
            SetValue();
        }

        private void SetValue()
        {
            AppPreferences ap = new AppPreferences(Application.Context);
            _textViewUserName.Text =ap.GetAccessKey(SharedPreferenceKey.EmployeeId);
        }

        private void OnClickUserImage(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void NaviageToSelfDiagnosis(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SlefDiagnosisActivity1));
            StartActivity(intent);

        }
    }
}