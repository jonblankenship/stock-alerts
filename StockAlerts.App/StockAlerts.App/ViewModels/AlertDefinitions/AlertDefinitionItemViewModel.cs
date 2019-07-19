using System;
using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.Resources.Model;

namespace StockAlerts.App.ViewModels.AlertDefinitions
{
    public class AlertDefinitionItemViewModel : ViewModelBase
    {
        private readonly AlertDefinition _alertDefinition;

        public AlertDefinitionItemViewModel(
            AlertDefinition alertDefinition,
            INavigationService navigationService,
            ILogger logger) : base(navigationService, logger)
        {
            _alertDefinition = alertDefinition ?? throw new ArgumentNullException(nameof(alertDefinition));
        }

        public string Symbol => _alertDefinition.Stock.Symbol;

        public string Name => _alertDefinition.Name;

        public string Description => "Price > 225 OR Price < 175"; // TODO: Populate and get from web service

        public decimal LastPrice => _alertDefinition.Stock.LastPrice;

        public AlertDefinition AlertDefinition => _alertDefinition;
    }
}
