using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Services;

namespace StockAlerts.Api.Controllers
{
    [Route("api/stocks")]
    [Authorize]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStocksService _stocksService;
        private readonly IDataUpdateService _dataUpdateService;

        public StocksController(
            IStocksService stocksService,
            IDataUpdateService dataUpdateService)
        {
            _stocksService = stocksService ?? throw new ArgumentNullException(nameof(stocksService));
            _dataUpdateService = dataUpdateService ?? throw new ArgumentNullException(nameof(dataUpdateService));
        }

        [HttpGet]
        [Route("{stockId}")]
        public async Task<IActionResult> GetAsync(Guid stockId)
        {
            var stock = await _stocksService.GetStockAsync(stockId);
            return new OkObjectResult(stock);
        }

        [HttpGet]
        public async Task<IActionResult> FindAsync(string startsWith, CancellationToken cancellationToken)
        {
            if (startsWith.Any())
            {
                var stocks = await _stocksService.FindStocksAsync(startsWith, cancellationToken);
                return new OkObjectResult(stocks);
            }

            return new OkObjectResult(new List<Stock>());
        }

        [HttpPost]
        [Route("update-infos")]
        public async Task<IActionResult> UpdateStockInfosAsync()
        {
            await _dataUpdateService.UpdateAllStockInfosAsync();

            return new NoContentResult();
        }
    }
}
