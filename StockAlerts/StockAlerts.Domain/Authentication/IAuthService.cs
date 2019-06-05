using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StockAlerts.Domain.Authentication
{

    public interface IAuthService
    {
        Task<ExchangeRefreshTokenResponse> ExchangeRefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken);

        ClaimsPrincipal GetAuthenticatedPrincipal(HttpRequest request);
    }
}
