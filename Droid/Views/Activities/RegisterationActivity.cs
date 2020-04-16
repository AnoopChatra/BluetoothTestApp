using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using BluetoothTestApp.Droid.Services.Uitilities;
using BluetoothTestApp.Models;
using BluetoothTestApp.Services;

namespace BluetoothTestApp.Droid
{
    [Activity(Label = "RegisterationActivity", Theme = "@style/ActionBarThemeWithBlue")]
    public class RegisterationActivity : AppCompatActivity
    {
        private Button _buttonRegister;
        private EditText _editTextEmployeeId;
        private EditText _editTextEmployeeEmail;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegistrationLayout);
            _buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);
            _editTextEmployeeId= FindViewById<EditText>(Resource.Id.employeeId);
            _editTextEmployeeEmail = FindViewById<EditText>(Resource.Id.employeeEmail);
            _buttonRegister.Click += OnRegisterButtonClick;
           // ProgressBar();

        }

        private bool IsEmailValid()
        {
            if (Regex.IsMatch(_editTextEmployeeEmail.Text,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase))
            {
                return true;
            }
          
            return false;
        }


        private void OnRegisterButtonClick(object sender, EventArgs e)
        {
            if (IsEmailValid() && _editTextEmployeeId.Text.Length>0 )
            {
                SendEmployeeDetailsToCloud().ConfigureAwait(false);
                AppPreferences ap = new AppPreferences(Application.Context);
                ap.SaveAccessKey(SharedPreferenceKey.EmployeeId,_editTextEmployeeId.Text);
                ap.SaveAccessKey(SharedPreferenceKey.EmployeeEmail, _editTextEmployeeEmail.Text);
            }
            else
                Toast.MakeText(this, "Please enter proper employee id and email address", ToastLength.Long).Show();
        }

        private async Task SendEmployeeDetailsToCloud()
        {

            Employee employee = new Employee()
            {
                employeeID = _editTextEmployeeId.Text,
                emailID = _editTextEmployeeEmail.Text
            };
            EmployeeService employeeService = new EmployeeService();

            //start progressbar
            HttpStatusCode result = await employeeService.RegisterEmployeeAsync(employee);
            //stop progressbar
            if (result == HttpStatusCode.Created)
            {
                Intent intent = new Intent(this, typeof(DashBoardActivity));
                StartActivity(intent);
            }

            else
            {
                Toast.MakeText(this, "Bad R", ToastLength.Long).Show();
            }
        }

        public void ProgressBar()
        {

            RelativeLayout layout = new RelativeLayout(this);
            ProgressBar progressBar = new ProgressBar(this);
            progressBar.Indeterminate = true;
            progressBar.Visibility = ViewStates.Visible;
            RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(100, 100);
            parameters.AddRule(LayoutRules.CenterInParent);
          
         
             
            layout.AddView(progressBar,parameters);

            SetContentView(layout);
        }
    }
}