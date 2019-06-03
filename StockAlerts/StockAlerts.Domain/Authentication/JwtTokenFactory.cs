using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Settings;

namespace StockAlerts.Domain.Authentication
{
    // https://medium.com/@st.mas29/microsoft-blazor-web-api-with-jwt-authentication-part-1-f33a44abab9d
    public class JwtTokenFactory : IJwtTokenFactory
    {
        private readonly IJwtTokenHandler _jwtTokenHandler;
        private readonly JwtIssuerOptions _jwtIssuerOptions;
        private readonly JwtOptions _jwtOptions;

        public JwtTokenFactory(
            IConfiguration config,
            IJwtTokenHandler jwtTokenHandler,
            ISettings settings)
        {
            _jwtTokenHandler = jwtTokenHandler ?? throw new ArgumentNullException(nameof(jwtTokenHandler));
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            _jwtIssuerOptions = settings.JwtIssuerOptions;
            _jwtOptions = settings.JwtOptions;

            ThrowIfInvalidOptions(_jwtIssuerOptions);
        }

        /// <summary>
        /// Builds the token used for authentication
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Obsolete]
        public string BuildToken(IdentityUser user, AppUser appUser)
        {
            // Create a claim based on the user's email.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(CustomClaimIdentifiers.AppUserId, appUser.AppUserId.ToString()),
            };

            // Creates a key from our private key that will be used in the security algorithm next
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            // Credentials that are encrypted which can only be created by our server using the private key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // this is the actual token that will be issued to the user
            var token = new JwtSecurityToken(_jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.ExpireTime),
                signingCredentials: creds);

            // return the token in the correct format using JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AccessToken> GenerateEncodedTokenAsync(string id, string userName, string appUserId)
        {
            var identity = GenerateClaimsIdentity(id, userName, appUserId);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, id),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtIssuerOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtIssuerOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(CustomClaimIdentifiers.Rol),
                identity.FindFirst(CustomClaimIdentifiers.Id),
                identity.FindFirst(CustomClaimIdentifiers.AppUserId)
            };

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
            _jwtIssuerOptions.Issuer,
            _jwtIssuerOptions.Audience,
            claims,
            _jwtIssuerOptions.NotBefore,
            _jwtIssuerOptions.Expiration,
            _jwtIssuerOptions.SigningCredentials);
            return new AccessToken(_jwtTokenHandler.WriteToken(jwt), (int)_jwtIssuerOptions.ValidFor.TotalSeconds);
        }

        private static ClaimsIdentity GenerateClaimsIdentity(string id, string userName, string appUserId)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(CustomClaimIdentifiers.Id, id),
                new Claim(CustomClaimIdentifiers.AppUserId, appUserId),
                new Claim(CustomClaimIdentifiers.Rol, CustomClaims.ApiAccess)
            });
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
    }
}
