using System;

namespace StockAlerts.Domain.Model
{
    public class PriceQuote
    {
        public PriceQuote()
        { { { } } }

        public PriceQuote(IQuote quote)
        {
            Symbol = quote.Symbol;
            LastPrice = quote.LastPrice;
            LastTime = quote.LastTime;
        }

        public string Symbol { get; set; }

        public decimal LastPrice { get; set; }

        public DateTimeOffset LastTime { get; set; }
    }
}
