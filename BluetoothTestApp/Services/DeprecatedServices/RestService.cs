using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BluetoothTestApp.Models;
using Newtonsoft.Json;

namespace BluetoothTestApp.Core.Services
{
    public class RestService
    {
        private const string url = "https://webapplication120200412213451.azurewebsites.net/api/Home";
        private readonly HttpClient _httpClient;
        public RestService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<int> SaveContactDetailAsync(ContactDetail item)
        {
            var uri = new Uri(string.Format(url, string.Empty));
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
           
            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);            
            return (int)response.StatusCode;
        }

        public void SaveContactDetail(ContactDetail item)
        {
            var uri = new Uri(string.Format(url, string.Empty));
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.PostAsync(uri, content);            
        }

        public async Task<int> SaveContactDetailsList(ContactDetailsList item)
        {
            var uri = new Uri(string.Format(url, string.Empty));
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
            return (int)response.StatusCode;
        }
    }
}
