using System;
using System.Collections.Generic;
using System.Text;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.OS;
using Android.Util;
using BluetoothTestApp.Droid.Services.Uitilities;
using Java.Util;

namespace BluetoothTestApp.Droid.Services.BluetoothLeClient
{
    public class BluetoothLEClientService
    {
        private static readonly BluetoothLEClientService _instance = null;

        private readonly BluetoothAdapter _bluetoothAdapter;
        private BluetoothLeScanCallback _bluetoothLeScanCallback;
        private IList<int> _employeeIdList;
        private IList<IBleDeviceScanResult> _lisenerList;

        public static BluetoothLEClientService Instance
        {
            get
            {
                if (null == _instance)
                {
                    return new BluetoothLEClientService();
                }
                else
                {
                    return _instance;
                }
            }
        }

        public BluetoothLEClientService()
        {
            _bluetoothAdapter = AndroidBluetoothServiceProvider.Instance.GetBluetoothAdapter();
            _bluetoothLeScanCallback = new BluetoothLeScanCallback();
            _employeeIdList = new List<int>();
            _lisenerList = new List<IBleDeviceScanResult>();
        }

        public void StartBluetoothLeScan()
        {            
            _bluetoothLeScanCallback.OnBleScanCallback += BleScancallback;
            _bluetoothAdapter.BluetoothLeScanner.StartScan(GetScanFilters(), GetScansettings(), _bluetoothLeScanCallback);
        }

        public void StopBluetoothLeScan()
        { 
            _bluetoothLeScanCallback.OnBleScanCallback -= BleScancallback;
            _bluetoothAdapter.BluetoothLeScanner.StopScan(_bluetoothLeScanCallback);
        }

        public void RegisterOberver(IBleDeviceScanResult listener)
        {
            _lisenerList.Add(listener);
        }

        private void BleScancallback(object sender, BleScanCallbackEventArgs eventArgs)
        {
            byte[] maufactureData = eventArgs.ScanResult.ScanRecord.GetManufacturerSpecificData(0); 
            int employeeId = BitConverter.ToInt32(maufactureData, 0);

            if (!_employeeIdList.Contains(employeeId))
            {
                _employeeIdList.Add(employeeId);

                foreach(var m in _lisenerList)
                {
                    m.OnNewBleDeviceFound(employeeId);
                }
            }            
        }
      
        private IList<ScanFilter> GetScanFilters()
        {
            IList<ScanFilter> filterList = new List<ScanFilter>();
            ScanFilter scanFilter = new ScanFilter.Builder()
                    .SetServiceUuid(new ParcelUuid(UARTProfile.UART_SERVICE))
                    .Build();
            filterList.Add(scanFilter);
            return filterList;
        }

        private ScanSettings GetScansettings()
        {
            return new ScanSettings.Builder()
                .SetScanMode(Android.Bluetooth.LE.ScanMode.Balanced)
                .Build(); ;                           
        }
    }
}
