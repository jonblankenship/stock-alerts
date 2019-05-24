using Newtonsoft.Json;

namespace StockAlerts.DataProviders.Intrinio.Model
{
    public class Security
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("exchange_ticker")]
        public string ExchangeTicker { get; set; }

        [JsonProperty("figi")]
        public string Figi { get; set; }

        [JsonProperty("composite_figi")]
        public string CompositeFigi { get; set; }
    }
}
