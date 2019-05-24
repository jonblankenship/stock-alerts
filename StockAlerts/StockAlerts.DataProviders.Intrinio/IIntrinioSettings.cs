using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.DataProviders.Intrinio
{
    public interface IIntrinioSettings
    {
        string ApiKey { get; set; }
    }
}
