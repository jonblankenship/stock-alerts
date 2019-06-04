using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Authentication
{

    public interface IAuthService
    {
        Task<ExchangeRefreshTokenResponse> ExchangeRefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken);
    }
}
