using StockAlerts.App.Services.RequestProvider;
using StockAlerts.App.Services.Settings;
using StockAlerts.Resources.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using StockAlerts.App.Extensions;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels;
using StockAlerts.App.ViewModels.AlertDefinitions;

namespace StockAlerts.App.Services.AlertDefinitions
{
    public class AlertDefinitionsService : IAlertDefinitionsService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigationService;
        private readonly ILogger _logger;

        public AlertDefinitionsService(
            IRequestProvider requestProvider,
            ISettingsService settingsService,
            INavigationService navigationService,
            ILogger logger)
        {
            _requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ObservableCollection<AlertDefinitionItemViewModel>> GetAlertDefinitionItemViewModelsAsync()
        {
            var alertDefinitions = (from ad in await _requestProvider.GetAsync<IEnumerable<AlertDefinition>>(
                                        "https://stockalerts.azurewebsites.net/api/alert-definitions", _settingsService.AuthAccessToken)
                                    select new AlertDefinitionItemViewModel(ad, _navigationService, _logger)).ToList();
            return alertDefinitions.ToObservableCollection();
        }
    }
}
