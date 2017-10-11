using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Mns.Utils
{
   public class ApiUtils
    {
        private HttpClient _httpClient;
        private string username = "admin_mns";
        private string password = "admin_mns";
        /// <summary>
        /// api调用初始化
        /// </summary>
        /// <param name="apiUrl">api服务器地址</param>
        public ApiUtils(string apiUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(apiUrl);
            _httpClient.DefaultRequestHeaders.Add("username", username);
            _httpClient.DefaultRequestHeaders.Add("password", password);
        }

     
        /// <summary>
        /// API,Post
        /// </summary>
        /// <param name="api"></param>
        /// <param name="requestJson">json格式传输</param>
        /// <param name="headerAuthentication"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string api, string requestJson)
        {
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.PostAsync(api, httpContent);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAsync(string api)
        {
            try
            {
                var response = await _httpClient.GetAsync(api);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
