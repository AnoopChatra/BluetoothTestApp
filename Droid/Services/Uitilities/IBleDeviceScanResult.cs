using System;
namespace BluetoothTestApp.Droid.Services
{
    public interface IBleDeviceScanResult
    {
        void OnNewBleDeviceFound(int employeeId);
    }
}
