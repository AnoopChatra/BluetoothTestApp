using System;
using Android.Bluetooth.LE;

namespace BluetoothTestApp.Droid.Services
{
    public class BluetoothLeScanCallback : ScanCallback
    {
        public EventHandler<BleScanCallbackEventArgs> OnBleScanCallback;

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
        {            
            OnBleScanCallback?.Invoke(this, new BleScanCallbackEventArgs(result));            
        }
    }
}
