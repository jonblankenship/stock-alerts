using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.Domain.Model;

namespace StockAlerts.Domain.Repositories
{
    public interface IStocksRepository
    {
        Task<Stock> GetStockAsync(Guid stockId);

        Task<IEnumerable<Stock>> FindStocksAsync(string symbolStartsWith);
    }
}
