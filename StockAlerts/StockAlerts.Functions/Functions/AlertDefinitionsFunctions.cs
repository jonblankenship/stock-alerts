using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using StockAlerts.Data;
using StockAlerts.Domain.Constants;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Factories;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;
using StockAlerts.Functions.Attributes;

namespace StockAlerts.Functions
{
    public class AlertDefinitionsFunctions : FunctionBase
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;
        private readonly IAlertCriteriaSpecificationFactory _alertCriteriaSpecificationFactory;
        private readonly INotificationsService _notificationsService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AlertDefinitionsFunctions(IAlertDefinitionsService alertDefinitionsService,
            IAlertDefinitionsRepository alertDefinitionsRepository,
            IAlertCriteriaSpecificationFactory alertCriteriaSpecificationFactory,
            INotificationsService notificationsService,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _alertDefinitionsService = alertDefinitionsService ?? throw new ArgumentNullException(nameof(alertDefinitionsService));
            _alertDefinitionsRepository = alertDefinitionsRepository ?? throw new ArgumentNullException(nameof(alertDefinitionsRepository));
            _alertCriteriaSpecificationFactory = alertCriteriaSpecificationFactory ?? throw new ArgumentNullException(nameof(alertCriteriaSpecificationFactory));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
    }
}
