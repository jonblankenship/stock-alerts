using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StockAlerts.Data.Tests.Unit.Utilities
{
    internal static class InMemoryDbContextFactory
    {
        public static async Task<ApplicationDbContext> CreateDatabaseContextAsync([CallerMemberName] string databaseName = "")
        {
            var context = CreateDatabaseContext<ApplicationDbContext>(databaseName);
            await TestDataSeeder.SeedTestDataAsync(context);
            return context;
        }

        public static T CreateDatabaseContext<T>([CallerMemberName] string databaseName = null)
            where T : DbContext
        {
            if (databaseName == null)
                throw new ArgumentNullException(nameof(databaseName));

            var options = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return (T)Activator.CreateInstance(typeof(T), options);
        }
    }
}
