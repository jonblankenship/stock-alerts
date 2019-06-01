using StockAlerts.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StockAlerts.Domain.Model
{
    public class AlertCriteria
    {
        public Guid AlertCriteriaId { get; set; }

        public Guid? AlertDefinitionId { get; set; }

        public Guid? ParentCriteriaId { get; set; }

        public CriteriaType Type { get; set; }

        public CriteriaOperator Operator { get; set; }

        public decimal? Level { get; set; }

        public AlertDefinition AlertDefinition { get; set; }

        public AlertCriteria ParentCriteria { get; set; }

        public ICollection<AlertCriteria> ChildrenCriteria { get; set; }

        public bool ContainsAlertCriteriaId(Guid alertCriteriaId)
        {
            if (AlertCriteriaId == alertCriteriaId) return true;

            return ChildrenCriteria.Any(c => c.ContainsAlertCriteriaId(alertCriteriaId));
        }
    }
}
