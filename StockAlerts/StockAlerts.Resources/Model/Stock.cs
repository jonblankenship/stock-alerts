using System;

namespace StockAlerts.Resources.Model
{
    public class Stock
    {
        public Guid StockId { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public decimal LastPrice { get; set; }
    }
}
