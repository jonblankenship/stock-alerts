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
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using StockAlerts.Domain.Authentication;
using StockAlerts.Domain.Settings;
using StockAlerts.Functions.Attributes;

namespace StockAlerts.Functions
{
    public class StocksFunctions : FunctionBase
    {
        private readonly IStocksService _stocksService;
        private readonly IAuthService _authService;

        public StocksFunctions(
            IStocksService stocksService,
            IAuthService authService)
        {
            _stocksService = stocksService ?? throw new ArgumentNullException(nameof(stocksService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [FunctionName("GetStock")]
        [HandleExceptions]
        public async Task<IActionResult> GetStockAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stocks/{stockId}")] HttpRequest req,
            string stockId,
            ILogger log)
        {
            log.LogInformation("Executing GetStockAsync.");

            if (_authService.GetAuthenticatedPrincipal(req) == null)
                return new UnauthorizedResult();

            var stock = await _stocksService.GetStockAsync(new Guid(stockId));
            return new OkObjectResult(stock);
        }
        
        [FunctionName("FindStocks")]
        [HandleExceptions]
        public async Task<IActionResult> FindStocksAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stocks")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Executing FindStocksAsync.");

            if (_authService.GetAuthenticatedPrincipal(req) == null)
                return new UnauthorizedResult();

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
