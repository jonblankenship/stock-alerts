using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.Domain.Model;

namespace StockAlerts.Domain.Repositories
{
    public interface IAlertDefinitionsRepository
    {
        /// <summary>
        /// Retrieves the list of <see cref="AlertDefinition"/>s for the given <see cref="appUserId"/>
        /// </summary>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        Task<IEnumerable<AlertDefinition>> GetAlertDefinitionsAsync(Guid appUserId);

        /// <summary>
        /// Retrieves the <see cref="AlertDefinition"/> for the given <see cref="alertDefinitionId"/>
        /// </summary>
        /// <param name="alertDefinitionId"></param>
        /// <returns></returns>
        Task<AlertDefinition> GetAlertDefinitionAsync(Guid alertDefinitionId);

        /// <summary>
        /// Retrieves the list of <see cref="AlertDefinition"/>s for the given <see cref="stockId"/>
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        Task<IEnumerable<AlertDefinition>> GetAlertDefinitionsByStockIdAsync(Guid stockId);

        /// <summary>
        /// Persists <see cref="alertDefinition"/> to the database
        /// </summary>
        /// <param name="alertDefinition"></param>
        /// <returns></returns>
        Task SaveAsync(AlertDefinition alertDefinition);
    }
}
