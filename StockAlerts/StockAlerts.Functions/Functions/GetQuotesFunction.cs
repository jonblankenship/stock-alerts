using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using StockAlerts.Domain.Services;
using StockAlerts.Domain.Settings;
using StockAlerts.Functions.Attributes;

namespace StockAlerts.Functions
{
    public class GetQuotesFunction
    {
        private readonly IDataUpdateService _dataUpdateService;
        private readonly IAppSettings _appSettings;

        public GetQuotesFunction(
            IDataUpdateService dataUpdateService,
            IAppSettings appSettings)
        {
            _dataUpdateService = dataUpdateService ?? throw new ArgumentNullException(nameof(dataUpdateService));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        [FunctionName("GetQuotes")]
        [HandleExceptions]
        public async Task GetQuotesAsync(
            [TimerTrigger("0 */1 8-16 * * 1-5", RunOnStartup = true)]TimerInfo myTimer,
            ILogger log)
        {            
            if (DateTimeOffset.UtcNow >= _appSettings.MarketOpenUtc &&
                DateTimeOffset.UtcNow <= _appSettings.MarketCloseUtc)
                await _dataUpdateService.UpdateQuotesForSubscribedStocksAsync();
        }
    }
}
