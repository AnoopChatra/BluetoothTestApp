using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace BluetoothTestApp.Droid
{
    [Activity(MainLauncher = true, Label = "Details", Icon = "@mipmap/icon")]
    public class WelcomeActivity : Activity
    {
        private EditText etEmployeeId;
        private Button btViewNearBy;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WelcomePage);

            etEmployeeId = FindViewById<EditText>(Resource.Id.etEmployeeId);
            btViewNearBy = FindViewById<Button>(Resource.Id.btViewNearby);

            btViewNearBy.Click += OnbtViewNearByClick;

        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        private void OnbtViewNearByClick(object sender, EventArgs e)
        {
            string employeeIdString = etEmployeeId.Text;            

            if (string.IsNullOrEmpty(employeeIdString) || employeeIdString.Length < 4 || employeeIdString.Equals("0000"))
            {
                Toast.MakeText(this, "Please enter minimum 4 digit employee id", ToastLength.Long).Show();
            }
            else
            {
                int employeeId = int.Parse(employeeIdString);
                Intent intent = new Intent(this, typeof(MainActivity));               
                intent.PutExtra("EmployeeId", employeeId);               
                StartActivity(intent);
            }
        }

    }
}
