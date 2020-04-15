using System;
namespace BluetoothTestApp.Models
{
    public class AuthRequestBody
    {
        public string appName { get; set; }
        public string appVersion { get; set; }
        public string hostTenant { get; set; }
        public string userTenant { get; set; }
        public string grant_type { get; set; }
    }
}
