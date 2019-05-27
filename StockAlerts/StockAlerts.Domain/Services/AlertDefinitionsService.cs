using StockAlerts.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Repositories;

namespace StockAlerts.Domain.Services
{
    public class AlertDefinitionsService : IAlertDefinitionsService
    {
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;

        public AlertDefinitionsService(IAlertDefinitionsRepository alertDefinitionsRepository)
        {
            _alertDefinitionsRepository = alertDefinitionsRepository ?? throw new ArgumentNullException(nameof(alertDefinitionsRepository));
        }

        public async Task<IEnumerable<AlertDefinition>> GetAlertDefinitionsAsync(Guid appUserId)
        {
            var alertDefinitions = await _alertDefinitionsRepository.GetAlertDefinitionsAsync(appUserId);
            return alertDefinitions;
        }

        public async Task<AlertDefinition> GetAlertDefinitionAsync(Guid alertDefinitionId)
        {
            var alertDefinition = await _alertDefinitionsRepository.GetAlertDefinitionAsync(alertDefinitionId);
            return alertDefinition;
        }

        public async Task EvaluateAlertAsync(AlertEvaluationMessage message)
        {
            var alertDefinition = await _alertDefinitionsRepository.GetAlertDefinitionAsync(message.AlertDefinitionId);

            await alertDefinition.EvaluateAsync(message);
        }
    }
}
