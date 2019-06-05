using StockAlerts.Domain.Authentication;
using System;
using System.Linq;
using System.Security.Claims;

namespace StockAlerts.Domain.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static void GuardIsAuthorizedForAppUserId(this ClaimsPrincipal user, Guid appUserId)
        {
            var appUserIdClaim = user.GetClaim(CustomClaimIdentifiers.AppUserId);

            if (!string.IsNullOrWhiteSpace(appUserIdClaim) && appUserIdClaim == appUserId.ToString())
            {
                return;
            }

            throw new UnauthorizedAccessException("User is not authorized to perform this action on this AppUser.");
        }

        public static string GetClaim(this ClaimsPrincipal user, string claimType)
        {
            return (from c in user.Claims
                where c.Type == claimType
                select c).SingleOrDefault()?.Value;
        }

        public static Guid GetAppUserIdClaim(this ClaimsPrincipal user) => new Guid(user.GetClaim(CustomClaimIdentifiers.AppUserId));
    }
}
