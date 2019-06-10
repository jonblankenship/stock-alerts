using System.Collections.Generic;
using StockAlerts.Domain.Model;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Services
{
    public interface IStockDataWebClient
    {
        Task<PriceQuote> GetRealTimePriceQuoteAsync(string symbol);

        Task<IList<StockInfo>> GetStockInfosAsync();
    }
}
