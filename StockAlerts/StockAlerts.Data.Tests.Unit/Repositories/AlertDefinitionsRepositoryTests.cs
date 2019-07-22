using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Core.Enums;
using StockAlerts.Data.Repositories;
using StockAlerts.Data.Tests.Unit.Utilities;
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
            var expectedAlertDefinition = await CreateAndSaveComplexAlertDefinitionAsync(context, mapper);

            // Act
            var result = await repository.GetAlertDefinitionAsync(expectedAlertDefinition.AlertDefinitionId);

            // Assert
            result.AlertDefinitionId.Should().Be(expectedAlertDefinition.AlertDefinitionId);
            result.AppUser.AppUserId.Should().Be(expectedAlertDefinition.AppUser.AppUserId);
            result.Stock.StockId.Should().Be(expectedAlertDefinition.Stock.StockId);

            AssertCriteria(expectedAlertDefinition.RootCriteria, result.RootCriteria);
        }

        [Fact]
        public async Task GetAlertDefinitionByStockIdAsync_UnknownStockId_EmptyArray()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var mapper = CreateMapper();
            var repository = CreateRepository(context, mapper);

            // Act
            var results = await repository.GetAlertDefinitionsByStockIdAsync(Guid.NewGuid());

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAlertDefinitionByStockIdAsync_KnownStockId_AlertDefinitionsReturned()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var mapper = CreateMapper();
            var repository = CreateRepository(context, mapper);
            var stock = await context.Stocks.FirstAsync();
            var expectedAlertDefinitionIds = (from ad in context.AlertDefinitions
                                              where ad.StockId == stock.StockId
                                              select ad.AlertDefinitionId).ToList();

            // Act
            var results = (await repository.GetAlertDefinitionsByStockIdAsync(stock.StockId)).ToList();

            // Assert
            results.Count.Should().Be(expectedAlertDefinitionIds.Count);
            foreach (var r in results)
            {
                expectedAlertDefinitionIds.Should().Contain(r.AlertDefinitionId);
            }
        }

        [Fact]
        public async Task SaveAsync_Insert_AlertDefinitionSaved()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var mapper = CreateMapper();
            var repository = CreateRepository(context, mapper);
            var alertDefinition = await CreateComplexAlertDefinitionAsync(context, mapper);

            // Act
            await repository.SaveAsync(alertDefinition);

            // Assert
            var result = await repository.GetAlertDefinitionAsync(alertDefinition.AlertDefinitionId);
            AssertAlertDefinition(alertDefinition, result);
        }

        [Fact]
        public async Task SaveAsync_Update_AlertDefinitionSaved()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var mapper = CreateMapper();
            var alertDefinition = await CreateAndSaveComplexAlertDefinitionAsync(context, mapper);
            alertDefinition.Name = "Updated Definition";
            alertDefinition.RootCriteria.ChildrenCriteria.Remove(alertDefinition.RootCriteria.ChildrenCriteria.Last());
            alertDefinition.RootCriteria.ChildrenCriteria.Add(
                new AlertCriteria
                {
                    Type = CriteriaType.Composite,
                    Operator = CriteriaOperator.And,
                    ChildrenCriteria = new List<AlertCriteria>
                    {
                        new AlertCriteria
                        {
                            Type = CriteriaType.DailyPercentageGainLoss,
                            Operator = CriteriaOperator.LessThanOrEqualTo,
                            Level = -0.02M
                        },
                        new AlertCriteria
                        {
                            Type = CriteriaType.DailyPercentageGainLoss,
                            Operator = CriteriaOperator.GreaterThanOrEqualTo,
                            Level = -0.06M
                        }
                    }
                }
            );

            // Create new context (with same DB name) to match service lifetime of context when running as a service
            context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context, mapper);

            // Act
            await repository.SaveAsync(alertDefinition);

            // Assert
            var result = await repository.GetAlertDefinitionAsync(alertDefinition.AlertDefinitionId);
            AssertAlertDefinition(alertDefinition, result);
        }

        private void AssertAlertDefinition(AlertDefinition expected, AlertDefinition actual)
        {

            actual.AlertDefinitionId.Should().Be(expected.AlertDefinitionId);
            actual.AppUser.AppUserId.Should().Be(expected.AppUser.AppUserId);
            actual.Stock.StockId.Should().Be(expected.Stock.StockId);
            
            AssertCriteria(expected.RootCriteria, actual.RootCriteria);
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

        private async Task<AlertDefinition> CreateComplexAlertDefinitionAsync(
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
                    Operator = CriteriaOperator.Or,
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

            return alertDefinition;
        }

        private async Task<AlertDefinition> CreateAndSaveComplexAlertDefinitionAsync(
            ApplicationDbContext context,
            IMapper mapper)
        {
            var alertDefinition = await CreateComplexAlertDefinitionAsync(context, mapper);

            var dataObject = mapper.Map<Data.Model.AlertDefinition>(alertDefinition);

            await context.AddAsync(dataObject);
            await context.SaveChangesAsync();

            return mapper.Map<AlertDefinition>(dataObject);
        }
    }
}
