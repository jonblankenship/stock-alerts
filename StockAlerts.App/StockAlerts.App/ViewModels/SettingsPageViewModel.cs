using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.Services.Settings;
using StockAlerts.App.ViewModels.Base;

namespace StockAlerts.App.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsPageViewModel(
            ISettingsService settingsService,
            INavigationService navigationService,
            ILogger logger) : base(navigationService, logger)
        {
            _settingsService = settingsService;
        }

        public bool UserIsLogged => !string.IsNullOrEmpty(_settingsService.AuthAccessToken);
    }
}
