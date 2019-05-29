using StockAlerts.Domain.Enums;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Specifications;
using System.Linq;

namespace StockAlerts.Domain.Factories
{
    public class AlertCriteriaSpecificationFactory : IAlertCriteriaSpecificationFactory
    {
        public ISpecification<AlertEvaluationMessage> CreateSpecification(AlertDefinition alertDefinition)
        {
            return CreateSpecification(alertDefinition, alertDefinition.RootCriteria);
        }

        public ISpecification<AlertEvaluationMessage> CreateSpecification(AlertDefinition alertDefinition, AlertCriteria alertCriteria)
        {
            switch (alertCriteria.Type)
            {
                case CriteriaType.Composite:
                    switch (alertCriteria.Operator)
                    {
                        case CriteriaOperator.And:
                            return CreateAndSpecification(alertDefinition, alertCriteria);
                        case CriteriaOperator.Or:
                            return CreateOrSpecification(alertDefinition, alertCriteria);
                    }
                    break;                        
                case CriteriaType.Price:
                    return CreatePriceSpecification(alertCriteria);
                case CriteriaType.DailyPercentageGainLoss:
                    return CreateDailyPercentageGainLossSpecification(alertCriteria);
            }         

            return null;
        }

        private ISpecification<AlertEvaluationMessage> CreateAndSpecification(
            AlertDefinition alertDefinition,
            AlertCriteria alertCriteria)
        {
            var andSpecification = new AndSpecification<AlertEvaluationMessage>();
            var childCriteria = (from ac in alertDefinition.AlertCriterias
                where ac.ParentCriteriaId.HasValue && ac.ParentCriteriaId.Value == alertCriteria.AlertCriteriaId
                select ac).ToList();

            foreach (var c in childCriteria)
            {
                andSpecification.AddChildSpecification(CreateSpecification(alertDefinition, c));
            }

            return andSpecification;
        }

        private ISpecification<AlertEvaluationMessage> CreateOrSpecification(
            AlertDefinition alertDefinition,
            AlertCriteria alertCriteria)
        {
            var orSpecification = new OrSpecification<AlertEvaluationMessage>();
            var childCriteria = (from ac in alertDefinition.AlertCriterias
                                 where ac.ParentCriteriaId.HasValue && ac.ParentCriteriaId.Value == alertCriteria.AlertCriteriaId
                                 select ac).ToList();

            foreach (var c in childCriteria)
            {
                orSpecification.AddChildSpecification(CreateSpecification(alertDefinition, c));
            }

            return orSpecification;
        }

        private ISpecification<AlertEvaluationMessage> CreatePriceSpecification(AlertCriteria alertCriteria) =>
            new PriceSpecification(alertCriteria);

        private ISpecification<AlertEvaluationMessage> CreateDailyPercentageGainLossSpecification(AlertCriteria alertCriteria) =>
            new DailyPercentageGainLossSpecification(alertCriteria);
    }
}
