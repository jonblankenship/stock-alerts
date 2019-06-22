namespace StockAlerts.Resources.Model.Authentication
{
    public class LoginRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string RemoteIpAddress { get; set; }
    }
}
