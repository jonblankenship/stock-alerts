using System.Threading.Tasks;

namespace StockAlerts.Forms.Services.Settings
{
    public interface ISettingsService
    {
        string AuthAccessToken { get; set; }

        bool GetValueOrDefault(string key, bool defaultValue);
        string GetValueOrDefault(string key, string defaultValue);
        Task AddOrUpdateValue(string key, bool value);
        Task AddOrUpdateValue(string key, string value);
    }
}
