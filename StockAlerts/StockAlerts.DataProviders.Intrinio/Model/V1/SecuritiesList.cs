using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace StockAlerts.DataProviders.Intrinio.Model.V1
{
    public partial class SecuritiesList
    {
        [JsonProperty("data")]
        public SecurityInfo[] Data { get; set; }

        [JsonProperty("result_count")]
        public long ResultCount { get; set; }

        [JsonProperty("page_size")]
        public long PageSize { get; set; }

        [JsonProperty("current_page")]
        public long CurrentPage { get; set; }

        [JsonProperty("total_pages")]
        public long TotalPages { get; set; }

        [JsonProperty("api_call_credits")]
        public long ApiCallCredits { get; set; }
    }
}
