using Microsoft.Azure.ServiceBus;
using StockAlerts.Domain.Settings;
using System;

namespace StockAlerts.Domain.Factories
{
    public class QueueClientFactory : IQueueClientFactory
    {
        private readonly ServiceBusSettings _serviceBusSetting;

        public QueueClientFactory(ISettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            _serviceBusSetting = settings.ServiceBusSettings;
        }

        public IQueueClient CreateClient(string queueName) => new QueueClient(_serviceBusSetting.ConnectionString, queueName);
    }
}
