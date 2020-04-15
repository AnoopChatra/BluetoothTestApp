using Android.Bluetooth.LE;

namespace BluetoothTestApp.Droid.Services
{
    public class AdvertisementCallback : AdvertiseCallback
    {
        public override void OnStartFailure(AdvertiseFailure errorCode)
        {
            base.OnStartFailure(errorCode);
        }

        public override void OnStartSuccess(AdvertiseSettings settingsInEffect)
        {
            base.OnStartSuccess(settingsInEffect);
        }       
    }
}
