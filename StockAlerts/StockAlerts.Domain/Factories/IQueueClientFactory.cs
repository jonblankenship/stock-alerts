using Microsoft.Azure.ServiceBus;

namespace StockAlerts.Domain.Factories
{
    public interface IQueueClientFactory
    {
        IQueueClient CreateClient(string queueName);
    }
}
