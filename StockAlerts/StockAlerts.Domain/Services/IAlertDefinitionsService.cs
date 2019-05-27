using StockAlerts.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.Domain.QueueMessages;

namespace StockAlerts.Domain.Services
{
    public interface IAlertDefinitionsService
    {
        Task<IEnumerable<AlertDefinition>> GetAlertDefinitionsAsync(Guid appUserId);

        Task<AlertDefinition> GetAlertDefinitionAsync(Guid alertDefinitionId);

        Task EvaluateAlertAsync(AlertEvaluationMessage message);
    }
}
