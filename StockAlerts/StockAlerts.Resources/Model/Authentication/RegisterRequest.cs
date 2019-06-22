namespace StockAlerts.Resources.Model.Authentication
{
    public class RegisterRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string TwitterHandle { get; set; }

        public string RemoteIpAddress { get; set; }
    }
}
