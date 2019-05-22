using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace StockAlerts.Functions
{
    public class PingFunction : FunctionBase
    {
        public PingFunction()
        { }

        [FunctionName("Ping")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ping")] HttpRequest req,
            ILogger log)
        {
            try
            {
                return new OkObjectResult($"StockAlerts API Ping: {DateTime.UtcNow:F}");
            }
            catch (Exception e)
            {
                return HandleException(e, req.HttpContext);
            }
        }
    }
}
