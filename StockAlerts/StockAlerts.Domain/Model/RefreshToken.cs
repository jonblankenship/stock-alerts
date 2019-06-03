using System;

namespace StockAlerts.Domain.Model
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; private set; }
        public string Token { get; private set; }
        public DateTime Expires { get; private set; }
        public Guid AppUserId { get; private set; }
        public bool Active => DateTime.UtcNow <= Expires;
        public string RemoteIpAddress { get; private set; }

        public RefreshToken(string token, DateTime expires, Guid appUserId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            AppUserId = appUserId;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
