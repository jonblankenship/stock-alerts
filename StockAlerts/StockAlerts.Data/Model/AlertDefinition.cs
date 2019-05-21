using System;
using StockAlerts.Domain.Enums;

namespace StockAlerts.Data.Model
{
    public class AlertDefinition
    {
        public Guid AlertDefinitionId { get; set; }

        public string Symbol { get; set; }

        public AlertDefinitionStatuses Status { get; set; }

        public AlertDefinitionType Type { get; set; }

        public decimal PriceLevel { get; set; }

        public ComparisonOperator ComparisonOperator { get; set; }
    }
}
