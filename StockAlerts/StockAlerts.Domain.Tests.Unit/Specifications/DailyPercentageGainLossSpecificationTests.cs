using FluentAssertions;
using StockAlerts.Domain.Enums;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Specifications;
using Xunit;

namespace StockAlerts.Domain.Tests.Unit.Specifications
{
    public class DailyPercentageGainLossSpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy_LessThan_LastPercentLessThanLevel_PrevLastPercentEqualLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThan, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.01M, PreviousLastPrice = 1.02M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public void IsSatisfiedBy_LessThan_LastPercentEqualLevel_PrevLastPercentGreaterThanLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThan, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.02M, PreviousLastPrice = 1.03M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_LessThanOrEqual_LastPercentLessThanLevel_PrevLastPercentGreaterThanLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThanOrEqualTo, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.01M, PreviousLastPrice = 1.03M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_LessThanOrEqual_LastPercentEqualLevel_PrevLastPercentGreaterThanLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThanOrEqualTo, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.02M, PreviousLastPrice = 1.03M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_LessThanOrEqual_LastPercentGreaterThanLevel_PrevLastPercentEqualLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThanOrEqualTo, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.03M, PreviousLastPrice = 1.02M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }


        [Fact]
        public void IsSatisfiedBy_GreaterThan_LastPercentGreaterThanLevel_PrevLastPercentEqualLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThan, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.03M, PreviousLastPrice = 1.02M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_GreaterThan_LastPercentEqualLevel_PrevLastPercentLessThanLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThan, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.02M, PreviousLastPrice = 1.01M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_GreaterThanOrEqual_LastPercentGreaterThanLevel_PrevLastPercentLessThanLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThanOrEqualTo, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.03M, PreviousLastPrice = 1.01M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_GreaterThanOrEqual_LastPercentEqualLevel_PrevLastPercentLessThanLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThanOrEqualTo, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.02M, PreviousLastPrice = 1.01M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_GreaterThanOrEqual_LastPercentLessThanLevel_PrevLastPercentEqualLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThanOrEqualTo, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.01M, PreviousLastPrice = 1.02M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_EqualTo_LastPercentLessThanLevel_PrevLastPercentEqualLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.Equals, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.01M, PreviousLastPrice = 1.02M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_EqualTo_LastPercentEqualToLevel_PrevLastPercentGreaterThanLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.Equals, Level = 0.02M };
            var spec = new DailyPercentageGainLossSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { OpenPrice = 1, LastPrice = 1.02M, PreviousLastPrice = 1.03M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }
    }
}
