using System;
using Android.Bluetooth.LE;
using BluetoothTestApp.Droid.Services.Uitilities;

namespace BluetoothTestApp.Droid.Services.BluetoothLeClient
{
    public class BluetoothLeScanCallback : ScanCallback
    {
        public EventHandler<BleScanCallbackEventArgs> OnBleScanCallback;

        public BluetoothLeScanCallback()
        {
        }

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
        {            
            OnBleScanCallback?.Invoke(this, new BleScanCallbackEventArgs(result));            
        }
    }
}
