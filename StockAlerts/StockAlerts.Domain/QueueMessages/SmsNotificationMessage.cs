namespace StockAlerts.Domain.QueueMessages
{
    public class SmsNotificationMessage
    {
        public string PhoneNumber { get; set; }

        public string Message { get; set; }
    }
}
