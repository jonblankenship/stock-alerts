using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Data.Repositories;
using StockAlerts.Data.Tests.Unit.Utilities;
using StockAlerts.Domain.Enums;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StockAlerts.Data.Tests.Unit.Repositories
{
    public class AlertDefinitionsRepositoryTests
    {
        [Fact]
        public async Task GetAlertDefinitionsAsync_UnknownAppUserIdProvided_EmptyListReturned()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context, CreateMapper());

            // Act
            var results = await repository.GetAlertDefinitionsAsync(Guid.NewGuid());

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAlertDefinitionsAsync_AppUserIdProvided_AlertDefinitionsReturned()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context, CreateMapper());
            var appUser = await context.AppUsers.FirstAsync();
            var expectedAlertDefinitionIds = (from ad in context.AlertDefinitions
                                              where ad.AppUserId == appUser.AppUserId
                                              select ad.AlertDefinitionId).ToList();

            // Act
            var results = (await repository.GetAlertDefinitionsAsync(appUser.AppUserId)).ToList();

            // Assert
            results.Count.Should().Be(expectedAlertDefinitionIds.Count);            
            foreach (var r in results)
            {
                expectedAlertDefinitionIds.Should().Contain(r.AlertDefinitionId);
            }
        }

        [Fact]
        public async Task GetAlertDefinitionAsync_InvalidAlertDefinitionId_NotFoundException()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context, CreateMapper());

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async() => await repository.GetAlertDefinitionAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetAlertDefinitionAsync_AlertDefinitionId_AlertDefinitionReturned()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var mapper = CreateMapper();
            var repository = CreateRepository(context, mapper);
            var expectedAlertDefinition = await AddComplexAlertDefinitionToContextAsync(context, mapper);

            // Act
            var result = await repository.GetAlertDefinitionAsync(expectedAlertDefinition.AlertDefinitionId);

            // Assert
            result.AlertDefinitionId.Should().Be(expectedAlertDefinition.AlertDefinitionId);
            result.AppUser.AppUserId.Should().Be(expectedAlertDefinition.AppUser.AppUserId);
            result.Stock.StockId.Should().Be(expectedAlertDefinition.Stock.StockId);

            AssertCriteria(expectedAlertDefinition.RootCriteria, result.RootCriteria);
        }

        private void AssertCriteria(AlertCriteria expected, AlertCriteria actual)
        {
            actual.AlertCriteriaId.Should().Be(expected.AlertCriteriaId);
            actual.Type.Should().Be(expected.Type);
            actual.Operator.Should().Be(expected.Operator);
            actual.Level.Should().Be(expected.Level);

            actual.ChildrenCriteria.Count.Should().Be(expected.ChildrenCriteria.Count);
            foreach (var actualAlertCriteria in actual.ChildrenCriteria)
            {
                var expectedAlertCriteria =
                    expected.ChildrenCriteria.Single(c => c.AlertCriteriaId == actualAlertCriteria.AlertCriteriaId);

                AssertCriteria(expectedAlertCriteria, actualAlertCriteria);
            }
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DataModelMappingProfile>();
            });

            return config.CreateMapper();
        }

        private IAlertDefinitionsRepository CreateRepository(
            ApplicationDbContext context, 
            IMapper mapper) =>
            new AlertDefinitionsRepository(context, mapper);

        private async Task<AlertDefinition> AddComplexAlertDefinitionToContextAsync(
            ApplicationDbContext context,
            IMapper mapper)
        {
            var appUser = mapper.Map<AppUser>(await context.AppUsers.FirstAsync());
            var stock = mapper.Map<Stock>(await context.Stocks.FirstAsync());
            var alertDefinition = new AlertDefinition
            {
                Name = "Complex Alert Definition",
                Stock = stock,
                AppUser = appUser,
                AlertTriggerHistories = new List<AlertTriggerHistory>
                {
                    new AlertTriggerHistory {TimeTriggered = DateTimeOffset.UtcNow.AddMinutes(-2342)},
                    new AlertTriggerHistory {TimeTriggered = DateTimeOffset.UtcNow.AddMinutes(-1142)}
                },
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
                                    Operator = CriteriaOperator.GreaterThanOrEqualTo,
                                    Level = 250
                                },
                                new AlertCriteria
                                {
                                    Type = CriteriaType.Composite,
                                    Operator = CriteriaOperator.And,
                                    ChildrenCriteria = new List<AlertCriteria>
                                    {
                                        new AlertCriteria
                                        {
                                            Type = CriteriaType.DailyPercentageGainLoss,
                                            Operator = CriteriaOperator.GreaterThanOrEqualTo,
                                            Level = 0.02M
                                        },
                                        new AlertCriteria
                                        {
                                            Type = CriteriaType.DailyPercentageGainLoss,
                                            Operator = CriteriaOperator.LessThanOrEqualTo,
                                            Level = 0.06M
                                        }
                                    }
                                }
                            }
                        },
                        new AlertCriteria
                        {
                            Type = CriteriaType.Price,
                            Operator = CriteriaOperator.LessThanOrEqualTo,
                            Level = 100
                        }
                    }
                }
            };

            var dataObject = mapper.Map<Data.Model.AlertDefinition>(alertDefinition);

            await context.AddAsync(dataObject);
            await context.SaveChangesAsync();

            return mapper.Map<AlertDefinition>(dataObject);
        }
    }
}
