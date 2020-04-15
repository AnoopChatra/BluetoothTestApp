using System;
namespace BluetoothTestApp.Models
{
    public class ContactDetail
    {        
        public int SourceEmployeeId { get; set; }
        public int ContactEmployeeId { get; set; }
        public int SignialStrength { get; set; }
        public string TimeStamp { get; set; }
    }
}
