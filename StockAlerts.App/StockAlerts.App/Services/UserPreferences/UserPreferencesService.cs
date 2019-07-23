using System.Threading;
using System.Threading.Tasks;
using Prism.Navigation;
using StockAlerts.App.Constants;
using StockAlerts.App.Services.Base;
using StockAlerts.App.Services.Settings;
using StockAlerts.App.Utility;

namespace StockAlerts.App.Services.UserPreferences
{
    public class UserPreferencesService : WebServiceClientBase, IUserPreferencesService
    {
        public UserPreferencesService(
            IHttpClientFactory httpClientFactory,
            ISettingsService settingsService,
            INavigationService navigationService) : base(httpClientFactory, settingsService, navigationService)
        {
        }

        public async Task<Resources.Model.UserPreferences> GetUserPreferencesAsync()
        {
            var userPreferences = await GetAsync<Resources.Model.UserPreferences>($"{MiscConstants.StockAlertsApiBaseUri}user-preferences", CancellationToken.None);
            return userPreferences;
        }

        public async Task<Resources.Model.UserPreferences> SaveUserPreferencesAsync(Resources.Model.UserPreferences userPreferences)
        {
            return await PutAsync($"{MiscConstants.StockAlertsApiBaseUri}user-preferences", userPreferences);
        }
    }
}
