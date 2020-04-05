using System;
using Android.Bluetooth;
using Android.Content;

namespace BluetoothTestApp.Droid.Services
{
    public class AndroidBluetoothServiceProvider
    {
        private static readonly AndroidBluetoothServiceProvider _instance = null;
        private BluetoothManager _bluetoothManager;
        private BluetoothAdapter _bluetoothAdapter;        

        public static AndroidBluetoothServiceProvider Instance
        {
            get
            {
                if (null == _instance)
                {
                    return new AndroidBluetoothServiceProvider();
                }
                else
                {
                    return _instance;
                }
            }
        }

        private AndroidBluetoothServiceProvider()
        {
        }

        public BluetoothManager GetBluetoothManager()
        {
            if(null == _bluetoothManager)
            {
               return (BluetoothManager)MainActivity.ActivityContext.GetSystemService(Context.BluetoothService);
            }
            else
            {
                return _bluetoothManager;
            }
        }

        public BluetoothAdapter GetBluetoothAdapter()
        {
            if(null == _bluetoothAdapter)
            {
                if(null == _bluetoothManager)
                {
                   return GetBluetoothManager().Adapter;
                }
                else
                {
                    return _bluetoothManager.Adapter;
                }                
            }
            else
            {
                return _bluetoothAdapter;
            }
        }
    }
}
