using StockAlerts.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAlerts.Domain.Enums;

namespace StockAlerts.Data.Tests.Unit.Utilities
{
    internal static class TestDataSeeder
    {
        public static async Task SeedTestDataAsync(ApplicationDbContext context)
        {
            var stocks = new List<Stock>
            {
                CreateStock("MSFT"),
                CreateStock("MRK"),
                CreateStock("AAPL"),
                CreateStock("JNJ"),
                CreateStock("KO")
            };
            await context.Stocks.AddRangeAsync(stocks);

            var appUsers = new List<AppUser>
            {
                CreateAppUser(),
                CreateAppUser()
            };
            await context.AppUsers.AddRangeAsync(appUsers);

            var alertDefinitions = new List<AlertDefinition>
            {
                CreateAlertDefinition(appUsers.First(), stocks.First(), AlertDefinitionType.PriceAlert, AlertDefinitionStatuses.Enabled, ComparisonOperator.GreaterThan, 120),
                CreateAlertDefinition(appUsers.First(), stocks.Skip(1).First(), AlertDefinitionType.PriceAlert, AlertDefinitionStatuses.Disabled, ComparisonOperator.LessThan, 90),
                CreateAlertDefinition(appUsers.First(), stocks.Skip(2).First(), AlertDefinitionType.PriceAlert, AlertDefinitionStatuses.Enabled, ComparisonOperator.LessThan, 50),
                CreateAlertDefinition(appUsers.Skip(1).First(), stocks.First(), AlertDefinitionType.PriceAlert, AlertDefinitionStatuses.Enabled, ComparisonOperator.GreaterThan, 92.50M),
            };
            await context.AlertDefinitions.AddRangeAsync(alertDefinitions);

            await context.SaveChangesAsync();
        }

        private static Stock CreateStock(string symbol) => 
            new Stock
            {
                Symbol = symbol,
                LastPrice = 0,
                Created = DateTimeOffset.UtcNow,
                Modified = DateTimeOffset.UtcNow
            };

        private static AppUser CreateAppUser() =>
            new AppUser
            { 
                Created = DateTimeOffset.UtcNow,
                Modified = DateTimeOffset.UtcNow
            };

        private static AlertDefinition CreateAlertDefinition(
            AppUser user,
            Stock stock,
            AlertDefinitionType type,
            AlertDefinitionStatuses status,
            ComparisonOperator comparisonOperator,
            decimal priceLevel) =>
            new AlertDefinition
            {
                AppUserId = user.AppUserId,
                StockId = stock.StockId,
                Type = type,
                Status = status,
                ComparisonOperator = comparisonOperator,
                PriceLevel = priceLevel
            };

    }
}
