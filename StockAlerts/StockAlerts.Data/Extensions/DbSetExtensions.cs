using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using StockAlerts.Data.Model;
using System.Collections.Generic;

namespace StockAlerts.Data.Extensions
{
    public static class DbSetExtensions
    {
        public static IIncludableQueryable<AlertDefinition, Stock> IncludeAllRelatedEntities(
            this DbSet<AlertDefinition> alertDefinitions) =>
            alertDefinitions
                .IncludeAllAlertCriteria()
                .Include(x => x.AppUser)
                .ThenInclude(x => x.UserPreferences)
                .Include(x => x.Stock);

        public static IIncludableQueryable<AlertDefinition, ICollection<AlertCriteria>> IncludeAllAlertCriteria(
            this DbSet<AlertDefinition> alertDefinitions) => 
            alertDefinitions
                .Include(x => x.RootCriteria)
                .ThenInclude(x => x.ChildrenCriteria)
                .ThenInclude(x => x.ChildrenCriteria)
                .ThenInclude(x => x.ChildrenCriteria)
                .ThenInclude(x => x.ChildrenCriteria);

    }
}
