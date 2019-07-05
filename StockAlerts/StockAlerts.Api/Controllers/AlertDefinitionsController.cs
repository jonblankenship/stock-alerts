using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Extensions;
using StockAlerts.Domain.Services;
using StockAlerts.Resources.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockAlerts.Api.Controllers
{
    [Route("api/alert-definitions")]
    [Authorize]
    [ApiController]
    public class AlertDefinitionsController : ControllerBase
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;
        private readonly IMapper _mapper;

        public AlertDefinitionsController(
            IAlertDefinitionsService alertDefinitionsService,
            IMapper mapper)
        {
            _alertDefinitionsService = alertDefinitionsService ?? throw new ArgumentNullException(nameof(alertDefinitionsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var alertDefinitions = await _alertDefinitionsService.GetAlertDefinitionsAsync(HttpContext.User.GetAppUserIdClaim());
            var resources = _mapper.Map<IList<AlertDefinition>>(alertDefinitions);
            return new OkObjectResult(resources);
        }

        [HttpGet]
        [Route("{alertDefinitionId}")]
        public async Task<IActionResult> GetAsync(Guid alertDefinitionId)
        {
            var alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinitionId);

            HttpContext.User.GuardIsAuthorizedForAppUserId(alertDefinition.AppUserId);

            var resource = _mapper.Map<Resources.Model.AlertDefinition>(alertDefinition);
            return new OkObjectResult(resource);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AlertDefinition requestBody)
        {
            if (requestBody.AlertDefinitionId != Guid.Empty)
                throw new BadRequestException("AlertDefinitionId must be empty on a POST.");

            var alertDefinition = _mapper.Map<Domain.Model.AlertDefinition>(requestBody);
            alertDefinition.AppUserId = HttpContext.User.GetAppUserIdClaim();
            await alertDefinition.SaveAsync();

            alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinition.AlertDefinitionId);
            var resource = _mapper.Map<AlertDefinition>(alertDefinition);
            return new OkObjectResult(resource);
        }

        [HttpPut]
        [Route("{alertDefinitionId}")]
        public async Task<IActionResult> PutAsync([FromBody] AlertDefinition requestBody, Guid alertDefinitionId)
        {
            if (requestBody.AlertDefinitionId != alertDefinitionId)
                throw new BadRequestException("AlertDefinitionId in body does not match alertDefinitionId provided in route.");

            var existingAlertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinitionId);
            HttpContext.User.GuardIsAuthorizedForAppUserId(existingAlertDefinition.AppUserId);

            var alertDefinition = _mapper.Map<Domain.Model.AlertDefinition>(requestBody);

            await alertDefinition.SaveAsync();

            alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinition.AlertDefinitionId);
            var resource = _mapper.Map<AlertDefinition>(alertDefinition);
            return new OkObjectResult(resource);
        }

        [HttpDelete]
        [Route("{alertDefinitionId}")]
        public async Task<IActionResult> DeleteAsync(Guid alertDefinitionId)
        {
            var alertDefinition = await _alertDefinitionsService.GetAlertDefinitionAsync(alertDefinitionId);
            HttpContext.User.GuardIsAuthorizedForAppUserId(alertDefinition.AppUserId);
            await alertDefinition.DeleteAsync();

            return new NoContentResult();
        }
    }
}
