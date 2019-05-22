using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.Domain.Model;

namespace StockAlerts.Domain.Repositories
{
    public interface IAlertDefinitionsRepository
    {
        Task<IEnumerable<AlertDefinition>> GetAlertDefinitionsAsync(Guid appUserId);

        Task<AlertDefinition> GetAlertDefinitionAsync(Guid alertDefinitionId);
    }
}
