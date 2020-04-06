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
        private IList<BluetoothDevice> _bluetoothDeviceList;

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
            _bluetoothDeviceList = new List<BluetoothDevice>();
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

        private void BleScancallback(object sender, BleScanCallbackEventArgs eventArgs)
        {
           byte[] km = eventArgs.ScanResult.ScanRecord.GetManufacturerSpecificData(0);
 
            var ouoin = BitConverter.ToInt32(km, 0);
            if (!_bluetoothDeviceList.Contains(eventArgs.ScanResult.Device))
            {
                _bluetoothDeviceList.Add(eventArgs.ScanResult.Device);
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
