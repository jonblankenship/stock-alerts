using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using StockAlerts.Domain.Services;
using StockAlerts.Functions.Attributes;

namespace StockAlerts.Functions
{
    public class GetQuotesFunction
    {
        private readonly IDataUpdateService _dataUpdateService;

        public GetQuotesFunction(IDataUpdateService dataUpdateService)
        {
            _dataUpdateService = dataUpdateService ?? throw new ArgumentNullException(nameof(dataUpdateService));
        }

        [FunctionName("GetQuotes")]
        [HandleExceptions]
        public async Task GetQuotesAsync(
            [TimerTrigger("0 */1 8-16 * * 1-5", RunOnStartup = true)]TimerInfo myTimer,
            ILogger log)
        {            
            await _dataUpdateService.UpdateQuotesForSubscribedStocksAsync();
        }
    }
}
