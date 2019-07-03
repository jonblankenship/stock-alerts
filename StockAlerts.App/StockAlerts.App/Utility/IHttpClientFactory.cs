using System;
using System.Net.Http;

namespace StockAlerts.App.Utility
{
    // Use our own IHttpClientFactory until (if?) Xamarin supports .NET Core
    public interface IHttpClientFactory
    {
        void AddHttpClient(string name, Action<HttpClient> configurationAction);

        HttpClient CreateClient(string name);
    }
}
