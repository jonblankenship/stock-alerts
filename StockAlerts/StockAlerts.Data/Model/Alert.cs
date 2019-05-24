using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using StockAlerts.Domain.Enums;

namespace StockAlerts.Data.Model
{
    public class Alert
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AlertId { get; set; }

        public Guid AlertDefinitionId { get; set; }

        public DateTime SentDateTime { get; set; }

        public NotificationType NotificationType { get; set; }

        public string Text { get; set; }
        
        public virtual AlertDefinition AlertDefinition { get; set; }
    }
}
