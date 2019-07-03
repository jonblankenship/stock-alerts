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

        public void AddHttpClient(string name, Action<HttpClient> configurationAction)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), $"{nameof(name)} must be provided.");
            if (_configurations.ContainsKey(name)) throw new ArgumentNullException(nameof(name), $"A client with the name {name} has already been added.");
            
            _configurations.Add(name, configurationAction);
        }

        public HttpClient CreateClient(string name)
        {
            if (!_clients.ContainsKey(name))
            {
                if (!_configurations.ContainsKey(name)) throw new ArgumentException($"A client by the name of {name} has not yet been registered.  Call {nameof(AddHttpClient)} first.");

                var httpClient = new HttpClient();

                _configurations[name].Invoke(httpClient);

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
