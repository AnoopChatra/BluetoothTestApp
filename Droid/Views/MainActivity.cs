using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using System.Collections.Generic;
using Android.Content;
using BluetoothTestApp.Droid.Services;
using Android.Support.V4.App;
using Android;
using Android.Content.PM;
using BluetoothTestApp.Droid.Services.BluetoothLeClient;

namespace BluetoothTestApp.Droid
{
    [Activity(Label = "BluetoothTestApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public static Context ActivityContext= null;
        private const int Request_Fine_Location = 0;
        private int employeeId;

        private BluetoothAdapter _bluetoothAdapter; 
        private BluetoothLeGattService _bluetoothLeService;
        private BluetoothLEClientService _bluetoothLeclintService;

        private IList<BluetoothDevice> mConnectedDevices;
        private ArrayAdapter<BluetoothDevice> mConnectedDevicesAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ActivityContext = this;

            employeeId = Intent.GetIntExtra("EmployeeId", 0);
             
            _bluetoothAdapter = AndroidBluetoothServiceProvider.Instance.GetBluetoothAdapter();           
            _bluetoothLeService = BluetoothLeGattService.Instance;
            _bluetoothLeclintService = BluetoothLEClientService.Instance;
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_bluetoothAdapter == null || !_bluetoothAdapter.IsEnabled)
            {
                //Bluetooth is disabled
                Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivity(enableBtIntent);
                Finish();
                return;
            }
           
            if (!_bluetoothAdapter.IsMultipleAdvertisementSupported)
            {
                Toast.MakeText(this, "No Advertising Support.",ToastLength.Short).Show();
                Finish();
                return;
            }
            
            _bluetoothLeService.OpenGattServer();           
            _bluetoothLeService.InitGattServer();
            _bluetoothLeService.StartGattAdvertising(employeeId);

            RequestLocationPermission();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
           
            if (requestCode == Request_Fine_Location)
            {
                if (grantResults[0] == Permission.Granted)
                {
                    _bluetoothLeclintService.StartBluetoothLeScan();
                }
                else
                {
                    RequestLocationPermission();
                }
                
                   
                
            }
        }

        private void RequestLocationPermission()
        {
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                // Camera permission has not been granted
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessFineLocation }, Request_Fine_Location);
            }
            else
            {
                _bluetoothLeclintService.StartBluetoothLeScan();
            }
        }
    }
}

