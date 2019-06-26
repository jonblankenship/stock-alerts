namespace StockAlerts.App.Services.Settings
{
    public interface ISettingsService
    {
        string AuthAccessToken { get; set; }

        string AuthRefreshToken { get; set; }
    }
}
