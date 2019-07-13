using StockAlerts.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockAlerts.Domain.Model
{
    public class AlertCriteria
    {
        private static IEnumerable<CriteriaOperator> _compositeOperators =
            new List<CriteriaOperator> {CriteriaOperator.And, CriteriaOperator.Or};

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

            return ChildrenCriteria?.Any(c => c.ContainsAlertCriteriaId(alertCriteriaId)) ?? false;
        }

        public IList<string> Validate(IList<string> errors)
        {
            if (errors == null)
                errors = new List<string>();

            if (Type == CriteriaType.Composite)
                ValidateComposite(errors);
            else
                ValidateNonComposite(errors);

            return errors;
        }

        private void ValidateComposite(IList<string> errors)
        {
            if (!_compositeOperators.Contains(Operator))
                errors.Add($"Alert Criteria of type Composite cannot have an operator of type {Operator}.");
            if (Level.HasValue)
                errors.Add("Alert Criteria of type Composite must have a null level.");
            if (ChildrenCriteria.Count < 1)
                errors.Add("Alert Criteria of type Composite should have at least one child.");
            
            foreach(var c in ChildrenCriteria)
                c.Validate(errors);
        }

        private void ValidateNonComposite(IList<string> errors)
        {
            if (_compositeOperators.Contains(Operator))
                errors.Add($"Non-composite Alert Criteria cannot have an operator of type {Operator}.");
            if (!Level.HasValue)
                errors.Add($"Alert Criteria of type {Type} must have a level.");
            if (ChildrenCriteria.Any())
                errors.Add("Non-composite Alert Criteria may not have children.");
        }
    }
}
