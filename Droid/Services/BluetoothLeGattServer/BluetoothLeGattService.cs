using System;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.OS;

namespace BluetoothTestApp.Droid.Services
{
    public class BluetoothLeGattService
    {
        private const int DefaultManufactureId = 0x0000;
        private static readonly BluetoothLeGattService _instance = null;

        private BluetoothLeAdvertiser _bluetoothLeAdvertiser;
        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothManager _bluetoothManager;
        private BluetoothGattServer _bluetoothGattServer;
       
        public static BluetoothLeGattService Instance {
            get
            {
                if (null == _instance)
                {
                    return new BluetoothLeGattService();
                }
                else
                {
                    return _instance;
                }
            }
        }

        private BluetoothLeGattService()
        {
            _bluetoothManager = AndroidBluetoothServiceProvider.Instance.GetBluetoothManager();
            _bluetoothAdapter = AndroidBluetoothServiceProvider.Instance.GetBluetoothAdapter();
            _bluetoothLeAdvertiser = _bluetoothAdapter.BluetoothLeAdvertiser;
        }

        public BluetoothGattServer GetBluetoothGattServer()
        {
            return _bluetoothGattServer;
        }

        public void OpenGattServer()
        {
            _bluetoothGattServer = _bluetoothManager.OpenGattServer(MainActivity.ActivityContext, new GattsServerCallback());
        }        

        public void InitGattServer()
        {
            BluetoothGattService UART_SERVICE = new BluetoothGattService(UARTProfile.UART_SERVICE,
                    GattServiceType.Primary);

            BluetoothGattCharacteristic TX_READ_CHAR =
                    new BluetoothGattCharacteristic(UARTProfile.TX_READ_CHAR,
                            //Read-only characteristic, supports notifications
                            GattProperty.Read | GattProperty.Notify,
                            GattPermission.Read);

            //Descriptor for read notifications
            BluetoothGattDescriptor TX_READ_CHAR_DESC = new BluetoothGattDescriptor(UARTProfile.TX_READ_CHAR_DESC, GattDescriptorPermission.Write);
            TX_READ_CHAR.AddDescriptor(TX_READ_CHAR_DESC);


            BluetoothGattCharacteristic RX_WRITE_CHAR =
                    new BluetoothGattCharacteristic(UARTProfile.RX_WRITE_CHAR,
                            //write permissions
                            GattProperty.Write, GattPermission.Write);


            UART_SERVICE.AddCharacteristic(TX_READ_CHAR);
            UART_SERVICE.AddCharacteristic(RX_WRITE_CHAR);

            _bluetoothGattServer.AddService(UART_SERVICE);
        }

        public void StartGattAdvertising(int advertisementData)
        {
            if (_bluetoothLeAdvertiser == null) return;

            AdvertiseSettings settings = new AdvertiseSettings.Builder()
                    .SetAdvertiseMode(AdvertiseMode.Balanced)
                    .SetConnectable(true)
                    .SetTimeout(0)
                    .SetTxPowerLevel(AdvertiseTx.PowerLow)
                    .Build();

            byte[] adData = BitConverter.GetBytes(advertisementData); //advertisementData
            AdvertiseData data = new AdvertiseData.Builder()                    
                    .AddManufacturerData(DefaultManufactureId, adData)
                    .AddServiceUuid(new ParcelUuid(UARTProfile.UART_SERVICE))
                    .Build();          

            _bluetoothLeAdvertiser.StartAdvertising(settings, data, new AdvertisementCallback());
        }

        public void CloseGattServer()
        {
            if (_bluetoothGattServer == null) return;
            _bluetoothGattServer.Close();
        }       
    }
}
