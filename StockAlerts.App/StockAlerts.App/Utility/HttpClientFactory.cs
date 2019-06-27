using System;
using System.Collections.Generic;
using System.Net.Http;

namespace StockAlerts.App.Utility
{
    // Naive HttpClientFactory implementation while we wait for Xamarin support .NET Core
    public class HttpClientFactory : IHttpClientFactory, IDisposable
    {
        private readonly IDictionary<string, Action<HttpClient>> _configurations = new Dictionary<string, Action<HttpClient>>();
        private readonly IDictionary<string, HttpClient> _clients = new Dictionary<string, HttpClient>();

        public void RegisterClient(string name, Action<HttpClient> configurationAction)
        {
            if (_configurations.ContainsKey(name))
                _configurations.Remove(name);

            _configurations.Add(name, configurationAction);
        }

        public HttpClient CreateClient(string name)
        {
            if (!_clients.ContainsKey(name))
            {
                var httpClient = new HttpClient();
                if (_configurations.ContainsKey(name))
                {
                    _configurations[name].Invoke(httpClient);
                }

                _clients.Add(name, httpClient);
            }

            return _clients[name];
        }

        public void Dispose()
        {
            foreach (var c in _clients)
            {
                c.Value.Dispose();
            }
        }
    }
}
