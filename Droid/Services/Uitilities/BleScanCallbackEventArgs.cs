using System;
using Android.Bluetooth.LE;

namespace BluetoothTestApp.Droid.Services.Uitilities
{
    public class BleScanCallbackEventArgs : EventArgs
    {
        public ScanResult ScanResult { get; set; }

        public BleScanCallbackEventArgs(ScanResult scanResult)
        {
            ScanResult = scanResult;
        }
    }
}
