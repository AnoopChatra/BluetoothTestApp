using System;
using System.Net;
using System.Threading.Tasks;
using BluetoothTestApp.Models;

namespace BluetoothTestApp.Services
{
    public class ProximityServices
    {
        private const string url = "https://gateway.eu1.mindsphere.io/api/covidbackend-ctblrdev/V1/proximity/add";
        private readonly HttpService _httpService;

        public ProximityServices()
        {
            _httpService = HttpService.Instance;
        }

        public void UploadProximityInfo(ProximityData proximityData)
        {
            _httpService.Post(url, proximityData);           
        }
    }
}
