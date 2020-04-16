using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.OS;
using BluetoothTestApp.Core.Services;
using BluetoothTestApp.Models;
using BluetoothTestApp.Services;

namespace BluetoothTestApp.Droid.Services
{
    public class BluetoothLEClientService 
    {
        private static readonly BluetoothLEClientService _instance = null;

        private readonly BluetoothAdapter _bluetoothAdapter;
        private Handler _handler;

        private BluetoothLeScanCallback _bluetoothLeScanCallback;
        private IList<int> _employeeIdList;
        private IList<IBleDeviceScanResult> _lisenerList;
        private int _employeeId;
        private RestService _restService;
        private IDictionary<int, int> detailDictionary = new Dictionary<int, int>();
        private readonly ProximityServices _proximityServices;

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
            _restService = new RestService();
            _lisenerList = new List<IBleDeviceScanResult>();
            _handler = new Handler(MainActivity.ActivityContext.MainLooper);
            _proximityServices = new ProximityServices();
        }

        public void StartBluetoothLeScan(int employeeId)
        {
            _handler.RemoveCallbacksAndMessages(null);
            _employeeId = employeeId;
            _bluetoothLeScanCallback.OnBleScanCallback += BleScancallback;
            _bluetoothAdapter.BluetoothLeScanner.StartScan(GetScanFilters(), GetScansettings(), _bluetoothLeScanCallback);
            _handler.PostDelayed(RestartScanning, 60000);
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

        private void RestartScanning()
        {
            StopBluetoothLeScan();
            Task.Delay(2000);
            StartBluetoothLeScan(_employeeId);
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
                    TimeStamp = DateTime.Now.ToString()
                };
                _restService.SaveContactDetail(contactDetail);
                ProximityData proximityData = GetProximityData(_employeeId, contactEmployeeId, eventArgs.ScanResult.Rssi);
                _proximityServices.UploadProximityInfo(proximityData);              
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
                .SetMatchMode(BluetoothScanMatchMode.Aggressive)
                .Build(); ;                           
        }

        private ProximityData GetProximityData(int sourceEmployeeId, int destinationEmployeeId, int signal)
        {
            ProximityData proximityData = new ProximityData();
            proximityData.sourceEmployeeId = sourceEmployeeId.ToString();
            proximityData.destEmployeeId = destinationEmployeeId.ToString();
            proximityData.signal = signal;

            proximityData.distance = 0;
            proximityData.duration = 0;
            OrganizationLocationData orgLocationData = new OrganizationLocationData();
            orgLocationData.building = "Keonics";
            orgLocationData.city = "Blore";
            orgLocationData.country = "India";
            orgLocationData.floor = "9";
            orgLocationData.orgUnit = "DTS";
            orgLocationData.location = "Ecity";
            orgLocationData.state = "Karnataka";
            proximityData.orgLocationDataDTO = orgLocationData;

            return proximityData;
        }      
    }
}
