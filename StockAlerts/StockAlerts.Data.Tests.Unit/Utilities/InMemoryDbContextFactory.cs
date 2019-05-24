using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StockAlerts.Data.Tests.Unit.Utilities
{
    internal static class InMemoryDbContextFactory
    {
        public static async Task<ApplicationDbContext> GetContextAsync([CallerMemberName] string caller = "")
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(caller)
                .Options;

            var context = new ApplicationDbContext(options);

            await TestDataSeeder.SeedTestDataAsync(context);

            return context;
        }
    }
}
