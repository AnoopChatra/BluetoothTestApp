using System;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.OS;

namespace BluetoothTestApp.Droid.Services
{
    public class BluetoothLeService
    {
        private static BluetoothLeService _instance = null;
        public static BluetoothLeService Instance {
            get
            {
                if (null == _instance)
                {
                    return new BluetoothLeService();
                }
                else
                {
                    return _instance;
                }
            }
        }

        public BluetoothGattServer OpenGattServer()
        {
            return MainActivity.mBluetoothManager.OpenGattServer(MainActivity.ActivityContext, new GattsServerCallback());
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

            MainActivity.mGattServer.AddService(UART_SERVICE);
        }

        public void StartGattAdvertising()
        {
            if (MainActivity.mBluetoothLeAdvertiser == null) return;

            AdvertiseSettings settings = new AdvertiseSettings.Builder()
                    .SetAdvertiseMode(AdvertiseMode.Balanced)
                    .SetConnectable(true)
                    .SetTimeout(0)
                    .SetTxPowerLevel(AdvertiseTx.PowerMedium)
                    .Build();

            AdvertiseData data = new AdvertiseData.Builder()
                    .SetIncludeDeviceName(true)
                    .AddServiceUuid(new ParcelUuid(UARTProfile.UART_SERVICE))
                    .Build();

            MainActivity.mBluetoothLeAdvertiser.StartAdvertising(settings, data, new AdvertisementCallback());
        }

        public void CloseGattServer()
        {
            if (MainActivity.mGattServer == null) return;
            MainActivity.mGattServer.Close();
        }
    }
}
