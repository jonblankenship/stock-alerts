using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StockAlerts.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockAlerts.Data.Model
{
    public class AlertCriteria : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AlertCriteriaId { get; set; }

        public Guid AlertDefinitionId { get; set; }

        public Guid? ParentCriteriaId { get; set; }

        public Guid? RootCriteriaId { get; set; }

        public CriteriaType Type { get; set; }

        public CriteriaOperator Operator { get; set; }

        public decimal? Level { get; set; }

        public virtual AlertDefinition AlertDefinition { get; set; }

        [ForeignKey(nameof(ParentCriteriaId))]
        public virtual AlertCriteria ParentCriteria { get; set; }

        [ForeignKey(nameof(RootCriteriaId))]
        public virtual AlertCriteria RootCriteria { get; set; }
    }
}
