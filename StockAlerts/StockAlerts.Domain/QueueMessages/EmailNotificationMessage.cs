namespace StockAlerts.Domain.QueueMessages
{
    public class EmailNotificationMessage
    {
        public string EmailAddress { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
