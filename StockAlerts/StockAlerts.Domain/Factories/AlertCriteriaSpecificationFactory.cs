using StockAlerts.Core.Enums;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Specifications;
using System;
using System.Collections.Generic;

namespace StockAlerts.Domain.Factories
{
    public class AlertCriteriaSpecificationFactory : IAlertCriteriaSpecificationFactory
    {
        private readonly IDictionary<CriteriaType, Func<AlertCriteria, ISpecification<AlertEvaluationMessage>>> _factoryMethods;

        public AlertCriteriaSpecificationFactory()
        {
            _factoryMethods = new Dictionary<CriteriaType, Func<AlertCriteria, ISpecification<AlertEvaluationMessage>>>
            {
                { CriteriaType.Composite, CreateCompositeSpecification },
                { CriteriaType.Price, CreatePriceSpecification },
                { CriteriaType.DailyPercentageGainLoss, CreateDailyPercentageGainLossSpecification }
            };
        }

        public ISpecification<AlertEvaluationMessage> CreateSpecification(AlertDefinition alertDefinition)
        {
            return CreateSpecification(alertDefinition.RootCriteria);
        }

        public ISpecification<AlertEvaluationMessage> CreateSpecification(AlertCriteria alertCriteria)
        {
            var factoryMethod = _factoryMethods[alertCriteria.Type];
            return factoryMethod.Invoke(alertCriteria);
        }

        private ISpecification<AlertEvaluationMessage> CreateCompositeSpecification(
            AlertCriteria alertCriteria)
        {
            CompositeSpecification<AlertEvaluationMessage> specification;
            
            if (alertCriteria.Operator == CriteriaOperator.And)
                specification = new AndSpecification<AlertEvaluationMessage>();
            else if (alertCriteria.Operator == CriteriaOperator.Or)
                specification = new OrSpecification<AlertEvaluationMessage>();
            else
                throw new ApplicationException("Operator not supported.");

            foreach (var c in alertCriteria.ChildrenCriteria)
            {
                specification.AddChildSpecification(CreateSpecification(c));
            }

            return specification;
        }

        private ISpecification<AlertEvaluationMessage> CreatePriceSpecification(
            AlertCriteria alertCriteria) =>
            new PriceSpecification(alertCriteria);

        private ISpecification<AlertEvaluationMessage> CreateDailyPercentageGainLossSpecification(
            AlertCriteria alertCriteria) =>
            new DailyPercentageGainLossSpecification(alertCriteria);
    }
}
