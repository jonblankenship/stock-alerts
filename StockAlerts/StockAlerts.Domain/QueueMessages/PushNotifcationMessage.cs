using System;

namespace StockAlerts.Domain.QueueMessages
{
    public class PushNotifcationMessage
    {
        public Guid AppUserId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}
