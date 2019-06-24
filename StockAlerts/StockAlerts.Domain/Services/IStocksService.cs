using StockAlerts.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Services
{
    public interface IStocksService
    {
        Task<Stock> GetStockAsync(Guid stockId);

        Task<IEnumerable<Stock>> FindStocksAsync(
            string symbolStartsWith,
            CancellationToken cancellationToken);
    }
}
