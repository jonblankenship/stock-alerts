using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using StockAlerts.Functions.Attributes;

namespace StockAlerts.Functions
{
    public class PingFunction : FunctionBase
    {
        public PingFunction()
        { }

        [FunctionName("Ping")]
        [HandleExceptions]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult($"StockAlerts API Ping: {DateTime.UtcNow:F}");
        }
    }
}
