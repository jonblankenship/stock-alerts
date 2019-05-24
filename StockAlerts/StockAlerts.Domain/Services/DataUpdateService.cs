using StockAlerts.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Services
{
    public class DataUpdateService : IDataUpdateService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IStockDataWebClient _stockDataWebClient;

        public DataUpdateService(
            IStocksRepository stocksRepository,
            IStockDataWebClient stockDataWebClient)
        {
            _stocksRepository = stocksRepository ?? throw new ArgumentNullException(nameof(stocksRepository));
            _stockDataWebClient = stockDataWebClient ?? throw new ArgumentNullException(nameof(stockDataWebClient));
        }

        public async Task UpdateQuotesForSubscribedStocksAsync()
        {
            var stocks = await _stocksRepository.GetSubscribedStocksAsync();

            foreach (var stock in stocks)
            {
                // TODO: Implement throttling and tracking of API calls to prevent overages

                var quote = await _stockDataWebClient.GetRealTimePriceQuoteAsync(stock.Symbol);

                stock.OnNewQuote(quote);
                await stock.SaveAsync();
            }
        }
    }
}
