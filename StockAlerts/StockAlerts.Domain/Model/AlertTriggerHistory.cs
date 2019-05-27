using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.Model
{
    public class AlertTriggerHistory
    {
        public Guid AlertTriggerHistoryId { get; set; }

        public Guid AlertDefinitionId { get; set; }

        public DateTimeOffset TimeTriggered { get; set; }
    }
}
