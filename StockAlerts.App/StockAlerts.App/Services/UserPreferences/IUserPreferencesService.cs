using System.Threading.Tasks;

namespace StockAlerts.App.Services.UserPreferences
{
    public interface IUserPreferencesService
    {
        Task<Resources.Model.UserPreferences> GetUserPreferencesAsync();

        Task<Resources.Model.UserPreferences> SaveUserPreferencesAsync(Resources.Model.UserPreferences userPreferences);
    }
}
