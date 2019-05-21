using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;

namespace StockAlerts.Functions
{
    public static class PingFunction
    {
        [FunctionName("Ping")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult($"StockAlerts API Ping: {DateTime.UtcNow:F}");
        }
    }
}
