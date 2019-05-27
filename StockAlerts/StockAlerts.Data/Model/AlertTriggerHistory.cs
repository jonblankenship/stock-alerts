using System;

namespace StockAlerts.Data.Model
{
    public class AlertTriggerHistory : Entity
    {
        public Guid AlertTriggerHistoryId { get; set; }

        public Guid AlertDefinitionId { get; set; }

        public DateTimeOffset TimeTriggered { get; set; }

        public virtual AlertDefinition AlertDefinition { get; set; }
    }
}
