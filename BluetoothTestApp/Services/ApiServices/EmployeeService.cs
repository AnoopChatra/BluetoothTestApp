using System;
using System.Net;
using System.Threading.Tasks;
using BluetoothTestApp.Models;

namespace BluetoothTestApp.Services
{
    public class EmployeeService
    {      
        private const string url = " https://gateway.eu1.mindsphere.io/api/covidbackend-ctblrdev/V1/employee/add";

        private readonly HttpService _httpService;

        public EmployeeService()
        {
            _httpService = HttpService.Instance;
        }

        public async Task<HttpStatusCode> RegisterEmployeeAsync(Employee employee)
        {
            return await _httpService.PostAsync(url, employee);
        }
    }
}
