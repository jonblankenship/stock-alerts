using FluentAssertions;
using StockAlerts.Core.Enums;
using StockAlerts.Core.Extensions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Specifications;
using Xunit;

namespace StockAlerts.Domain.Tests.Unit.Specifications
{
    public class PriceSpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy_NoOperator_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria();
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_LessThan_LastPriceLessThanLevel_PrevLastPriceEqualLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThan, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 49.5M, PreviousLastPrice = 50 };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public void IsSatisfiedBy_LessThan_LastPriceEqualLevel_PrevLastPriceGreaterThanLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThan, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 50M, PreviousLastPrice = 50.1M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_LessThanOrEqual_LastPriceLessThanLevel_PrevLastPriceGreaterThanLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThanOrEqualTo, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 49.9M, PreviousLastPrice = 50.1M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_LessThanOrEqual_LastPriceEqualLevel_PrevLastPriceGreaterThanLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThanOrEqualTo, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 50, PreviousLastPrice = 50.1M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_LessThanOrEqual_LastPriceGreaterThanLevel_PrevLastPriceEqualLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.LessThanOrEqualTo, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 51, PreviousLastPrice = 50 };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }


        [Fact]
        public void IsSatisfiedBy_GreaterThan_LastPriceGreaterThanLevel_PrevLastPriceEqualLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThan, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 50.5M, PreviousLastPrice = 50 };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_GreaterThan_LastPriceEqualLevel_PrevLastPriceLessThanLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThan, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 50M, PreviousLastPrice = 49.9M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_GreaterThanOrEqual_LastPriceGreaterThanLevel_PrevLastPriceLessThanLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThanOrEqualTo, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 50.1M, PreviousLastPrice = 49.9M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_GreaterThanOrEqual_LastPriceEqualLevel_PrevLastPriceLessThanLevel_True()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThanOrEqualTo, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 50, PreviousLastPrice = 49.9M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_GreaterThanOrEqual_LastPriceLessThanLevel_PrevLastPriceEqualLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.GreaterThanOrEqualTo, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 49, PreviousLastPrice = 50 };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_EqualTo_LastPriceLessThanLevel_PrevLastPriceEqualLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.Equals, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 49, PreviousLastPrice = 50 };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_EqualTo_LastPriceEqualToLevel_PrevLastPriceGreaterThanLevel_False()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Operator = CriteriaOperator.Equals, Level = 50 };
            var spec = new PriceSpecification(alertCriteria);
            var message = new AlertEvaluationMessage { LastPrice = 50, PreviousLastPrice = 50.1M };

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }
    }
}
