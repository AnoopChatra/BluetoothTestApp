using System;
using Java.Util;

namespace BluetoothTestApp.Droid.Services
{
    public class UARTProfile
    {
        public static UUID UART_SERVICE = UUID.FromString("6e400001-b5a3-f393-e0a9-e50e24dcca9e");

        //RX, Write characteristic
        public static UUID RX_WRITE_CHAR = UUID.FromString("6e400002-b5a3-f393-e0a9-e50e24dcca9e");

        //TX READ Notify
        public static UUID TX_READ_CHAR = UUID.FromString("6e400003-b5a3-f393-e0a9-e50e24dcca9e");
        public static UUID TX_READ_CHAR_DESC = UUID.FromString("00002902-0000-1000-8000-00805f9b34fb");
    }
}
