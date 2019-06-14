using StockAlerts.Forms.Services.Settings;
using StockAlerts.Forms.ViewModels.Base;

namespace StockAlerts.Forms.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public bool UserIsLogged => !string.IsNullOrEmpty(_settingsService.AuthAccessToken);
    }
}
