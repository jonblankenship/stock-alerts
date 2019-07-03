using StockAlerts.App.Services.Settings;
using StockAlerts.Resources.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prism.Navigation;
using StockAlerts.App.Constants;
using StockAlerts.App.Extensions;
using StockAlerts.App.Services.Base;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.Utility;
using StockAlerts.App.ViewModels.AlertDefinitions;

namespace StockAlerts.App.Services.AlertDefinitions
{
    public class AlertDefinitionsService : WebServiceClientBase, IAlertDefinitionsService
    {
        private readonly ILogger _logger;

        public AlertDefinitionsService(
            IHttpClientFactory httpClientFactory,
            ISettingsService settingsService,
            INavigationService navigationService,
            ILogger logger) : base(httpClientFactory, settingsService, navigationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ObservableCollection<AlertDefinitionItemViewModel>> GetAlertDefinitionItemViewModelsAsync()
        {
            // TODO: Get base URL from settings
            var alertDefinitions = await GetAsync<IEnumerable<AlertDefinition>>($"{MiscConstants.StockAlertsApiBaseUri}alert-definitions", CancellationToken.None);
            if (alertDefinitions == null)
            {
                return new ObservableCollection<AlertDefinitionItemViewModel>();
            }

            var alertDefinitionVms = (from ad in alertDefinitions
                                      select new AlertDefinitionItemViewModel(ad, NavigationService, _logger)).ToList();
            return alertDefinitionVms.ToObservableCollection();
        }
    }
}
