using StockAlerts.Domain.QueueMessages;
using System;
using StockAlerts.Domain.Enums;
using StockAlerts.Domain.Model;

namespace StockAlerts.Domain.Specifications
{
    public class PriceSpecification : ISpecification<AlertEvaluationMessage>
    {
        private readonly AlertCriteria _alertCriteria;

        public PriceSpecification(AlertCriteria alertCriteria)
        {
            _alertCriteria = alertCriteria ?? throw new ArgumentNullException(nameof(alertCriteria));
        }

        public bool IsSatisfiedBy(AlertEvaluationMessage candidate)
        {
            if (_alertCriteria.Operator == CriteriaOperator.GreaterThan)
            {
                return candidate.LastPrice > _alertCriteria.Level &&
                    candidate.PreviousLastPrice <= _alertCriteria.Level;
            }

            if (_alertCriteria.Operator == CriteriaOperator.GreaterThanOrEqualTo)
            {
                return candidate.LastPrice >= _alertCriteria.Level &&
                       candidate.PreviousLastPrice < _alertCriteria.Level;
            }

            if (_alertCriteria.Operator == CriteriaOperator.Equals)
            {
                return candidate.LastPrice == _alertCriteria.Level &&
                       candidate.PreviousLastPrice != _alertCriteria.Level;

            }

            if (_alertCriteria.Operator == CriteriaOperator.LessThanOrEqualTo)
            {
                return candidate.LastPrice <= _alertCriteria.Level &&
                       candidate.PreviousLastPrice > _alertCriteria.Level;

            }

            if (_alertCriteria.Operator == CriteriaOperator.LessThan)
            {
                return candidate.LastPrice < _alertCriteria.Level &&
                       candidate.PreviousLastPrice >= _alertCriteria.Level;
            }

            return false;
        }
    }
}
