using System;
using System.Threading.Tasks;
using StockAlerts.Domain.Repositories;

namespace StockAlerts.Domain.Model
{
    public class Stock
    {
        private readonly IStocksRepository _stocksRepository;

        public Stock()
        { }

        public Stock(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository ?? throw new ArgumentNullException(nameof(stocksRepository));
        }

        public Guid StockId { get; set; }

        public string Symbol { get; set; }

        public decimal LastPrice { get; set; }

        public DateTimeOffset LastTime { get; set; }
        
        public void OnNewQuote(PriceQuote quote)
        {
            if (Symbol.ToUpper() != quote.Symbol.ToUpper())
                throw new ApplicationException($"Attempted to update stock ({Symbol}) with a quote for a different stock ({quote.Symbol}).");

            LastTime = quote.LastTime;
            LastPrice = quote.LastPrice;
        }

        public async Task SaveAsync()
        {
            if (_stocksRepository == null)
                throw new ApplicationException($"{nameof(Stock)} instantiated without an {nameof(IStocksRepository)}.");

            await _stocksRepository.SaveAsync(this);
        } 
    }
}
