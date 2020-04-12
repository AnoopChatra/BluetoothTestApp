using System;
using System.Collections.Generic;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.OS;
using BluetoothTestApp.Core.Services;
using BluetoothTestApp.Droid.Services.Uitilities;
using BluetoothTestApp.Models;
using BluetoothTestApp.Services;

namespace BluetoothTestApp.Droid.Services.BluetoothLeClient
{
    public class BluetoothLEClientService
    {
        private static readonly BluetoothLEClientService _instance = null;

        private readonly BluetoothAdapter _bluetoothAdapter;

        private BluetoothLeScanCallback _bluetoothLeScanCallback;
        private IList<int> _employeeIdList;
        private IList<IBleDeviceScanResult> _lisenerList;
        private int _employeeId;
        private RestQueueService _restQueueService;
        private IDictionary<int, int> detailDictionary = new Dictionary<int, int>();

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
            _restQueueService = new RestQueueService();
            _employeeIdList = new List<int>();
            _lisenerList = new List<IBleDeviceScanResult>();
        }

        public void StartBluetoothLeScan(int employeeId)
        {
            _employeeId = employeeId;
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
            int contactEmployeeId = BitConverter.ToInt32(maufactureData, 0);

            if (!_employeeIdList.Contains(contactEmployeeId))
            {
                _employeeIdList.Add(contactEmployeeId);

                detailDictionary.Add(contactEmployeeId, 1);

                foreach(var m in _lisenerList)
                {
                    m.OnNewBleDeviceFound(contactEmployeeId);
                }
            }
            else
            {
                ContactDetail contactDetail = new ContactDetail
                {
                    SourceEmployeeId = _employeeId,
                    ContactEmployeeId = contactEmployeeId,
                    SignialStrength = eventArgs.ScanResult.Rssi,
                    TimeStamp = DateTime.Now
                };
                _restQueueService.EnqueueContactDetail(contactDetail);
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
                .SetScanMode(Android.Bluetooth.LE.ScanMode.LowPower)
                .Build(); ;                           
        }
    }
}
