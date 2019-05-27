using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockAlerts.DataProviders.Intrinio.Model;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;

namespace StockAlerts.DataProviders.Intrinio
{
    public class IntrinioClient : IStockDataWebClient
    {
        private readonly IIntrinioSettings _settings;
        private readonly IApiCallsRepository _apiCallsRepository;
        private readonly HttpClient _httpClient;

        public IntrinioClient(
            IHttpClientFactory httpClientFactory,
            IIntrinioSettings settings,
            IApiCallsRepository apiCallsRepository)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _apiCallsRepository = apiCallsRepository ?? throw new ArgumentNullException(nameof(apiCallsRepository));
            _httpClient = httpClientFactory.CreateClient(Apis.Intrinio);
        }

        public async Task<PriceQuote> GetRealTimePriceQuoteAsync(string symbol)
        {
            var url = $"securities/{symbol.ToUpper()}/prices/realtime";
            var response = await _httpClient.GetAsync($"{url}?api_key={_settings.ApiKey}");

            // Log the API call
            await LogApiCallAsync(url, response.StatusCode);

            // Process response
            if (response.IsSuccessStatusCode)
            {
                var quoteJson = await response.Content.ReadAsStringAsync();
                var quote = JsonConvert.DeserializeObject<RealTimeQuote>(quoteJson);
                return new PriceQuote(quote);
            }

            return null;
        }

        private async Task LogApiCallAsync(string route, HttpStatusCode responseCode)
        {
            var apiCall = new ApiCall(_apiCallsRepository)
            {
                Api = Apis.Intrinio,
                CallTime = DateTimeOffset.UtcNow,
                Route = route,
                ResponseCode = responseCode
            };

            await apiCall.SaveAsync();
        }
    }
}
