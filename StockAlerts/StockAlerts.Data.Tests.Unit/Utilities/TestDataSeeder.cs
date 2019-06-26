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
                CreateStock("MSFT", "Microsoft"),
                CreateStock("MRK", "Merck"),
                CreateStock("AAPL", "Apple"),
                CreateStock("JNJ", "Johnson and Johnson"),
                CreateStock("KO", "Coca Cola")
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
                CreateAlertDefinition(appUsers.First(), stocks.First(), AlertDefinitionStatuses.Enabled),
                CreateAlertDefinition(appUsers.First(), stocks.Skip(1).First(), AlertDefinitionStatuses.Disabled),
                CreateAlertDefinition(appUsers.First(), stocks.Skip(2).First(), AlertDefinitionStatuses.Enabled),
                CreateAlertDefinition(appUsers.Skip(1).First(), stocks.First(), AlertDefinitionStatuses.Enabled),
            };
            await context.AlertDefinitions.AddRangeAsync(alertDefinitions);

            await context.SaveChangesAsync();
        }

        private static Stock CreateStock(string symbol, string name) => 
            new Stock
            {
                Symbol = symbol,
                Name = name,
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
            AlertDefinitionStatuses status) =>
            new AlertDefinition
            {
                AppUserId = user.AppUserId,
                StockId = stock.StockId,
                Status = status
            };

    }
}
