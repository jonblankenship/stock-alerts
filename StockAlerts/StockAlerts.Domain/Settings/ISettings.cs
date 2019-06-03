using StockAlerts.Domain.Authentication;

namespace StockAlerts.Domain.Settings
{
    public interface ISettings
    {
        AppSettings AppSettings { get; }

        ServiceBusSettings ServiceBusSettings { get; }

        JwtIssuerOptions JwtIssuerOptions { get; }

        JwtOptions JwtOptions { get; }
        
        AuthSettings AuthSettings { get; set; }
    }
}
