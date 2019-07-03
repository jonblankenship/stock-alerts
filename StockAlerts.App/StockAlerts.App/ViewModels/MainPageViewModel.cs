using System;
using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.Services.Settings;
using StockAlerts.App.ViewModels.Base;

namespace StockAlerts.App.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;

        public MainPageViewModel(
            ISettingsService settingsService,
            INavigationService navigationService, 
            ILogger logger) : base(navigationService, logger)
        {
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }
    }
}
