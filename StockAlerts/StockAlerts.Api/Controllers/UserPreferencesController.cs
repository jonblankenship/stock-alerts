using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StockAlerts.Data.Model;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Extensions;
using StockAlerts.Domain.Services;
using System;
using System.Threading.Tasks;

namespace StockAlerts.Api.Controllers
{
    [Route("api/user-preferences")]
    [ApiController]
    public class UserPreferencesController : ControllerBase
    {
        private readonly IUserPreferencesService _userPreferencesService;
        private readonly IMapper _mapper;

        public UserPreferencesController(
            IUserPreferencesService userPreferencesService,
            IMapper mapper)
        {
            _userPreferencesService = userPreferencesService ?? throw new ArgumentNullException(nameof(userPreferencesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var userPreferences = await _userPreferencesService.GetUserPreferencesAsync(HttpContext.User.GetAppUserIdClaim());
            var resources = _mapper.Map<Resources.Model.UserPreferences>(userPreferences);
            return new OkObjectResult(resources);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UserPreferences requestBody)
        {
            if (requestBody.AppUserId != HttpContext.User.GetAppUserIdClaim())
                throw new BadRequestException("AppUserId in body does not match the AppUserId of the logged in user.");

            await _userPreferencesService.GetUserPreferencesAsync(HttpContext.User.GetAppUserIdClaim());
            var userPreferences = _mapper.Map<Domain.Model.UserPreferences>(requestBody);
            await userPreferences.SaveAsync();

            userPreferences = await _userPreferencesService.GetUserPreferencesAsync(HttpContext.User.GetAppUserIdClaim());
            var resource = _mapper.Map<Resources.Model.UserPreferences>(userPreferences);
            return new OkObjectResult(resource);
        }
    }
}
