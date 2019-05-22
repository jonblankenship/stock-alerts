using System;

namespace StockAlerts.Domain.Model
{
    public class AlertDefinition
    {
        public Guid AlertDefinitionId { get; set; }

        public decimal PriceLevel { get; set; }
    }
}
