using StockAlerts.Core.Enums;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using System;
using StockAlerts.Core.Extensions;

namespace StockAlerts.Domain.Specifications
{
    public class DailyPercentageGainLossSpecification : ISpecification<AlertEvaluationMessage>
    {
        private readonly AlertCriteria _alertCriteria;

        public DailyPercentageGainLossSpecification(AlertCriteria alertCriteria)
        {
            _alertCriteria = alertCriteria ?? throw new ArgumentNullException(nameof(alertCriteria));
        }

        public bool IsSatisfiedBy(AlertEvaluationMessage candidate)
        {
            var percentageGainLoss = (candidate.LastPrice - candidate.OpenPrice) / candidate.OpenPrice;
            var previousPercentageGainLoss = (candidate.PreviousLastPrice - candidate.OpenPrice) / candidate.OpenPrice;

            if (_alertCriteria.Operator == CriteriaOperator.GreaterThan)
            {
                return percentageGainLoss > _alertCriteria.Level &&
                       previousPercentageGainLoss <= _alertCriteria.Level;
            }

            if (_alertCriteria.Operator == CriteriaOperator.GreaterThanOrEqualTo)
            {
                return percentageGainLoss >= _alertCriteria.Level &&
                       previousPercentageGainLoss < _alertCriteria.Level;
            }

            if (_alertCriteria.Operator == CriteriaOperator.Equals)
            {
                return percentageGainLoss == _alertCriteria.Level &&
                       previousPercentageGainLoss != _alertCriteria.Level;
            }

            if (_alertCriteria.Operator == CriteriaOperator.LessThanOrEqualTo)
            {
                return percentageGainLoss <= _alertCriteria.Level &&
                       previousPercentageGainLoss > _alertCriteria.Level;
            }

            if (_alertCriteria.Operator == CriteriaOperator.LessThan)
            {
                return percentageGainLoss < _alertCriteria.Level &&
                       previousPercentageGainLoss >= _alertCriteria.Level;
            }

            return false;
        }
    }
}
