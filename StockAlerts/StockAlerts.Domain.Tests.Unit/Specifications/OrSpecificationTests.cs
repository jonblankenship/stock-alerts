using FluentAssertions;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Specifications;
using Xunit;

namespace StockAlerts.Domain.Tests.Unit.Specifications
{
    public class OrSpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy_NoChildren_False()
        {
            // Arrange
            var spec = new OrSpecification<AlertEvaluationMessage>();
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_OneTrueChild_True()
        {
            // Arrange
            var spec = new OrSpecification<AlertEvaluationMessage>();
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_OneFalseChild_False()
        {
            // Arrange
            var spec = new OrSpecification<AlertEvaluationMessage>();
            spec.AddChildSpecification(new FalseSpecification<AlertEvaluationMessage>());
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_TwoTrueChildren_True()
        {
            // Arrange
            var spec = new OrSpecification<AlertEvaluationMessage>();
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_TwoFalseChildren_False()
        {
            // Arrange
            var spec = new OrSpecification<AlertEvaluationMessage>();
            spec.AddChildSpecification(new FalseSpecification<AlertEvaluationMessage>());
            spec.AddChildSpecification(new FalseSpecification<AlertEvaluationMessage>());
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_OneTrueAndOneFalseChildren_True()
        {
            // Arrange
            var spec = new OrSpecification<AlertEvaluationMessage>();
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            spec.AddChildSpecification(new FalseSpecification<AlertEvaluationMessage>());
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_ThreeTrueChildren_True()
        {
            // Arrange
            var spec = new OrSpecification<AlertEvaluationMessage>();
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_TwoTrueAndOneFalseChildren_True()
        {
            // Arrange
            var spec = new OrSpecification<AlertEvaluationMessage>();
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            spec.AddChildSpecification(new TrueSpecification<AlertEvaluationMessage>());
            spec.AddChildSpecification(new FalseSpecification<AlertEvaluationMessage>());
            var message = new AlertEvaluationMessage();

            // Act
            var result = spec.IsSatisfiedBy(message);

            // Assert
            result.Should().BeTrue();
        }
    }
}
