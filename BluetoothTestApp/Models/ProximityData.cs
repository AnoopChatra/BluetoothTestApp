using System;
namespace BluetoothTestApp.Models
{
    public class ProximityData
    {
        public string destEmployeeId { get; set; }
        public int distance { get; set; }
        public int duration { get; set; }
        public OrganizationLocationData orgLocationDataDTO { get; set; }
        public int signal { get; set; }
        public string sourceEmployeeId { get; set; }
       
    }
}
