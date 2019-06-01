using FluentAssertions;
using Moq;
using StockAlerts.Domain.Enums;
using StockAlerts.Domain.Factories;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;
using StockAlerts.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StockAlerts.Domain.Tests.Unit.Factories
{
    public class AlertCriteriaSpecificationFactoryTests
    {
        [Fact]
        public void CreateSpecification_AlertDefinitionWithSingleCriteria_SpecificationCreated()
        {
            // Arrange
            var factory = new AlertCriteriaSpecificationFactory();
            var alertDefinitionsRepositoryMock = new Mock<IAlertDefinitionsRepository>();
            var notificationServiceMock = new Mock<INotificationsService>();
            var alertDefinition = new AlertDefinition(alertDefinitionsRepositoryMock.Object, factory, notificationServiceMock.Object)
            {
                RootCriteria = new AlertCriteria
                    {
                        Type = CriteriaType.Price,
                        Operator = CriteriaOperator.LessThanOrEqualTo,
                        Level = 50
                    }
            };

            // Act
            var specification = factory.CreateSpecification(alertDefinition);

            // Assert
            specification.Should().BeOfType<PriceSpecification>();
        }

        [Fact]
        public void CreateSpecification_AlertDefinitionWithMultipleCriteria_SpecificationCreated()
        {
            // Arrange
            var factory = new AlertCriteriaSpecificationFactory();
            var alertDefinitionsRepositoryMock = new Mock<IAlertDefinitionsRepository>();
            var notificationServiceMock = new Mock<INotificationsService>();
            var rootCriteriaId = Guid.NewGuid();
            var alertDefinition = new AlertDefinition(alertDefinitionsRepositoryMock.Object, factory, notificationServiceMock.Object)
            {
                RootCriteria = new AlertCriteria
                    {
                        Type = CriteriaType.Composite,
                        Operator = CriteriaOperator.And,
                        ChildrenCriteria = new List<AlertCriteria>
                        {
                            new AlertCriteria
                            {
                                Type = CriteriaType.Price,
                                Operator = CriteriaOperator.LessThanOrEqualTo,
                                Level = 50
                            },
                            new AlertCriteria
                            {
                                Type = CriteriaType.DailyPercentageGainLoss,
                                Operator = CriteriaOperator.LessThanOrEqualTo,
                                Level = -0.02M
                            }
                        }
                    }
            };

            // Act
            var specification = factory.CreateSpecification(alertDefinition);

            // Assert
            specification.Should().BeOfType<AndSpecification<AlertEvaluationMessage>>();
            var children = (specification as CompositeSpecification<AlertEvaluationMessage>).Children;
            children.Count.Should().Be(2);
            children.First().Should().BeOfType<PriceSpecification>();
            children.Last().Should().BeOfType<DailyPercentageGainLossSpecification>();
        }

        [Fact]
        public void CreateSpecification_AlertDefinitionWithMultipleComposites_SpecificationCreated()
        {
            // Arrange
            var factory = new AlertCriteriaSpecificationFactory();
            var alertDefinitionsRepositoryMock = new Mock<IAlertDefinitionsRepository>();
            var notificationServiceMock = new Mock<INotificationsService>();
            var alertDefinition = new AlertDefinition(alertDefinitionsRepositoryMock.Object, factory, notificationServiceMock.Object)
            {
                RootCriteria = new AlertCriteria
                    {
                        Type = CriteriaType.Composite,
                        Operator = CriteriaOperator.And,
                        ChildrenCriteria = new List<AlertCriteria>
                        {
                            new AlertCriteria
                            {
                                Type = CriteriaType.Composite,
                                Operator = CriteriaOperator.Or,
                                ChildrenCriteria = new List<AlertCriteria>
                                {
                                    new AlertCriteria
                                    {
                                        Type = CriteriaType.Price,
                                        Operator = CriteriaOperator.LessThanOrEqualTo,
                                        Level = 50
                                    },
                                    new AlertCriteria
                                    {
                                        Type = CriteriaType.DailyPercentageGainLoss,
                                        Operator = CriteriaOperator.LessThanOrEqualTo,
                                        Level = -0.02M
                                    }
                                }
                            },
                            new AlertCriteria
                            {
                                Type = CriteriaType.Composite,
                                Operator = CriteriaOperator.Or,
                                ChildrenCriteria = new List<AlertCriteria>
                                {
                                    new AlertCriteria
                                    {
                                        Type = CriteriaType.Price,
                                        Operator = CriteriaOperator.GreaterThanOrEqualTo,
                                        Level = 80
                                    },
                                    new AlertCriteria
                                    {
                                        Type = CriteriaType.DailyPercentageGainLoss,
                                        Operator = CriteriaOperator.GreaterThanOrEqualTo,
                                        Level = 0.02M
                                    }
                                }
                            }
                        }
                    }
            };

            // Act
            var specification = factory.CreateSpecification(alertDefinition);

            // Assert
            specification.Should().BeOfType<AndSpecification<AlertEvaluationMessage>>();
            var rootChildren = (specification as CompositeSpecification<AlertEvaluationMessage>).Children;
            rootChildren.Count.Should().Be(2);
            rootChildren.First().Should().BeOfType<OrSpecification<AlertEvaluationMessage>>();
            var firstOrChildren = (rootChildren.First() as CompositeSpecification<AlertEvaluationMessage>).Children;
            firstOrChildren.Count.Should().Be(2);
            firstOrChildren.First().Should().BeOfType<PriceSpecification>();
            firstOrChildren.Last().Should().BeOfType<DailyPercentageGainLossSpecification>();
            var secondOrChildren = (rootChildren.Last() as CompositeSpecification<AlertEvaluationMessage>).Children;
            secondOrChildren.Count.Should().Be(2);
            secondOrChildren.First().Should().BeOfType<PriceSpecification>();
            secondOrChildren.Last().Should().BeOfType<DailyPercentageGainLossSpecification>();
        }
    }
}
