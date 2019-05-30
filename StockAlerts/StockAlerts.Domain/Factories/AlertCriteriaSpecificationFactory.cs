using System;
using System.Collections.Generic;
using StockAlerts.Domain.Enums;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Specifications;
using System.Linq;

namespace StockAlerts.Domain.Factories
{
    public class AlertCriteriaSpecificationFactory : IAlertCriteriaSpecificationFactory
    {
        private readonly IDictionary<CriteriaType, Func<AlertDefinition, AlertCriteria, ISpecification<AlertEvaluationMessage>>> _factoryMethods;

        public AlertCriteriaSpecificationFactory()
        {
            _factoryMethods = new Dictionary<CriteriaType, Func<AlertDefinition, AlertCriteria, ISpecification<AlertEvaluationMessage>>>
            {
                { CriteriaType.Composite, CreateCompositeSpecification },
                { CriteriaType.Price, CreatePriceSpecification },
                { CriteriaType.DailyPercentageGainLoss, CreateDailyPercentageGainLossSpecification }
            };
        }

        public ISpecification<AlertEvaluationMessage> CreateSpecification(AlertDefinition alertDefinition)
        {
            return CreateSpecification(alertDefinition, alertDefinition.RootCriteria);
        }

        public ISpecification<AlertEvaluationMessage> CreateSpecification(AlertDefinition alertDefinition, AlertCriteria alertCriteria)
        {
            var factoryMethod = _factoryMethods[alertCriteria.Type];
            return factoryMethod.Invoke(alertDefinition, alertCriteria);
        }

        private ISpecification<AlertEvaluationMessage> CreateCompositeSpecification(
            AlertDefinition alertDefinition,
            AlertCriteria alertCriteria)
        {
            CompositeSpecification<AlertEvaluationMessage> specification;
            
            if (alertCriteria.Operator == CriteriaOperator.And)
                specification = new AndSpecification<AlertEvaluationMessage>();
            else if (alertCriteria.Operator == CriteriaOperator.Or)
                specification = new OrSpecification<AlertEvaluationMessage>();
            else
                throw new ApplicationException("Operator not supported.");

            var childCriteria = from ac in alertDefinition.AlertCriterias
                                where ac.ParentCriteriaId.HasValue && ac.ParentCriteriaId.Value == alertCriteria.AlertCriteriaId
                                select ac;

            foreach (var c in childCriteria)
            {
                specification.AddChildSpecification(CreateSpecification(alertDefinition, c));
            }

            return specification;
        }

        private ISpecification<AlertEvaluationMessage> CreatePriceSpecification(
            AlertDefinition alertDefinition, 
            AlertCriteria alertCriteria) =>
            new PriceSpecification(alertCriteria);

        private ISpecification<AlertEvaluationMessage> CreateDailyPercentageGainLossSpecification(
            AlertDefinition alertDefinition, 
            AlertCriteria alertCriteria) =>
            new DailyPercentageGainLossSpecification(alertCriteria);
    }
}
