using System.Threading.Tasks;

namespace StockAlerts.Domain.Services
{
    public interface IDataUpdateService
    {
        Task UpdateQuotesForSubscribedStocksAsync();
    }
}
