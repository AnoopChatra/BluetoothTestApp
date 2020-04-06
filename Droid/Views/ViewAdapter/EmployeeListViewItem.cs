using System;
namespace BluetoothTestApp.Droid.Views.ViewAdapter
{
    public class EmployeeListViewItem
    {
        public string EmployeeId { get; set; }
        public string TimeStamp { get; set; }

        public EmployeeListViewItem(string employeeId, string timeStamp)
        {
            EmployeeId = employeeId;
            TimeStamp = timeStamp;
        }
    }
}
