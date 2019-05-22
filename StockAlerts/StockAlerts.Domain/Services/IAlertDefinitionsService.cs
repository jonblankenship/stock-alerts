using StockAlerts.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Services
{
    public interface IAlertDefinitionsService
    {
        Task<IEnumerable<AlertDefinition>> GetAlertDefinitionsAsync(Guid appUserId);

        Task<AlertDefinition> GetAlertDefinitionAsync(Guid alertDefinitionId);
    }
}
