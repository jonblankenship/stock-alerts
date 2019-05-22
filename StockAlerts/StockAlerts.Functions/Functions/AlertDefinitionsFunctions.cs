using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Services;
using StockAlerts.Functions.Attributes;

namespace StockAlerts.Functions
{
    public class AlertDefinitionsFunctions : FunctionBase
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;

        public AlertDefinitionsFunctions(IAlertDefinitionsService alertDefinitionsService)
        {
            _alertDefinitionsService = alertDefinitionsService ?? throw new ArgumentNullException(nameof(alertDefinitionsService));
        }

        [FunctionName("GetAlertDefinitions")]
        [HandleExceptions]
        public async Task<IActionResult> GetAlertDefinitionsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "alert-definitions")] HttpRequest req,
            ILogger log)
        {
            // TODO: Get userId from req.HttpContext.User claims
            var userId = MiscConstants.AppUserId;
            var alertDefinitions = await _alertDefinitionsService.GetAlertDefinitionsAsync(userId);
            return new OkObjectResult(alertDefinitions);
        }

        [FunctionName("GetAlertDefinition")]
        [HandleExceptions]
        public async Task<IActionResult> GetAlertDefinitionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "alert-definitions/{alertDefinitionId}")] HttpRequest req,
            string alertDefinitionId,
            ILogger log)
        {
            var alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(new Guid(alertDefinitionId));
            return new OkObjectResult(alertDefinition);
        }
    }
}
