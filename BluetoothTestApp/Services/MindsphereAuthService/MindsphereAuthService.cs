using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BluetoothTestApp.Models;
using Newtonsoft.Json;

namespace BluetoothTestApp.Services
{
    public class MindsphereAuthService
    {
        /*
        private const string _clientId = "ctblrdev-covidbackend-1.0.0";
        private const string _secretKey = "k76iWOpe00RQO1538FxbggP9jH5GYD1Q5IfuLf4FR4j";

        Generate _authKey from https://www.base64encode.org/
        copy _clientId + _secretKey and hit encode button.
        */

        private const string _authServiceUrl = "https://gateway.eu1.mindsphere.io/api/technicaltokenmanager/v3/oauth/token";
        private const string _authKey = "Y3RibHJkZXYtY292aWRiYWNrZW5kLTEuMC4wOms3NmlXT3BlMDBSUU8xNTM4RnhiZ2dQOWpINUdZRDFRNUlmdUxmNEZSNEo=";
        private const string _appName = "covidbackend";
        private const string _xSpaceAuthKey = "Basic" + " " + _authKey;

        private const string _appVersion = "1.0.0";
        private const string _hostTenant = "ctblrdev";
        private const string _userTenant = "ctblrdev";
        private const string _grant_type = "client_credentials";

        private static MindsphereAuthService _instance;
        public static MindsphereAuthService Instance
        {
            get
            {
                if(null != _instance)
                {
                    return _instance;
                }
                else
                {
                    return _instance = new MindsphereAuthService();
                }
            }
        }       

        private readonly AuthRequestBody _authRequestBodyModel;
        private readonly HttpClient _httpClient;

        public string AccessToken { get; private set; }
        public DateTime AccessTokenTimeStamp { get; private set; }

        private MindsphereAuthService()
        {
            AccessToken = "qwerty"; //Initializing with dummy token
            _authRequestBodyModel = new AuthRequestBody();
            _authRequestBodyModel.appName = _appName;
            _authRequestBodyModel.appVersion = _appVersion;
            _authRequestBodyModel.hostTenant = _hostTenant;
            _authRequestBodyModel.userTenant = _userTenant;
            _authRequestBodyModel.grant_type = _grant_type;

            _httpClient = new HttpClient();
        }

        public async Task<bool> GenerateAccessToken()
        {
            bool isGeneratteAuthTokenSuccess = false;
            try
            {
                var uri = new Uri(string.Format(_authServiceUrl, string.Empty));
                var json = JsonConvert.SerializeObject(_authRequestBodyModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.Add("x-space-auth-key", _xSpaceAuthKey);

                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    MindSphereAccessToken mindSphereAccessToken = JsonConvert.DeserializeObject<MindSphereAccessToken>(responseBody);
                    AccessToken = mindSphereAccessToken.access_token;
                    isGeneratteAuthTokenSuccess = true;
                    AccessTokenTimeStamp = DateTime.Now;
                }
            }
            catch
            {
                isGeneratteAuthTokenSuccess = false;
            }            
            return isGeneratteAuthTokenSuccess;                      
        }      
    }
}
