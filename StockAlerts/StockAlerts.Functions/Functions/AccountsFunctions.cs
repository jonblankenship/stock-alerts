using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using StockAlerts.Functions.Attributes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockAlerts.Domain.Authentication;

namespace StockAlerts.Functions
{
    public class AccountsFunctions
    {
        private readonly IAccountsService _accountsService;

        public AccountsFunctions(
            IAccountsService accountsService)
        {
            _accountsService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));
        }

        [FunctionName("RegisterAccountFunction")]
        [HandleExceptions]
        public async Task<IActionResult> RegisterAccountFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "accounts/register")] HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("Executing RegisterAccountFunction.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var registerRequest = JsonConvert.DeserializeObject<RegisterRequest>(requestBody);
            registerRequest.RemoteIpAddress = req.HttpContext.Connection.RemoteIpAddress?.ToString();

            await _accountsService.RegisterUserAsync(registerRequest, cancellationToken);

            var loginRequest = new LoginRequest(registerRequest);

            return await LoginAsync(loginRequest, cancellationToken);
        }

        [FunctionName("LoginFunction")]
        [HandleExceptions]
        public async Task<IActionResult> LoginFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "accounts/login")] HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("Executing LoginFunction.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var loginRequest = JsonConvert.DeserializeObject<LoginRequest>(requestBody);
            loginRequest.RemoteIpAddress = req.HttpContext.Connection.RemoteIpAddress?.ToString();

            return await LoginAsync(loginRequest, cancellationToken);
        }

        private async Task<IActionResult> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var response = await _accountsService.LoginAsync(loginRequest, cancellationToken);

            return new OkObjectResult(response);
        }
    }
}
