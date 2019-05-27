using Microsoft.Extensions.Configuration;

namespace StockAlerts.Domain.Settings
{
    public class Settings : ISettings
    {
        // Section names
        private const string AppSettingsSection = "AppSettings";
        private const string ServiceBusSettingsSection = "ServiceBusSettings";

        public void Initialize(IConfiguration configuration, bool isDevelopment)
        {
            IsDevelopment = isDevelopment;

            AppSettings = configuration.GetSection(AppSettingsSection).Get<AppSettings>();

            ServiceBusSettings = configuration.GetSection(ServiceBusSettingsSection).Get<ServiceBusSettings>();

            ServiceBusSettings.ConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");
        }

        public bool IsDevelopment { get; private set; }

        public ServiceBusSettings ServiceBusSettings { get; private set; }

        public AppSettings AppSettings { get; private set; }
    }
}
