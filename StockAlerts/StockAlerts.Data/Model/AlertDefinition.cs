using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockAlerts.Domain.Enums;

namespace StockAlerts.Data.Model
{
    public class AlertDefinition : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AlertDefinitionId { get; set; }

        public Guid AppUserId { get; set; }

        public Guid StockId { get; set; }

        public AlertDefinitionStatuses Status { get; set; }

        public AlertDefinitionType Type { get; set; }

        public decimal PriceLevel { get; set; }

        public ComparisonOperator ComparisonOperator { get; set; }

        public virtual Stock Stock { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
