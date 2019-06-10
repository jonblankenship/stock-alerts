using Newtonsoft.Json;
using StockAlerts.DataProviders.Intrinio.Model.V1;
using StockAlerts.DataProviders.Intrinio.Model.V2;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockAlerts.DataProviders.Intrinio
{
    public class IntrinioClient : IStockDataWebClient
    {
        private readonly IIntrinioSettings _settings;
        private readonly IApiCallsRepository _apiCallsRepository;
        private readonly HttpClient _httpClientV1;
        private readonly HttpClient _httpClientV2;

        public IntrinioClient(
            IHttpClientFactory httpClientFactory,
            IIntrinioSettings settings,
            IApiCallsRepository apiCallsRepository)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _apiCallsRepository = apiCallsRepository ?? throw new ArgumentNullException(nameof(apiCallsRepository));
            _httpClientV1 = httpClientFactory.CreateClient(Apis.IntrinioV1);
            _httpClientV2 = httpClientFactory.CreateClient(Apis.IntrinioV2);
        }

        public async Task<PriceQuote> GetRealTimePriceQuoteAsync(string symbol)
        {
            var url = $"securities/{symbol.ToUpper()}/prices/realtime";
            var response = await _httpClientV1.GetAsync($"{url}?api_key={_settings.ApiKey}");

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

        public async Task<IList<StockInfo>> GetStockInfosAsync()
        {
            var url = $"securities?us_only=yes";
            var response = await _httpClientV2.GetAsync($"{url}&api_key={_settings.ApiKey}");

            // Log the API call
            await LogApiCallAsync(url, response.StatusCode);

            // Process response
            if (response.IsSuccessStatusCode)
            {
                var securitiesListJson = await response.Content.ReadAsStringAsync();
                var securities = JsonConvert.DeserializeObject<SecuritiesList>(securitiesListJson);
                var results = new List<StockInfo>();

                // TODO: Handle paged results
                foreach(var s in securities.Data)
                    results.Add(new StockInfo(s));

                return results;
            }

            return null;
        }

        private async Task LogApiCallAsync(string route, HttpStatusCode responseCode)
        {
            var apiCall = new ApiCall(_apiCallsRepository)
            {
                Api = Apis.IntrinioV1,
                CallTime = DateTimeOffset.UtcNow,
                Route = route,
                ResponseCode = responseCode
            };

            await apiCall.SaveAsync();
        }
    }
}
