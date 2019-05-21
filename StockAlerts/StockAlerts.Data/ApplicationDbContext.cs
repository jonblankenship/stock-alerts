using Microsoft.EntityFrameworkCore;
using StockAlerts.Data.Model;

namespace StockAlerts.Data
{
    public class ApplicationDbContext : DbContext
    {
        //public ApplicationDbContext()
        //{ }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        { }

        public DbSet<AlertDefinition> AlertDefinitions { get; set; }
    }
}
