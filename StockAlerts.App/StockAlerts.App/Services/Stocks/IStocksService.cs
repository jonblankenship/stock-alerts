using StockAlerts.Resources.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.App.Services.Stocks
{
    public interface IStocksService
    {
        Task<IEnumerable<Stock>> FindStocksAsync(string searchString, CancellationToken cancellationToken);
    }
}
