using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace StockAlerts.Data.Model
{
    public class ApiCall : Entity
    {
        public Guid ApiCallId { get; set; }

        public string Api { get; set; }

        public string Route { get; set; }

        public HttpStatusCode ResponseCode { get; set; }

        public DateTimeOffset CallTime { get; set; }
    }
}
