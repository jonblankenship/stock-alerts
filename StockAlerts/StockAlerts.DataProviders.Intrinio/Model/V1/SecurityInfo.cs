using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using StockAlerts.Domain.Model;

namespace StockAlerts.DataProviders.Intrinio.Model.V1
{
    public partial class SecurityInfo : IStockInfo
    {
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("figi_ticker")]
        public string FigiTicker { get; set; }

        [JsonProperty("figi")]
        public string Figi { get; set; }

        [JsonProperty("composite_figi")]
        public string CompositeFigi { get; set; }

        [JsonProperty("composite_figi_ticker")]
        public string CompositeFigiTicker { get; set; }

        [JsonProperty("security_name")]
        public string SecurityName { get; set; }

        [JsonProperty("market_sector")]
        public MarketSector MarketSector { get; set; }

        [JsonProperty("security_type")]
        public string SecurityType { get; set; }

        [JsonProperty("stock_exchange")]
        public StockExchange StockExchange { get; set; }

        [JsonProperty("last_crsp_adj_date")]
        public DateTimeOffset LastCrspAdjDate { get; set; }

        [JsonProperty("currency")]
        public Currency Currency { get; set; }

        [JsonProperty("delisted_security")]
        public bool DelistedSecurity { get; set; }

        [JsonProperty("primary_security")]
        public bool PrimarySecurity { get; set; }
    }

    public enum Currency { Usd };

    public enum MarketSector { Equity };

    public enum StockExchange { Nasdaq, Nyse };
}
