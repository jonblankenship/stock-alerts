using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using StockAlerts.App.Exceptions;
using StockAlerts.App.Extensions;
using StockAlerts.App.Services.Accounts;
using StockAlerts.App.Services.Settings;

namespace StockAlerts.App.Services.RequestProvider
{
    public class RequestProvider : IRequestProvider
    {
        private readonly ISettingsService _settingsService;
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider(
            ISettingsService settingsService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));

            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public Task<TResult> GetAsync<TResult>(string uri) =>
            GetAsync<TResult>(uri, string.Empty, CancellationToken.None);

        public Task<TResult> GetAsync<TResult>(string uri, string token) =>
            GetAsync<TResult>(uri, token, CancellationToken.None);

        public async Task<TResult> GetAsync<TResult>(string uri, string token, CancellationToken cancellationToken)
        {
            HttpClient httpClient = CreateHttpClient(token);
            var response = await ExecuteAuthenticatedRequestAsync(async() => await httpClient.GetAsync(uri, cancellationToken), cancellationToken);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "") =>
            PostAsync<TResult, TResult>(uri, data, token, header);

        public async Task<TResult> PostAsync<TData, TResult>(string uri, TData data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                httpClient.AddHeaderParameter(header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await ExecuteAuthenticatedRequestAsync(async () => await httpClient.PostAsync(uri, content), CancellationToken.None);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                httpClient.AddHeaderParameter(header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await ExecuteAuthenticatedRequestAsync(async () => await httpClient.PutAsync(uri, content), CancellationToken.None);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            await ExecuteAuthenticatedRequestAsync(async () => await httpClient.DeleteAsync(uri), CancellationToken.None);
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }

        private async Task<HttpResponseMessage> ExecuteAuthenticatedRequestAsync(
            Func<Task<HttpResponseMessage>> operation,
            CancellationToken cancellationToken)
        {
            var response = await operation();
            if (response.StatusCode == HttpStatusCode.Unauthorized &&
                response.Headers.FirstOrDefault(h => h.Key == "Token-Expired").Value.FirstOrDefault() == "true")
            {
                //await _accountService.ExchangeRefreshTokenAsync(_settingsService.AuthAccessToken, _settingsService.AuthRefreshToken, cancellationToken);
                response = await operation();
            }

            await HandleResponse(response);

            return response;
        }
    }
}
