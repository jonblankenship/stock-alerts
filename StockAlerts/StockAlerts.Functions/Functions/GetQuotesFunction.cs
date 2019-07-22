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
        private readonly ISettings _settings;

        public GetQuotesFunction(
            IDataUpdateService dataUpdateService,
            ISettings settings)
        {
            _dataUpdateService = dataUpdateService ?? throw new ArgumentNullException(nameof(dataUpdateService));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        [FunctionName("GetQuotes")]
        [HandleExceptions]
        public async Task GetQuotesAsync(
            [TimerTrigger("0 */1 8-16 * * 1-5", RunOnStartup = true)]TimerInfo myTimer,
            ILogger log)
        {
            if (DateTimeOffset.UtcNow >= _settings.AppSettings.MarketOpenUtc &&
                DateTimeOffset.UtcNow <= _settings.AppSettings.MarketCloseUtc)
            {
                log.Log(LogLevel.Information, "Executing GetQuotes: Performing update quotes for subscribed stocks.");
                await _dataUpdateService.UpdateQuotesForSubscribedStocksAsync();
            }
            else
            {
                log.Log(LogLevel.Information, "Executing GetQuotes: Outside of market hours - no update.");
            }
        }
    }
}
