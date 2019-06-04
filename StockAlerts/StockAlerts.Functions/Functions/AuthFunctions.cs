using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockAlerts.Domain.Authentication;
using StockAlerts.Functions.Attributes;

namespace StockAlerts.Functions
{
    public class AuthFunctions
    {
        private readonly IAuthService _authService;

        public AuthFunctions(IAuthService authService)
        {
            _authService = authService;
        }

        [FunctionName("RefreshTokenFunction")]
        [HandleExceptions]
        public async Task<IActionResult> RefreshTokenFunction(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/refresh-token")] HttpRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            log.LogInformation("Executing RefreshTokenFunction.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var exchangeRefreshTokenRequest = JsonConvert.DeserializeObject<ExchangeRefreshTokenRequest>(requestBody);

            var result = await _authService.ExchangeRefreshTokenAsync(exchangeRefreshTokenRequest.AccessToken, exchangeRefreshTokenRequest.RefreshToken, cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
