using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.Authentication
{
    public class ExchangeRefreshTokenResponse : ResponseMessage
    {
        public AccessToken AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public ExchangeRefreshTokenResponse() { }

        public ExchangeRefreshTokenResponse(bool success = false, string message = null) : base(success, message)
        {
        }

        public ExchangeRefreshTokenResponse(AccessToken accessToken, string refreshToken, bool success = false, string message = null) : base(success, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
