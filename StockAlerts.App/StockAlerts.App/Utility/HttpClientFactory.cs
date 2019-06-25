using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace StockAlerts.App.Utility
{
    // Naive HttpClientFactory implementation while we wait for Xamarin support .NET Core
    public class HttpClientFactory : IHttpClientFactory
    {
        private const string DefaultHttpClientKey = "DefaultHttpClient";

        private readonly IDictionary<string, HttpClient> _clients = new Dictionary<string, HttpClient>();

        public HttpClient CreateClient() => CreateClient(DefaultHttpClientKey);

        public HttpClient CreateClient(string name)
        {
            if (!_clients.ContainsKey(name))
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _clients.Add(name, new HttpClient());
            }

            return _clients[name];
        }
    }
}
