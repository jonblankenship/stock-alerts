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
    public class StocksFunctions : FunctionBase
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
            try
            {
                var stock = await _stocksService.GetStockAsync(new Guid(stockId));
                return new OkObjectResult(stock);
            }
            catch (Exception e)
            {
                return HandleException(e, req.HttpContext);
            }
        }

        [FunctionName("FindStocks")]
        public async Task<IActionResult> FindStocksAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stocks")] HttpRequest req,
            ILogger log)
        {
            try
            { 
                var startsWith = req.Query["startsWith"];            

                if (startsWith.Any())
                {
                    var stocks = await _stocksService.FindStocksAsync(startsWith);
                    return new OkObjectResult(stocks);
                }

                return new OkObjectResult(new List<Stock>());
            }
            catch (Exception e)
            {
                return HandleException(e, req.HttpContext);
            }
        }
    }
}
