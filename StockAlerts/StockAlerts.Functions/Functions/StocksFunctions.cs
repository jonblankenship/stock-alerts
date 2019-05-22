using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAlerts.Functions
{
    public class StocksFunctions
    {
        private readonly IStocksService _stocksService;

        public StocksFunctions(IStocksService stocksService)
        {
            _stocksService = stocksService ?? throw new ArgumentNullException(nameof(stocksService));
        }

        [FunctionName("GetStock")]
        public async Task<IActionResult> GetStockAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stocks/{stockId}")] HttpRequest req,
            string stockId,
            ILogger log)
        {
            var stock = await _stocksService.GetStockAsync(new Guid(stockId));
            return new OkObjectResult(stock);
        }

        [FunctionName("FindStocks")]
        public async Task<IActionResult> FindStocksAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stocks")] HttpRequest req,
            ILogger log)
        {
            var startsWith = req.Query["startsWith"];            

            if (startsWith.Any())
            {
                var stocks = await _stocksService.FindStocksAsync(startsWith);
                return new OkObjectResult(stocks);
            }

            return new OkObjectResult(new List<Stock>());
        }
    }
}
