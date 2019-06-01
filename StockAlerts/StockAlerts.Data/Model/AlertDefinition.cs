using StockAlerts.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockAlerts.Data.Model
{
    public class AlertDefinition : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AlertDefinitionId { get; set; }

        public Guid AppUserId { get; set; }

        public Guid StockId { get; set; }

        public AlertDefinitionStatuses Status { get; set; }
        
        public DateTimeOffset? LastSent { get; set; }

        public string Name { get; set; }

        public virtual Stock Stock { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual ICollection<AlertTriggerHistory> AlertTriggerHistories { get; set; }

        public virtual AlertCriteria RootCriteria { get; set; }
    }
}
