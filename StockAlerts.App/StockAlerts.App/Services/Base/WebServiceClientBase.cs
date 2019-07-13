using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Prism.Navigation;
using StockAlerts.App.Constants;
using StockAlerts.App.Extensions;
using StockAlerts.App.Services.RequestProvider;
using StockAlerts.App.Services.Settings;
using StockAlerts.App.Utility;
using StockAlerts.Resources.Model.Authentication;

namespace StockAlerts.App.Services.Base
{
    public abstract class WebServiceClientBase
    {
        private readonly ISettingsService _settingsService;
        private readonly JsonSerializerSettings _serializerSettings;
        protected readonly INavigationService NavigationService;
        protected readonly HttpClient HttpClient;

        protected WebServiceClientBase(
            IHttpClientFactory httpClientFactory,
            ISettingsService settingsService,
            INavigationService navigationService)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            HttpClient = httpClientFactory.CreateClient(MiscConstants.StockAlertsApi);

            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        protected Task<TResult> GetAsync<TResult>(string uri) => GetAsync<TResult>(uri, CancellationToken.None);

        protected async Task<TResult> GetAsync<TResult>(string uri, CancellationToken cancellationToken)
        {
            HttpClient.EnsureAuthTokenSet(_settingsService.AuthAccessToken);
            var response = await ExecuteAuthenticatedRequestAsync(async () => await HttpClient.GetAsync(uri, cancellationToken), cancellationToken);

            if (!response.IsSuccessStatusCode)
                return default(TResult);

            var serialized = await response.Content.ReadAsStringAsync();
            var result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings), cancellationToken);

            return result;
        }

        protected Task<TResult> PostAsync<TResult>(string uri, TResult data, string header = "") =>
            PostAsync<TResult, TResult>(uri, data, header);

        protected async Task<TResult> PostAsync<TData, TResult>(string uri, TData data, string header = "")
        {
            HttpClient.EnsureAuthTokenSet(_settingsService.AuthAccessToken);

            if (!string.IsNullOrEmpty(header))
            {
                HttpClient.AddHeaderParameter(header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await ExecuteAuthenticatedRequestAsync(async () => await HttpClient.PostAsync(uri, content), CancellationToken.None);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        protected Task<TResult> PutAsync<TResult>(string uri, TResult data, string header = "") =>
            PutAsync<TResult, TResult>(uri, data, header);

        protected async Task<TResult> PutAsync<TData, TResult>(string uri, TData data, string header = "")
        {
            HttpClient.EnsureAuthTokenSet(_settingsService.AuthAccessToken);

            if (!string.IsNullOrEmpty(header))
            {
                HttpClient.AddHeaderParameter(header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await ExecuteAuthenticatedRequestAsync(async () => await HttpClient.PutAsync(uri, content), CancellationToken.None);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        // Similar methods for DELETE

        private async Task<HttpResponseMessage> ExecuteAuthenticatedRequestAsync(
            Func<Task<HttpResponseMessage>> operation,
            CancellationToken cancellationToken)
        {
            var response = await operation();

            var content = await response.Content.ReadAsStringAsync();
            Debug.Write(content);


            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (response.Headers.Contains("Token-Expired") &&
                    response.Headers.FirstOrDefault(h => h.Key == "Token-Expired").Value.FirstOrDefault() == "true")
                {
                    await ExchangeRefreshTokenAsync(_settingsService.AuthAccessToken, _settingsService.AuthRefreshToken, cancellationToken);
                    response = await operation();
                }
            }

            await HandleResponse(response);

            return response;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await NavigationService.NavigateAsync("LoginPage");
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestExceptionEx(response.StatusCode, content);
                }
            }
        }

        private async Task ExchangeRefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
        {
            var exchangeRefreshTokenRequest = new ExchangeRefreshTokenRequest { AccessToken = accessToken, RefreshToken = refreshToken };

            _settingsService.AuthAccessToken = string.Empty;
            var result = await PostAsync<ExchangeRefreshTokenRequest, ExchangeRefreshTokenResponse>($"{MiscConstants.StockAlertsApiBaseUri}refresh-tokens", exchangeRefreshTokenRequest);

            _settingsService.AuthAccessToken = result.AccessToken.Token;
            _settingsService.AuthRefreshToken = result.RefreshToken;
        }
    }
}
