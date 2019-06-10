using StockAlerts.Domain.Repositories;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using StockAlerts.Domain.Factories;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Settings;

namespace StockAlerts.Domain.Services
{
    public class DataUpdateService : IDataUpdateService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;
        private readonly IStockDataWebClient _stockDataWebClient;
        private readonly IQueueClient _alertProcessingQueueClient;

        public DataUpdateService(
            IStocksRepository stocksRepository,
            IAlertDefinitionsRepository alertDefinitionsRepository,
            IStockDataWebClient stockDataWebClient,
            IQueueClientFactory queueClientFactory,
            ISettings settings)
        {
            _stocksRepository = stocksRepository ?? throw new ArgumentNullException(nameof(stocksRepository));
            _alertDefinitionsRepository = alertDefinitionsRepository ?? throw new ArgumentNullException(nameof(alertDefinitionsRepository));
            _stockDataWebClient = stockDataWebClient ?? throw new ArgumentNullException(nameof(stockDataWebClient));
            
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (queueClientFactory == null) throw new ArgumentNullException(nameof(queueClientFactory));

            _alertProcessingQueueClient = queueClientFactory.CreateClient(settings.ServiceBusSettings.AlertProcessingQueue);
        }

        public async Task UpdateQuotesForSubscribedStocksAsync()
        {
            var stocks = await _stocksRepository.GetSubscribedStocksAsync();

            foreach (var stock in stocks)
            {
                // TODO: Implement throttling and tracking of API calls to prevent overages

                var quote = await _stockDataWebClient.GetRealTimePriceQuoteAsync(stock.Symbol);

                if (quote != null)
                {
                    stock.OnNewQuote(quote);
                    await stock.SaveAsync();

                    // Enqueue an alert processing message if the last price changed from the previous quote
                    if (stock.LastPrice != stock.PreviousLastPrice)
                    {
                        await EnqueueAlertEvaluationMessages(stock);
                    }
                }
            }
        }

        public async Task UpdateAllStockInfosAsync()
        {
            var stockInfos = await _stockDataWebClient.GetStockInfosAsync();
            foreach (var s in stockInfos)
            {
                var stock = await _stocksRepository.GetStockAsync(s.Ticker);
                if (stock == null)
                {
                    stock = new Stock(_stocksRepository);
                }

                stock.Symbol = s.Ticker;
                stock.Name = s.SecurityName;

                await stock.SaveAsync();
            }
        }

        private async Task EnqueueAlertEvaluationMessages(Stock stock)
        {
            var alertDefinitions = await _alertDefinitionsRepository.GetAlertDefinitionsByStockIdAsync(stock.StockId);
            foreach (var alertDefinition in alertDefinitions)
            {
                // Create and enqueue SB message
                var alertProcessingMessage = new AlertEvaluationMessage
                {
                    AlertDefinitionId = alertDefinition.AlertDefinitionId,
                    LastPrice = stock.LastPrice,
                    PreviousLastPrice = stock.PreviousLastPrice
                };
                var messageBodyJson = JsonConvert.SerializeObject(alertProcessingMessage);

                var message = new Message(Encoding.UTF8.GetBytes(messageBodyJson));

                await _alertProcessingQueueClient.SendAsync(message);
            }
        }
    }
}
