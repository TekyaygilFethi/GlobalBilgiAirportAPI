using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace AirportDistanceCalculator.Business.HttpRequestFolder
{
    public class HttpRequestHelper : IDisposable
    {
        private readonly HttpClient _client;

        public HttpRequestHelper()
        {
            _client = new HttpClient();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<T> Get<T>(string url)
        {
            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseContext = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseContext);

        }
    }
}
