using StockAlerts.Domain.Enums;
using System;
using System.Collections.Generic;

namespace StockAlerts.Resources.Model
{
    public class AlertCriteria
    {
        public Guid AlertCriteriaId { get; set; }

        public Guid? AlertDefinitionId { get; set; }

        public Guid? ParentCriteriaId { get; set; }

        public CriteriaType Type { get; set; }

        public CriteriaOperator Operator { get; set; }

        public decimal? Level { get; set; }

        public ICollection<AlertCriteria> ChildrenCriteria { get; set; }
    }
}
