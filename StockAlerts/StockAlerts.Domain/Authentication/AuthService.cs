using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Settings;

namespace StockAlerts.Domain.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenValidator _jwtTokenValidator;
        private readonly IAppUsersRepository _appUsersRepository;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly ITokenFactory _tokenFactory;
        private readonly ISettings _settings;

        public AuthService(
            IJwtTokenValidator jwtTokenValidator,
            IAppUsersRepository appUsersRepository,
            IJwtTokenFactory jwtTokenFactory,
            ITokenFactory tokenFactory,
            ISettings settings)
        {
            _jwtTokenValidator = jwtTokenValidator ?? throw new ArgumentNullException(nameof(jwtTokenValidator));
            _appUsersRepository = appUsersRepository ?? throw new ArgumentNullException(nameof(appUsersRepository));
            _jwtTokenFactory = jwtTokenFactory ?? throw new ArgumentNullException(nameof(jwtTokenFactory));
            _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<ExchangeRefreshTokenResponse> ExchangeRefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken)
        {
            var currentPrincipal = _jwtTokenValidator.GetPrincipalFromToken(accessToken, _settings.AuthSettings.SecretKey);

            // invalid token/signing key was passed and we can't extract user claims
            if (currentPrincipal != null)
            {
                var appUserId = currentPrincipal.Claims.First(c => c.Type == CustomClaimIdentifiers.AppUserId).Value;
                var appUser = await _appUsersRepository.GetAppUserAsync(new Guid(appUserId), cancellationToken);

                if (appUser.HasValidRefreshToken(refreshToken))
                {
                    var jwtToken = await _jwtTokenFactory.GenerateEncodedTokenAsync(appUser.UserId.ToString(), appUser.UserName, appUser.AppUserId.ToString());
                    var newRefreshToken = _tokenFactory.GenerateToken();
                    appUser.RemoveRefreshToken(refreshToken); // delete the token we've exchanged
                    appUser.AddRefreshToken(newRefreshToken, appUser.UserId, ""); // add the new one
                    await appUser.SaveAsync(cancellationToken);
                    return new ExchangeRefreshTokenResponse(jwtToken, newRefreshToken, true);
                }
            }

            return new ExchangeRefreshTokenResponse(false, "Invalid token.");
        }
    }
}
