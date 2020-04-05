using Android.App;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using System.Collections.Generic;
using Android.Content;
using BluetoothTestApp.Droid.Services;

namespace BluetoothTestApp.Droid
{
    [Activity(Label = "BluetoothTestApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        public static Context ActivityContext= null;     

        private BluetoothAdapter _bluetoothAdapter; 
        private BluetoothLeGattService _bluetoothLeService;

        private IList<BluetoothDevice> mConnectedDevices;
        private ArrayAdapter<BluetoothDevice> mConnectedDevicesAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ActivityContext = this;

            _bluetoothAdapter = AndroidBluetoothServiceProvider.Instance.GetBluetoothAdapter();           
            _bluetoothLeService = BluetoothLeGattService.Instance;
        }

        protected override void OnResume()
        {
            base.OnResume();
            /*
             * Make sure bluettoth is enabled
             */
            if (_bluetoothAdapter == null || !_bluetoothAdapter.IsEnabled)
            {
                //Bluetooth is disabled
                Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivity(enableBtIntent);
                Finish();
                return;
            }

            /*
             * Check for advertising support.
            */
            if (!_bluetoothAdapter.IsMultipleAdvertisementSupported)
            {
                Toast.MakeText(this, "No Advertising Support.",ToastLength.Short).Show();
                Finish();
                return;
            }

            
            _bluetoothLeService.OpenGattServer();           
            _bluetoothLeService.InitGattServer();
            _bluetoothLeService.StartGattAdvertising("750210");
        }

        protected override void OnPause()
        {
            base.OnPause();

        }
    }
}

