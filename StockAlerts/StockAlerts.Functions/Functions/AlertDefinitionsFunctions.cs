using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockAlerts.Data;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Services;
using StockAlerts.Functions.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StockAlerts.Functions
{
    public class AlertDefinitionsFunctions : FunctionBase
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;
        private readonly IMapper _mapper;

        public AlertDefinitionsFunctions(
            IAlertDefinitionsService alertDefinitionsService,
            IMapper mapper)
        {
            _alertDefinitionsService = alertDefinitionsService ?? throw new ArgumentNullException(nameof(alertDefinitionsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [FunctionName("GetAlertDefinitions")]
        [HandleExceptions]
        public async Task<IActionResult> GetAlertDefinitionsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "alert-definitions")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Executing GetAlertDefinitionsAsync.");

            // TODO: Get userId from req.HttpContext.User claims
            var userId = MiscConstants.AppUserId;
            var alertDefinitions = await _alertDefinitionsService.GetAlertDefinitionsAsync(userId);
            var resources = _mapper.Map<IList<Resources.Model.AlertDefinition>>(alertDefinitions);
            return new OkObjectResult(resources);
        }

        [FunctionName("GetAlertDefinition")]
        [HandleExceptions]
        public async Task<IActionResult> GetAlertDefinitionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "alert-definitions/{alertDefinitionId}")] HttpRequest req,
            string alertDefinitionId,
            ILogger log)
        {
            log.LogInformation("Executing GetAlertDefinitionAsync.");

            var alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(new Guid(alertDefinitionId));
            var resource = _mapper.Map<Resources.Model.AlertDefinition>(alertDefinition);
            return new OkObjectResult(resource);
        }

        [FunctionName("PostAlertDefinition")]
        [HandleExceptions]
        public async Task<IActionResult> PostAlertDefinitionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "alert-definitions")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Executing PostAlertDefinitionAsync.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var body = JsonConvert.DeserializeObject<Resources.Model.AlertDefinition>(requestBody);

            if (body.AlertDefinitionId != Guid.Empty)
                throw new BadRequestException("AlertDefinitionId must be empty on a POST.");

            var alertDefinition = _mapper.Map<AlertDefinition>(body);

            await alertDefinition.SaveAsync();

            alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinition.AlertDefinitionId);
            var resource = _mapper.Map<Resources.Model.AlertDefinition>(alertDefinition);
            return new OkObjectResult(resource);
        }

        [FunctionName("PutAlertDefinition")]
        [HandleExceptions]
        public async Task<IActionResult> PutAlertDefinitionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "alert-definitions/{alertDefinitionId}")] HttpRequest req,
            string alertDefinitionId,
            ILogger log)
        {
            log.LogInformation("Executing PutAlertDefinitionAsync.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var body = JsonConvert.DeserializeObject<Resources.Model.AlertDefinition>(requestBody);

            var alertDefinitionIdGuid = new Guid(alertDefinitionId);
            if (body.AlertDefinitionId != alertDefinitionIdGuid)
                throw new BadRequestException("AlertDefinitionId in body does not match alertDefinitionId provided in route.");

            var alertDefinition = _mapper.Map<AlertDefinition>(body);

            await alertDefinition.SaveAsync();

            alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinition.AlertDefinitionId);
            var resource = _mapper.Map<Resources.Model.AlertDefinition>(alertDefinition);
            return new OkObjectResult(resource);
        }

        [FunctionName("DeleteAlertDefinition")]
        [HandleExceptions]
        public async Task<IActionResult> DeleteAlertDefinitionAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "alert-definitions/{alertDefinitionId}")] HttpRequest req,
            string alertDefinitionId,
            ILogger log)
        {
            log.LogInformation("Executing DeleteAlertDefinitionAsync.");

            var alertDefinitionIdGuid = new Guid(alertDefinitionId);

            var alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinitionIdGuid);
            await alertDefinition.DeleteAsync();

            return new NoContentResult();
        }
    }
}
