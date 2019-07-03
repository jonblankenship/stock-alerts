using StockAlerts.App.Constants;
using StockAlerts.App.Services.RequestProvider;
using StockAlerts.App.Services.Settings;
using StockAlerts.Resources.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.App.Services.Stocks
{
    public class StocksService : IStocksService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public StocksService(
            IRequestProvider requestProvider,
            ISettingsService settingsService)
        {
            _requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public async Task<IEnumerable<Stock>> FindStocksAsync(string searchString, CancellationToken cancellationToken)
        {
            var url = $"{MiscConstants.StockAlertsApiBaseUri}stocks?startsWith={searchString}";

            var stocks = await _requestProvider.GetAsync<IEnumerable<Stock>>(url, _settingsService.AuthAccessToken, cancellationToken);

            return stocks;
        }
    }
}
