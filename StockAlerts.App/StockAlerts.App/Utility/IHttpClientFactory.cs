using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace StockAlerts.App.Utility
{
    // Use our own IHttpClientFactory until (if?) Xamarin supports .NET Core
    public interface IHttpClientFactory
    {
        HttpClient CreateClient();

        HttpClient CreateClient(string name);
    }
}
