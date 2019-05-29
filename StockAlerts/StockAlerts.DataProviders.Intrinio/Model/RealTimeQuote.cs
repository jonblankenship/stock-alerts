using Newtonsoft.Json;
using System;
using StockAlerts.Domain.Model;

namespace StockAlerts.DataProviders.Intrinio.Model
{
    public class RealTimeQuote : IQuote
    {
        [JsonProperty("last_price")]
        public decimal LastPrice { get; set; }

        [JsonProperty("last_time")]
        public DateTimeOffset LastTime { get; set; }

        [JsonProperty("bid_price")]
        public object BidPrice { get; set; }

        [JsonProperty("bid_size")]
        public object BidSize { get; set; }

        [JsonProperty("ask_price")]
        public object AskPrice { get; set; }

        [JsonProperty("ask_size")]
        public object AskSize { get; set; }

        [JsonProperty("open_price")]
        public decimal OpenPrice { get; set; }

        [JsonProperty("high_price")]
        public double HighPrice { get; set; }

        [JsonProperty("low_price")]
        public double LowPrice { get; set; }

        [JsonProperty("exchange_volume")]
        public long ExchangeVolume { get; set; }

        [JsonProperty("market_volume")]
        public long MarketVolume { get; set; }

        [JsonProperty("updated_on")]
        public DateTimeOffset UpdatedOn { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("security")]
        public Security Security { get; set; }

        public string Symbol => Security?.Ticker;
    }
}
