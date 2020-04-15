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
    [Activity(Label = "RegisterationActivity", Theme = "@style/ActionBarThemeWithBlue")]
    public class RegisterationActivity : AppCompatActivity
    {
        private Button _buttonRegister;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegistrationLayout);
            _buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);
            _buttonRegister.Click += NavigateToDashBoard;

        }

        private void NavigateToDashBoard(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(DashBoardActivity));
            StartActivity(intent);
        }
    }
}