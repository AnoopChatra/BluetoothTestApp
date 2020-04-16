using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using BluetoothTestApp.Models;
using BluetoothTestApp.Services;

namespace BluetoothTestApp.Droid.Views
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
            _buttonRegister.Click += NavigateToDashBoard;

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


        private void NavigateToDashBoard(object sender, EventArgs e)
        {
            if (IsEmailValid() && _editTextEmployeeId.Text.Length>0 )
            {
                //Task<bool> result =SendEmployeeDetailsToCloud();
                //if (result.Result)
                //{
                    Intent intent = new Intent(this, typeof(DashBoardActivity));
                    StartActivity(intent);
                //}
                //else
                //{
                //    Toast.MakeText(this, "Bad R", ToastLength.Long).Show();
                //}
            }
            else
                Toast.MakeText(this, "Please enter proper employee id and email address", ToastLength.Long).Show();
        }

        //private async Task<bool> SendEmployeeDetailsToCloud()
        //{
        //    Employee employee = new Employee()
        //    {
        //        employeeID = _editTextEmployeeId.Text,
        //        emailID = _editTextEmployeeEmail.Text
        //    };
        //    EmployeeService employeeService=new EmployeeService();
        //    Task < HttpStatusCode > =
        //                     await Task.Factory.StartNew(() => employeeService.RegisterEmployee(employee));
        //  if(result==HttpStatusCode.OK)
        //    {
        //        return true;
        //    }
        //    return false;
        
        //}
    }
}