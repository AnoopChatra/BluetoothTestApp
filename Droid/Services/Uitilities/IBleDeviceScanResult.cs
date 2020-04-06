using System;
namespace BluetoothTestApp.Droid.Services.Uitilities
{
    public interface IBleDeviceScanResult
    {
        void OnNewBleDeviceFound(int employeeId);
    }
}
