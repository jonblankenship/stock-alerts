using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Data.Repositories;
using StockAlerts.Data.Tests.Unit.Utilities;
using StockAlerts.Domain.Enums;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Repositories;
using Xunit;

namespace StockAlerts.Data.Tests.Unit.Repositories
{
    public class StocksRepositoryTests
    {
        [Fact]
        public async Task GetStockAsync_StockIdDoesNotExist_NotFoundException()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await repository.GetStockAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task GetStockAsync_StockIdExists_StockReturned()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context);
            var existingStock = await context.Stocks.FirstAsync();

            // Act
            var stock = await repository.GetStockAsync(existingStock.StockId);

            // Assert
            stock.StockId.Should().Be(existingStock.StockId);
            stock.Symbol.Should().Be(existingStock.Symbol);
            stock.LastPrice.Should().Be(existingStock.LastPrice);
        }

        [Fact]
        public async Task FindStocksAsync_NoMatch_EmptyListReturned()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context);

            // Act
            var stocks = await repository.FindStocksAsync("XYZ", CancellationToken.None);

            // Assert
            stocks.Should().BeEmpty();
        }

        [Fact]
        public async Task FindStocksAsync_Match_StocksReturned()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context);

            // Act
            var stocks = await repository.FindStocksAsync("M", CancellationToken.None);

            // Assert
            stocks.Count().Should().Be(2);
            stocks.Any(s => s.Symbol == "MSFT").Should().BeTrue();
            stocks.Any(s => s.Symbol == "MRK").Should().BeTrue();
        }

        [Fact]
        public async Task GetSubscribedStocksAsync_StocksWithEnabledAlertDefinitionsReturned()
        {
            // Arrange
            var context = await InMemoryDbContextFactory.CreateDatabaseContextAsync();
            var repository = CreateRepository(context);
            var expectedSymbols = await (from a in context.AlertDefinitions
                                         where a.Status == AlertDefinitionStatuses.Enabled
                                         select a.Stock.Symbol).Distinct().ToListAsync();

            // Act
            var stocks = (await repository.GetSubscribedStocksAsync()).ToList();

            // Assert
            stocks.Count.Should().Be(expectedSymbols.Count);
            foreach (var symbol in expectedSymbols)
            {
                stocks.Count(s => s.Symbol == symbol).Should().Be(1);
            }
        }

        private IStocksRepository CreateRepository(ApplicationDbContext context)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DataModelMappingProfile>();
            });

            var mapper = config.CreateMapper();
            return new StocksRepository(context, mapper);
        }
    }
}
