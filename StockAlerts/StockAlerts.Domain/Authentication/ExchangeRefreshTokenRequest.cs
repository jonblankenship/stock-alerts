﻿namespace StockAlerts.Domain.Authentication
{
    public class ExchangeRefreshTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
