using System.Threading.Tasks;

namespace StockAlerts.App.Services.Settings
{
    public interface ISettingsService
    {
        string AuthAccessToken { get; set; }

        string AuthRefreshToken { get; set; }

        bool GetValueOrDefault(string key, bool defaultValue);
        string GetValueOrDefault(string key, string defaultValue);
        Task AddOrUpdateValue(string key, bool value);
        Task AddOrUpdateValue(string key, string value);
    }
}
