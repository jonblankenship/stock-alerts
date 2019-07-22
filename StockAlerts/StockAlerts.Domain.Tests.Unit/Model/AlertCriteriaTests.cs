using FluentAssertions;
using StockAlerts.Core.Enums;
using StockAlerts.Core.Extensions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Specifications;
using Xunit;

namespace StockAlerts.Domain.Tests.Unit.Model
{
    public class AlertCriteriaTests
    {
        [Fact]
        public void ToString_Price_ExpectedStringReturned()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.Equals, Level = 50.50M };

            // Act
            var description = alertCriteria.ToString();

            // Assert
            description.Should().Be($"{CriteriaType.Price.ToDisplayString()} {CriteriaOperator.Equals.ToDisplayString()} {50.50M:C}");
        }
        [Fact]
        public void ToString_DailyPercentageGain_ExpectedStringReturned()
        {
            // Arrange
            var alertCriteria = new AlertCriteria { Type = CriteriaType.DailyPercentageGainLoss, Operator = CriteriaOperator.Equals, Level = 0.02M };

            // Act
            var description = alertCriteria.ToString();

            // Assert
            description.Should().Be($"{CriteriaType.DailyPercentageGainLoss.ToDisplayString()} {CriteriaOperator.Equals.ToDisplayString()} {0.02M:P2}");
        }

        [Fact]
        public void ToString_And_OneSimpleChildCriteria_ExpectedStringReturned()
        {
            var alertCriteria1 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.Equals, Level = 50.50M };

            var rootCriteria = new AlertCriteria
            {
                Type = CriteriaType.Composite,
                Operator = CriteriaOperator.And,
                ChildrenCriteria =
                {
                    alertCriteria1
                }
            };

            // Act
            var description = rootCriteria.ToString();

            // Assert
            description.Should().Be(alertCriteria1.ToString());
        }

        [Fact]
        public void ToString_TwoSimpleChildCriteria_ExpectedStringReturned()
        {
            var alertCriteria1 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.GreaterThan, Level = 50.50M };
            var alertCriteria2 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.LessThan, Level = 52.00M };

            var rootCriteria = new AlertCriteria
            {
                Type = CriteriaType.Composite,
                Operator = CriteriaOperator.And,
                ChildrenCriteria =
                {
                    alertCriteria1,
                    alertCriteria2
                }
            };

            // Act
            var description = rootCriteria.ToString();

            // Assert
            description.Should().Be($"{alertCriteria1} AND {alertCriteria2}");
        }

        [Fact]
        public void ToString_ThreeSimpleChildCriteria_ExpectedStringReturned()
        {
            var alertCriteria1 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.GreaterThan, Level = 50.50M };
            var alertCriteria2 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.LessThan, Level = 52.00M };
            var alertCriteria3 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.LessThan, Level = 54.00M };

            var rootCriteria = new AlertCriteria
            {
                Type = CriteriaType.Composite,
                Operator = CriteriaOperator.And,
                ChildrenCriteria =
                {
                    alertCriteria1,
                    alertCriteria2,
                    alertCriteria3
                }
            };

            // Act
            var description = rootCriteria.ToString();

            // Assert
            description.Should().Be($"{alertCriteria1} AND {alertCriteria2} AND {alertCriteria3}");
        }

        [Fact]
        public void ToString_CompositeChildCriteria_ExpectedStringReturned()
        {
            var alertCriteria1 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.GreaterThan, Level = 50.50M };
            var alertCriteria2 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.LessThan, Level = 52.00M };
            var alertCriteria3 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.GreaterThan, Level = 51.50M };
            var alertCriteria4 = new AlertCriteria { Type = CriteriaType.Price, Operator = CriteriaOperator.LessThan, Level = 53.00M };

            var andCriteria = new AlertCriteria
            {
                Type = CriteriaType.Composite,
                Operator = CriteriaOperator.And,
                ChildrenCriteria =
                {
                    alertCriteria1,
                    alertCriteria2
                }
            };

            var orCriteria = new AlertCriteria
            {
                Type = CriteriaType.Composite,
                Operator = CriteriaOperator.Or,
                ChildrenCriteria =
                {
                    alertCriteria3,
                    alertCriteria4
                }
            };

            var rootCriteria = new AlertCriteria
            {
                Type = CriteriaType.Composite,
                Operator = CriteriaOperator.And,
                ChildrenCriteria =
                {
                    andCriteria,
                    orCriteria
                }
            };

            // Act
            var description = rootCriteria.ToString();

            // Assert
            description.Should().Be($"({andCriteria}) AND ({orCriteria})");
        }
    }
}
