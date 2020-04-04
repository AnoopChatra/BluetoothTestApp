using System;
using System.Text;
using Android.Bluetooth;
using Java.Text;
using Java.Util;

namespace BluetoothTestApp.Droid.Services
{
    public class GattsServerCallback : BluetoothGattServerCallback
    {
        string TAG = "GattCallback";
        private byte[] storage;
       
        public GattsServerCallback()
        {
        }

        public override void OnConnectionStateChange(BluetoothDevice bluetoothDevice, ProfileState status, ProfileState newState)
        {
            
            base.OnConnectionStateChange(bluetoothDevice, status, newState);
            //Console.WriteLine(TAG, "onConnectionStateChange "
            //        + UARTProfile.getStatusDescription(status) + " "
            //        + UARTProfile.getStateDescription(newState));

            if (newState == ProfileState.Connected)
            {
                postDeviceChange(bluetoothDevice, true);

            }
            else if (newState == ProfileState.Disconnected)
            {
                postDeviceChange(bluetoothDevice, false);
            }
        }

        public override void OnServiceAdded(GattStatus status, BluetoothGattService service)
        {
            Console.WriteLine(TAG, "Our gatt server service was added.");
            base.OnServiceAdded(status, service);
        }

        public override void OnCharacteristicReadRequest(BluetoothDevice device, int requestId, int offset, BluetoothGattCharacteristic characteristic)
        {
            storage = hexStringToByteArray("1111");
            base.OnCharacteristicReadRequest(device, requestId, offset, characteristic);
            Console.WriteLine(TAG, "READ called onCharacteristicReadRequest " + characteristic.Uuid.ToString());
            if (UARTProfile.TX_READ_CHAR.Equals(characteristic.Uuid))
            {
                MainActivity.mGattServer.SendResponse(device,
                        requestId,
                        GattStatus.Success,
                        0,
                        storage);
            }
        }

        public override void OnCharacteristicWriteRequest(BluetoothDevice device, int requestId, BluetoothGattCharacteristic characteristic, bool preparedWrite, bool responseNeeded, int offset, byte[] value)
        {
            base.OnCharacteristicWriteRequest(device, requestId, characteristic, preparedWrite, responseNeeded, offset, value);
            Console.WriteLine(TAG, "onCharacteristicWriteRequest " + characteristic.Uuid.ToString());

            if (UARTProfile.RX_WRITE_CHAR.Equals(characteristic.Uuid))
            {

                //IMP: Copy the received value to storage
                storage = value;
                if (responseNeeded)
                {
                    MainActivity.mGattServer.SendResponse(device,
                            requestId,
                            GattStatus.Success,
                            0,
                            value);
                    Console.WriteLine(TAG, "Received  data on " + characteristic.Uuid.ToString());
                    Console.WriteLine(TAG, "Received data" + bytesToHex(value));

                }

                //IMP: Respond
                sendOurResponse(null);
            }         

        }


        public override void OnNotificationSent(BluetoothDevice device,GattStatus status)
        {
            Console.WriteLine(TAG, "onNotificationSent");
            base.OnNotificationSent(device, status);
        }

        private void postDeviceChange(BluetoothDevice device, bool toAdd)
        {
            //Update UI
        }

        //Helper function converts byte array to hex string
        //for priting
        private string bytesToHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


        //Helper function converts hex string into
        //byte array
        private byte[] hexStringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void sendOurResponse(BluetoothDevice device)
        {
           
                BluetoothGattCharacteristic readCharacteristic = MainActivity.mGattServer.GetService(UARTProfile.UART_SERVICE)
                        .GetCharacteristic(UARTProfile.TX_READ_CHAR);

                byte[] notify_msg = storage;
                string hexStorage = bytesToHex(storage);
               Console.WriteLine(TAG, "received string = " + bytesToHex(storage));


                if (hexStorage.Equals("77686F616D69"))
                {
                    notify_msg = Encoding.ASCII.GetBytes("I am echo an machine");
                }
                else if (bytesToHex(storage).Equals("64617465"))
                {
                    DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
                    Date date = new Date();
                    notify_msg = Encoding.ASCII.GetBytes(dateFormat.Format(date));

                }
                else
                {
                    //TODO: Do nothing send what you received. Basically echo
                }
                readCharacteristic.SetValue(notify_msg);
                Console.WriteLine(TAG, "Sending Notifications" + notify_msg);
                bool is_notified = MainActivity.mGattServer.NotifyCharacteristicChanged(device, readCharacteristic, false);
                Console.WriteLine(TAG, "Notifications =" + is_notified);
        }      

    }

}
