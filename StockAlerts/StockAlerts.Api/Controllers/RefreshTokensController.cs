using Microsoft.AspNetCore.Mvc;
using StockAlerts.Domain.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.Api.Controllers
{
    [Route("api/refresh-tokens")]
    [ApiController]
    public class RefreshTokensController : ControllerBase
    {
        private readonly IAuthService _authService;

        public RefreshTokensController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost]
        public async Task<IActionResult> ExchangeRefreshTokenAsync([FromBody] ExchangeRefreshTokenRequest exchangeRefreshTokenRequest, CancellationToken cancellationToken)
        {
            var result = await _authService.ExchangeRefreshTokenAsync(exchangeRefreshTokenRequest.AccessToken, exchangeRefreshTokenRequest.RefreshToken, cancellationToken);

            return new OkObjectResult(result);
        }
    }
}
