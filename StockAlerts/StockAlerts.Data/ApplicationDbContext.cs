using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StockAlerts.Data.Model;

namespace StockAlerts.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<AlertDefinition> AlertDefinitions { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<ApiCall> ApiCalls { get; set; }

        public static void ConfigureStartupOptions(
            bool isDevelopment,
            IConfigurationRoot configuration,
            DbContextOptionsBuilder optionsBuilder
        )
        {
            var connectionString = isDevelopment
                ? configuration.GetConnectionString("LocalStockAlertsDatabase")
                : configuration.GetConnectionString("StockAlertsDatabase");
            optionsBuilder
                //.UseLazyLoadingProxies() // Needed to support loading of recursive tree of AlertCriteria
                .UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AlertCriteria>()
                .HasMany(ac => ac.ChildrenCriteria)
                .WithOne(ac => ac.ParentCriteria)
                .HasForeignKey(ac => ac.ParentCriteriaId);

            modelBuilder.Entity<AlertDefinition>()
                .HasOne(ad => ad.RootCriteria)
                .WithOne(ac => ac.AlertDefinition)
                .HasForeignKey<AlertCriteria>(ac => ac.AlertDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddAuditInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((Entity)entry.Entity).Created = DateTime.UtcNow;
                }
                ((Entity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }
    }
}
