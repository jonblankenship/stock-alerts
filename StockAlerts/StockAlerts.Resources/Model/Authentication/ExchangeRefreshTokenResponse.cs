namespace StockAlerts.Resources.Model.Authentication
{

    public class ExchangeRefreshTokenResponse
    {
        public AccessToken AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
