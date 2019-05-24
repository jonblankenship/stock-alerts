using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockAlerts.DataProviders.Intrinio.Model;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Services;

namespace StockAlerts.DataProviders.Intrinio
{
    public class IntrinioClient : IStockDataWebClient
    {
        private readonly IIntrinioSettings _settings;
        private readonly HttpClient _httpClient;

        public IntrinioClient(
            IHttpClientFactory httpClientFactory,
            IIntrinioSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _httpClient = httpClientFactory.CreateClient(HttpsClients.IntrinioApi);
        }

        public async Task<PriceQuote> GetRealTimePriceQuoteAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"securities/{symbol.ToUpper()}/prices/realtime?api_key={_settings.ApiKey}");
            var quoteJson = await response.Content.ReadAsStringAsync();
            var quote = JsonConvert.DeserializeObject<RealTimeQuote>(quoteJson);
            return new PriceQuote(quote);
        }
    }
}
