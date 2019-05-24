using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.Model
{
    public interface IQuote
    {
        string Symbol { get; }

        decimal LastPrice { get; }

        DateTimeOffset LastTime { get; }
    }
}
