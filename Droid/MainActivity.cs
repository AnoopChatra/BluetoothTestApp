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
        public static BluetoothLeAdvertiser mBluetoothLeAdvertiser;
        public static BluetoothGattServer mGattServer;
        public static BluetoothManager mBluetoothManager;

        private BluetoothAdapter mBluetoothAdapter;
       

        private BluetoothLeService mbluetoothLeService;

        private IList<BluetoothDevice> mConnectedDevices;
        private ArrayAdapter<BluetoothDevice> mConnectedDevicesAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            ActivityContext = this;

            mBluetoothManager = (BluetoothManager)GetSystemService(BluetoothService);
            mBluetoothAdapter = mBluetoothManager.Adapter;
            mBluetoothLeAdvertiser = mBluetoothAdapter.BluetoothLeAdvertiser;
            mbluetoothLeService = BluetoothLeService.Instance;
        }

        protected override void OnResume()
        {
            base.OnResume();
            /*
             * Make sure bluettoth is enabled
             */
            if (mBluetoothAdapter == null || !mBluetoothAdapter.IsEnabled)
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
            if (!mBluetoothAdapter.IsMultipleAdvertisementSupported)
            {
                Toast.MakeText(this, "No Advertising Support.",ToastLength.Short).Show();
                Finish();
                return;
            }

            
            mGattServer = mbluetoothLeService.OpenGattServer();           
            mbluetoothLeService.InitGattServer();
            mbluetoothLeService.StartGattAdvertising();
        }

        protected override void OnPause()
        {
            base.OnPause();

        }
    }
}

