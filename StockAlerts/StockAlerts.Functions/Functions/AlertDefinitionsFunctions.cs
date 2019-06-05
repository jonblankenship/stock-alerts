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
using StockAlerts.Domain.Authentication;

namespace StockAlerts.Functions
{
    public class AlertDefinitionsFunctions : FunctionBase
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AlertDefinitionsFunctions(
            IAlertDefinitionsService alertDefinitionsService,
            IAuthService authService,
            IMapper mapper)
        {
            _alertDefinitionsService = alertDefinitionsService ?? throw new ArgumentNullException(nameof(alertDefinitionsService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [FunctionName("GetAlertDefinitions")]
        [HandleExceptions]
        public async Task<IActionResult> GetAlertDefinitionsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "alert-definitions")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Executing GetAlertDefinitionsAsync.");

            var claimsPrincipal = _authService.GetAuthenticatedPrincipal(req);
            if (claimsPrincipal == null)
                return new UnauthorizedResult();

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

            var claimsPrincipal = _authService.GetAuthenticatedPrincipal(req);
            if (claimsPrincipal == null)
                return new UnauthorizedResult();

            // TODO: Only allow retrieval of alert definitions owned by claimsPrincipal
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

            var claimsPrincipal = _authService.GetAuthenticatedPrincipal(req);
            if (claimsPrincipal == null)
                return new UnauthorizedResult();

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var body = JsonConvert.DeserializeObject<Resources.Model.AlertDefinition>(requestBody);            

            if (body.AlertDefinitionId != Guid.Empty)
                throw new BadRequestException("AlertDefinitionId must be empty on a POST.");

            var alertDefinition = _mapper.Map<AlertDefinition>(body);

            // TODO: Assign AppUserId of claimsPrincipal to alert definition
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

            var claimsPrincipal = _authService.GetAuthenticatedPrincipal(req);
            if (claimsPrincipal == null)
                return new UnauthorizedResult();

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var body = JsonConvert.DeserializeObject<Resources.Model.AlertDefinition>(requestBody);

            var alertDefinitionIdGuid = new Guid(alertDefinitionId);
            if (body.AlertDefinitionId != alertDefinitionIdGuid)
                throw new BadRequestException("AlertDefinitionId in body does not match alertDefinitionId provided in route.");

            var alertDefinition = _mapper.Map<AlertDefinition>(body);

            // TODO: Only allow update of alert definitions owned by the claimsPrincipal
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

            var claimsPrincipal = _authService.GetAuthenticatedPrincipal(req);
            if (claimsPrincipal == null)
                return new UnauthorizedResult();

            var alertDefinitionIdGuid = new Guid(alertDefinitionId);

            // TODO: Only allow delete of alert definitions owned by the claimsPrincipal
            var alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinitionIdGuid);
            await alertDefinition.DeleteAsync();

            return new NoContentResult();
        }
    }
}
