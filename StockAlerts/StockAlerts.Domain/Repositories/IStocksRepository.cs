using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.Domain.Model;

namespace StockAlerts.Domain.Repositories
{
    public interface IStocksRepository
    {
        /// <summary>
        /// Gets a <see cref="Stock"/> by <see cref="stockId"/>
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        Task<Stock> GetStockAsync(Guid stockId);

        /// <summary>
        /// Finds all <see cref="Stock"/>s whose symbol starts with <see cref="symbolStartsWith"/>
        /// </summary>
        /// <param name="symbolStartsWith"></param>
        /// <returns></returns>
        Task<IEnumerable<Stock>> FindStocksAsync(string symbolStartsWith);

        /// <summary>
        /// Persists <see cref="stock"/> to the database
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        Task SaveAsync(Stock stock);

        /// <summary>
        /// Gets the distinct list of <see cref="Stock"/> that are the subject of an active <see cref="AlertDefinition"/>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Stock>> GetSubscribedStocksAsync();
    }
}