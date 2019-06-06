using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockAlerts.Domain.Authentication;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Extensions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Services;
using StockAlerts.Functions.Attributes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StockAlerts.Functions
{
    public class UserPreferencesFunctions : FunctionBase
    {
        private readonly IUserPreferencesService _userPreferencesService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public UserPreferencesFunctions(
            IUserPreferencesService userPreferencesService,
            IAuthService authService,
            IMapper mapper)
        {
            _userPreferencesService = userPreferencesService ?? throw new ArgumentNullException(nameof(userPreferencesService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [FunctionName("GetUserPreferences")]
        [HandleExceptions]
        public async Task<IActionResult> GetUserPreferencesAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user-preferences")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Executing GetUserPreferencesAsync.");

            var claimsPrincipal = _authService.GetAuthenticatedPrincipal(req);
            if (claimsPrincipal == null)
                return new UnauthorizedResult();

            var userPreferences = await _userPreferencesService.GetUserPreferencesAsync(claimsPrincipal.GetAppUserIdClaim());
            var resources = _mapper.Map<Resources.Model.UserPreferences>(userPreferences);
            return new OkObjectResult(resources);
        }

        [FunctionName("PutUserPreferences")]
        [HandleExceptions]
        public async Task<IActionResult> PutUserPreferencesAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "user-preferences")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Executing PutUserPreferencesAsync.");

            var claimsPrincipal = _authService.GetAuthenticatedPrincipal(req);
            if (claimsPrincipal == null)
                return new UnauthorizedResult();

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var body = JsonConvert.DeserializeObject<Resources.Model.UserPreferences>(requestBody);

            if (body.AppUserId != claimsPrincipal.GetAppUserIdClaim())
                throw new BadRequestException("AppUserId in body does not match the AppUserId of the logged in user.");

            await _userPreferencesService.GetUserPreferencesAsync(claimsPrincipal.GetAppUserIdClaim());

            var userPreferences = _mapper.Map<UserPreferences>(body);

            await userPreferences.SaveAsync();

            userPreferences = await _userPreferencesService.GetUserPreferencesAsync(claimsPrincipal.GetAppUserIdClaim());
            var resource = _mapper.Map<Resources.Model.UserPreferences>(userPreferences);
            return new OkObjectResult(resource);
        }
    }
}
