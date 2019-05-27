namespace StockAlerts.Domain.Settings
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }

        public string AlertProcessingQueue { get; set; }

        public string EmailNotificationQueue { get; set; }

        public string SmsNotificationQueue { get; set; }
        
        public string PushNotificationQueue { get; set; }
    }
}
