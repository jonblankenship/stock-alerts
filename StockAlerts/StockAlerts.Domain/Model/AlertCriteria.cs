using StockAlerts.Domain.Enums;
using System;

namespace StockAlerts.Domain.Model
{
    public class AlertCriteria
    {
        public Guid AlertCriteriaId { get; set; }

        public Guid AlertDefinitionId { get; set; }

        public Guid? ParentCriteriaId { get; set; }

        public Guid? RootCriteriaId { get; set; }

        public CriteriaType Type { get; set; }

        public CriteriaOperator Operator { get; set; }

        public decimal? Level { get; set; }

        public virtual AlertDefinition AlertDefinition { get; set; }

        public virtual AlertCriteria ParentCriteria { get; set; }

        public virtual AlertCriteria RootCriteria { get; set; }
    }
}
