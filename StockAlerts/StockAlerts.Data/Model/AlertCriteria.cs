using StockAlerts.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using StockAlerts.Core.Enums;

namespace StockAlerts.Data.Model
{
    public class AlertCriteria : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AlertCriteriaId { get; set; }

        public Guid? AlertDefinitionId { get; set; }

        public Guid? ParentCriteriaId { get; set; }

        public CriteriaType Type { get; set; }

        public CriteriaOperator Operator { get; set; }

        public decimal? Level { get; set; }

        public virtual AlertDefinition AlertDefinition { get; set; }

        public virtual AlertCriteria ParentCriteria { get; set; }

        public virtual ICollection<AlertCriteria> ChildrenCriteria { get; set; }
    }
}
