using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BluetoothTestApp.Models;
using Newtonsoft.Json;

namespace BluetoothTestApp.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private const string Logtag = "HttpService";
        private readonly object _lockObject;

        private static HttpService _httpServiceInstance;
        public static HttpService Instance
        {
            get
            {
                if (null == _httpServiceInstance)
                {
                    return _httpServiceInstance = new HttpService();
                }
                else
                {
                    return _httpServiceInstance;
                }
            }
        }           

        private HttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpStatusCode> Post(string url, object body)
        {
            try
            {
                var uri = new Uri(string.Format(url, string.Empty));
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + MindsphereAuthService.Instance.AccessToken);
                
                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

                if(HttpStatusCode.Unauthorized == response.StatusCode)
                {
                    await RefreshToken(response);
                    await Post(url, body);                   
                }
                return response.StatusCode;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(Logtag + "-- exception in post --" + exception.Message + exception.StackTrace);
            }
            return HttpStatusCode.BadRequest;
        }

        private async Task RefreshToken(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            TokenExpired tokenExpired = JsonConvert.DeserializeObject<TokenExpired>(responseBody);
            if (tokenExpired.error.Equals("invalid_token"))
            {
                await MindsphereAuthService.Instance.GenerateAccessToken();               
            }
        }
    }
}
