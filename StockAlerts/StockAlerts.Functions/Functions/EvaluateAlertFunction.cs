using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Services;

namespace StockAlerts.Functions
{
    public class EvaluateAlertFunction
    {
        private readonly IAlertDefinitionsService _alertDetDefinitionsService;

        public EvaluateAlertFunction(IAlertDefinitionsService alertDetDefinitionsService)
        {
            _alertDetDefinitionsService = alertDetDefinitionsService;
        }

        [FunctionName("EvaluateAlertFunction")]
        public async Task RunAsync([ServiceBusTrigger("alertprocessingqueue", Connection = "ServiceBusConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var message = JsonConvert.DeserializeObject<AlertEvaluationMessage>(myQueueItem);

            await _alertDetDefinitionsService.EvaluateAlertAsync(message);
        }
    }
}
