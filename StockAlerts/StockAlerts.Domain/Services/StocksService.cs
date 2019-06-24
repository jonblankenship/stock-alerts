using StockAlerts.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StockAlerts.Domain.Repositories;

namespace StockAlerts.Domain.Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;

        public StocksService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository ?? throw new ArgumentNullException(nameof(stocksRepository));
        }

        public async Task<Stock> GetStockAsync(Guid stockId)
        {
            var stock = await _stocksRepository.GetStockAsync(stockId);
            return stock;
        }

        public async Task<IEnumerable<Stock>> FindStocksAsync(
            string symbolStartsWith,
            CancellationToken cancellationToken)
        {
            var stocks = await _stocksRepository.FindStocksAsync(symbolStartsWith, cancellationToken);
            return stocks;
        }
    }
}
