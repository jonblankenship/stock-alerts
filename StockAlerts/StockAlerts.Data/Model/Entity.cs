using System;

namespace StockAlerts.Data.Model
{
    public abstract class Entity
    {
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Modified { get; set; }
    }
}
