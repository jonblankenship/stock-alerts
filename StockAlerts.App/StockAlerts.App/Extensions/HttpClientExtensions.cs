using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace StockAlerts.App.Extensions
{
    public static class HttpClientExtensions
    {
        public static void EnsureAuthTokenSet(this HttpClient httpClient, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public static void AddHeaderParameter(this HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }
    }
}
