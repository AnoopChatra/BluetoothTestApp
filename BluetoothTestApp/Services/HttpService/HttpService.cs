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
        private HttpClient _httpClient;   
        
        private const string Logtag = "HttpService";
        private bool _isRefreshTokenCalled;

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

        public async Task<HttpStatusCode> PostAsync(string url, object body)
        {
            try
            {               
                var uri = new Uri(string.Format(url, string.Empty));
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                
                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

                if(HttpStatusCode.Unauthorized == response.StatusCode)
                {
                    await RefreshToken(response);
                    await PostAsync(url, body);                   
                }
                return response.StatusCode;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(Logtag + "-- exception in post --" + exception.Message + exception.StackTrace);
            }
            return HttpStatusCode.BadRequest;
        }

        public void Post(string url, object body)
        {
            try
            {
                ////TODO : need to await the call ideally.
                RefreshToken();
                var uri = new Uri(string.Format(url, string.Empty));
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                GetHttpClient().PostAsync(uri, content);               
            }
            catch (Exception exception)
            {
                Debug.WriteLine(Logtag + "-- exception in post --" + exception.Message + exception.StackTrace);
            }           
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

        private HttpClient GetHttpClient()
        {
            if(null == _httpClient)
            {
                _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + MindsphereAuthService.Instance.AccessToken);
                return _httpClient;
            }
            else
            {
                return _httpClient;
            }
        }

        private async Task RefreshToken()
        {
            if (!_isRefreshTokenCalled)
            {
                _isRefreshTokenCalled = true;
                TimeSpan timeSpan = DateTime.Now - MindsphereAuthService.Instance.AccessTokenTimeStamp;
                if (timeSpan.Minutes > 25)
                {
                    await MindsphereAuthService.Instance.GenerateAccessToken();
                }
                _isRefreshTokenCalled = false;
            }
        }        
    }
}
