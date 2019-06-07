using Microsoft.Extensions.Configuration;
using StockAlerts.Domain.Authentication;

namespace StockAlerts.Domain.Settings
{
    public class Settings : ISettings
    {
        // Section names
        private const string AppSettingsSection = "AppSettings";
        private const string ServiceBusSettingsSection = "ServiceBusSettings";
        private const string JwtIssuerOptionsSection = "JwtIssuerOptions";
        private const string JwtOptionsSection = "Jwt";
        private const string AuthSettingsSection = "AuthSettings";
        private const string EmailSenderOptionsSection = "EmailSenderOptions";

        public void Initialize(IConfiguration configuration, bool isDevelopment)
        {
            IsDevelopment = isDevelopment;

            AppSettings = configuration.GetSection(AppSettingsSection).Get<AppSettings>();

            ServiceBusSettings = configuration.GetSection(ServiceBusSettingsSection).Get<ServiceBusSettings>();

            ServiceBusSettings.ConnectionString = configuration.GetValue<string>("ServiceBusConnectionString");

            JwtIssuerOptions = configuration.GetSection(JwtIssuerOptionsSection).Get<JwtIssuerOptions>();

            JwtOptions = configuration.GetSection(JwtOptionsSection).Get<JwtOptions>();

            AuthSettings = configuration.GetSection(AuthSettingsSection).Get<AuthSettings>();

            EmailSenderOptions = configuration.GetSection($"{EmailSenderOptionsSection}").Get<EmailSenderOptions>();
        }

        public bool IsDevelopment { get; private set; }

        public ServiceBusSettings ServiceBusSettings { get; private set; }

        public AppSettings AppSettings { get; private set; }

        public JwtIssuerOptions JwtIssuerOptions { get; private set; }

        public JwtOptions JwtOptions { get; private set; }

        public AuthSettings AuthSettings { get; set; }

        public EmailSenderOptions EmailSenderOptions { get; set; }

        public string WebAppBaseUrl => IsDevelopment ? "http://localhost:7071" : "https://stockalerts.azurewebsites.new";
    }
}
