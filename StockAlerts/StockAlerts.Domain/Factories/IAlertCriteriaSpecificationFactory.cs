using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Specifications;

namespace StockAlerts.Domain.Factories
{
    public interface IAlertCriteriaSpecificationFactory
    {
        ISpecification<AlertEvaluationMessage> CreateSpecification(AlertDefinition alertDefinition);

        ISpecification<AlertEvaluationMessage> CreateSpecification(AlertDefinition alertDefinition, AlertCriteria alertCriteria);
    }
}
