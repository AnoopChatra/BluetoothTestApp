using System;
using System.Net;
using System.Threading.Tasks;
using BluetoothTestApp.Models;

namespace BluetoothTestApp.Services
{
    public class EmployeeService
    {      
        private const string url = "https://gateway.eu1.mindsphere.io/api/covidbackend-ctblrdev/V1/proximity/add";
        private HttpService _httpService;

        public EmployeeService()
        {
        }

        public async Task<HttpStatusCode> RegisterEmployee(Employee employee)
        {
            return await _httpService.Post(url, employee);
        }
    }
}
