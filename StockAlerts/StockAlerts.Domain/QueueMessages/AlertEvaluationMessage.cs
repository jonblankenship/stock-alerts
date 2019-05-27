using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.QueueMessages
{
    public class AlertEvaluationMessage
    {
        public Guid AlertDefinitionId { get; set; }

        public decimal LastPrice { get; set; }

        public decimal PreviousLastPrice { get; set; }
    }
}
