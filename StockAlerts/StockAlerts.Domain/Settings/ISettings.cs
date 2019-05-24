namespace StockAlerts.Domain.Settings
{
    public interface ISettings
    {
        AppSettings AppSettings { get; }

        ServiceBusSettings ServiceBusSettings { get; }
    }
}
